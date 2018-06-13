using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Air_Traffic_Simulation
{
    public partial class Form1 : Form
    {
        //TODO: make sure the airplane spawns at the center of the airfield

        //AIRPLAIN GRAPHICS

        Image airplaneImage;
        Rectangle airplaneRect;

        //RADAR
        Random r = new Random();
        Timer t = new Timer();
        int width, height;
        int hand = 150;
        int u; //in degree
        int cx, cy; //center of the circle
        int x, y; //hand coordinates
        int tx, ty, lim = 20;
        Bitmap bmp;
        Pen p;
        Graphics g;


        //GRID
        Grid grid;
        Point p1, p2, p3, p4;
        Pen pGrid;
        Graphics gGrid;
        Bitmap bmpGrid;


        // CHECKPOINT CIRCLE POINT LOL
        Graphics grCircle;
        Pen pnCircle;


        //WEATHER VALUES
        string wdComboBox;
        int temp, precIntencity, windSpeed;
        WindDirection windDirection;
        SolidBrush weatherBrush = new SolidBrush(Color.White);
        WeatherConditions weather;
        Image weatherImage;
        Rectangle weatherRect;
        private bool weatherActive = true;


        //List<Checkpoint> checkpoints;
        string dir;
        string serializationFile;
        private bool settingTakeOffDirection = false;
        int AddingCheckpoints = 0;
        int RemovingCheckpoints = 0;
        int AddingAirplanes = 0;
        int RemovingAirplanes = 0;
        int RandomAirplane = 0;
        static int cpName = 0;
        static int apName = 0;
        static int fnName = 0;
        BinaryFormatter bf;

        private Airplane selectedAirplane;

        //Crashed airplanes list
        List<Airplane> crashedAirplanes;
        Image explosionImage;

        //SIMULATION
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;


        private Airstrip landingStrip;
        List<Checkpoint> checkpoints;
        List<Airplane> airplaneList;

        /// <summary>
        /// A collection of checkpoints, which represent the places on the grid, for which an airplane can be aiming
        /// in torder to exit the airspace.
        /// </summary>
        private List<Checkpoint> takeOffDirectionCheckpoints;

        private List<Airplane> landedAirplanes;
        private List<Airplane> successfulylExitedAirspace;


        public Form1()
        {
            dir = @"..\..\Saved";
            serializationFile = Path.Combine(dir, "Checkpoints.bin");

            

            checkpoints = new List<Checkpoint>();
            airplaneList = new List<Airplane>();
            crashedAirplanes = new List<Airplane>();
            landedAirplanes = new List<Airplane>();
            takeOffDirectionCheckpoints = new List<Checkpoint>();
            successfulylExitedAirspace = new List<Airplane>();
            InitializeComponent();
            nSpeed.Enabled = false;
            weatherRect = new Rectangle(x, y, 60, 60);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //GRID VALUES
            width = this.pictureBox1.Width;
            height = this.pictureBox1.Height;
            grid = new Grid(width, height);


            //WEATHER GUI
            labelWind.Text = trackBarWindSpeed.Value.ToString() + "m/s";
            labelTemp.Text = trackBarTemperature.Value.ToString() + "°C";
            labelPrec.Text = trackBarPrecipitation.Value.ToString() + "%";
            comboBoxWindDirection.Items.AddRange(new object[]
                {"NORTH", "NORTH-EAST", "NORTH-WEST", "SOUTH", "SOUTH-EAST", "SOUTH-WEST", "EAST", "WEST"});

            Array values = Enum.GetValues(typeof(WindDirection));
            WindDirection randomDirection = (WindDirection)values.GetValue(r.Next(values.Length));
            comboBoxWindDirection.SelectedItem = randomDirection.ToString();
            //WEATHER VALUES
            windSpeed = trackBarWindSpeed.Value;
            temp = trackBarTemperature.Value;
            precIntencity = trackBarPrecipitation.Value;
            weatherRect.X = r.Next(grid.ColumnsOfCells * Cell.Width - Cell.Width);
            weatherRect.Y = r.Next(grid.RowsOfCells * Cell.Width - Cell.Width);

            

            weather = new WeatherConditions(windSpeed, windDirection, temp, precIntencity);
            //LabelChange();


            //RADAR

            //create bitmap
            bmp = new Bitmap(width + 1, height + 1);

            //background color
            pictureBox1.BackColor = Color.Black;

            //center
            cx = width / 2;
            cy = height / 2;

            //initial degree of hand
            u = 0;

            //timer
            //t.Interval = 1; //miliseconds
            //t.Tick += new EventHandler(this.t_Tick);
            //t.Start();


            //GRID

            //pictureBox2.BackColor = Color.Transparent;
            bmpGrid = new Bitmap(width, height);
            gGrid = Graphics.FromImage(bmpGrid);
            pGrid = new Pen(Color.Green, 1f);

            grid.MakeGrid();

            foreach (Cell c in grid.listOfCells)
            {
                p1 = new Point(c.x, c.y);
                p2 = new Point(c.x + Cell.Width, c.y);
                p3 = new Point(c.x + Cell.Width, c.y + Cell.Width);
                p4 = new Point(c.x, c.y + Cell.Width);

                gGrid.DrawLine(pGrid, p1, p2);
                gGrid.DrawLine(pGrid, p1, p4);
                gGrid.DrawLine(pGrid, p2, p3);
                gGrid.DrawLine(pGrid, p3, p4);
            }

            int x = r.Next(100, 200);
            //int y = r.Next(100, 200);
            //weatherBlock = new Rectangle(x, x + 50, 50, 50);

            pictureBox1.Image = bmpGrid;
            PaintGrid();
            makeAirstrip();
            populateTakeOffDirections();

            simSpeedComboBox.SelectedIndex = 2;
            rbLanding.Checked = true;
        }

        /// <summary>
        /// Populates a list of all the <see cref="Checkpoint"/>s that can be set as
        /// the take off direction of a landed airplane.
        /// </summary>
        private void populateTakeOffDirections()
        {
            int takeOffDirectionsCounter = 0;
            foreach (Cell c in grid.listOfCells)
            {
                if (c.Type == CellType.BORDER)
                {
                    string name = "outer checkpoint" + takeOffDirectionsCounter++;

                    Checkpoint a = new Checkpoint(name, c.GetCenter().X, c.GetCenter().Y, c, checkpoints, landingStrip,
                        takeOffDirectionCheckpoints);
                    takeOffDirectionCheckpoints.Add(a);
                }
            }
        }

        /// <summary>
        /// Initializes and draws the airstrip.
        /// </summary>
        private void makeAirstrip()
        {
            //using the x and y of the last cell in the grid as comparison, because in some cases it might
            //happen so that the picturebox is actually bigger than the grid, so using the picturebox' 
            //width and height would lead to wrong results
            Cell middleCell = grid.listOfCells
                .Where(cell => cell.ContainsPoint(grid.listOfCells[grid.listOfCells.Count - 1].x / 2,
                    grid.listOfCells[grid.listOfCells.Count - 1].y / 2)).ElementAt(0);
            landingStrip = new Airstrip("Strip A", middleCell.x, middleCell.y, true, 360);
            Point p = new Point(middleCell.x, middleCell.y);

            Brush pen = new SolidBrush(Color.Green);
            Graphics g = this.pictureBox1.CreateGraphics();
            gGrid.FillRectangle(pen, p.X, p.Y, Cell.Width, Cell.Width);
        }

        /// <summary>
        /// Moves the weather based on wind direction
        /// </summary>
        private void weatherMovement()
        {
            try
            {
                foreach (Checkpoint c in this.checkpoints)
                {
                    foreach (Cell cl in grid.listOfCells)
                    {
                        if (cl.ContainsPoint((int) c.CoordinateX, (int) c.CoordinateY) == true)
                        {
                            if (weatherRect.Contains((int) c.CoordinateX, (int) c.CoordinateY))
                            {
                                weatherOnCheckpoint(c, EventArgs.Empty);
                            }
                            else
                            {
                                if (windDirection == WindDirection.NORTH)
                                {
                                    weatherRect.Y += 10;
                                }
                                else if (windDirection == WindDirection.NORTHEAST)
                                {
                                    weatherRect.Y += 10;
                                    weatherRect.X -= 10;
                                }
                                else if (windDirection == WindDirection.NORTHWEST)
                                {
                                    weatherRect.Y += 10;
                                    weatherRect.X += 10;
                                }
                                else if (windDirection == WindDirection.SOUTH)
                                {
                                    weatherRect.Y -= 10;
                                }
                                else if (windDirection == WindDirection.SOUTHEAST)
                                {
                                    weatherRect.Y -= 10;
                                    weatherRect.X -= 10;
                                }
                                else if (windDirection == WindDirection.SOUTHWEST)
                                {
                                    weatherRect.Y -= 10;
                                    weatherRect.X += 10;
                                }
                                else if (windDirection == WindDirection.EAST)
                                {
                                    weatherRect.X -= 10;
                                }
                                else if (windDirection == WindDirection.WEST)
                                {
                                    weatherRect.X += 10;
                                }
                            }
                        }
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("Something went wrong with checkpoints list.");
            }
        }

        /// <summary>
        /// Event handling method that triggers when the airplane has reached it's final destination.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void airplaneHasReachedTheAirport(Object sender, EventArgs e)
        {
            ((Airplane) sender).OnAirportReached -= airplaneHasReachedTheAirport;
            this.landedAirplanes.Add((Airplane) sender);
            landedAirplanesListBox.Items.Add((Airplane) sender);
            this.airplaneList.Remove((Airplane) sender);
            allFlightsListBox.Items.Remove(sender);
        }

        /// <summary>
        /// Event handling method that triggers when the airplane has reached the end of airspace
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void airplaneHasReachedTheEndOfTheAirspace(Object sender, EventArgs e)
        {
            ((Airplane) sender).OnAirspaceExit -= airplaneHasReachedTheEndOfTheAirspace;
            successfulylExitedAirspace.Add(((Airplane) sender));
            allFlightsListBox.Items.Remove(((Airplane) sender));
            airplaneList.Remove(((Airplane) sender));
        }

        /// <summary>
        /// Event handling method that triggers when the danger zones of two or more aiplanes have collided and they take part in a crash.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        private void airplaneCrashed(Object p1, Object p2)
        {
            UnsubscribePlane(p1);
            UnsubscribePlane(p2);
        }

        /// <summary>
        /// Unsubscribes an airplane from the event handler
        /// </summary>
        /// <param name="p1"></param>
        private void UnsubscribePlane(Object p1)
        {
            ((Airplane) p1).OnCrash -= airplaneCrashed;
            this.airplaneList.Remove((Airplane) p1);
            allFlightsListBox.Items.Remove(p1);
            crashedAirplanes.Add((Airplane) p1);
            Console.WriteLine(((Airplane) p1).Name + " has crashed!");
        }

        
        private void t_Tick(object sender, EventArgs e)
        {
        }

        private void Header_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void Header_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void Header_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        /// <summary>
        /// Exits the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void button4_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// Enables menu for adding checkpoints
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddCheckpoint_Click(object sender, EventArgs e)
        {
            if (btnAddCheckpoint.Text == "Add")
            {
                btnRemoveCheckpoint.Enabled = false;
                btnAddAirplane.Enabled = false;
                btnRemoveAirplane.Enabled = false;
                trackBar1.Enabled = false;
                AddingCheckpoints = 1;
                btnAddCheckpoint.Text = "Stop";
            }
            else if (btnAddCheckpoint.Text == "Stop")
            {
                btnRemoveCheckpoint.Enabled = true;
                if (RandomAirplane == 0)
                {
                    btnAddAirplane.Enabled = true;
                    btnRemoveAirplane.Enabled = true;
                }

                trackBar1.Enabled = true;
                AddingCheckpoints = 0;
                btnAddCheckpoint.Text = "Add";
            }
        }

        /// <summary>
        /// Sets new value for the temperature if trackBar value is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBarTemperature_ValueChanged(object sender, EventArgs e)
        {
            temp = trackBarTemperature.Value;
            labelTemp.Text = temp.ToString() + "°C";
            LabelChange();
        }

        /// <summary>
        /// Function that saves the current state of the application to a file, that can be loaded afterword.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveData_Click(object sender, EventArgs e)
        {
            SavingObjects so = new SavingObjects(airplaneList, landedAirplanes, checkpoints);

            SaveFileDialog SaveFileDialogMain = new SaveFileDialog();
            SaveFileDialogMain.Filter = "Binary Files (*.bin)|*.bin";
            SaveFileDialogMain.DefaultExt = "bin";
            SaveFileDialogMain.AddExtension = true;
            if (SaveFileDialogMain.ShowDialog() == DialogResult.OK)
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                string filename = SaveFileDialogMain.FileName;
                FileStream filestream = new FileStream(filename, FileMode.Create, FileAccess.Write);
                bformatter.Serialize(filestream, so);
                filestream.Close();
                filestream = null;
            }

            //using (Stream stream = File.Open(serializationFile, FileMode.Create))
            //{
            //    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            //    bformatter.Serialize(stream, checkpoints);
            //}
        }

        /// <summary>
        /// Generates and draws the route between the example airplane and the airfield.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void calcRouteBtn_Click(object sender, EventArgs e)
        {
            #region MissingCheckpointsErrorDisplay

            //generates the message box that informs the user that some areas are missing points
            bool[] allZonesCheck = new bool[] {false, false, false, false};
            string lacking =
                $"   - UPPER{Environment.NewLine}   - MIDDLE{Environment.NewLine}   - LOWER{Environment.NewLine}   - FINAL";

            foreach (Checkpoint point in checkpoints)
            {
                if (point.ParentCellType == CellType.UPPER)
                {
                    lacking = lacking.Replace($"   - UPPER{Environment.NewLine}", string.Empty);
                    allZonesCheck[0] = true;
                }
                else if (point.ParentCellType == CellType.MIDDLE)
                {
                    lacking = lacking.Replace($"   - MIDDLE{Environment.NewLine}", string.Empty);
                    allZonesCheck[1] = true;
                }
                else if (point.ParentCellType == CellType.LOWER)
                {
                    lacking = lacking.Replace($"   - LOWER{Environment.NewLine}", string.Empty);
                    allZonesCheck[2] = true;
                }
                else if (point.ParentCellType == CellType.FINAL)
                {
                    lacking = lacking.Replace("   - FINAL", string.Empty);
                    allZonesCheck[3] = true;
                }
            }

            if (allZonesCheck.Contains(false))
            {
                MessageBox.Show(this,
                    $"There seem to be no checkpoints in the following zones:{Environment.NewLine}{Environment.NewLine}{lacking}",
                    "Missing Checkpoint", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            #endregion

            foreach (Airplane plane in airplaneList)
            {
                if (plane.IsLanding)
                {
                    plane.CalculateShortestPathToAirstrip(this.checkpoints, this.landingStrip);
                }
                else
                {
                    plane.FindShortestPathLeavingAirspace(checkpoints);
                }
            }


            this.Invalidate();
            //pictureBox1.Invalidate();
        }

        /// <summary>
        /// Creates a line between two points on the grid.
        /// </summary>
        /// <param name="a">The initial point.</param>
        /// <param name="b">The end point.</param>
        public void ConnectDots(Point a, Point b, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Yellow);
            e.Graphics.DrawLine(pen, a, b);
        }

        /// <summary>
        /// The method that catches the mouse click on the picture 
        /// box and decides what to do with it..
        /// </summary>
        /// <param name="sender">The picture box that caused the method call.</param>
        /// <param name="e">The MouseEventArgs that carries the coordinates of the mouse click on the grid</param>
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (AddingCheckpoints == 1)
            {
                foreach (Cell c in grid.listOfCells)
                {
                    if (c.ContainsPoint(e.X, e.Y) == true)
                    {
                        bool exists = false;
                        Point p = c.GetCenter();
                        foreach (Checkpoint mm in checkpoints)
                        {
                            if (mm.CoordinateX == p.X && mm.CoordinateY == p.Y)
                            {
                                MessageBox.Show("Checkpoint already exists at this location", "WARNING");
                                exists = true;
                            }
                        }

                        if (!exists)
                        {
                            if (c.Type == CellType.BORDER)
                            {
                                MessageBox.Show("Selected area is for airplanes only.");
                            }
                            else
                            {
                                //PaintCircle(p);

                                pictureBox1.Invalidate();
                                cpName++;
                                string name = "cp" + cpName;

                                Checkpoint a = new Checkpoint(name, p.X, p.Y, c, checkpoints, landingStrip,
                                    takeOffDirectionCheckpoints);
                                a.OnWeatherPassing += weatherOnCheckpoint;
                                checkpoints.Add(a);
                                MessageBox.Show("Added checkpoint  " + a.Name + "  With coordinates: (" +
                                                a.CoordinateX +
                                                "," + a.CoordinateY + ")");
                            }
                        }
                    }
                }
            }
            else if (RemovingCheckpoints == 1)
            {
                foreach (Cell c in grid.listOfCells)
                {
                    if (c.ContainsPoint(e.X, e.Y) == true)
                    {
                        Point p = c.GetCenter();
                        foreach (Checkpoint bee in checkpoints)
                        {
                            if (bee.CoordinateX == p.X && bee.CoordinateY == p.Y)
                            {
                                PaintCircleB(p);
                                Checkpoint v = bee;
                                checkpoints.Remove(bee);
                                MessageBox.Show("Successfully removed checkpoint " + v.Name + " With coordinates(" +
                                                v.CoordinateX + "," + v.CoordinateY + ").");
                                break;
                            }
                        }
                    }
                }
            }
            else if (AddingAirplanes == 1)
            {
                foreach (Cell c in grid.listOfCells)
                {
                    if (c.ContainsPoint(e.X, e.Y) == true)
                    {
                        bool exists = false;
                        Point p = c.GetCenter();
                        foreach (Airplane mm in airplaneList)
                        {
                            if (mm.CoordinateX == p.X && mm.CoordinateY == p.Y)
                            {
                                MessageBox.Show("Airplane already exists at this location", "WARNING");
                                exists = true;
                            }
                        }

                        if (!exists)
                        {
                            if (c.Type == CellType.BORDER)
                            {
                                pictureBox1.Invalidate();

                                apName++;
                                fnName += 6 * 2 / 3;
                                string name = "Airplane" + apName;
                                string flight = "fn" + fnName + "z";

                                Airplane a = new Airplane(name, p.X, p.Y, Convert.ToDouble(nSpeed.Value), flight);
                                a.OnAirportReached += airplaneHasReachedTheAirport;
                                a.OnCrash += airplaneCrashed;
                                airplaneList.Add(a);
                                allFlightsListBox.Items.Add(a);

                                allFlightsListBox.SelectedIndex = allFlightsListBox.Items.Count - 1;

                                MessageBox.Show("Added Airplane  " + a.Name + "\nCoordinates: \t(" + a.CoordinateX +
                                                "," + a.CoordinateY + ")" + "\nFlight number: \t(" + flight + ") " +
                                                "\nSpeed: \t\t(" + nSpeed.Value + "kts)");
                            }
                            else
                            {
                                MessageBox.Show("This area is used for checkpoints only");
                            }
                        }
                    }
                }
            }
            else if (RemovingAirplanes == 1)
            {
                foreach (Cell c in grid.listOfCells)
                {
                    if (c.ContainsPoint(e.X, e.Y) == true)
                    {
                        Point p = c.GetCenter();
                        foreach (Airplane bee in airplaneList)
                        {
                            if (bee.CoordinateX == p.X && bee.CoordinateY == p.Y)
                            {
                                PaintRectangleY(p);
                                Airplane v = bee;
                                bee.OnAirportReached -= airplaneHasReachedTheAirport;
                                bee.OnCrash -= airplaneCrashed;
                                airplaneList.Remove(bee);
                                allFlightsListBox.Items.Remove(bee);

                                MessageBox.Show("Successfully removed Airplane " + v.Name + " With coordinates(" +
                                                v.CoordinateX + "," + v.CoordinateY + ").");
                                break;
                            }
                        }
                    }
                }
            }
            else if (settingTakeOffDirection)
            {
                setAirplaneTakeOffDirection(e);
                settingTakeOffDirection = false;
            }
        }

        /// <summary>
        /// Finds the checkpoint (in the outermost circle) that has been clicked on and sets it
        /// as the take off direction for the landed airplane.
        /// </summary>
        /// <param name="e">The MouseEventArgs that carries the coordinates of the mouse click on the grid.</param>
        private void setAirplaneTakeOffDirection(MouseEventArgs e)
        {
            Console.WriteLine(this.selectedAirplane);
            foreach (Cell c in grid.listOfCells)
            {
                if (c.ContainsPoint(e.X, e.Y) == true)
                {
                    foreach (Checkpoint exitPoint in takeOffDirectionCheckpoints)
                    {
                        if (Math.Abs(exitPoint.CoordinateX - c.GetCenter().X) < 2 &&
                            Math.Abs(exitPoint.CoordinateY - c.GetCenter().Y) < 2)
                        {
                            this.selectedAirplane.exitDestination = exitPoint;
                            selectedAirplane.IsLanding = false;
                            airplaneList.Add(selectedAirplane);
                            landedAirplanes.Remove(selectedAirplane);
                            selectedAirplane.FindShortestPathLeavingAirspace(checkpoints);
                            selectedAirplane.OnAirspaceExit += airplaneHasReachedTheEndOfTheAirspace;
                            ClearListboxes();
                            UpdateListboxes();
                            break;
                        }
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Enables menu for adding airplanes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddAirplane_Click(object sender, EventArgs e)
        {
            if (btnAddAirplane.Text == "Add")
            {
                btnRemoveAirplane.Enabled = false;
                btnAddCheckpoint.Enabled = false;
                btnRemoveCheckpoint.Enabled = false;
                trackBar1.Enabled = false;
                nSpeed.Enabled = true;
                AddingAirplanes = 1;
                btnAddAirplane.Text = "Stop";
            }
            else if (btnAddAirplane.Text == "Stop")
            {
                btnRemoveAirplane.Enabled = true;
                btnAddCheckpoint.Enabled = true;
                btnRemoveCheckpoint.Enabled = true;
                trackBar1.Enabled = true;
                nSpeed.Enabled = false;
                AddingAirplanes = 0;
                btnAddAirplane.Text = "Add";
            }
        }

        /// <summary>
        /// Enables to remove airplanes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveAirplane_Click(object sender, EventArgs e)
        {
            if (btnRemoveAirplane.Text == "Remove")
            {
                btnAddAirplane.Enabled = false;
                btnAddCheckpoint.Enabled = false;
                btnRemoveCheckpoint.Enabled = false;
                trackBar1.Enabled = false;
                RemovingAirplanes = 1;
                btnRemoveAirplane.Text = "Stop";
            }
            else if (btnRemoveAirplane.Text == "Stop")
            {
                btnAddAirplane.Enabled = true;
                btnAddCheckpoint.Enabled = true;
                btnRemoveCheckpoint.Enabled = true;
                trackBar1.Enabled = true;
                RemovingAirplanes = 0;
                btnRemoveAirplane.Text = "Remove";
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (trackBar1.Value == 1)
            {
                //airplaneList.Clear();
                btnAddAirplane.Enabled = false;
                btnRemoveAirplane.Enabled = false;
                RandomAirplane = 1;
                Random random = new Random();
                int rnd = random.Next(1, 10);
                for (int i = 0; i < rnd; i++)
                {
                    CreateRandomAirplane();
                }
            }

            if (trackBar1.Value == 0)
            {
                btnAddAirplane.Enabled = true;
                btnRemoveAirplane.Enabled = true;
                RandomAirplane = 0;
            }
        }

        /// <summary>
        /// Changes value for wind label when trackbar value is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBarWindSpeed_Scroll(object sender, EventArgs e)
        {
            LabelChange();
        }

        /// <summary>
        /// Drawign danger zone around airplane
        /// </summary>
        /// <param name="airplane"></param>
        public void DrawDangerArea(Airplane airplane, PaintEventArgs e)
        {
            Pen p = new Pen(Color.Black);
            e.Graphics.DrawEllipse(p, airplane.Rect);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();


            foreach (Airplane p in airplaneList.ToArray())
            {
                if (p.ShortestPath.Count != 0)
                {
                    p.MoveTowardsNextPoint();
                }

                if (airplaneList.Count() != 0)
                {
                    for (int i = 0; i < airplaneList.Count(); i++)
                    {
                        if (p != airplaneList[i])
                        {
                            p.DangerCheck(airplaneList[i]);
                        }
                    }
                }
            }

            if (weatherActive)
            {
                pictureBox1.Invalidate();
                weatherMovement();
            }
        }

        /// <summary>
        /// Uploads data from a file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUploadData_Click(object sender, EventArgs e)
        {
            //using (Stream stream = File.Open(serializationFile, FileMode.Open))
            //{
            //    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            //    checkpoints = (List<Checkpoint>)bformatter.Deserialize(stream);
            //}


            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Binary Files (*.bin)|*.bin";
            ofd.DefaultExt = "bin";
            ofd.AddExtension = true;
            //using (Stream stream = File.Open(serializationFile, FileMode.Create))


            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                string filename = ofd.FileName;
                FileStream filestream = new FileStream(filename, FileMode.Open, FileAccess.Read);

                // REMOVE ALL DATA FROM CURRENT SESSION

                foreach (Checkpoint bee in checkpoints)
                {
                    Checkpoint B = bee;
                    Point p = new Point(Convert.ToInt32(B.CoordinateX), Convert.ToInt32(B.CoordinateY));
                    PaintCircleB(p);
                    cpName = 0;
                }


                checkpoints = null;
                airplaneList = null;
                landedAirplanes = null;
                SavingObjects so = null;

                so = (SavingObjects) bformatter.Deserialize(filestream);
                checkpoints = so.getCheckpoints;
                airplaneList = so.getAirplanes;
                landedAirplanes = so.getGroundplanes;

                filestream.Close();
                filestream = null;

                if (checkpoints != null)
                {
                    foreach (Checkpoint a in checkpoints)
                    {
                        pictureBox1.Invalidate();
                        //PaintCircle(new Point(Convert.ToInt32(a.CoordinateX), Convert.ToInt32(a.CoordinateY)));
                        cpName++;
                    }
                }

                if (airplaneList != null)
                {
                    foreach (Airplane a in airplaneList)
                    {
                        apName++;
                        fnName += 6 * 2 / 3;
                        a.OnAirportReached += airplaneHasReachedTheAirport;
                        a.OnCrash += airplaneCrashed;
                        allFlightsListBox.Items.Add(a);
                    }

                    pictureBox1.Invalidate();
                }

                if (landedAirplanes != null)
                {
                    foreach (Airplane a in landedAirplanes)
                    {
                        allFlightsListBox.Items.Add(a.Name + "\t" + a.FlightNumber);
                    }
                }
            }

            //foreach (Checkpoint a in checkpoints)
            //{
            //    PaintCircle(new Point(Convert.ToInt32(a.CoordinateX),Convert.ToInt32(a.CoordinateY)));
            //    cpName++;
            //}
        }

        /// <summary>
        /// Enables to remove checkpoints
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveCheckpoint_Click(object sender, EventArgs e)
        {
            if (btnRemoveCheckpoint.Text == "Remove")
            {
                btnAddCheckpoint.Enabled = false;
                btnAddAirplane.Enabled = false;
                btnRemoveAirplane.Enabled = false;
                trackBar1.Enabled = false;
                RemovingCheckpoints = 1;
                btnRemoveCheckpoint.Text = "Stop";
            }
            else if (btnRemoveCheckpoint.Text == "Stop")
            {
                btnAddCheckpoint.Enabled = true;
                if (RandomAirplane == 0)
                {
                    btnAddAirplane.Enabled = true;
                    btnRemoveAirplane.Enabled = true;
                }

                trackBar1.Enabled = true;
                RemovingCheckpoints = 0;
                btnRemoveCheckpoint.Text = "Remove";
            }
        }

        /// <summary>
        /// Enables or disables weather movement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toggleWeatherBtn_Click(object sender, EventArgs e)
        {
            if (weatherActive)
            {
                toggleWeatherBtn.Text = "Enable weather movement";
            }
            else
            {
                toggleWeatherBtn.Text = "Disable weather movement";
            }

            weatherActive = !weatherActive;
        }

        /// <summary>
        /// Changes wind direction from combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxWindDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxWindDirection.SelectedItem.ToString())
            {
                case "NORTH":
                    windDirection = WindDirection.NORTH;
                    break;
                case "NORTH-EAST":
                    windDirection = WindDirection.NORTHEAST;
                    break;
                case "NORTH-WEST":
                    windDirection = WindDirection.NORTHWEST;
                    break;
                case "SOUTH":
                    windDirection = WindDirection.SOUTH;
                    break;
                case "SOUTH-EAST":
                    windDirection = WindDirection.SOUTHEAST;
                    break;
                case "SOUTH-WEST":
                    windDirection = WindDirection.SOUTHWEST;
                    break;
                case "EAST":
                    windDirection = WindDirection.EAST;
                    break;
                case "WEST":
                    windDirection = WindDirection.WEST;
                    break;
            }
            LabelChange();
        }

        private void trackBarPrecipitation_ValueChanged_1(object sender, EventArgs e)
        {
            precIntencity = trackBarPrecipitation.Value;
            labelPrec.Text = precIntencity.ToString() + "%";
            LabelChange();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string checks = "";
            if (checkpoints.Count >= 1)
            {
                foreach (Checkpoint a in checkpoints)
                {
                    checks += "\nCheckpoint: " + a.Name + ", Coordinates: (" + a.CoordinateX + "," + a.CoordinateY +
                              ")";
                }

                MessageBox.Show(checks, "List of checkpoints");
            }
            else
            {
                MessageBox.Show("No checkpoints created", "Warning");
            }

            ;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(weather.Probability.ToString());
        }

        private void trackBarPrecipitation_ValueChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Changes speed of the simulation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simSpeedComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            timerSimRunning.Interval = (int) (1000 / Convert.ToDouble(simSpeedComboBox.SelectedItem.ToString()));
        }

        private void allFlightsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            btTakeOff.Enabled = false;

            selectedAirplane = (Airplane) allFlightsListBox.SelectedItem;
            if (selectedAirplane == null)
            {
                return;
            }

            string planePath = String.Join(" --> ", selectedAirplane.ShortestPath);

            planeInfoTextBox.Text =
                $"{selectedAirplane} - flying{Environment.NewLine}Path: {planePath}{Environment.NewLine}Coordinates: ({selectedAirplane.CoordinateX}, {selectedAirplane.CoordinateY})   Speed: {selectedAirplane.Speed}";
        }


        private void landedAirplanesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            btTakeOff.Enabled = true;

            selectedAirplane = (Airplane) landedAirplanesListBox.SelectedItem;

            planeInfoTextBox.Text =
                $"{selectedAirplane} - landed{Environment.NewLine}";
        }

        /// <summary>
        /// Changes wind speed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBarWindSpeed_ValueChanged(object sender, EventArgs e)
        {
            windSpeed = trackBarWindSpeed.Value;
            labelWind.Text = windSpeed.ToString() + "m/s";
            LabelChange();
        }

        /// <summary>
        /// Starts simlation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlaySimulation_Click(object sender, EventArgs e)
        {
            if (timerSimRunning.Enabled)
            {
                timerSimRunning.Stop();
            }
            else
            {
                pictureBox1.Invalidate();
                timerSimRunning.Start();
            }
        }

        /// <summary>
        /// Exits application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (timerSimRunning.Enabled)
            {
                MessageBox.Show("Simulation is running, please stop simulation and try again!");
            }
            else if (timerSimRunning.Enabled == false)
            {
                pictureBox1.Invalidate();
                ClearListboxes();
                ClearLists();
                MessageBox.Show("Simulation cleared!");
                //ToDo: add a method that saves and overview to a file!
            }
        }

        /// <summary>
        /// Clear all lisboxes.
        /// </summary>
        private void ClearListboxes()
        {
            landedAirplanesListBox.Items.Clear();
            allFlightsListBox.Items.Clear();
        }

        /// <summary>
        /// Clear all lists.
        /// </summary>
        private void ClearLists()
        {
            checkpoints.Clear();
            airplaneList.Clear();
            landedAirplanes.Clear();
            crashedAirplanes.Clear();
        }

        private void panel9_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btTakeOff_Click(object sender, EventArgs e)
        {
            if (settingTakeOffDirection)
            {
                //TODO: log that the take off has been cancelled
                settingTakeOffDirection = false;
            }
            else
            {
                MessageBox.Show(
                    "Please select a cell from the outermost circle, through which you would like to see the airplane exit the airspace.",
                    "Take off set-up. Airplane: " + selectedAirplane, MessageBoxButtons.OK);

                this.settingTakeOffDirection = true;
            }
        }

        /// <summary>
        /// Updates lisboxes with updated data.
        /// </summary>
        public void UpdateListboxes()
        {
            foreach (var item in landedAirplanes)
            {
                landedAirplanesListBox.Items.Add(item);
            }

            foreach (var item in airplaneList)
            {
                allFlightsListBox.Items.Add(item);
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            foreach (Checkpoint c in checkpoints)
            {
                foreach (Cell cl in grid.listOfCells)
                {
                    if (cl.ContainsPoint((int) c.CoordinateX, (int) c.CoordinateY) == true)
                    {
                        Point p = cl.GetCenter();
                        PaintCircle(p, e);
                    }
                }
            }

            foreach (var p in airplaneList)
            {
                DrawDangerArea(p, e);
            }

            foreach (Airplane p in airplaneList.ToArray())
            {
                Point point = new Point((int) p.CoordinateX, (int) p.CoordinateY);

                if (p.Equals(selectedAirplane))
                {
                    PaintSelectedAirplane(point, e);


                    if (selectedAirplane.ShortestPath.Count != 0)
                    {
                        var ppp = selectedAirplane.ShortestPath.Last;

                        Point a = new Point(Convert.ToInt32(selectedAirplane.ShortestPath.Last.Value.CoordinateX),
                            Convert.ToInt32(selectedAirplane.ShortestPath.Last.Value.CoordinateY));

                        while (ppp != null)
                        {
                            Point b = new Point(Convert.ToInt32(ppp.Value.CoordinateX),
                                Convert.ToInt32(ppp.Value.CoordinateY));
                            ConnectDots(a, b, e);
                            a = new Point(Convert.ToInt32(ppp.Value.CoordinateX),
                                Convert.ToInt32(ppp.Value.CoordinateY));


                            ppp = ppp.Previous;
                        }
                    }
                }
                else
                {
                    PaintAirplane(point, e);
                }
            }

            if (weatherActive)
            {
                Point weatherPoint = new Point(weatherRect.X, weatherRect.Y);
                PaintWeather(weatherPoint, e); 
            }
        }

        /// <summary>
        /// Closes application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        public void PaintCircle(Point p, PaintEventArgs e)
        {
            float x = p.X - 3;
            float y = p.Y - 3;
            float width = 2 * 3;
            float height = 2 * 3;
            Pen pen = new Pen(Color.Blue);
            e.Graphics.DrawEllipse(pen, x, y, width, height);
        }

        public void PaintCircleB(Point p)
        {
            float x = p.X - 3;
            float y = p.Y - 3;
            float width = 2 * 3;
            float height = 2 * 3;
            Pen pen = new Pen(Color.Black);
            Graphics g = this.pictureBox1.CreateGraphics();
            g.DrawEllipse(pen, x, y, width, height);
        }

        public void PaintSelectedCrashSight(Point p)
        {
            int x = p.X - 3;
            int y = p.Y - 3;
            Graphics g = this.pictureBox1.CreateGraphics();
            explosionImage = Properties.Resources.explosion;
            airplaneRect = new Rectangle(x - 20, y - 20, 40, 40);
            g.DrawImage(explosionImage, airplaneRect);
        }

        /// <summary>
        /// Paints selected airplane
        /// </summary>
        /// <param name="p"></param>
        /// <param name="e"></param>
        public void PaintSelectedAirplane(Point p, PaintEventArgs e)
        {
            int x = p.X - 3;
            int y = p.Y - 3;
            airplaneImage = Properties.Resources.SelectedairplanePic;
            airplaneRect = new Rectangle(x - 20, y - 20, 40, 40);
            e.Graphics.DrawImage(airplaneImage, airplaneRect);
        }

        /// <summary>
        /// Paints airplane
        /// </summary>
        /// <param name="p"></param>
        /// <param name="e"></param>
        public void PaintAirplane(Point p, PaintEventArgs e)
        {
            int x = p.X - 3;
            int y = p.Y - 3;
            airplaneImage = Properties.Resources.airplanePic;
            airplaneRect = new Rectangle(x - 20, y - 20, 40, 40);
            e.Graphics.DrawImage(airplaneImage, airplaneRect);
        }

        /// <summary>
        /// Paints weather condition
        /// </summary>
        /// <param name="p"></param>
        /// <param name="e"></param>
        public void PaintWeather(Point p, PaintEventArgs e)
        {
            
            int x = p.X - 3;
            int y = p.Y - 3;

            Graphics g = this.pictureBox1.CreateGraphics();
            if (weather.PrecipitationType == PrecipitationType.RAIN)
            {
                weatherImage = Properties.Resources.snow;
            }
            else if (weather.PrecipitationType == PrecipitationType.SNOW)
            {
                weatherImage = Properties.Resources.snow;
            }
            else if (weather.PrecipitationType == PrecipitationType.HAIL)
            {
                weatherImage = Properties.Resources.hail;
            }
            else
            {
                weatherImage = Properties.Resources.clear;
            }

            e.Graphics.DrawImage(weatherImage, weatherRect);
        }

        public void PaintRectangleY(Point p)
        {
            float x = p.X - 3;
            float y = p.Y - 3;
            float width = 2 * 3;
            float height = 2 * 3;
            Pen pen = new Pen(Color.Yellow);
            Graphics g = this.pictureBox1.CreateGraphics();
            g.DrawRectangle(pen, x, y, width, height);
        }

        /// <summary>
        /// Changes label text
        /// </summary>
        public void LabelChange()
        {
            weather = new WeatherConditions(windSpeed, windDirection, temp, precIntencity);
            weather.SetProbability();
            lbPrecipitationType.Text = weather.GetPrecipitationType().ToString();
            lbProbability.Text = weather.Probability.ToString();
            lbVisibility.Text = weather.GetVisibility().ToString();
            //MessageBox.Show(windDirection.ToString());
        }

        /// <summary>
        /// Method to create random airplane on border area.
        /// </summary>
        private void CreateRandomAirplane()
        {
            // Refresh();
            int speed = 5;
            int s;
            int x;
            int y;
            Boolean exists = false;

            do
            {
                Random random = new Random();
                speed = 5;
                s = random.Next(1, 5);
                x = 1;
                y = 1;
                exists = false;

                while (speed % 25 != 0)
                {
                    speed = random.Next(100, 400);
                }

                if (s == 1)
                {
                    while (x % Cell.Width != Cell.Width / 2)
                    {
                        x = random.Next(Cell.Width / 2, (grid.ColumnsOfCells + 1) * Cell.Width);
                    }

                    y = Cell.Width;
                }

                else if (s == 2)
                {
                    while (x % Cell.Width != Cell.Width / 2)
                    {
                        x = random.Next(Cell.Width / 2, (grid.ColumnsOfCells + 1) * Cell.Width);
                    }

                    y = (grid.RowsOfCells + 1) * Cell.Width;
                }

                else if (s == 3)
                {
                    while (y % Cell.Width != Cell.Width / 2)
                    {
                        y = random.Next(Cell.Width / 2, (grid.RowsOfCells + 1) * Cell.Width);
                    }

                    x = (grid.ColumnsOfCells + 1) * Cell.Width;
                }

                else if (s == 4)
                {
                    while (y % Cell.Width != Cell.Width / 2)
                    {
                        y = random.Next(Cell.Width / 2, (grid.RowsOfCells + 1) * Cell.Width);
                    }

                    x = Cell.Width;
                }

                if (airplaneList.Count > 0)
                {
                    foreach (Airplane mm in airplaneList)
                    {
                        if (mm.CoordinateX == x && mm.CoordinateY == y)
                        {
                            exists = true;
                        }
                    }
                }
            } while (exists == true);


            apName++;
            fnName += 6 * 2 / 3;
            string name = "Airplane" + apName;
            string flight = "fn" + fnName + "z";
            Airplane one = new Airplane(name, x, y, speed, flight);
            airplaneList.Add(one);

            this.Invalidate();
            one.OnAirportReached += airplaneHasReachedTheAirport;
            one.OnCrash += airplaneCrashed;
            allFlightsListBox.Items.Add(one);
        }

        private void weatherOnCheckpoint(Object sender, EventArgs e)
        {
            this.checkpoints.Remove((Checkpoint) sender);
        }

        // PAINT GRID

        /// <summary>
        /// Paints grid with different zones
        /// </summary>
        private void PaintGrid()
        {
            Rectangle rect;
            SolidBrush b = new SolidBrush(Color.Yellow);

            foreach (Cell c in grid.listOfCells)
            {
                switch (c.Type)
                {
                    case CellType.BORDER:
                        b = new SolidBrush(Color.Yellow);
                        break;
                    case CellType.UPPER:
                        b = new SolidBrush(Color.Aqua);
                        break;
                    case CellType.MIDDLE:
                        b = new SolidBrush(Color.LightSkyBlue);
                        break;
                    case CellType.LOWER:
                        b = new SolidBrush(Color.Gainsboro);
                        break;
                    case CellType.FINAL:
                        b = new SolidBrush(Color.Chocolate);
                        break;
                    default:
                        continue;
                }

                rect = new Rectangle(c.x, c.y, Cell.Width, Cell.Width);
                gGrid.FillRectangle(b, rect);
                gGrid.DrawRectangle(pGrid, rect);
            }
        }
    }
}