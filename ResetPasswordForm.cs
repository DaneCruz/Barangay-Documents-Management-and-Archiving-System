using System;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Windows.Forms;
using BCrypt.Net;

namespace BARANGAY
{
    public partial class ResetPasswordForm : Form
    {
        private SQLiteConnection conn;
        private string username;

        public ResetPasswordForm(string username)
        {
            InitializeComponent();
            this.username = username;
            conn = new SQLiteConnection("Data Source=database.db;Version=3"); // Adjust connection string as per your database location
        }

        private void ResetPasswordForm_Load(object sender, EventArgs e)
        {
            txtUsername.Text = username; // Set the username text
            txtUsername.ReadOnly = true; // Make username readonly
            txtSpecialKey.Focus(); // Set focus to special key textbox

            try
            {
                conn.Open();
                // Perform any additional setup if needed
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to database: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close(); // Close the form if connection fails
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = textConfirmNewPassword.Text.Trim();
            string specialKey = txtSpecialKey.Text.Trim();

            // Validate inputs
            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword) || string.IsNullOrEmpty(specialKey))
            {
                MessageBox.Show("Please enter the new password, confirm password, and special key.", "Empty Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Password Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsPasswordStrong(txtNewPassword.Text))
            {
                MessageBox.Show("Password is not strong enough. It should be at least 8 characters long and contain a mix of uppercase, lowercase, numbers, and symbols.", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                conn.Open();

                // Check the special key
                string checkKeyQuery = "SELECT special_key FROM login WHERE username = @username";
                using (SQLiteCommand checkKeyCmd = new SQLiteCommand(checkKeyQuery, conn))
                {
                    checkKeyCmd.Parameters.AddWithValue("@username", username);
                    string storedSpecialKey = (string)checkKeyCmd.ExecuteScalar();

                    if (specialKey != storedSpecialKey)
                    {
                        MessageBox.Show("Special key does not match.", "Invalid Special Key", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Update password in the database
                string updatePasswordQuery = "UPDATE login SET password = @password WHERE username = @username";
                using (SQLiteCommand updateCmd = new SQLiteCommand(updatePasswordQuery, conn))
                {
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword); // Hash the new password

                    updateCmd.Parameters.AddWithValue("@password", hashedPassword);
                    updateCmd.Parameters.AddWithValue("@username", username);

                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Password updated successfully.", "Password Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close(); // Close the reset password form
                        new FormLogIn().Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update password.", "Password Reset Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private bool IsPasswordStrong(string password)
        {
            // Implement your password strength rules here
            return password.Length >= 8 &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsLower) &&
                   password.Any(char.IsDigit) &&
                   password.Any(ch => !char.IsLetterOrDigit(ch));
        }
        private void ResetPasswordForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (conn.State == ConnectionState.Open)
                conn.Close(); // Close connection when form is closed
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
            new FormLogIn().Show();
        }

        private void checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (checkbox.Checked)
            {
                txtNewPassword.PasswordChar = '\0';
                textConfirmNewPassword.PasswordChar = '\0';
            }
            else
            {
                txtNewPassword.PasswordChar = '*';
                textConfirmNewPassword.PasswordChar = '*';
            }
        }

        private void txtSpecialKey_TextChanged(object sender, EventArgs e)
        {

        }

        private void ResetPasswordForm_Load_1(object sender, EventArgs e)
        {

        }
    }
}
