﻿using iText.Forms;
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
using System.Drawing.Printing;


namespace BARANGAY
{
    public partial class FrmAccountsBBCF : Form
    {
        SQLiteConnection conn;
        SQLiteCommand cmd;
        FormCertifications f; // Changed type to FormCertifications to match the calling form
        public string _ID;
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
                    string sql = "UPDATE bbcf SET Name=@Name, address=@address, business_name=@business_name, business_type=@business_type, day_of_issuance=@day_of_issuance, monthyear_of_issuance=@monthyear_of_issuance, or_date=@or_date, amount=@amount,  administered_by=@administered_by WHERE id = @ID";
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
            PrintToPdf(txtOwnersName.Text, txtAddress.Text, txtBusinessName.Text, txtBusinessType.Text, txtDay.Text, txtMonthYear.Text, dtRegisteredOn.Text, txtAmount.Text);
        }
        private void PrintToPdf(string name, string address, string businessname, string businesstype, string day, string monthyear, string ordate, string amount)
        {
            try
            {
                // Use an absolute path or ensure the relative path is correct
                string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"BBCF Template.pdf");
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string outputPath = Path.Combine(desktopPath, @"Barangay Business Clearance Template.pdf");
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Ensure the template file exists
                if (!File.Exists(templatePath))
                {
                    MessageBox.Show($"Template file not found at: {templatePath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Show SaveFileDialog to allow user to specify output path and name
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF Files (*.pdf)|*.pdf",
                    Title = "Save PDF File"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    

                    using (PdfReader reader = new PdfReader(templatePath))
                    using (PdfWriter writer = new PdfWriter(outputPath))
                    using (PdfDocument pdfDoc = new PdfDocument(reader, writer))
                    {
                        PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
                        IDictionary<string, PdfFormField> fields = form.GetAllFormFields();

                        // Case-insensitive field lookup
                        string nameFieldName = fields.Keys.FirstOrDefault(k => k.Equals("OwnersNameField", StringComparison.OrdinalIgnoreCase));
                        string addressFieldName = fields.Keys.FirstOrDefault(k => k.Equals("AddressField", StringComparison.OrdinalIgnoreCase));
                        string businessnameFieldName = fields.Keys.FirstOrDefault(k => k.Equals("BusinessNameField", StringComparison.OrdinalIgnoreCase));
                        string businesstypeFieldName = fields.Keys.FirstOrDefault(k => k.Equals("BusinessTypeField", StringComparison.OrdinalIgnoreCase));
                        string dayFieldName = fields.Keys.FirstOrDefault(k => k.Equals("DayField", StringComparison.OrdinalIgnoreCase));
                        string monthyearFieldName = fields.Keys.FirstOrDefault(k => k.Equals("MonthYearField", StringComparison.OrdinalIgnoreCase));
                        string ordateFieldName = fields.Keys.FirstOrDefault(k => k.Equals("OrDateField", StringComparison.OrdinalIgnoreCase));
                        string amountFieldName = fields.Keys.FirstOrDefault(k => k.Equals("AmountField", StringComparison.OrdinalIgnoreCase));

                        if (string.IsNullOrEmpty(nameFieldName) || string.IsNullOrEmpty(addressFieldName) || string.IsNullOrEmpty(businessnameFieldName) || string.IsNullOrEmpty(businesstypeFieldName) || string.IsNullOrEmpty(dayFieldName) || string.IsNullOrEmpty(monthyearFieldName) || string.IsNullOrEmpty(ordateFieldName) || string.IsNullOrEmpty(amountFieldName))
                        {
                            MessageBox.Show("One or more form fields are missing in the template PDF.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Set field values
                        fields[nameFieldName].SetValue(name);
                        fields[addressFieldName].SetValue(address);
                        fields[businessnameFieldName].SetValue(businessname);
                        fields[businesstypeFieldName].SetValue(businesstype);
                        fields[dayFieldName].SetValue(day);
                        fields[monthyearFieldName].SetValue(monthyear);
                        fields[ordateFieldName].SetValue(ordate);
                        fields[amountFieldName].SetValue(amount);

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
