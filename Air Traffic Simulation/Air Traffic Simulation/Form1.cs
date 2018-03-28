﻿using System;
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
        //radar

        Timer t = new Timer();
        int width = 300, height = 300, hand = 150;
        int u; //in degree
        int cx, cy; //center of the circle
        int x, y; //hand coordinates
        int tx, ty, lim = 20;


        // SIMULATION VARIABLES

        int temp, prec, wind;
        List<Checkpoint> checkpoints;
        string dir;
        string serializationFile;

        double probability;

        int AddingCheckpoints = 0;
        static int cpName = 0;
        BinaryFormatter bf;

        // END OF SIMULATION VARIABLES


        Bitmap bmp;
        Pen p;
        Graphics g;

        //simulation

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public Form1()
        {
            dir = @"..\..\Saved";
            serializationFile = Path.Combine(dir, "Checkpoints.bin");
            checkpoints = new List<Checkpoint>();
            InitializeComponent();
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
            

            //radar

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
            t.Interval = 1; //miliseconds
            t.Tick += new EventHandler(this.t_Tick);
            t.Start();
        }

        //methods for radar

        private void t_Tick (object sender, EventArgs e)
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

            //draw perpendicular line
            g.DrawLine(p, new Point(cx, 0), new Point(cx, height)); //up - down
            g.DrawLine(p, new Point(0, cy), new Point(width, cy)); //left - right

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
            { u = 0; }
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (AddingCheckpoints == 1)
            {
                cpName++;
                string name = "cp" + cpName;
                checkpoints.Add(new Checkpoint(name, 10, 10));
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
                AddingCheckpoints = 1;
                btnAddCheckpoint.Text = "Stop";
            }
            else if (btnAddCheckpoint.Text == "Stop")
            {
                AddingCheckpoints = 0;
                btnAddCheckpoint.Text = "Add";
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void trackBarTemperature_ValueChanged(object sender, EventArgs e)
        {
            temp = trackBarTemperature.Value;
            labelTemp.Text = temp.ToString() + "°C";
            Simulate();
        }

        private void btnSaveData_Click(object sender, EventArgs e)
        {
            using (Stream stream = File.Open(serializationFile, FileMode.Create))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                bformatter.Serialize(stream, checkpoints);
            }
               
           
        }

        private void btnUploadData_Click(object sender, EventArgs e)
        {
            using (Stream stream = File.Open(serializationFile, FileMode.Open))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                checkpoints = (List<Checkpoint>)bformatter.Deserialize(stream);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach(Checkpoint a in checkpoints)
            {
                MessageBox.Show(a.Name);
            }
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

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

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
                        probability -= (((temp * (-0.005))* ((temp * (-0.005))) + ((prec * (0.015))* (prec * (0.015))) + ((wind * (0.01))* (wind * (0.01)) * 2)));
            }
            if (temp >= 0 && temp <= 29)
            {
                probability -= (((temp * (0.007)) * ((temp * (0.007))) + ((prec * (0.01)) * (prec * (0.01))) + ((wind * (0.008)) * (wind * (0.008)) * 2)));
            }
            if (temp >= 30 && temp <= 39)
            {
                probability -= (((temp * (0.013)) * ((temp * (0.013))) + ((prec * (0.01)) * (prec * (0.01))) + ((wind * (0.008)) * (wind * (0.008)) * 2)));
            }
            if (temp >= 40)
            {
                probability -= (((temp * (0.016)) * ((temp * (0.016))) + ((prec * (0.01)) * (prec * (0.01))) + ((wind * (0.008)) * (wind * (0.008)) *2)));
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
    }
}
