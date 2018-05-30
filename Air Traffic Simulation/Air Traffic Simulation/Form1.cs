using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Air_Traffic_Simulation
{
    public partial class Form1 : Form
    {
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


        // PAINT GRID
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


        //SIMULATION
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;


        //TODO: remove testing variables
        private Airstrip landingStrip;

        List<Airplane> airplanes;
        List<Checkpoint> checkpoints;
        List<Airplane> airplaneList;
        private List<Airplane> landedAirplanes;


        public Form1()
        {
            dir = @"..\..\Saved";
            serializationFile = Path.Combine(dir, "Checkpoints.bin");


            airplanes = new List<Airplane>();
            checkpoints = new List<Checkpoint>();
            airplaneList = new List<Airplane>();
            landedAirplanes = new List<Airplane>();
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


            //WEATHER VALUES
            windSpeed = trackBarWindSpeed.Value;
            temp = trackBarTemperature.Value;
            precIntencity = trackBarPrecipitation.Value;

            switch (wdComboBox)
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

            weather = new WeatherConditions(windSpeed, windDirection, temp, precIntencity);
            LabelChange();


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

            simSpeedComboBox.SelectedIndex = 2;
            rbLanding.Checked = true;
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

        private void weatherMovement()
        {
            //if (weather.PrecipitationType == PrecipitationType.RAIN)
            //{
            //    weatherBrush.Color = Color.FromArgb(125, 92, 92, 92);
            //}
            //else if (weather.PrecipitationType == PrecipitationType.SNOW)
            //{
            //    weatherBrush.Color = Color.FromArgb(125, 205, 205, 205);
            //}
            //else if (weather.PrecipitationType == PrecipitationType.HAIL)
            //{
            //    weatherBrush.Color = Color.FromArgb(125, 175, 75, 75);
            //}
            //else
            //{
            //    weatherBrush.Color = Color.FromArgb(0, 250, 75, 75);
            //}

            //Pen p = new Pen(Color.Black);

            if (weatherRect.X < 0 || weatherRect.Y < 0)
            {
                weatherRect.X += r.Next(20);
                weatherRect.Y += r.Next(20);
            }
            else if (weatherRect.X > 580 || weatherRect.Y > 580)
            {
                weatherRect.X -= r.Next(-20, 0);
                weatherRect.Y -= r.Next(-20, 0);
            }
            else
            {
                weatherRect.X += r.Next(-20, 20);
                weatherRect.Y += r.Next(-20, 20);
            }


            //Graphics g = this.pictureBox1.CreateGraphics();
            //g.DrawEllipse(p, weatherBlock);
            //g.FillEllipse(weatherBrush, weatherBlock);
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
                                Point point = cl.GetCenter();
                                PaintCircle(point);
                            }
                        }
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("Something went wrong with checkpoints list.");
            }

            Point weatherPoint = new Point(weatherRect.X, weatherRect.Y);
            PaintWeather(weatherPoint);
        }

        private void airplaneHasReachedTheAirport(Object sender, EventArgs e)
        {
            ((Airplane) sender).OnAirportReached -= airplaneHasReachedTheAirport;
            this.landedAirplanes.Add((Airplane) sender);
            landedAirplanesListBox.Items.Add((Airplane) sender);
            this.airplaneList.Remove((Airplane) sender);
            allFlightsListBox.Items.Remove(sender);
        }

        //RADAR METHOD
        private void t_Tick(object sender, EventArgs e)
        {
            p = new Pen(Color.Green, 1f);
            g = Graphics.FromImage(bmp);

            //calculate x & y coordinates of hand
            int tu = (u - lim) % 360;

            if (u >= 0 && u <= 180)
            {
                //right half
                //u in degree is converted into radian
                x = cx + (int) (hand * Math.Sin(Math.PI * u / 180));
                y = cy - (int) (hand * Math.Cos(Math.PI * u / 180));
            }
            else
            {
                x = cx - (int) (hand * -Math.Sin(Math.PI * u / 180));
                y = cy - (int) (hand * Math.Cos(Math.PI * u / 180));
            }

            if (tu >= 0 && tu <= 180)
            {
                //right half
                //u in degree is converted into radian
                tx = cx + (int) (hand * Math.Sin(Math.PI * tu / 180));
                ty = cy - (int) (hand * Math.Cos(Math.PI * tu / 180));
            }
            else
            {
                tx = cx - (int) (hand * -Math.Sin(Math.PI * tu / 180));
                ty = cy - (int) (hand * Math.Cos(Math.PI * tu / 180));
            }

            //draw circle
            g.DrawEllipse(p, 0, 0, width, height); //bigger circle
            g.DrawEllipse(p, 80, 80, width - 160, height - 160); //smaller circle

            //draw hand
            g.DrawLine(new Pen(Color.Black, 1f), new Point(cx, cy), new Point(tx, ty));
            g.DrawLine(p, new Point(cx, cy), new Point(x, y));

            //load bitmap in picturebox
            pictureBox1.Image = bmp;

            //dispose
            p.Dispose();
            g.Dispose();

            //update
            u++;
            if (u == 360)
            {
                u = 0;
            }
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

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

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

        private void trackBarTemperature_ValueChanged(object sender, EventArgs e)
        {
            temp = trackBarTemperature.Value;
            labelTemp.Text = temp.ToString() + "°C";
            LabelChange();
        }

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
            Refresh();

            #region MissingCheckpointsErrorDisplay

            //generates the message box that informs the user that some areas are missing points
            bool[] allZonesCheck = new bool[] { false, false, false, false };
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
                MessageBox.Show(
                    $"There seem to be no checkpoints in the following zones:{Environment.NewLine}{Environment.NewLine}{lacking}",
                    "Missing Checkpoint", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            #endregion

            foreach (Checkpoint c in checkpoints)
            {
                foreach (Cell cl in grid.listOfCells)
                {
                    if (cl.ContainsPoint((int) c.CoordinateX, (int) c.CoordinateY) == true)
                    {
                        Point p = cl.GetCenter();
                        PaintCircle(p);
                    }
                }
            }

            foreach (Airplane p in airplaneList.ToArray())
            {
                Point point = new Point((int) p.CoordinateX, (int) p.CoordinateY);
                PaintAirplane(point);
            }


            foreach (Airplane plane in airplaneList)
            {
                plane.calculateShortestPath(this.checkpoints, this.landingStrip);

                Point a = new Point(Convert.ToInt32(landingStrip.CoordinateX),
                    Convert.ToInt32(landingStrip.CoordinateY));


                var ppp = plane.ShortestPath.Last;
                while (ppp != null)
                {
                    Point b = new Point(Convert.ToInt32(ppp.Value.CoordinateX), Convert.ToInt32(ppp.Value.CoordinateY));
                    ConnectDots(a, b);
                    a = new Point(Convert.ToInt32(ppp.Value.CoordinateX), Convert.ToInt32(ppp.Value.CoordinateY));
                    ppp = ppp.Previous;
                }
            }
        }

        /// <summary>
        /// Creates a line between two points on the grid.
        /// </summary>
        /// <param name="a">The initial point.</param>
        /// <param name="b">The end point.</param>
        public void ConnectDots(Point a, Point b)
        {
            Pen pen = new Pen(Color.Yellow);
            Graphics g = this.pictureBox1.CreateGraphics();
            g.DrawLine(pen, a, b);
        }

        /// <summary>
        /// The method that does the actual addition of checkpoints.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                                PaintCircle(p);

                                cpName++;
                                string name = "cp" + cpName;

                                Checkpoint a = new Checkpoint(name, p.X, p.Y, c, checkpoints, landingStrip);
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
                                PaintAirplane(p);

                                apName++;
                                fnName += 6 * 2 / 3;
                                string name = "Airplane" + apName;
                                string flight = "fn" + fnName + "z";

                                Airplane a = new Airplane(name, p.X, p.Y, Convert.ToDouble(nSpeed.Value), flight);
                                a.OnAirportReached += airplaneHasReachedTheAirport;
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
        }

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


        private void trackBarWindSpeed_Scroll(object sender, EventArgs e)
        {
            LabelChange();
        }
        /// <summary>
        /// Drawign danger zone around airplane
        /// </summary>
        /// <param name="airplane"></param>
        public void DrawDangerArea(Airplane airplane)
        {
            Pen p = new Pen(Color.Black);
            Graphics g = this.pictureBox1.CreateGraphics();
            g.DrawEllipse(p, airplane.Rect);
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            Refresh();
            foreach (Checkpoint c in checkpoints)
            {
                foreach (Cell cl in grid.listOfCells)
                {
                    if (cl.ContainsPoint((int) c.CoordinateX, (int) c.CoordinateY) == true)
                    {
                        Point p = cl.GetCenter();
                        PaintCircle(p);
                    }
                }
            }

            foreach (Airplane p in airplaneList.ToArray())
            {
                if (p.ShortestPath.Count != 0)
                {
                    p.MoveTowardsNextPoint();
                }
                Point point = new Point((int) p.CoordinateX, (int) p.CoordinateY);
                DrawDangerArea(p);

                if (p.Equals(selectedAirplane))
                {
                    PaintSelectedAirplane(point);

                    Point a = new Point(Convert.ToInt32(landingStrip.CoordinateX),
                        Convert.ToInt32(landingStrip.CoordinateY));

                    var ppp = selectedAirplane.ShortestPath.Last;
                    string planePath = String.Empty;
                    while (ppp != null)
                    {
                        Point b = new Point(Convert.ToInt32(ppp.Value.CoordinateX), Convert.ToInt32(ppp.Value.CoordinateY));
                        ConnectDots(a, b);
                        a = new Point(Convert.ToInt32(ppp.Value.CoordinateX), Convert.ToInt32(ppp.Value.CoordinateY));


                        ppp = ppp.Previous;
                    }
                }
                else
                {
                    PaintAirplane(point);
                }
            }

            if (weatherActive)
                weatherMovement();
        }

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
                        PaintCircle(new Point(Convert.ToInt32(a.CoordinateX), Convert.ToInt32(a.CoordinateY)));
                        cpName++;
                    }
                }

                if (airplaneList != null)
                {
                    foreach (Airplane a in airplaneList)
                    {
                        apName++;
                        fnName += 6 * 2 / 3;
                        PaintAirplane(new Point(Convert.ToInt32(a.CoordinateX), Convert.ToInt32(a.CoordinateY)));
                        a.OnAirportReached += airplaneHasReachedTheAirport;
                    }
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

        //wind direction changed in combobox
        private void comboBoxWindDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            wdComboBox = (String) comboBoxWindDirection.SelectedItem;
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

        private void simSpeedComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            timerSimRunning.Interval = (int) (1000 / Convert.ToDouble(simSpeedComboBox.SelectedItem.ToString()));
        }

        private void allFlightsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedAirplane = (Airplane) allFlightsListBox.SelectedItem;
            if (selectedAirplane == null)
            {
                return;
            }
