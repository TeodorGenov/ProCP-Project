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


        Timer t = new Timer();
        int width = 800, height = 460, hand = 150;
        int u; //in degree
        int cx, cy; //center of the circle
        int x, y; //hand coordinates
        int tx, ty, lim = 20;
        Bitmap bmp;
        Pen p;
        Graphics g;


        //GRID


        Grid grid = new Grid();
        Point p1, p2, p3, p4;
        Pen pGrid;
        Graphics gGrid;
        Bitmap bmpGrid;


        // CHECKPOINT CIRCLE POINT LOL


        Graphics grCircle;
        Pen pnCircle;


        // SIMULATION VARIABLES


        int temp, prec, wind;

        //List<Checkpoint> checkpoints;
        string dir;
        string serializationFile;

        double probability;

        int AddingCheckpoints = 0;
        int RemovingCheckpoints = 0;
        int AddingAirplanes = 0;
        int RemovingAirplanes = 0;
        int RandomAirplane = 0;
        static int cpName = 0;
        static int apName = 0;
        static int fnName = 0;
        
        BinaryFormatter bf;


        // END OF SIMULATION VARIABLES


        // PAINT GRID
        Rectangle rect;
        SolidBrush brush = new SolidBrush(Color.Purple);

        


        //simulation

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;


        //TODO: remove testing variables
        private Airplane testPlane;
        private Airstrip testStrip;

        List<AbstractCheckpoint> checkpoints;
        List<Airplane> airplanes;


        public Form1()
        {
            dir = @"..\..\Saved";
            serializationFile = Path.Combine(dir, "Checkpoints.bin");
            //checkpoints = new List<Checkpoint>();
            checkpoints = new List<AbstractCheckpoint>();
            airplanes = new List<Airplane>();
            InitializeComponent();
            nSpeed.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Slider info
            temp = trackBarTemperature.Value;
            prec = trackBarPrecipitation.Value;
            wind = trackBarWindSpeed.Value;
            labelWind.Text = trackBarWindSpeed.Value.ToString() + "m/s";
            labelTemp.Text = trackBarTemperature.Value.ToString() + "°C";
            labelPrec.Text = trackBarPrecipitation.Value.ToString() + "%";
            Simulate();


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
                p2 = new Point(c.x + c.width, c.y);
                p3 = new Point(c.x + c.width, c.y + c.width);
                p4 = new Point(c.x, c.y + c.width);

                gGrid.DrawLine(pGrid, p1, p2);
                gGrid.DrawLine(pGrid, p1, p4);
                gGrid.DrawLine(pGrid, p2, p3);
                gGrid.DrawLine(pGrid, p3, p4);
            }

            pictureBox1.Image = bmpGrid;
            PaintGrid();
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
            Simulate();
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

        private void addTestAirplaneAndStrip(object sender, EventArgs e)
        {
            //TODO: Fix mismatching var types of Doubles for Checkpoint locations and Ints for Cell locations
            //TODO: Remove following test lines:
            testPlane = new Airplane("FB123", 290, 440, 300, "FB321");
            testStrip = new Airstrip("Strip A", 550, 50, true, 360);

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

                    string name = "aircraft" + testPlane;
                }
                else if (c.ContainsPoint(Convert.ToInt32(testStrip.CoordinateX),
                    Convert.ToInt32(testStrip.CoordinateY)))
                {
                    Point p = c.GetCenter();

                    //slightly, uh, artistically collaborated PaintCircle
                    float x = p.X - 3;
                    float y = p.Y - 3;
                    float width = 2 * 3;
                    float height = 2 * 3;
                    Brush pen = new SolidBrush(Color.Green);
                    Graphics g = this.pictureBox1.CreateGraphics();
                    g.FillRectangle(pen, x, y, width, height);

                    string name = "aircraft" + testPlane;
                }
            }

            MessageBox.Show("Added test airplane  " + testPlane.Name + "  With coordinates: (" + testPlane.CoordinateX +
                            "," + testPlane.CoordinateY +
                            ")\nThat's the red empty square in the center bottom of the screen." +
                            "\n\n..and test airstrip  " + testStrip.Name + "  With coordinates: (" +
                            testStrip.CoordinateX + "," + testStrip.CoordinateY +
                            ")\n(That's the green filled square in the upper right of the screen.)");

            button5.Enabled = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            testPlane.calculateShortestPath(this.checkpoints, this.testStrip);

            Point a = new Point(Convert.ToInt32(testStrip.CoordinateX), Convert.ToInt32(testStrip.CoordinateY));


            var pp = testStrip.ShortestPath.Last;

            while (pp != null)
            {
                Point b = new Point(Convert.ToInt32(pp.Value.CoordinateX), Convert.ToInt32(pp.Value.CoordinateY));
                Console.WriteLine($"a: {a.X}, {a.Y} \t b: {b.X}, {b.Y}");
                ConnectDots(a, b);
                a = new Point(Convert.ToInt32(pp.Value.CoordinateX), Convert.ToInt32(pp.Value.CoordinateY));
                pp = pp.Previous;
            }
        }

        public void ConnectDots(Point a, Point b)
        {
            Pen pen = new Pen(Color.Yellow);
            Graphics g = this.pictureBox1.CreateGraphics();
            g.DrawLine(pen, a, b);
        }

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
                            if (c.x == 0 || c.y == 0 || c.x == bmpGrid.Width - 20 || c.y == bmpGrid.Height - 20)
                            {
                                MessageBox.Show("Selected area is for airplanes only.");
                            }
                            else
                            {
                               PaintCircle(p);

                            cpName++;
                            string name = "cp" + cpName;
                            Checkpoint a = new Checkpoint(name, p.X, p.Y);
                            checkpoints.Add(a);
                            MessageBox.Show("Added checkpoint  " + a.Name + "  With coordinates: (" + a.CoordinateX +
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
                        foreach (Airplane mm in airplanes)
                        {
                            if (mm.CoordinateX == p.X && mm.CoordinateY == p.Y)
                            {
                                MessageBox.Show("Airplane already exists at this location", "WARNING");
                                exists = true;
                            }
                        }

                        if (!exists)
                        {
                            if (c.x == 0 || c.y == 0 || c.x == bmpGrid.Width - 20 || c.y == bmpGrid.Height - 20)
                            {
                                PaintRectangle(p);

                                apName++;
                                fnName += 6 * 2 / 3;
                                string name = "Airplane" + apName;
                                string flight = "fn" + fnName + "z";
                                Airplane a = new Airplane(name, p.X, p.Y, Convert.ToDouble(nSpeed.Value), flight);
                                airplanes.Add(a);
                                MessageBox.Show("Added Airplane  " + a.Name + "\nCoordinates: \t(" + a.CoordinateX +
                                                "," + a.CoordinateY + ")" + "\nFlight number: \t(" + flight + ") " + "\nSpeed: \t\t(" + nSpeed.Value + "kmh)");

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
                        foreach (Airplane bee in airplanes)
                        {
                            if (bee.CoordinateX == p.X && bee.CoordinateY == p.Y)
                            {
                                PaintRectangleP(p);
                                Airplane v = bee;
                                airplanes.Remove(bee);
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

                //checkpoints = (List<Checkpoint>) bformatter.Deserialize(filestream);
                checkpoints = (List<AbstractCheckpoint>) bformatter.Deserialize(filestream);

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
            MessageBox.Show(probability.ToString());
        }

        private void trackBarPrecipitation_ValueChanged(object sender, EventArgs e)
        {
            prec = trackBarPrecipitation.Value;
            labelPrec.Text = prec.ToString() + "%";
            Simulate();
        }

        private void trackBarWindSpeed_ValueChanged(object sender, EventArgs e)
        {
            wind = trackBarWindSpeed.Value;
            labelWind.Text = wind.ToString() + "m/s";
            Simulate();
        }

        private void btnPlaySimulation_Click(object sender, EventArgs e)
        {
        }

        // SIMULATION ITSELF


        private void Simulate()
        {
            // Airplanes gets performance loss for air that is 40c or more, cold air doesnt impact the take off or landing except if its snowing (when <0c and percipation is also around 15-30 and more) then sometimes airplanes wont take off or land.

            // Wind speed is usually not a harm if it is a headwind or tailwind (From front or back), however if it is from side then if its around 15m/s or more the flight must be canceled (Keep in mind while choosing wind direction and checking the airstrip at which place it is placed.

            // In here we calculate probability for a good flight (1 means it is 100percent safe and everyone is gonna fly happy, however less < 0.99 and so on will impact on the airplanes speed, take off, landing and etc.

            probability = 1;

            if (temp < 0)
            {
                probability -= (((temp * (-0.005)) * ((temp * (-0.005))) + ((prec * (0.015)) * (prec * (0.015))) +
                                 ((wind * (0.01)) * (wind * (0.01)) * 2)));
            }

            if (temp >= 0 && temp <= 29)
            {
                probability -= (((temp * (0.007)) * ((temp * (0.007))) + ((prec * (0.005)) * (prec * (0.005))) +
                                 ((wind * (0.008)) * (wind * (0.008)) * 2)));
            }

            if (temp >= 30 && temp <= 39)
            {
                probability -= (((temp * (0.013)) * ((temp * (0.013))) + ((prec * (0.005)) * (prec * (0.005))) +
                                 ((wind * (0.008)) * (wind * (0.008)) * 2)));
            }

            if (temp >= 40)
            {
                probability -= (((temp * (0.016)) * ((temp * (0.016))) + ((prec * (0.005)) * (prec * (0.005))) +
                                 ((wind * (0.008)) * (wind * (0.008)) * 2)));
            }

            if (probability < 0)
            {
                probability = 0;
            }

            prob.Text = probability.ToString();

            //if (trackBarPrecipitation.Value < 10)
            //{
            //    if (trackBarTemperature.Value > 0 || trackBarTemperature.Value <= 39)
            //    {
            //        if (trackBarWindSpeed.Value <= 5)
            //        {
            //            probability -= 0;
            //        }
            //        if (trackBarWindSpeed.Value >=6 || trackBarWindSpeed.Value <= 10)
            //        {
            //            probability -= 0.01;
            //        }
            //        if (trackBarWindSpeed.Value >= 11 || trackBarWindSpeed.Value <= 15)
            //        {
            //            probability -= 0.03;
            //        }
            //        if (trackBarWindSpeed.Value >= 16 || trackBarWindSpeed.Value <= 20)
            //        {
            //            probability -= 0.06;
            //        }
            //        if (trackBarWindSpeed.Value >= 21 || trackBarWindSpeed.Value <= 30)
            //        {
            //            probability -= 0.1;
            //        }
            //        if (trackBarWindSpeed.Value >= 31 || trackBarWindSpeed.Value <= 50)
            //        {
            //            probability -= 0.2;
            //        }
            //        if (trackBarWindSpeed.Value >= 51 || trackBarWindSpeed.Value <= 75)
            //        {
            //            probability -= 0.4;
            //        }
            //        if (trackBarWindSpeed.Value >= 76 || trackBarWindSpeed.Value <= 100)
            //        {
            //            probability -= 0.8;
            //        }
            //    }
            //    if (trackBarTemperature.Value <= 0)
            //    {
            //        if (trackBarWindSpeed.Value <= 5)
            //        {
            //            probability -= 0.02;
            //        }
            //        if (trackBarWindSpeed.Value >= 6 || trackBarWindSpeed.Value <= 10)
            //        {
            //            probability -= 0.04;
            //        }
            //        if (trackBarWindSpeed.Value >= 11 || trackBarWindSpeed.Value <= 15)
            //        {
            //            probability -= 0.08;
            //        }
            //        if (trackBarWindSpeed.Value >= 16 || trackBarWindSpeed.Value <= 20)
            //        {
            //            probability -= 0.15;
            //        }
            //        if (trackBarWindSpeed.Value >= 21 || trackBarWindSpeed.Value <= 30)
            //        {
            //            probability -= 0.25;
            //        }
            //        if (trackBarWindSpeed.Value >= 31 || trackBarWindSpeed.Value <= 50)
            //        {
            //            probability -= 0.5;
            //        }
            //        if (trackBarWindSpeed.Value >= 51 || trackBarWindSpeed.Value <= 75)
            //        {
            //            probability -= 0.7;
            //        }
            //        if (trackBarWindSpeed.Value >= 76 || trackBarWindSpeed.Value <= 100)
            //        {
            //            probability -= 0.9;
            //        }
            //    }


            //     if (trackBarTemperature.Value > 0)
            //    {
            //        if (trackBarWindSpeed.Value <= 5)
            //        {
            //            probability -= 0;
            //        }
            //        if (trackBarWindSpeed.Value >= 6 || trackBarWindSpeed.Value <= 10)
            //        {
            //            probability -= 0.01;
            //        }
            //        if (trackBarWindSpeed.Value >= 11 || trackBarWindSpeed.Value <= 15)
            //        {
            //            probability -= 0.03;
            //        }
            //        if (trackBarWindSpeed.Value >= 16 || trackBarWindSpeed.Value <= 20)
            //        {
            //            probability -= 0.06;
            //        }
            //        if (trackBarWindSpeed.Value >= 21 || trackBarWindSpeed.Value <= 30)
            //        {
            //            probability -= 0.1;
            //        }
            //        if (trackBarWindSpeed.Value >= 31 || trackBarWindSpeed.Value <= 50)
            //        {
            //            probability -= 0.2;
            //        }
            //        if (trackBarWindSpeed.Value >= 51 || trackBarWindSpeed.Value <= 75)
            //        {
            //            probability -= 0.4;
            //        }
            //        if (trackBarWindSpeed.Value >= 76 || trackBarWindSpeed.Value <= 100)
            //        {
            //            probability -= 0.8;
            //        }
            //    }


            //}
        }

        public void PaintCircle(Point p)
        {
            float x = p.X - 3;
            float y = p.Y - 3;
            float width = 2 * 3;
            float height = 2 * 3;
            Pen pen = new Pen(Color.Yellow);
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
        public void PaintRectangleP(Point p)
        {
            float x = p.X - 3;
            float y = p.Y - 3;
            float width = 2 * 3;
            float height = 2 * 3;
            Pen pen = new Pen(Color.Purple);
            Graphics g = this.pictureBox1.CreateGraphics();
            g.DrawRectangle(pen, x, y, width, height);
        }
        public void PaintGrid()
        {
            foreach (Cell c in grid.listOfCells)
            {
                if (c.x == 0 || c.y == 0 || c.x == bmpGrid.Width - 20 || c.y == bmpGrid.Height - 20)
                {
                    rect = new Rectangle(c.x, c.y, 20, 20);
                    gGrid.FillRectangle(brush, rect);
                    gGrid.DrawRectangle(pGrid, rect);
                }
            }
        }
    }
}