using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using BCrypt.Net;

namespace BARANGAY
{
    public partial class FormLogIn : Form
    {
        private SQLiteConnection conn;
        public FormLogIn()
        {
            InitializeComponent();
            conn = new SQLiteConnection("Data Source=database.db;Version=3");
        }

        private void FormLogIn_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {
            new FormRegister().Show();
            this.Hide();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                conn.Open();

                // Secure parameterized query with password hashing
                string loginQuery = "SELECT username, password FROM login WHERE username = @username";
                using (SQLiteCommand cmd = new SQLiteCommand(loginQuery, conn))
                {
                    // Input validation (also trim whitespace)
                    string username = txtUsername.Text.Trim();
                    string password = txtPassword.Text.Trim();
                    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                    {
                        MessageBox.Show("Please enter both username and password.", "Empty Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    cmd.Parameters.AddWithValue("@username", username);

                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            // Hash verification
                            string hashedPasswordFromDb = dr["password"].ToString();

                            if (BCrypt.Net.BCrypt.Verify(password, hashedPasswordFromDb))
                            {
                                // Successful login (open MainMenu on the UI thread)
                                string retrievedUsername = dr["username"].ToString();
                                this.BeginInvoke(new Action(() =>
                                {
                                    MainMenu mainMenuForm = new MainMenu(retrievedUsername);
                                    mainMenuForm.Show();
                                    this.Hide();
                                }));
                            }
                            else
                            {
                                // Incorrect password
                                MessageBox.Show("Incorrect password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtPassword.Text = ""; // Clear the password field
                                txtPassword.Focus();   // Set focus back to the password field
                            }
                        }
                        else
                        {
                            // Username not found
                            MessageBox.Show("Invalid username.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtUsername.Text = "";
                            txtUsername.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Optional: Log the exception for debugging
                // You can use a logging library (e.g., log4net, NLog) or write to a file
                // Example: 
                // File.AppendAllText("error_log.txt", $"Login Error: {ex.ToString()}\n"); 
            }
            finally
            {
                conn.Close();
            }
        }




        private void button2_Click_1(object sender, EventArgs e)
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtUsername.Focus();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) // Show/hide password
        {
            txtPassword.PasswordChar = checkbox.Checked ? '\0' : '*'; // Use checkbox.Checked directly
        }

        private void label6_Click(object sender, EventArgs e) // Register link
        {
            new FormRegister().Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }
    }
}