using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BARANGAY
{
    public partial class ManageAccount : Form
    {
        public ManageAccount()
        {
            InitializeComponent();
        }

        private void ManageAccount_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            new MainMenu().Show();
            this.Hide();
        }
        private string _loggedInUsername = ""; // To store the username

        // Event handler for the UserLoggedIn event
        public ManageAccount(string username)
        {
            InitializeComponent();
            _loggedInUsername = username;
        }
        private void MainMenu_UserLoggedIn(string username)
        {
            _loggedInUsername = username; // Store the username
            panel6.Invalidate(); // Trigger a repaint of panel6 to show the username
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
