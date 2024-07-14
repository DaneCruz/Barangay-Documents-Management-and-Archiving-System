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
    public partial class FrmAccounts : Form
    {
        SQLiteConnection conn;
        SQLiteCommand cmd;
        FormID f;
        public string _ID;
        private FormBP formBP;
        Capture _capture;
        bool _streaming;

        public FrmAccounts(FormID f)
        {
            InitializeComponent();
            conn = new SQLiteConnection("Data Source=database.db;Version=3");
            cmd = new SQLiteCommand();
            this.f = f;
            InitializeCamera(); // Initialize camera on form load
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

        public FrmAccounts(FormBP formBP)
        {
            this.formBP = formBP;
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
                    string sql = "INSERT INTO id_card (Name, birth_date, status, address, Guardian, Relationship, Contact_Number, Registered_On, Expires_On, Condition, id_num, image) " +
                                "VALUES (@Name, @birth_date, @status, @address, @Guardian, @Relationship, @Contact_Number, @Registered_On, @Expires_On, @Condition, @id_num, @image)"; // Included id_num and image in column list
                    cmd = new SQLiteCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@birth_date", dtBirthDate.Value);
                    cmd.Parameters.AddWithValue("@status", cboStatus.Text);
                    cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@Guardian", txtGuardian.Text);
                    cmd.Parameters.AddWithValue("@Relationship", txtRelationship.Text);
                    cmd.Parameters.AddWithValue("@Contact_Number", txtContactNumber.Text);
                    cmd.Parameters.AddWithValue("@Registered_On", dtRegisteredOn.Value);
                    cmd.Parameters.AddWithValue("@Expires_On", dtExpiresOn.Value);
                    cmd.Parameters.AddWithValue("@Condition", cboCondition.Text);
                    cmd.Parameters.AddWithValue("@id_num", id_num.Text);
                    cmd.Parameters.AddWithValue("@image", savedImagePath); // Add image path parameter
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
            txtGuardian.Clear();
            txtContactNumber.Clear();
            txtRelationship.Clear();
            cboStatus.SelectedIndex = -1;
            cboCondition.SelectedIndex = -1;
            dtBirthDate.Value = DateTime.Now;
            dtRegisteredOn.Value = DateTime.Now;
            dtExpiresOn.Value = DateTime.Now;
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            id_num.Clear();
            txtName.Focus();
        }

        private void cboCondition_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Do you want to update this record?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conn.Open();
                    string sql = "UPDATE id_card SET Name=@Name, birth_date=@birth_date, status=@status, address=@address, Guardian=@Guardian, Relationship=@Relationship, Contact_Number=@Contact_Number, Registered_On=@Registered_On, Expires_On=@Expires_On, Condition=@Condition, id_num=@id_num, image=@image WHERE id = @ID";
                    cmd = new SQLiteCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@birth_date", dtBirthDate.Value);
                    cmd.Parameters.AddWithValue("@status", cboStatus.Text);
                    cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@Guardian", txtGuardian.Text);
                    cmd.Parameters.AddWithValue("@Relationship", txtRelationship.Text);
                    cmd.Parameters.AddWithValue("@Contact_Number", txtContactNumber.Text);
                    cmd.Parameters.AddWithValue("@Registered_On", dtRegisteredOn.Value);
                    cmd.Parameters.AddWithValue("@Expires_On", dtExpiresOn.Value);
                    cmd.Parameters.AddWithValue("@Condition", cboCondition.Text);
                    cmd.Parameters.AddWithValue("@ID", _ID); // Ensure _ID is assigned correctly
                    cmd.Parameters.AddWithValue("@id_num", id_num.Text);
                    cmd.Parameters.AddWithValue("@image", savedImagePath); // Ensure savedImagePath is set correctly
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Record has been successfully updated!", "Update Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Reset form fields, refresh parent form, and dispose current form
                    clear();    // Reset form fields
                    f.LoadRecord(); // Refresh parent form or data grid
                    this.Dispose(); // Dispose the current form if needed
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            clear();
            this.Close();
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

        private string savedImagePath;

        private void btn_imgSave_Click(object sender, EventArgs e)
        {
            try
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Title = "Save Your Photo",
                    Filter = "JPEG Image|*.jpg",
                    InitialDirectory = @"C:\Users\user\source\repos\Project - Copy\ID Images" // Set your desired folder path here
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (pictureBox3.Image != null)
                    {
                        pictureBox3.Image.Save(saveFileDialog.FileName, ImageFormat.Jpeg); // Save captured image
                        savedImagePath = saveFileDialog.FileName; // Store the path
                        MessageBox.Show("Picture Saved Successfully!");
                    }
                    else
                    {
                        MessageBox.Show("No image to save.", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during image save: {ex.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadImage(string imagePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(imagePath))
                {
                    if (System.IO.File.Exists(imagePath))
                    {
                        pictureBox3.Image = Image.FromFile(imagePath); // Load image into pictureBox3
                    }
                    else
                    {
                        MessageBox.Show("Image file does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Image path is empty or null.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmAccounts_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_capture != null)
            {
                _capture.Dispose(); // Release camera capture resources
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btn_uploadSigntr_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Title = "Select Signature Image",
                    Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var signatureImage = Image.FromFile(openFileDialog.FileName);
                    pictureBox2.Image = signatureImage; // Preview signature image in pictureBox2
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while uploading the signature: {ex.Message}", "Upload Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void id_num_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            PrintToPdf(txtName.Text, txtAddress.Text, txtContactNumber.Text);
        }

        private void PrintToPdf(string name, string address, string Contact_Number)
        {
            try
            {
                // Use an absolute path or ensure the relative path is correct
                string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Template.pdf");
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string outputPath = Path.Combine(desktopPath, @"FilledTemplate.pdf");
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
                    string nameFieldName = fields.Keys.FirstOrDefault(k => k.ToLower() == "namefield");
                    string addressFieldName = fields.Keys.FirstOrDefault(k => k.ToLower() == "addressfield");
                    string contactFieldName = fields.Keys.FirstOrDefault(k => k.ToLower() == "contactnumberfield");

                    if (string.IsNullOrEmpty(nameFieldName) || string.IsNullOrEmpty(addressFieldName) || string.IsNullOrEmpty(contactFieldName))
                    {
                        MessageBox.Show("One or more form fields are missing in the template PDF.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Use more specific field setting methods if available
                    fields[nameFieldName].SetValue(name);
                    fields[addressFieldName].SetValue(address);
                    fields[contactFieldName].SetValue(Contact_Number);

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
