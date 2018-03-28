using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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


        int AddingCheckpoints = 0;

        Bitmap bmp;
        Pen p;
        Graphics g;

        //simulation

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
            if (AddingCheckpoints == 1);
           
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
    }
}
