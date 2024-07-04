﻿using BARANGAY.userControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BARANGAY
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private string _loggedInUsername = ""; // To store the username

        // Event handler for the UserLoggedIn event
        public MainMenu(string username)
        {
            InitializeComponent();
            _loggedInUsername = username; // Store the username
            UpdateGreetingLabel(); // Update the greeting label
            timer1.Start(); // Start the timer
        }
        private void UpdateGreetingLabel()
        {
            if (!string.IsNullOrEmpty(_loggedInUsername))
            {
                label4.Text = $"Hello! {_loggedInUsername}";
            }
            else
            {
                label4.Text = ""; // Handle case when username is empty or null
            }
        }
        private void MainMenu_UserLoggedIn(string username)
        {
            _loggedInUsername = username; // Store the username
            UpdateGreetingLabel(); // Update the greeting label
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            int y = Screen.PrimaryScreen.Bounds.Height;
            int x = Screen.PrimaryScreen.Bounds.Width;
            this.Height = y - 30;
            this.Width = x;
            this.Left = 0;
            this.Top = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormID f = new FormID();
            f.TopLevel = false;
            panel3.Controls.Add(f);
            f.LoadRecord();
            f.BringToFront();
            f.Show();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            FormCertifications f = new FormCertifications();
            f.TopLevel = false;
            panel3.Controls.Add(f);
            f.BringToFront();
            f.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            new FormLogIn().Show();
            this.Hide();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormBP f = new FormBP();
            f.TopLevel = false;
            panel3.Controls.Add(f);
            f.BringToFront();
            f.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            FormBRIC f = new FormBRIC();
            f.TopLevel = false;
            panel3.Controls.Add(f);
            f.BringToFront();
            f.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UCBC f = new UCBC();
            f.TopLevel = false;
            panel3.Controls.Add(f);
            f.BringToFront();
            f.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = DateTime.Now.ToLongDateString();
            label3.Text = DateTime.Now.ToLongTimeString();
        }

   

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}
