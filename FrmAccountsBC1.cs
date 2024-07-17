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
using iText.Forms.Fields;
using iText.Forms;
using iText.Kernel.Pdf;
using System.IO;
using iText.Kernel.Exceptions;

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
                    string sql = "INSERT INTO barangay_clearance (Name, birth_date, status, address, purpose,day_of_issuance, monthyear_of_issuance, Registered_On, Expires_On) " +
                                 "VALUES (@Name, @birth_date, @status, @address, @purpose, @day_of_issuance, @monthyear_of_issuance, @Registered_On, @Expires_On)";
                    cmd = new SQLiteCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@birth_date", dtBirthDate.Value);
                    cmd.Parameters.AddWithValue("@status", cboStatus.Text);
                    cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@purpose", txtContactNumber.Text);
                    cmd.Parameters.AddWithValue("@day_of_issuance", txtDay.Text);
                    cmd.Parameters.AddWithValue("@monthyear_of_issuance", txtMonthYear.Text);
                    cmd.Parameters.AddWithValue("@Registered_On", dtRegisteredOn.Value);
                    cmd.Parameters.AddWithValue("@Expires_On", dtExpiresOn.Value);
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
            txtDay.Clear();
            txtMonthYear.Clear();
            dtRegisteredOn.Value = DateTime.Now;
            dtExpiresOn.Value = DateTime.Now;
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
                    string sql = "UPDATE barangay_clearance SET Name=@Name, birth_date=@birth_date, status=@status, address=@address, purpose=@purpose, day_of_issuance=@day_of_issuance, monthyear_of_issuance=@monthyear_of_issuance, Registered_On=@Registered_On, Expires_On=@Expires_On WHERE id = @ID";
                    cmd = new SQLiteCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@birth_date", dtBirthDate.Value);
                    cmd.Parameters.AddWithValue("@status", cboStatus.Text);
                    cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@purpose", txtContactNumber.Text);
                    cmd.Parameters.AddWithValue("@day_of_issuance", txtDay.Text);
                    cmd.Parameters.AddWithValue("@monthyear_of_issuance", txtMonthYear.Text);
                    cmd.Parameters.AddWithValue("@Registered_On", dtRegisteredOn.Value);
                    cmd.Parameters.AddWithValue("@Expires_On", dtExpiresOn.Value);
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

        private void btn_print_Click(object sender, EventArgs e)
        {
            PrintToPdf(txtName.Text, dtBirthDate.Text, cboStatus.Text, txtAddress.Text, txtContactNumber.Text, txtDay.Text, txtMonthYear.Text, dtRegisteredOn.Text, dtExpiresOn.Text);
        }

        private void PrintToPdf(string name, string birth_date, string status, string address, string purpose, string day, string monthyear, string Registered_On, string Expires_On)
        {
            try
            {
                // Use an absolute path or ensure the relative path is correct
                string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"BC Template.pdf");
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string outputPath = Path.Combine(desktopPath, @"Barangay Clearance Template.pdf");
                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                if (!File.Exists(templatePath))
                {
                    MessageBox.Show($"Template file not found at: {templatePath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                {
                    using (PdfReader reader = new PdfReader(templatePath))
                    using (PdfWriter writer = new PdfWriter(outputPath))
                    using (PdfDocument pdfDoc = new PdfDocument(reader, writer))
                    {
                        PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
                        IDictionary<string, PdfFormField> fields = form.GetAllFormFields();
                        string nameFieldName = fields.Keys.FirstOrDefault(k => k.Equals("NameField", StringComparison.OrdinalIgnoreCase));
                        string addressFieldName = fields.Keys.FirstOrDefault(k => k.Equals("AddressField", StringComparison.OrdinalIgnoreCase));
                        string StatusField = fields.Keys.FirstOrDefault(k => k.Equals("StatusField", StringComparison.OrdinalIgnoreCase));
                        string Birth_dateField = fields.Keys.FirstOrDefault(k => k.Equals("Birth_dateField", StringComparison.OrdinalIgnoreCase));
                        string purposeField = fields.Keys.FirstOrDefault(k => k.Equals("purposeField", StringComparison.OrdinalIgnoreCase));
                        string dayFieldName = fields.Keys.FirstOrDefault(k => k.Equals("DayField", StringComparison.OrdinalIgnoreCase));
                        string monthyearFieldName = fields.Keys.FirstOrDefault(k => k.Equals("MonthYearField", StringComparison.OrdinalIgnoreCase));
                        string registerFieldName = fields.Keys.FirstOrDefault(k => k.ToLower() == "registerfield");
                        string expireFieldName = fields.Keys.FirstOrDefault(k => k.ToLower() == "expirefield");

                        if (string.IsNullOrEmpty(nameFieldName) || string.IsNullOrEmpty(addressFieldName) || string.IsNullOrEmpty(StatusField) || string.IsNullOrEmpty(Birth_dateField) || string.IsNullOrEmpty(dayFieldName) || string.IsNullOrEmpty(monthyearFieldName) || string.IsNullOrEmpty(purposeField))
                        {
                            MessageBox.Show("One or more form fields are missing in the template PDF.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        fields[nameFieldName].SetValue(name);
                        fields[addressFieldName].SetValue(address);
                        fields[StatusField].SetValue(status);
                        fields[Birth_dateField].SetValue(birth_date);
                        fields[dayFieldName].SetValue(day);
                        fields[monthyearFieldName].SetValue(monthyear);
                        fields[purposeField].SetValue(purpose);
                        fields[registerFieldName].SetValue(Registered_On);
                        fields[expireFieldName].SetValue(Expires_On);

                        form.FlattenFields();
                    }

                    // Inform the user of successful PDF creation
                    MessageBox.Show("PDF filled and saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            catch (PdfException pdfEx)
            {
                MessageBox.Show($"A PDF error occurred while filling the PDF: {pdfEx.Message}", "PDF Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ioEx)
            {
                MessageBox.Show($"An IO error occurred while filling the PDF: {ioEx.Message}", "IO Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unknown error occurred while filling the PDF: {ex.Message}", "Unknown Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
