using BARANGAY.userControl;
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
            _loggedInUsername = username;
        }
        private void MainMenu_UserLoggedIn(string username)
        {
            _loggedInUsername = username; // Store the username
            panel6.Invalidate(); // Trigger a repaint of panel6 to show the username
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
            new ManageAccount().Show();
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
            UCBC c = new UCBC();
            c.TopLevel = false;
            panel3.Controls.Add(c);
            c.BringToFront();
            c.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = DateTime.Now.ToLongDateString();
            label3.Text = DateTime.Now.ToLongTimeString();
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {
            if (!string.IsNullOrEmpty(_loggedInUsername))
            {
                using (Font font = new Font("Century Gothic", 12, FontStyle.Bold))
                using (Brush brush = new SolidBrush(Color.Black))
                {
                    // Draw "Hello!" first
                    string greeting = "Hello!";
                    e.Graphics.DrawString(greeting, font, brush, 0, 0); // Start at the top-left (0,0)

                    // Measure the width of the greeting
                    SizeF greetingSize = e.Graphics.MeasureString(greeting, font);

                    // Calculate the starting position for the username
                    float x = greetingSize.Width + 5; // Add a small margin (5 pixels) after the greeting
                    float y = 0; // Keep the username on the same line

                    // Draw the username right after the greeting
                    e.Graphics.DrawString(_loggedInUsername, font, brush, x, y);
                }
            }
        }
    }
}
