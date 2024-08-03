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
using iText.IO.Image;
using iText.Layout;
using iText.Layout.Element;
using System.Xml.Linq;

namespace BARANGAY
{
    public partial class FrmAccountsBRIC : Form
    {
        SQLiteConnection conn;
        SQLiteCommand cmd;
        FormBRIC f;
        public string _ID;
        Capture _capture;
        bool _streaming;

        public FrmAccountsBRIC(FormBRIC f)
        {
            InitializeComponent();
            conn = new SQLiteConnection("Data Source=database.db;Version=3");
            cmd = new SQLiteCommand();
            // Initialize the FormBRIC object
            this.f = f; // Corrected to use the passed-in form
        }
        
        private void Streaming(object sender, EventArgs e)
        {
            try
            {
                var frame = _capture.QueryFrame()?.ToImage<Bgr, byte>();
                if (frame != null)
                {
                    var bmp = frame.Bitmap;
                    pictureBox1.Image = bmp; // Display frame in pictureBox1
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during streaming: {ex.Message}", "Streaming Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public FrmAccountsBRIC()
        {
            InitializeComponent(); // Added to initialize components
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
                    string sql = "INSERT INTO residency (name, birth_date, status, address, Condition, issued, valid_until,administered_by, reference, start_year) " +
                                 "VALUES (@name, @birth_date, @status, @address, @Condition, @issued, @valid_until, @administered_by, @reference, @start_year)";
                    cmd = new SQLiteCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@birth_date", dtBirthDate.Value);
                    cmd.Parameters.AddWithValue("@status", cboStatus.Text);
                    cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@Condition", cboCondition.Text);
                    cmd.Parameters.AddWithValue("@start_year", txtResidency.Text);
                    cmd.Parameters.AddWithValue("issued", dtIssued.Value);
                    cmd.Parameters.AddWithValue("valid_until", dtValidUntil.Value);
                    cmd.Parameters.AddWithValue("@administered_by", txtAdministeredBy.Text);
                    cmd.Parameters.AddWithValue("@reference", txtReference.Text);
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
            cboStatus.SelectedIndex = 0;
            cboCondition.SelectedIndex = 0;
            dtBirthDate.Value = DateTime.Now;
            dtIssued.Value = DateTime.Now;
            dtValidUntil.Value = DateTime.Now;
            txtAdministeredBy.Clear();
            txtReference.Clear();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            pictureBox3.Image = null;
            txtName.Focus();
        }

        private void cboCondition_KeyPress(object sender, KeyPressEventArgs e)
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
                    string sql = "UPDATE residency SET name=@name, birth_date=@birth_date, status=@status, address=@address, Condition=@Condition, issued=@issued, valid_until=@valid_until, administered_by=@administered_by, start_year=@start_year, reference=@reference WHERE id = @ID";
                    cmd = new SQLiteCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@birth_date", dtBirthDate.Value);
                    cmd.Parameters.AddWithValue("@status", cboStatus.Text);
                    cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@Condition", cboCondition.Text);
                    cmd.Parameters.AddWithValue("@start_year", txtResidency.Text);
                    cmd.Parameters.AddWithValue("@issued", dtIssued.Value);
                    cmd.Parameters.AddWithValue("@valid_until", dtValidUntil.Value);
                    cmd.Parameters.AddWithValue("@administered_by", txtAdministeredBy.Text);
                    cmd.Parameters.AddWithValue("@reference", txtReference.Text);
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

        private void cboStatus_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btn_captureImg_Click(object sender, EventArgs e)
        {
            try
            {
                var frame = _capture.QueryFrame()?.ToImage<Bgr, byte>();
                if (frame != null)
                {
                    var bmp = frame.Bitmap;
                    pictureBox3.Image = bmp;
                    MessageBox.Show("Image captured successfully!", "Capture Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to capture image. Please try again.", "Capture Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during image capture: {ex.Message}", "Capture Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmAccounts_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_capture != null)
            {
                _capture.Dispose(); // Release camera capture resources
            }
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            PrintToPdf(txtName.Text, txtAddress.Text, dtBirthDate.Text, cboStatus.Text, dtIssued.Text, dtIssued.Text, dtValidUntil.Text, txtAdministeredBy.Text , txtReference.Text , txtResidency.Text, pictureBox3.Image);
        }
        private void AddImageToPdf(byte[] imageBytes, Document document, float x, float y, float width, float height)
        {
            ImageData imageData = ImageDataFactory.Create(imageBytes);
            iText.Layout.Element.Image pdfImage = new iText.Layout.Element.Image(imageData)
                .SetFixedPosition(x, y)
                .ScaleToFit(width, height);
            // Add the image to the document
            document.Add(pdfImage);
        }
        private void PrintToPdf(string name, string address, string birth_date, string status, string issued, string issued1, string ValidUntil, string administered_by, string reference, string start_year, System.Drawing.Image image)
        {
            try
            {
                // Use an absolute path or ensure the relative path is correct
                string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Certificate_Of_Residency_Template.pdf");

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
                    using (Document document = new Document(pdfDoc))
                    {
                        PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
                        IDictionary<string, PdfFormField> fields = form.GetAllFormFields();

                        // Case-insensitive field lookup
                        string paragraph1 = fields.Keys.FirstOrDefault(k => k == "ParagraphField");
                        string issuedFieldName = fields.Keys.FirstOrDefault(k => k == "IssuedField");
                        string issued1FieldName = fields.Keys.FirstOrDefault(k => k == "Issued1Field");
                        string validuntilFieldName = fields.Keys.FirstOrDefault(k => k == "ValidUntilField");
                        string administered_by_FieldName = fields.Keys.FirstOrDefault(k => k == "Administered_By_Field");
                        string referenceFieldName = fields.Keys.FirstOrDefault(k => k == "ReferenceField");

                        if (string.IsNullOrEmpty(paragraph1) || string.IsNullOrEmpty(issuedFieldName) || string.IsNullOrEmpty(issued1FieldName) || string.IsNullOrEmpty(validuntilFieldName) || string.IsNullOrEmpty(administered_by_FieldName) || string.IsNullOrEmpty(referenceFieldName))
                        {
                            MessageBox.Show("One or more form fields are missing in the template PDF.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Use more specific field setting methods if available
                        fields[paragraph1].SetValue("    " + "This is to certify that " + name + ", born on " + birth_date + ", " + status + ", is a resident of " + address + ", DISTRICT IV, Quezon City since " + start_year );
                        fields[issuedFieldName].SetValue("Issued this day " + issued + " " + " at Barangay Krus Na Ligas, District IV, Quezon City.");
                        fields[issued1FieldName].SetValue(issued);
                        fields[validuntilFieldName].SetValue(ValidUntil);
                        fields[referenceFieldName].SetValue(reference);
                        fields[administered_by_FieldName].SetValue(administered_by);

                        if (image != null)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                                byte[] imageBytes = memoryStream.ToArray();

                                // Adjust the x, y, width, and height values as needed
                                float x = 60;  // X position
                                float y = 213; // Y position
                                float width = 105; // Width of the image
                                float height = 300; // Height of the image

                                // Add image to PDF
                                AddImageToPdf(imageBytes, document, x, y, width, height);
                            }
                        }
                        else
                        {
                            MessageBox.Show("No image", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        form.FlattenFields();
                    }

                    // Inform the user of successful PDF creation
                    MessageBox.Show("PDF filled and saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Open the PDF with the default PDF reader
                    try
                    {
                        System.Diagnostics.Process.Start(outputPath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error opening PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
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

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            clear();
            this.Close();
        }

        private void dtIssued_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void StopCamera()
        {
            if (_capture != null)
            {
                Application.Idle -= Streaming;
                _capture.Dispose();
                _capture = null;
                _streaming = false;
            }
        }
        private void InitializeCamera()
        {
            try
            {
                _capture = new Capture(); // Initialize camera capture
                if (_capture == null || _capture.Ptr == IntPtr.Zero)
                {
                    MessageBox.Show("Failed to open webcam. Please make sure it is connected and try again.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Application.Idle += Streaming; // Start streaming frames
                _streaming = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing webcam capture: {ex.Message}", "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_openclose_Click_1(object sender, EventArgs e)
        {
            if (_streaming)
            {
                StopCamera();
                pictureBox1.Image = null;
            }
            else
            {
                InitializeCamera();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox3.Image = null;
        }

        private void dtIssued_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dtValidUntil_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtAdministeredBy_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
