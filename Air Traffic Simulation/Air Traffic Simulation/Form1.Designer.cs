namespace Air_Traffic_Simulation
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.prob = new System.Windows.Forms.Label();
            this.labelWind = new System.Windows.Forms.Label();
            this.labelPrec = new System.Windows.Forms.Label();
            this.labelTemp = new System.Windows.Forms.Label();
            this.trackBarPrecipitation = new System.Windows.Forms.TrackBar();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxWindDirection = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.trackBarWindSpeed = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.trackBarTemperature = new System.Windows.Forms.TrackBar();
            this.rbTakeOff = new System.Windows.Forms.RadioButton();
            this.btnPlaySimulation = new System.Windows.Forms.Button();
            this.rbLanding = new System.Windows.Forms.RadioButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnUploadData = new System.Windows.Forms.Button();
            this.btnSaveData = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnRemoveCheckpoint = new System.Windows.Forms.Button();
            this.btnAddCheckpoint = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.Header = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.calcRouteBtn = new System.Windows.Forms.Button();
            this.testAirplaneAndStrip = new System.Windows.Forms.Button();
            this.timerWeather = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPrecipitation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarWindSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTemperature)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel6.SuspendLayout();
            this.Header.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.prob);
            this.panel1.Controls.Add(this.labelWind);
            this.panel1.Controls.Add(this.labelPrec);
            this.panel1.Controls.Add(this.labelTemp);
            this.panel1.Controls.Add(this.trackBarPrecipitation);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.comboBoxWindDirection);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.trackBarWindSpeed);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.trackBarTemperature);
            this.panel1.Controls.Add(this.rbTakeOff);
            this.panel1.Controls.Add(this.btnPlaySimulation);
            this.panel1.Controls.Add(this.rbLanding);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 42);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(272, 578);
            this.panel1.TabIndex = 0;
            // 
            // prob
            // 
            this.prob.AutoSize = true;
            this.prob.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prob.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.prob.Location = new System.Drawing.Point(34, 430);
            this.prob.Name = "prob";
            this.prob.Size = new System.Drawing.Size(0, 19);
            this.prob.TabIndex = 18;
            // 
            // labelWind
            // 
            this.labelWind.AutoSize = true;
            this.labelWind.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWind.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.labelWind.Location = new System.Drawing.Point(34, 371);
            this.labelWind.Name = "labelWind";
            this.labelWind.Size = new System.Drawing.Size(0, 19);
            this.labelWind.TabIndex = 17;
            // 
            // labelPrec
            // 
            this.labelPrec.AutoSize = true;
            this.labelPrec.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPrec.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.labelPrec.Location = new System.Drawing.Point(34, 320);
            this.labelPrec.Name = "labelPrec";
            this.labelPrec.Size = new System.Drawing.Size(0, 19);
            this.labelPrec.TabIndex = 16;
            // 
            // labelTemp
            // 
            this.labelTemp.AutoSize = true;
            this.labelTemp.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTemp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.labelTemp.Location = new System.Drawing.Point(34, 267);
            this.labelTemp.Name = "labelTemp";
            this.labelTemp.Size = new System.Drawing.Size(0, 19);
            this.labelTemp.TabIndex = 15;
            // 
            // trackBarPrecipitation
            // 
            this.trackBarPrecipitation.Location = new System.Drawing.Point(121, 294);
            this.trackBarPrecipitation.Maximum = 100;
            this.trackBarPrecipitation.Name = "trackBarPrecipitation";
            this.trackBarPrecipitation.Size = new System.Drawing.Size(148, 45);
            this.trackBarPrecipitation.TabIndex = 14;
            this.trackBarPrecipitation.TabStop = false;
            this.trackBarPrecipitation.Value = 24;
            this.trackBarPrecipitation.ValueChanged += new System.EventHandler(this.trackBarPrecipitation_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 294);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(109, 21);
            this.label7.TabIndex = 13;
            this.label7.Text = "Precipitation";
            // 
            // comboBoxWindDirection
            // 
            this.comboBoxWindDirection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxWindDirection.FormattingEnabled = true;
            this.comboBoxWindDirection.Location = new System.Drawing.Point(131, 393);
            this.comboBoxWindDirection.Name = "comboBoxWindDirection";
            this.comboBoxWindDirection.Size = new System.Drawing.Size(128, 29);
            this.comboBoxWindDirection.TabIndex = 12;
            this.comboBoxWindDirection.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 396);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(122, 21);
            this.label6.TabIndex = 11;
            this.label6.Text = "Wind direction";
            // 
            // trackBarWindSpeed
            // 
            this.trackBarWindSpeed.Location = new System.Drawing.Point(121, 345);
            this.trackBarWindSpeed.Maximum = 75;
            this.trackBarWindSpeed.Name = "trackBarWindSpeed";
            this.trackBarWindSpeed.Size = new System.Drawing.Size(148, 45);
            this.trackBarWindSpeed.TabIndex = 9;
            this.trackBarWindSpeed.TabStop = false;
            this.trackBarWindSpeed.Scroll += new System.EventHandler(this.trackBarWindSpeed_Scroll);
            this.trackBarWindSpeed.ValueChanged += new System.EventHandler(this.trackBarWindSpeed_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 345);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 21);
            this.label5.TabIndex = 8;
            this.label5.Text = "Wind speed";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 243);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 21);
            this.label4.TabIndex = 7;
            this.label4.Text = "Temperature";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(88, 207);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 21);
            this.label3.TabIndex = 6;
            this.label3.Text = "Air Control";
            // 
            // trackBarTemperature
            // 
            this.trackBarTemperature.Location = new System.Drawing.Point(121, 243);
            this.trackBarTemperature.Maximum = 50;
            this.trackBarTemperature.Minimum = -30;
            this.trackBarTemperature.Name = "trackBarTemperature";
            this.trackBarTemperature.Size = new System.Drawing.Size(148, 45);
            this.trackBarTemperature.TabIndex = 5;
            this.trackBarTemperature.TabStop = false;
            this.trackBarTemperature.Value = 24;
            this.trackBarTemperature.ValueChanged += new System.EventHandler(this.trackBarTemperature_ValueChanged);
            // 
            // rbTakeOff
            // 
            this.rbTakeOff.AutoSize = true;
            this.rbTakeOff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbTakeOff.Location = new System.Drawing.Point(177, 168);
            this.rbTakeOff.Name = "rbTakeOff";
            this.rbTakeOff.Size = new System.Drawing.Size(92, 25);
            this.rbTakeOff.TabIndex = 4;
            this.rbTakeOff.Text = "Take Off";
            this.rbTakeOff.UseVisualStyleBackColor = true;
            // 
            // btnPlaySimulation
            // 
            this.btnPlaySimulation.FlatAppearance.BorderSize = 0;
            this.btnPlaySimulation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlaySimulation.ForeColor = System.Drawing.Color.White;
            this.btnPlaySimulation.Image = ((System.Drawing.Image)(resources.GetObject("btnPlaySimulation.Image")));
            this.btnPlaySimulation.Location = new System.Drawing.Point(0, 452);
            this.btnPlaySimulation.Name = "btnPlaySimulation";
            this.btnPlaySimulation.Size = new System.Drawing.Size(269, 123);
            this.btnPlaySimulation.TabIndex = 1;
            this.btnPlaySimulation.TabStop = false;
            this.btnPlaySimulation.UseVisualStyleBackColor = true;
            this.btnPlaySimulation.Click += new System.EventHandler(this.btnPlaySimulation_Click);
            // 
            // rbLanding
            // 
            this.rbLanding.AutoSize = true;
            this.rbLanding.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbLanding.Location = new System.Drawing.Point(12, 168);
            this.rbLanding.Name = "rbLanding";
            this.rbLanding.Size = new System.Drawing.Size(90, 25);
            this.rbLanding.TabIndex = 3;
            this.rbLanding.Text = "Landing";
            this.rbLanding.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(55)))), ((int)(((byte)(79)))));
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(272, 125);
            this.panel3.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Palatino Linotype", 36F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(158)))), ((int)(((byte)(209)))));
            this.label1.Location = new System.Drawing.Point(8, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(251, 63);
            this.label1.TabIndex = 0;
            this.label1.Text = "Air Traffic";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(49, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(170, 21);
            this.label2.TabIndex = 2;
            this.label2.Text = "Simulation Outcome";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button3);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(1127, 42);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 578);
            this.panel2.TabIndex = 1;
            // 
            // button3
            // 
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(-2, 490);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(199, 73);
            this.button3.TabIndex = 19;
            this.button3.TabStop = false;
            this.button3.Text = "Show probability";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button1
            // 
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(0, 411);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(199, 73);
            this.button1.TabIndex = 18;
            this.button1.TabStop = false;
            this.button1.Text = "get list";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnUploadData);
            this.panel4.Controls.Add(this.btnSaveData);
            this.panel4.Controls.Add(this.label11);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 228);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(200, 177);
            this.panel4.TabIndex = 2;
            // 
            // btnUploadData
            // 
            this.btnUploadData.FlatAppearance.BorderSize = 0;
            this.btnUploadData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUploadData.ForeColor = System.Drawing.Color.White;
            this.btnUploadData.Location = new System.Drawing.Point(-2, 98);
            this.btnUploadData.Name = "btnUploadData";
            this.btnUploadData.Size = new System.Drawing.Size(199, 73);
            this.btnUploadData.TabIndex = 17;
            this.btnUploadData.TabStop = false;
            this.btnUploadData.Text = "Upload";
            this.btnUploadData.UseVisualStyleBackColor = true;
            this.btnUploadData.Click += new System.EventHandler(this.btnUploadData_Click);
            // 
            // btnSaveData
            // 
            this.btnSaveData.FlatAppearance.BorderSize = 0;
            this.btnSaveData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveData.ForeColor = System.Drawing.Color.White;
            this.btnSaveData.Location = new System.Drawing.Point(0, 28);
            this.btnSaveData.Name = "btnSaveData";
            this.btnSaveData.Size = new System.Drawing.Size(199, 73);
            this.btnSaveData.TabIndex = 16;
            this.btnSaveData.TabStop = false;
            this.btnSaveData.Text = "Save";
            this.btnSaveData.UseVisualStyleBackColor = true;
            this.btnSaveData.Click += new System.EventHandler(this.btnSaveData_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(48, 3);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(100, 21);
            this.label11.TabIndex = 15;
            this.label11.Text = "Data Menu";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btnRemoveCheckpoint);
            this.panel6.Controls.Add(this.btnAddCheckpoint);
            this.panel6.Controls.Add(this.label8);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(200, 228);
            this.panel6.TabIndex = 1;
            // 
            // btnRemoveCheckpoint
            // 
            this.btnRemoveCheckpoint.FlatAppearance.BorderSize = 0;
            this.btnRemoveCheckpoint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveCheckpoint.ForeColor = System.Drawing.Color.White;
            this.btnRemoveCheckpoint.Location = new System.Drawing.Point(1, 114);
            this.btnRemoveCheckpoint.Name = "btnRemoveCheckpoint";
            this.btnRemoveCheckpoint.Size = new System.Drawing.Size(199, 73);
            this.btnRemoveCheckpoint.TabIndex = 16;
            this.btnRemoveCheckpoint.TabStop = false;
            this.btnRemoveCheckpoint.Text = "Remove";
            this.btnRemoveCheckpoint.UseVisualStyleBackColor = true;
            this.btnRemoveCheckpoint.Click += new System.EventHandler(this.btnRemoveCheckpoint_Click);
            // 
            // btnAddCheckpoint
            // 
            this.btnAddCheckpoint.FlatAppearance.BorderSize = 0;
            this.btnAddCheckpoint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddCheckpoint.ForeColor = System.Drawing.Color.White;
            this.btnAddCheckpoint.Location = new System.Drawing.Point(1, 35);
            this.btnAddCheckpoint.Name = "btnAddCheckpoint";
            this.btnAddCheckpoint.Size = new System.Drawing.Size(199, 73);
            this.btnAddCheckpoint.TabIndex = 15;
            this.btnAddCheckpoint.TabStop = false;
            this.btnAddCheckpoint.Text = "Add";
            this.btnAddCheckpoint.UseVisualStyleBackColor = true;
            this.btnAddCheckpoint.Click += new System.EventHandler(this.btnAddCheckpoint_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(25, 3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(151, 21);
            this.label8.TabIndex = 15;
            this.label8.Text = "Checkpoint Menu";
            // 
            // Header
            // 
            this.Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(45)))), ((int)(((byte)(100)))));
            this.Header.Controls.Add(this.button4);
            this.Header.Controls.Add(this.button2);
            this.Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.Header.Location = new System.Drawing.Point(0, 0);
            this.Header.Name = "Header";
            this.Header.Size = new System.Drawing.Size(1327, 42);
            this.Header.TabIndex = 0;
            this.Header.Paint += new System.Windows.Forms.PaintEventHandler(this.Header_Paint);
            this.Header.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Header_MouseDown);
            this.Header.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Header_MouseMove);
            this.Header.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Header_MouseUp);
            // 
            // button4
            // 
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Image = ((System.Drawing.Image)(resources.GetObject("button4.Image")));
            this.button4.Location = new System.Drawing.Point(1245, 0);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(40, 40);
            this.button4.TabIndex = 2;
            this.button4.TabStop = false;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button2
            // 
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.Location = new System.Drawing.Point(1291, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(36, 40);
            this.button2.TabIndex = 0;
            this.button2.TabStop = false;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.t_Tick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(5, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(844, 500);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint_1);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.calcRouteBtn);
            this.panel5.Controls.Add(this.testAirplaneAndStrip);
            this.panel5.Controls.Add(this.pictureBox1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(272, 42);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(855, 578);
            this.panel5.TabIndex = 2;
            // 
            // calcRouteBtn
            // 
            this.calcRouteBtn.Location = new System.Drawing.Point(467, 512);
            this.calcRouteBtn.Name = "calcRouteBtn";
            this.calcRouteBtn.Size = new System.Drawing.Size(187, 51);
            this.calcRouteBtn.TabIndex = 2;
            this.calcRouteBtn.Text = "Calculate Route";
            this.calcRouteBtn.UseVisualStyleBackColor = true;
            this.calcRouteBtn.Click += new System.EventHandler(this.calcRouteButtonClick);
            // 
            // testAirplaneAndStrip
            // 
            this.testAirplaneAndStrip.Font = new System.Drawing.Font("Century Gothic", 10F);
            this.testAirplaneAndStrip.Location = new System.Drawing.Point(660, 512);
            this.testAirplaneAndStrip.Name = "testAirplaneAndStrip";
            this.testAirplaneAndStrip.Size = new System.Drawing.Size(187, 54);
            this.testAirplaneAndStrip.TabIndex = 1;
            this.testAirplaneAndStrip.Text = "Add test airplane and strip";
            this.testAirplaneAndStrip.UseVisualStyleBackColor = true;
            this.testAirplaneAndStrip.Click += new System.EventHandler(this.addTestAirplaneAndStrip);
            // 
            // timerWeather
            // 
            this.timerWeather.Enabled = true;
            this.timerWeather.Interval = 10;
            this.timerWeather.Tick += new System.EventHandler(this.timerWeather_Tick);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(45)))), ((int)(((byte)(73)))));
            this.ClientSize = new System.Drawing.Size(1327, 620);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Header);
            this.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(158)))), ((int)(((byte)(209)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPrecipitation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarWindSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTemperature)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.Header.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel Header;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbLanding;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar trackBarTemperature;
        private System.Windows.Forms.RadioButton rbTakeOff;
        private System.Windows.Forms.Button btnPlaySimulation;
        private System.Windows.Forms.TrackBar trackBarWindSpeed;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxWindDirection;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TrackBar trackBarPrecipitation;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button btnRemoveCheckpoint;
        private System.Windows.Forms.Button btnAddCheckpoint;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnUploadData;
        private System.Windows.Forms.Button btnSaveData;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label labelTemp;
        private System.Windows.Forms.Label labelWind;
        private System.Windows.Forms.Label labelPrec;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label prob;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button testAirplaneAndStrip;
        private System.Windows.Forms.Button calcRouteBtn;
        private System.Windows.Forms.Timer timerWeather;
    }
}

