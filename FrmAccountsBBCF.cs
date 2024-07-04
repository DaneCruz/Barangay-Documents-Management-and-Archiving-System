using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace BARANGAY
{
    public partial class FrmAccountsBBCF : Form
    {
        SQLiteConnection conn;
        SQLiteCommand cmd;
        FormCertifications f; // Changed type to FormCertifications to match the calling form
        public string _ID;

        public FrmAccountsBBCF(FormCertifications form) // Constructor accepting FormCertifications instance
        {
            InitializeComponent();
            conn = new SQLiteConnection("Data Source=database.db;Version=3");
            cmd = new SQLiteCommand();
            this.f = form; // Initialize the FormCertifications instance
        }

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
                    string sql = "INSERT INTO bbcf (Name, birth_date, status, address, Contact_Number, date_of_issuance, place_of_issuance) " +
                                 "VALUES (@Name, @birth_date, @status, @address, @Contact_Number, @date_of_issuance, @place_of_issuance)";
                    cmd = new SQLiteCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@birth_date", dtBirthDate.Value);
                    cmd.Parameters.AddWithValue("@status", cboStatus.Text);
                    cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@Contact_Number", txtContactNumber.Text);
                    cmd.Parameters.AddWithValue("@date_of_issuance", dtRegisteredOn.Value);
                    cmd.Parameters.AddWithValue("@place_of_issuance", txtPlace.Text);
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
            txtName.Clear();
            txtAddress.Clear();
            txtContactNumber.Clear();
            cboStatus.SelectedIndex = -1;
            dtBirthDate.Value = DateTime.Now;
            dtRegisteredOn.Value = DateTime.Now;
            txtPlace.Clear();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            txtName.Focus();
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
                    string sql = "UPDATE bbcf SET Name=@Name, birth_date=@birth_date, status=@status, address=@address, Contact_Number=@Contact_Number, date_of_issuance=@date_of_issuance, place_of_issuance=@place_of_issuance WHERE id = @ID";
                    cmd = new SQLiteCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@birth_date", dtBirthDate.Value);
                    cmd.Parameters.AddWithValue("@status", cboStatus.Text);
                    cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@Contact_Number", txtContactNumber.Text);
                    cmd.Parameters.AddWithValue("@date_of_issuance", dtRegisteredOn.Value);
                    cmd.Parameters.AddWithValue("@place_of_issuance", txtPlace.Text);
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

        private void FrmAccountsBBCF_Load(object sender, EventArgs e)
        {

        }
    }
}