//            int row, column;
//            Cell c = null;
//            foreach (var cell in grid.listOfCells)
//            {
//                if (cell.ContainsPoint((int) selected.CoordinateX, (int) selected.CoordinateY))
//                {
//                    c = cell;
//                }
//            }


            //row = c.id / grid.RowsOfCells;
            //column = c.id % (grid.ColumnsOfCells+1);

            var ppp = selectedAirplane.ShortestPath.First;
            string planePath = String.Empty;
            while (ppp != null)
            {
                planePath += ppp.Value.Name + " --> ";
                ppp = ppp.Next;
            }

            planeInfoTextBox.Text =
                $"{selectedAirplane} - flying{Environment.NewLine}Path: {planePath}{Environment.NewLine}Coordinates: ({selectedAirplane.CoordinateX}, {selectedAirplane.CoordinateY})   Speed: {selectedAirplane.Speed}";
        }

        private void landedAirplanesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedAirplane = (Airplane) allFlightsListBox.SelectedItem;

            planeInfoTextBox.Text =
                $"{selectedAirplane} - landed{Environment.NewLine}";
        }

        private void trackBarWindSpeed_ValueChanged(object sender, EventArgs e)
        {
            windSpeed = trackBarWindSpeed.Value;
            labelWind.Text = windSpeed.ToString() + "m/s";
            LabelChange();
        }

        private void btnPlaySimulation_Click(object sender, EventArgs e)
        {
            if (timerSimRunning.Enabled)
            {
                timerSimRunning.Stop();
            }
            else
            {
                foreach (Airplane p in airplaneList)
                {
                    Point point = new Point((int) p.CoordinateX, (int) p.CoordinateY);
                    PaintAirplane(point);
                }

                timerSimRunning.Start();
            }
        }

        public void PaintCircle(Point p)
        {
            float x = p.X - 3;
            float y = p.Y - 3;
            float width = 2 * 3;
            float height = 2 * 3;
            Pen pen = new Pen(Color.Blue);
            Graphics g = this.pictureBox1.CreateGraphics();
            g.DrawEllipse(pen, x, y, width, height);
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

        public void PaintSelectedAirplane(Point p)
        {
            int x = p.X - 3;
            int y = p.Y - 3;
            Graphics g = this.pictureBox1.CreateGraphics();
            airplaneImage = Properties.Resources.SelectedairplanePic;
            airplaneRect = new Rectangle(x - 20, y - 20, 40, 40);
            g.DrawImage(airplaneImage, airplaneRect);
        }

        public void PaintAirplane(Point p)
        {
            int x = p.X - 3;
            int y = p.Y - 3;
            Graphics g = this.pictureBox1.CreateGraphics();
            airplaneImage = Properties.Resources.airplanePic;
            airplaneRect = new Rectangle(x - 20, y - 20, 40, 40);
            g.DrawImage(airplaneImage, airplaneRect);
        }

        public void PaintWeather(Point p)
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

            g.DrawImage(weatherImage, weatherRect);
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

        public void LabelChange()
        {
            weather = new WeatherConditions(windSpeed, windDirection, temp, precIntencity);
            weather.SetProbability();
            lbPrecipitationType.Text = weather.GetPrecipitationType().ToString();
            lbProbability.Text = weather.Probability.ToString();
            lbVisibility.Text = weather.GetVisibility().ToString();
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
                    while (x % Cell.Width != Cell.Width/2)
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

                    y = (grid.RowsOfCells + 1)* Cell.Width;
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

            PaintAirplane(new Point(Convert.ToInt32(one.CoordinateX - 20), Convert.ToInt32(one.CoordinateY - 20)));
            one.OnAirportReached += airplaneHasReachedTheAirport;
            allFlightsListBox.Items.Add(one);
        }

        private void weatherOnCheckpoint(Object sender, EventArgs e)
        {
            this.checkpoints.Remove((Checkpoint) sender);
        }
    }
}