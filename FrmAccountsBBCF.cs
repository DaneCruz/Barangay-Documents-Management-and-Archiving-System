using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing.Imaging;
using iText.Kernel.Exceptions;


namespace BARANGAY
{
    public partial class FrmAccountsBBCF : Form
    {
        SQLiteConnection conn;
        SQLiteCommand cmd;
        FormCertifications f; // Changed type to FormCertifications to match the calling form
        public string _ID;
        private FormBP formBP;
        Capture _capture;
        bool _streaming;

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
                    string sql = "INSERT INTO bbcf (Name, address, business_name, business_type, day_of_issuance, monthyear_of_issuance, or_date, amount, administered_by) " +
                                 "VALUES (@Name, @address, @business_name, @business_type, @day_of_issuance, @monthyear_of_issuance, @or_date, @amount, @administered_by)";
                    cmd = new SQLiteCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Name", txtOwnersName.Text);
                    cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@business_name", txtBusinessName.Text);
                    cmd.Parameters.AddWithValue("@business_type", txtBusinessType.Text);
                    cmd.Parameters.AddWithValue("@day_of_issuance", txtDay.Text);
                    cmd.Parameters.AddWithValue("@monthyear_of_issuance", txtMonthYear.Text);
                    cmd.Parameters.AddWithValue("@or_date", dtRegisteredOn.Value);
                    cmd.Parameters.AddWithValue("@amount", txtAmount.Text);
                    cmd.Parameters.AddWithValue("@administered_by", txtAdministeredBy.Text);
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
            txtOwnersName.Clear();
            txtAddress.Clear();
            txtAmount.Clear();
            txtBusinessName.Clear();
            txtBusinessType.Clear();
            txtDay.Clear();
            txtMonthYear.Clear();
            dtRegisteredOn.Value = DateTime.Now;
            txtAdministeredBy.Clear();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            txtOwnersName.Focus();
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
                    string sql = "UPDATE bbcf SET Name=@Name, birth_date=@birth_date, address=@address, Contact_Number=@Contact_Number, date_of_issuance=@date_of_issuance, place_of_issuance=@place_of_issuance, administered_by=@administered_by WHERE id = @ID";
                    cmd = new SQLiteCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Name", txtOwnersName.Text);
                    cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@business_name", txtBusinessName.Text);
                    cmd.Parameters.AddWithValue("@business_type", txtBusinessType.Text);
                    cmd.Parameters.AddWithValue("@day_of_issuance", txtDay.Text);
                    cmd.Parameters.AddWithValue("@monthyear_of_issuance", txtMonthYear.Text);
                    cmd.Parameters.AddWithValue("@or_date", dtRegisteredOn.Value);
                    cmd.Parameters.AddWithValue("@amount", txtAmount.Text);
                    cmd.Parameters.AddWithValue("@administered_by", txtAdministeredBy.Text);
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


        private void btn_print_Click(object sender, EventArgs e)
        {
            PrintToPdf(txtOwnersName.Text, txtAddress.Text);
        }
        private void PrintToPdf(string name, string address)
        {
            try
            {
                // Use an absolute path or ensure the relative path is correct
                string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Template.pdf");
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string outputPath = Path.Combine(desktopPath, @"Certificate of Residency Template.pdf");
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

                using (PdfReader reader = new PdfReader(templatePath))
                using (PdfWriter writer = new PdfWriter(outputPath))
                using (PdfDocument pdfDoc = new PdfDocument(reader, writer))
                {
                    PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
                    IDictionary<string, PdfFormField> fields = form.GetAllFormFields();

                    // Case-insensitive field lookup
                    string nameFieldName = fields.Keys.FirstOrDefault(k => k == "NameField");
                    string addressFieldName = fields.Keys.FirstOrDefault(k => k == "AddressField");
                    string birth_dateFieldName = fields.Keys.FirstOrDefault(k => k == "Birth_dateField");
                    string statusFieldName = fields.Keys.FirstOrDefault(k => k == "StatusField");

                    if (string.IsNullOrEmpty(nameFieldName) || string.IsNullOrEmpty(birth_dateFieldName) || string.IsNullOrEmpty(statusFieldName) || string.IsNullOrEmpty(addressFieldName))
                    {
                        MessageBox.Show("One or more form fields are missing in the template PDF.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Use more specific field setting methods if available
                    fields[nameFieldName].SetValue(name);
                    fields[addressFieldName].SetValue(address);

                    form.FlattenFields();
                }

                MessageBox.Show("PDF filled and saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (PdfException pdfEx)
            {
                // Log the error for debugging
                // Provide more specific error messages based on the exception
                MessageBox.Show($"A PDF error occurred while filling the PDF: {pdfEx.Message}", "PDF Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ioEx)
            {
                // Log the error for debugging
                // Provide more specific error messages based on the exception
                MessageBox.Show($"An IO error occurred while filling the PDF: {ioEx.Message}", "IO Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Log the error for debugging
                // Provide more specific error messages based on the exception
                MessageBox.Show($"An unknown error occurred while filling the PDF: {ex.Message}", "Unknown Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
