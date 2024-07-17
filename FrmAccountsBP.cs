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
using iText.Kernel.Exceptions;
using iText.Kernel.Pdf;
using System.Drawing.Printing;
using System.IO;


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
                    string sql = "INSERT INTO business_permit (business_name, business_type, business_address, business_owner) " +
                                 "VALUES (@business_name, @business_type, @business_address, @business_owner)";
                    cmd = new SQLiteCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@business_name", txtName.Text);
                    cmd.Parameters.AddWithValue("@business_type", txtBusinessType.Text);
                    cmd.Parameters.AddWithValue("@business_address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@business_owner", txtOwnerName.Text);
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
            txtOwnerName.Clear();
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
                    string sql = "UPDATE business_permit SET Business_Name=@business_name, business_type=@business_type, business_address=@business_address, busienss_owner=@business_owner WHERE id = @ID";
                    cmd = new SQLiteCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@business_name", txtName.Text);
                    cmd.Parameters.AddWithValue("@business_type", txtBusinessType.Text);
                    cmd.Parameters.AddWithValue("@business_address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@business_owner", txtOwnerName.Text);
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

        private void button1_Click(object sender, EventArgs e)
        {
            PrintToPdf(txtName.Text, txtAddress.Text, txtBusinessType.Text, txtOwnerName.Text);
        }
        private void PrintToPdf(string business_name, string business_address, string business_type, string business_owner )
        {
            try
            {
                // Use an absolute path or ensure the relative path is correct
                string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Template.pdf");

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
                    string outputPath = saveFileDialog.FileName;

                    using (PdfReader reader = new PdfReader(templatePath))
                    using (PdfWriter writer = new PdfWriter(outputPath))
                    using (PdfDocument pdfDoc = new PdfDocument(reader, writer))
                    {
                        PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
                        IDictionary<string, PdfFormField> fields = form.GetAllFormFields();

                        // Case-insensitive field lookup
                        string businessnameFieldName = fields.Keys.FirstOrDefault(k => k.Equals("BusinessNameField", StringComparison.OrdinalIgnoreCase));
                        string businesstypeFieldName = fields.Keys.FirstOrDefault(k => k.Equals("BusinessTypeField", StringComparison.OrdinalIgnoreCase));
                        string addressFieldName = fields.Keys.FirstOrDefault(k => k.Equals("AddressField", StringComparison.OrdinalIgnoreCase));
                        string ownerFieldName = fields.Keys.FirstOrDefault(k => k.Equals("OwnerField", StringComparison.OrdinalIgnoreCase));

                        if (string.IsNullOrEmpty(businessnameFieldName) || string.IsNullOrEmpty(addressFieldName) || string.IsNullOrEmpty(businesstypeFieldName) || string.IsNullOrEmpty(ownerFieldName))
                        {
                            MessageBox.Show("One or more form fields are missing in the template PDF.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Set field values
                        fields[ownerFieldName].SetValue(business_owner);
                        fields[addressFieldName].SetValue(business_address);
                        fields[businessnameFieldName].SetValue(business_name);
                        fields[businesstypeFieldName].SetValue(business_type);

                        form.FlattenFields();
                    }

                    // Inform the user of successful PDF creation
                    MessageBox.Show("PDF filled and saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Open the PDF in print mode
                    System.Diagnostics.Process printProcess = new System.Diagnostics.Process();
                    printProcess.StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = outputPath,
                        UseShellExecute = true,
                        Verb = "print"
                    };
                    printProcess.Start();
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
