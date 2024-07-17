
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
    public partial class FormCertifications : Form
    {
        SQLiteConnection conn;
        SQLiteCommand cmd;
        SQLiteDataReader dr;
        public string _ID;

        public FormCertifications()
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
                    FrmAccountsBBCF f = new FrmAccountsBBCF(this);
                    f.btnSave.Enabled = false;
                    f._ID = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    f.txtOwnersName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    f.txtAddress.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                    f.txtAmount.Text = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();
                    f.txtBusinessName.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                    f.txtBusinessType.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                    f.txtDay.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                    f.txtMonthYear.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
                    f.dtRegisteredOn.Value = DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString());
                    f.txtAdministeredBy.Text = dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString();
                    f.ShowDialog();
                }
                else if (colName == "btnDelete1")
                {
                    if (MessageBox.Show("Do you want to delete this?", clsvar._title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        conn.Open();
                        cmd = new SQLiteCommand("delete FROM bbcf WHERE id like'" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'", conn);
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

        private void FormCertifications_Load(object sender, EventArgs e)
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
            FrmAccountsBBCF f = new FrmAccountsBBCF(this);
            f.btnUpdate.Enabled = false;
            f.ShowDialog();
        }

        public void LoadRecord()
        {
            try
            {
                dataGridView1.Rows.Clear();
                conn.Open();
                cmd = new SQLiteCommand("SELECT * FROM bbcf", conn);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    dataGridView1.Rows.Add(dr["id"].ToString(), dr["Name"].ToString(), dr["address"].ToString(), dr["business_name"].ToString(), dr["business_type"].ToString(), dr["day_of_issuance"].ToString(), dr["monthyear_of_issuance"].ToString(), DateTime.Parse(dr["or_date"].ToString()).ToShortDateString(), dr["amount"].ToString(), dr["administered_by"].ToString());
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

        private void btn_search_Click(object sender, EventArgs e)
        {
            FilterRecords(searchBox.Text);
        }
        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            FilterRecords(searchBox.Text);
        }

        private void FilterRecords(string searchTerm)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                bool isVisible = false;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().ToLower().Contains(searchTerm.ToLower()))
                    {
                        isVisible = true;
                        break;
                    }
                }
                row.Visible = isVisible;
            }
        }
    }
}
