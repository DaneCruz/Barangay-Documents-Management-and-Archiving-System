using BARANGAY.userControl;
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
    public partial class FormBP : Form
    {
        SQLiteConnection conn;
        SQLiteCommand cmd;
        SQLiteDataReader dr;
        public string _ID;

        public FormBP()
        {
            InitializeComponent();
            conn = new SQLiteConnection("Data Source=database.db;Version=3");
        }

<<<<<<< HEAD

=======
>>>>>>> 40c9b05a9e0db0e85d3fc823c41164e91dfbd121
        private void addUserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            panelContainer.Controls.Clear();
            panelContainer.Controls.Add(userControl);
            userControl.BringToFront();
        }
<<<<<<< HEAD

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
                string colName = dataGridView1.Columns[e.ColumnIndex].Name;
                if (colName == "btnEdit1")
                {
                    FrmAccounts f = new FrmAccounts(this);
                    f.btnSave.Enabled = false;
                    f._ID = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    f.txtName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    f.txtAddress.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                    f.txtGuardian.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                    f.txtContactNumber.Text = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
                    f.txtRelationship.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
                    f.cboStatus.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                    f.cboCondition.Text = dataGridView1.Rows[e.RowIndex].Cells[10].Value.ToString();
                    f.dtBirthDate.Value = DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                    f.dtRegisteredOn.Value = DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString());
                    f.dtExpiresOn.Value = DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString());
                    f.ShowDialog();
                }
                else if (colName == "btnDelete1")
                {
                    if (MessageBox.Show("Do you want to delete this ?", clsvar._title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        conn.Open();
                        cmd = new SQLiteCommand("delete FROM id_card WHERE id like'" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'", conn);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        MessageBox.Show("Record has been successfully deleted", clsvar._title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadRecord();
                    }
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show(ex.Message, clsvar._title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FormBP_Load(object sender, EventArgs e)
        {
            try
            {
                LoadRecord();
                MessageBox.Show("Data loaded successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }
=======
>>>>>>> 40c9b05a9e0db0e85d3fc823c41164e91dfbd121

        public void LoadRecord()
        {
            try
            {
                dataGridView1.Rows.Clear();
                conn.Open();
                cmd = new SQLiteCommand("SELECT * FROM business_permit", conn);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    dataGridView1.Rows.Add(dr["id"].ToString(), dr["business_name"].ToString(), dr["business_address"].ToString(), dr["business_owner"].ToString());
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

        private void button1_Click(object sender, EventArgs e)
        {
            FrmAccounts add = new FrmAccounts(this);
            add.btnUpdate.Enabled = false;
            add.ShowDialog();
        }
<<<<<<< HEAD
    }
}
=======

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
                        FrmAccountsBP f = new FrmAccountsBP(this);
                        f.btnSave.Enabled = false;
                        f._ID = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                        f.txtName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                        f.txtAddress.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                        f.txtContactNumber.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                        f.txtBusinessType.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                        f.ShowDialog();
                    }
                    else if (colName == "btnDelete1")
                    {
                        if (MessageBox.Show("Do you want to delete this?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            conn.Open();
                            cmd = new SQLiteCommand("DELETE FROM business_permit WHERE id = @id", conn);
                            cmd.Parameters.AddWithValue("@id", dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            cmd.ExecuteNonQuery();
                            conn.Close();
                            MessageBox.Show("Record has been successfully deleted", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FormBP_Load(object sender, EventArgs e)
        {
            try
            {
                LoadRecord();
                MessageBox.Show("Data loaded successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmAccountsBP f = new FrmAccountsBP(this);
            f.btnUpdate.Enabled = false;
            f.ShowDialog();
        }

        public void LoadRecord()
        {
            try
            {
                dataGridView1.Rows.Clear();
                conn.Open();
                cmd = new SQLiteCommand("SELECT * FROM business_permit", conn);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    dataGridView1.Rows.Add(dr["id"].ToString(), dr["Business_Name"].ToString(), dr["Business_Type"].ToString(), dr["Business_Address"].ToString(), dr["Name_of_Owner"].ToString());
                }
                dr.Close();
                conn.Close();
                dataGridView1.ClearSelection();
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}



>>>>>>> 40c9b05a9e0db0e85d3fc823c41164e91dfbd121
