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

                // Parameterized query (secure)
                string login = "SELECT username, password FROM login WHERE username = @username";
                using (SQLiteCommand cmd = new SQLiteCommand(login, conn))
                {
                    // Input validation 
                    if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
                    {
                        MessageBox.Show("Please enter username and password.", "Empty Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    cmd.Parameters.AddWithValue("@username", txtUsername.Text);


                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            string hashedPasswordFromDb = dr["password"].ToString();

                            if (BCrypt.Net.BCrypt.Verify(txtPassword.Text, hashedPasswordFromDb))
                            {
                                // Successful login
                                string retrievedUsername = dr["username"].ToString();

                                MainMenu mainMenuForm = new MainMenu(retrievedUsername);
                                mainMenuForm.Show();
                                this.Hide();
                            }
                            else
                            {
                                // Incorrect password
                                MessageBox.Show("Incorrect password. Please try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtPassword.Text = "";
                                txtPassword.Focus();
                            }
                        }
                        else
                        {
                            // Username not found
                            MessageBox.Show("Invalid username. Please try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtUsername.Text = "";
                            txtUsername.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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