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
        Rectangle weatherBlock;
        WeatherConditions weather;

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
        private Airplane testPlane;
        private Airplane testPlane2;
        private Airstrip landingStrip;

        List<Airplane> airplanes;

        List<Checkpoint> checkpoints;
        List<Airplane> airplaneList;
        private List<Airplane> planesOnTheGround;


        public Form1()
        {
            dir = @"..\..\Saved";
            serializationFile = Path.Combine(dir, "Checkpoints.bin");


            airplanes = new List<Airplane>();
            checkpoints = new List<Checkpoint>();
            airplaneList = new List<Airplane>();
            planesOnTheGround = new List<Airplane>();
            testPlane = new Airplane(name: "FB123", coordinateX: 35, coordinateY: 64, speed: 300,
                flightNumber: "FB321");
            testPlane.OnAirportReached += airplaneHasReachedTheAirport;
            airplaneList.Add(testPlane);
            //testPlane2 = new Airplane(name: "FB123", coordinateX: 100, coordinateY: 100, speed: 300,
            //     flightNumber: "FB321");
            // airplaneList.Add(testPlane2);
            InitializeComponent();
            nSpeed.Enabled = false;
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
            comboBoxWindDirection.Items.AddRange(new object[]{ "NORTH", "NORTH-EAST", "NORTH-WEST", "SOUTH", "SOUTH-EAST", "SOUTH-WEST", "EAST", "WEST" });
            



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
            weatherBlock = new Rectangle(x, x + 50, 50, 50);

            timerWeather.Interval = 500;
            timerWeather.Start();

            pictureBox1.Image = bmpGrid;
            PaintGrid();
            makeAirstrip();
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

        private void timerWeather_Tick(object sender, EventArgs e)
        {
            Refresh();

            if (weather.PrecipitationType == PrecipitationType.RAIN)
            {
                weatherBrush.Color = Color.FromArgb(125, 92, 92, 92);
            }
            else if (weather.PrecipitationType == PrecipitationType.SNOW)
            {
                weatherBrush.Color = Color.FromArgb(125, 205, 205, 205);
            }
            else if (weather.PrecipitationType == PrecipitationType.HAIL)
            {
                weatherBrush.Color = Color.FromArgb(125, 175, 75, 75);
            }
            else
            {
                weatherBrush.Color = Color.FromArgb(0, 250, 75, 75);
            }

            Pen p = new Pen(Color.Black);
            if (weatherBlock.X < 0 || weatherBlock.Y < 0)
            {
                weatherBlock.X += r.Next(20);
                weatherBlock.Y += r.Next(20);
            }
            else if (weatherBlock.X > 580 || weatherBlock.Y > 580)
            {
                weatherBlock.X -= r.Next(-20, 0);
                weatherBlock.Y -= r.Next(-20, 0);
            }
            else
            {
                weatherBlock.X += r.Next(-20, 20);
                weatherBlock.Y += r.Next(-20, 20);
            }


            Graphics g = this.pictureBox1.CreateGraphics();
            g.DrawEllipse(p, weatherBlock);
            g.FillEllipse(weatherBrush, weatherBlock);
        }


        private void airplaneHasReachedTheAirport(Object sender, EventArgs e)
        {
            this.planesOnTheGround.Add((Airplane) sender);
            this.airplaneList.Remove((Airplane) sender);
            Refresh();
        }

        private void addTestAirplaneAndStrip(object sender, EventArgs e)
        {
            //TODO: Remove following test lines:

            foreach (Cell c in grid.listOfCells)
            {
                if (c.ContainsPoint(Convert.ToInt32(testPlane.CoordinateX), Convert.ToInt32(testPlane.CoordinateY)))
                {
                    Point p = c.GetCenter();

                    //slightly, uh, artistically collaborated PaintCircle
                    float x = p.X - 3;
                    float y = p.Y - 3;
                    float width = 2 * 3;
                    float height = 2 * 3;
                    Pen pen = new Pen(Color.Red);
                    Graphics g = this.pictureBox1.CreateGraphics();
                    g.DrawRectangle(pen, x, y, width, height);
                }
            }
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
                x = cx + (int)(hand * Math.Sin(Math.PI * u / 180));
                y = cy - (int)(hand * Math.Cos(Math.PI * u / 180));
            }
            else
            {
                x = cx - (int)(hand * -Math.Sin(Math.PI * u / 180));
                y = cy - (int)(hand * Math.Cos(Math.PI * u / 180));
            }

            if (tu >= 0 && tu <= 180)
            {
                //right half
                //u in degree is converted into radian
                tx = cx + (int)(hand * Math.Sin(Math.PI * tu / 180));
                ty = cy - (int)(hand * Math.Cos(Math.PI * tu / 180));
            }
            else
            {
                tx = cx - (int)(hand * -Math.Sin(Math.PI * tu / 180));
                ty = cy - (int)(hand * Math.Cos(Math.PI * tu / 180));
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
            SaveFileDialog SaveFileDialogMain = new SaveFileDialog();
            SaveFileDialogMain.Filter = "Binary Files (*.bin)|*.bin";
            SaveFileDialogMain.DefaultExt = "bin";
            SaveFileDialogMain.AddExtension = true;
            if (SaveFileDialogMain.ShowDialog() == DialogResult.OK)
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                string filename = SaveFileDialogMain.FileName;
                FileStream filestream = new FileStream(filename, FileMode.Create, FileAccess.Write);
                bformatter.Serialize(filestream, checkpoints);
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
            Console.WriteLine(airplaneList.Count);
            foreach (Airplane plane in airplaneList)
            {
                plane.calculateShortestPath(this.checkpoints);


                Point a = new Point(Convert.ToInt32(landingStrip.CoordinateX),
                    Convert.ToInt32(landingStrip.CoordinateY));


                var ppp = plane.ShortestPath.Last;
                string planePath = String.Empty;
                while (ppp != null)
                {
                    planePath += ppp.Value.Name + " -> ";

                    Point b = new Point(Convert.ToInt32(ppp.Value.CoordinateX), Convert.ToInt32(ppp.Value.CoordinateY));
                    ConnectDots(a, b);
                    a = new Point(Convert.ToInt32(ppp.Value.CoordinateX), Convert.ToInt32(ppp.Value.CoordinateY));
                    ppp = ppp.Previous;
                }

                //TODO: remove planepath print
                Console.WriteLine(planePath);

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
                    MessageBox.Show(
                        $"There seem to be no checkpoints in the following zones:{Environment.NewLine}{Environment.NewLine}{lacking}",
                        "Missing Checkpoint", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                                PaintRectangle(p);

                                apName++;
                                fnName += 6 * 2 / 3;
                                string name = "Airplane" + apName;
                                string flight = "fn" + fnName + "z";

                                Airplane a = new Airplane(name, p.X, p.Y, Convert.ToDouble(nSpeed.Value), flight);
                                a.OnAirportReached += airplaneHasReachedTheAirport;
                                airplaneList.Add(a);
                                MessageBox.Show("Added Airplane  " + a.Name + "\nCoordinates: \t(" + a.CoordinateX +
                                                "," + a.CoordinateY + ")" + "\nFlight number: \t(" + flight + ") " +
                                                "\nSpeed: \t\t(" + nSpeed.Value + "kmh)");
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
                                airplaneList.Remove(bee);
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
                btnAddAirplane.Enabled = false;
                btnRemoveAirplane.Enabled = false;
                RandomAirplane = 1;
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
                    Pen pen = new Pen(Color.Red);
                    Graphics g = this.pictureBox1.CreateGraphics();
                    g.DrawRectangle(pen, (float) p.CoordinateX, (float) p.CoordinateY, 10, 10);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (timerPlaneMovement.Enabled)
            {
                timerPlaneMovement.Stop();
            }
            else
            {
                foreach (Airplane p in airplaneList)
                {
                    Pen pen = new Pen(Color.Red);
                    Graphics g = this.pictureBox1.CreateGraphics();
                    g.DrawRectangle(pen, (float) p.CoordinateX, (float) p.CoordinateY, 10, 10);
                }

                timerPlaneMovement.Start();
            }
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

                checkpoints = (List<Checkpoint>) bformatter.Deserialize(filestream);

                filestream.Close();
                filestream = null;


                foreach (Checkpoint a in checkpoints)
                {
                    PaintCircle(new Point(Convert.ToInt32(a.CoordinateX), Convert.ToInt32(a.CoordinateY)));
                    cpName++;
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


        private void stopWeatherBtn_Click(object sender, EventArgs e)
        {
            if (timerWeather.Enabled)
            {
                timerWeather.Stop();
            }
            else
            {
                timerWeather.Start();
            }
		}

        private void calcRouteBtn_Click(object sender, EventArgs e)
        {

        }
        
        //wind direction changed in combobox
        private void comboBoxWindDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            wdComboBox = (String)comboBoxWindDirection.SelectedItem;
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
            precIntencity = trackBarPrecipitation.Value;
            labelPrec.Text = precIntencity.ToString() + "%";
            LabelChange();
        }

        private void trackBarWindSpeed_ValueChanged(object sender, EventArgs e)
        {
            windSpeed = trackBarWindSpeed.Value;
            labelWind.Text = windSpeed.ToString() + "m/s";
            LabelChange();
        }

        private void btnPlaySimulation_Click(object sender, EventArgs e)
        {
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

        public void PaintRectangle(Point p)
        {
            float x = p.X - 3;
            float y = p.Y - 3;
            float width = 2 * 3;
            float height = 2 * 3;
            Pen pen = new Pen(Color.Red);
            Graphics g = this.pictureBox1.CreateGraphics();
            g.DrawRectangle(pen, x, y, width, height);
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
    }
}