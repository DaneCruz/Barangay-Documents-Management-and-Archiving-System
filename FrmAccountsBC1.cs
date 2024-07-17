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
    public partial class FrmAccountsBC1 : Form
    {
        SQLiteConnection conn;
        SQLiteCommand cmd;
        UCBC f;
        public string _ID;

        public FrmAccountsBC1(UCBC f)
        {
            InitializeComponent();
            conn = new SQLiteConnection("Data Source=database.db;Version=3");
            cmd = new SQLiteCommand();
            this.f = f;
        }

        // Remove the default constructor if it's not needed.
        // If you need it for some reason, ensure InitializeComponent() is called.
        // public FrmAccountsBC1()
        // {
        //     InitializeComponent();
        // }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Do you want to save this record?", "Save Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conn.Open();
                    string sql = "INSERT INTO barangay_clearance (last_name, first_name, middle_name, birth_date, status, address, purpose) " +
                                 "VALUES (@last_name, @first_name, @middle_name, @birth_date, @status, @address, @purpose)";
                    cmd = new SQLiteCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@last_name", txtLastName.Text);
                    cmd.Parameters.AddWithValue("@first_name", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("@middle_name", txtMiddleName.Text);
                    cmd.Parameters.AddWithValue("@birth_date", dtBirthDate.Value);
                    cmd.Parameters.AddWithValue("@status", cboStatus.Text);
                    cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@purpose", txtContactNumber.Text);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Record has been successfully saved!", "Save Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                    f.LoadRecord();
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void clear()
        {
            txtLastName.Clear();
            txtAddress.Clear();
            txtContactNumber.Clear();
            cboStatus.SelectedIndex = -1;
            dtBirthDate.Value = DateTime.Now;
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            txtLastName.Focus();
        }

        private void cboStatus_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            clear();
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Do you want to update this record?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conn.Open();
                    string sql = "UPDATE barangay_clearance SET last_name=@last_name, first_name=@first_name, middle_name=@middle_name, birth_date=@birth_date, status=@status, address=@address, purpose=@purpose WHERE id = @ID";
                    cmd = new SQLiteCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@last_name", txtLastName.Text);
                    cmd.Parameters.AddWithValue("@first_name", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("@middle_name", txtMiddleName.Text);
                    cmd.Parameters.AddWithValue("@birth_date", dtBirthDate.Value);
                    cmd.Parameters.AddWithValue("@status", cboStatus.Text);
                    cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@purpose", txtContactNumber.Text);
                    cmd.Parameters.AddWithValue("@ID", _ID);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Record has been successfully updated!", "Update Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                    f.LoadRecord();
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FrmAccountsBC1_Load(object sender, EventArgs e)
        {

        }
    }
}
