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
    public partial class FrmAccountsBP : Form
    {
        SQLiteConnection conn;
        SQLiteCommand cmd;
        FormBP parentForm;
        public string _ID;

        public FrmAccountsBP(FormBP parent)
        {
            InitializeComponent();
            conn = new SQLiteConnection("Data Source=database.db;Version=3");
            cmd = new SQLiteCommand();
            this.parentForm = parent;
        }

        private void FrmAccountsBP_Load(object sender, EventArgs e)
        {
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
                    string sql = "INSERT INTO business_permit (Business_Name, Business_Type, Business_address, Name_Of_Owner) " +
                                 "VALUES (@Business_Name, @Business_Type, @Business_address, @Name_Of_Owner)";
                    cmd = new SQLiteCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Business_Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@Business_Type", txtBusinessType.Text);
                    cmd.Parameters.AddWithValue("@Business_address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@Name_Of_Owner", txtContactNumber.Text);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Record has been successfully saved!", "Save Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                    parentForm.LoadRecord();
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
            txtBusinessType.Clear();
            txtAddress.Clear();
            txtContactNumber.Clear();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            txtName.Focus();
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
                    string sql = "UPDATE business_permit SET Business_Name=@Business_Name, Business_Type=@Business_Type, Business_address=@Business_address, Name_Of_Owner=@Name_Of_Owner WHERE id = @ID";
                    cmd = new SQLiteCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Business_Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@Business_Type", txtBusinessType.Text);
                    cmd.Parameters.AddWithValue("@Business_address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@Name_Of_Owner", txtContactNumber.Text);
                    cmd.Parameters.AddWithValue("@ID", _ID);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Record has been successfully updated!", "Update Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                    parentForm.LoadRecord();
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
