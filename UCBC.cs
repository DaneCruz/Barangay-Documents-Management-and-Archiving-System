using BARANGAY;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Runtime.InteropServices;

namespace BARANGAY
{
    public partial class UCBC : Form
    {
        SQLiteConnection conn;
        SQLiteCommand cmd;
        SQLiteDataReader dr;
        public string _ID;

        public UCBC()
        {
            InitializeComponent();
            conn = new SQLiteConnection("Data Source=database.db;Version=3");
        }

        private void addUserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            panelContainer.Controls.Clear();
            panelContainer.Controls.Add(userControl);
            userControl.BringToFront();
        }


        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    string colName = dataGridView1.Columns[e.ColumnIndex].Name;
                    if (colName == "btnEdit1")
                    {
                        FrmAccountsBC1 f = new FrmAccountsBC1(this);
                        f.btnSave.Enabled = false;
                        f._ID = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                        f.txtName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                        f.txtAddress.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                        f.txtContactNumber.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                        f.cboStatus.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                        f.dtBirthDate.Value = DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                        f.ShowDialog();
                    }
                    else if (colName == "btnDelete1")
                    {
                        if (MessageBox.Show("Do you want to delete this?", clsvar._title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            conn.Open();
                            cmd = new SQLiteCommand("DELETE FROM barangay_clearance WHERE id = @id", conn);
                            cmd.Parameters.AddWithValue("@id", dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            cmd.ExecuteNonQuery();
                            conn.Close();
                            MessageBox.Show("Record has been successfully deleted", clsvar._title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadRecord();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Index is out of bounds.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show(ex.Message, clsvar._title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UCBC_Load(object sender, EventArgs e)
        {
            try
            {
                LoadRecord();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmAccountsBC1 f = new FrmAccountsBC1(this);
            f.btnUpdate.Enabled = false;
            f.ShowDialog();
        }

        public void LoadRecord()
        {
            try
            {
                dataGridView1.Rows.Clear();
                conn.Open();
                cmd = new SQLiteCommand("SELECT * FROM barangay_clearance", conn);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    dataGridView1.Rows.Add(dr["id"].ToString(), dr["Name"].ToString(), DateTime.Parse(dr["birth_date"].ToString()).ToShortDateString(), dr["Status"].ToString(), dr["Address"].ToString(), dr["purpose"].ToString());
                }
                dr.Close();
                conn.Close();
                dataGridView1.ClearSelection();
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show(ex.Message, clsvar._title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
