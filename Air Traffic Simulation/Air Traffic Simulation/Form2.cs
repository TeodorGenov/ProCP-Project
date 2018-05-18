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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            bunifuMaterialTextbox1.Text = "";
            bunifuMaterialTextbox2.Text = "";
            bunifuGauge1.Value = 50;
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void bunifuLabel3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            var newForm = new Form1();
            newForm.Show();
            this.Hide();
        }

        private void bunifuGauge1_Load(object sender, EventArgs e)
        {

        }
    }
}
