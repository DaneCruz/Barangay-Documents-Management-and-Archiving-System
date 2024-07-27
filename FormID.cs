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
    public partial class FormID : Form
    {
        SQLiteConnection conn;
        SQLiteCommand cmdID;
        SQLiteDataReader dr;
        public string _ID;

        public FormID()
        {
            InitializeComponent();
            conn = new SQLiteConnection("Data Source=database.db;Version=3");
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
                string colName = dataGridView1.Columns[e.ColumnIndex].Name;
                if (colName == "btnEdit1")
                {
                    FrmAccounts f = new FrmAccounts(this);
                    f.btnSave.Enabled = false;
                    f._ID = dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString();
                    f.txtLastName.Text = dataGridView1.Rows[e.RowIndex].Cells["last_name"].Value.ToString();
                    f.txtFirstName.Text = dataGridView1.Rows[e.RowIndex].Cells["first_name"].Value.ToString();
                    f.txtMiddleName.Text = dataGridView1.Rows[e.RowIndex].Cells["middle_name"].Value.ToString();
                    f.txtAddress.Text = dataGridView1.Rows[e.RowIndex].Cells["address"].Value.ToString();
                    f.txtGuardian.Text = dataGridView1.Rows[e.RowIndex].Cells["guardian"].Value.ToString();
                    f.txtContactNumber.Text = dataGridView1.Rows[e.RowIndex].Cells["contact_number"].Value.ToString();
                    f.txtRelationship.Text = dataGridView1.Rows[e.RowIndex].Cells["relationship"].Value.ToString();
                    f.cboStatus.Text = dataGridView1.Rows[e.RowIndex].Cells["status"].Value.ToString();
                    f.cboCondition.Text = dataGridView1.Rows[e.RowIndex].Cells["condition"].Value.ToString();
                    f.id_num.Text = dataGridView1.Rows[e.RowIndex].Cells["id_num"].Value.ToString();
                    f.dtBirthDate.Value = DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells["birth_date"].Value.ToString());
                    f.dtRegisteredOn.Value = DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells["register"].Value.ToString());
                    f.dtExpiresOn.Value = DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells["expire"].Value.ToString());
                    f.ShowDialog();
                }
                else if (colName == "btnDelete1")
                {
                    if (MessageBox.Show("Do you want to delete this ?", clsvar._title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        conn.Open();
                        cmdID = new SQLiteCommand("delete FROM id_card WHERE id like'" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'", conn);
                        cmdID.ExecuteNonQuery();
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
                // You can add additional logging or handling for the exception here if needed
            }
        }

        private void FormID_Load(object sender, EventArgs e)
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            FrmAccounts f = new FrmAccounts(this);
            f.btnUpdate.Enabled = false; 
            f.ShowDialog();
        }
        public void LoadRecord()
        {
            try
            {
                dataGridView1.Rows.Clear();
                conn.Open();
                cmdID = new SQLiteCommand("SELECT * FROM id_card", conn); dr = cmdID.ExecuteReader();
                while (dr.Read())
                {
                    dataGridView1.Rows.Add(dr["id"].ToString(), dr["last_name"].ToString(), dr["first_name"].ToString(), dr["middle_name"].ToString(), DateTime.Parse(dr["birth_date"].ToString()).ToShortDateString(), dr["Status"].ToString(), dr["Address"].ToString(), dr["Guardian"].ToString(), dr["Relationship"].ToString(), dr["Contact_Number"].ToString(), DateTime.Parse(dr["Registered_On"].ToString()).ToShortDateString(), DateTime.Parse(dr["Expires_On"].ToString()).ToShortDateString(), dr["Condition"].ToString(), dr["id_num"].ToString());
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
            // Convert the search term to lowercase for case-insensitive comparison
            string lowerSearchTerm = searchTerm.ToLower();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                bool isVisible = false;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null)
                    {
                        // Convert cell value to lowercase for case-insensitive comparison
                        string lowerCellValue = cell.Value.ToString().ToLower();

                        if (lowerCellValue.Contains(lowerSearchTerm))
                        {
                            isVisible = true;
                            break;
                        }
                    }
                }
                row.Visible = isVisible;
            }
        }


    }
}
