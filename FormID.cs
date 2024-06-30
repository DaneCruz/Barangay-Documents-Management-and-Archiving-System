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
    public partial class FormID : Form
    {
        SQLiteConnection conn;
        SQLiteCommand cmd;
        SQLiteDataReader dr;
        public string _ID;

        public FormID()
        {
            InitializeComponent();
            conn = new SQLiteConnection("Data Source=database.db;Version=3");
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        private void addUserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            panelContainer.Controls.Clear();
            panelContainer.Controls.Add(userControl);
            userControl.BringToFront();
        }
            private void guna2Button1_Click(object sender, EventArgs e)
        {
            UCPI2 uc = new UCPI2();
            addUserControl(uc);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            UCUP2 uc = new UCUP2();
            addUserControl(uc);
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            UCDC2 uc = new UCDC2();
            addUserControl(uc);
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
            }catch(Exception ex)
            {
                conn.Close();
                MessageBox.Show(ex.Message, clsvar._title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FormID_Load(object sender, EventArgs e)
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
                cmd = new SQLiteCommand("SELECT * FROM id_card", conn);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    dataGridView1.Rows.Add(dr["id"].ToString(), dr["Name"].ToString(), dr["birth_date"].ToString(), dr["Status"].ToString(), dr["Address"].ToString(), dr["Guardian"].ToString(), dr["Relationship"].ToString(), dr["Registered_On"].ToString(), dr["Expires_On"].ToString(), dr["Condition"].ToString());
                }
                dr.Close();
                conn.Close();
                dataGridView1.ClearSelection();

            }catch(Exception ex)
            {
                conn.Close();
                MessageBox.Show(ex.Message, clsvar._title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
