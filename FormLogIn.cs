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

        private void button1_Click_1(object sender, EventArgs e) // Login button
        {
            try
            {
                conn.Open(); // Open connection within the try block

                // Parameterized query for security (prevents SQL injection)
                string login = "SELECT * FROM login WHERE username = @username AND password = @password";
                using (SQLiteCommand cmd = new SQLiteCommand(login, conn))
                {
                    // Check if username or password fields are empty
                    if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
                    {
                        MessageBox.Show("Please enter username and password.", "Empty Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // Exit method early to prevent further execution
                    }

                    // Add parameters to the command
                    cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@password", txtPassword.Text);

                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            // Successful login
                            new MainMenu().Show();
                            this.Hide();
                        }
                        else
                        {
                            // Failed login attempt
                            MessageBox.Show("Invalid Username or Password. Please try again.",
                                            "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            // Clear password field and refocus on username field
                            txtPassword.Text = "";
                            txtUsername.Focus();
                        }
                    }
                }
            }
            catch (SQLiteException ex) // Catch SQLite-specific exceptions
            {
                MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close(); // Ensure connection is always closed in the finally block
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
    }
}