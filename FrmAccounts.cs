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

namespace BARANGAY
{
    public partial class FrmAccounts : Form
    {
        SQLiteConnection conn;
        SQLiteCommand cmd;
        FormID f;
        public string _ID;
        Capture _capture;
        bool _streaming;

        public FrmAccounts(FormID f)
        {
            InitializeComponent();
            conn = new SQLiteConnection("Data Source=database.db;Version=3");
            cmd = new SQLiteCommand();
            this.f = f;
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
                    string sql = "INSERT INTO id_card (last_name, first_name, middle_name, birth_date, status, address, Guardian, Relationship, Contact_Number, Registered_On, Expires_On, Condition, id_num) " +
                                "VALUES (@last_name, @first_name, @middle_name, @birth_date, @status, @address, @Guardian, @Relationship, @Contact_Number, @Registered_On, @Expires_On, @Condition, @id_num)"; // Included id_num and image in column list
                    cmd = new SQLiteCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@last_name", txtLastName.Text);
                    cmd.Parameters.AddWithValue("@first_name", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("@middle_name", txtMiddleName.Text);
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
            txtLastName.Clear();
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
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            txtLastName.Focus();
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

                    // Verify the existence of the record before attempting to update
                    string checkSql = "SELECT COUNT(*) FROM id_card WHERE id = @id";
                    cmd = new SQLiteCommand(checkSql, conn);
                    cmd.Parameters.AddWithValue("@id", _ID);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count == 0)
                    {
                        MessageBox.Show("Record not found. Please check the ID and try again.", "Record Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        conn.Close();
                        return;
                    }

                    string sql = "UPDATE id_card SET " +
                                 "last_name=@last_name, first_name=@first_name, middle_name=@middle_name, " +
                                 "birth_date=@birth_date, status=@status, address=@address, " +
                                 "Guardian=@Guardian, Relationship=@Relationship, Contact_Number=@Contact_Number, " +
                                 "Registered_On=@Registered_On, Expires_On=@Expires_On, Condition=@Condition, " +
                                 "id_num=@id_num" +
                                 "WHERE id = @id";

                    cmd = new SQLiteCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", _ID);
                    cmd.Parameters.AddWithValue("@last_name", txtLastName.Text);
                    cmd.Parameters.AddWithValue("@first_name", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("@middle_name", txtMiddleName.Text);
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

                    cmd.ExecuteNonQuery();
                    conn.Close();

                    MessageBox.Show("Record has been successfully updated!", "Update Record", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reset form fields, refresh parent form, and dispose current form
                    clear();
                    f.LoadRecord();
                    this.Dispose();
                }
            }
            catch (SQLiteException sqlEx)
            {
                MessageBox.Show($"SQLite error: {sqlEx.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
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
                    MessageBox.Show("Failed to capture image. Pleaseimag try again.", "Capture Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    var signatureImage = System.Drawing.Image.FromFile(openFileDialog.FileName);
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
            PrintToPdf(txtLastName.Text, txtFirstName.Text, txtMiddleName.Text, dtBirthDate.Text, cboStatus.Text, txtAddress.Text, id_num.Text, txtGuardian.Text, txtRelationship.Text, txtAddress.Text, txtContactNumber.Text, dtRegisteredOn.Text, dtExpiresOn.Text, pictureBox3.Image);
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

        private void PrintToPdf(string last_name, string first_name, string middle_name, string birth_date, string status, string address, string id_num, string Guardian, string Relationship, string address1, string Contact_Number, string Registered_On, string Expires_On, System.Drawing.Image image)
        {
            try
            {
                // Use an absolute path or ensure the relative path is correct
                string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"ID Template.pdf");

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
                        string lastnameFieldName = fields.Keys.FirstOrDefault(k => k.ToLower() == "lastnamefield");
                        string firstnameFieldName = fields.Keys.FirstOrDefault(k => k.ToLower() == "firstnamefield");
                        string middlenameFieldName = fields.Keys.FirstOrDefault(k => k.ToLower() == "middlenamefield");
                        string birthdateFieldName = fields.Keys.FirstOrDefault(k => k.ToLower() == "birthdatefield");
                        string statusFieldName = fields.Keys.FirstOrDefault(k => k.ToLower() == "statusfield");
                        string addressFieldName = fields.Keys.FirstOrDefault(k => k.ToLower() == "addressfield");
                        string idnumFieldName = fields.Keys.FirstOrDefault(k => k.ToLower() == "idnumfield");
                        string guardianFieldName = fields.Keys.FirstOrDefault(k => k.ToLower() == "guardianfield");
                        string relationshipFieldName = fields.Keys.FirstOrDefault(k => k.ToLower() == "relationshipfield");
                        string address1FieldName = fields.Keys.FirstOrDefault(k => k.ToLower() == "addressfield1");
                        string contactFieldName = fields.Keys.FirstOrDefault(k => k.ToLower() == "contactnumberfield");
                        string registerFieldName = fields.Keys.FirstOrDefault(k => k.ToLower() == "registerfield");
                        string expireFieldName = fields.Keys.FirstOrDefault(k => k.ToLower() == "expirefield");

                        if (string.IsNullOrEmpty(lastnameFieldName) ||
                           string.IsNullOrEmpty(firstnameFieldName) ||
                           string.IsNullOrEmpty(middlenameFieldName) ||
                           string.IsNullOrEmpty(birthdateFieldName) ||
                           string.IsNullOrEmpty(statusFieldName) ||
                           string.IsNullOrEmpty(addressFieldName) ||
                           string.IsNullOrEmpty(guardianFieldName) ||
                           string.IsNullOrEmpty(relationshipFieldName) ||
                           string.IsNullOrEmpty(idnumFieldName) ||
                           string.IsNullOrEmpty(address1FieldName) ||
                           string.IsNullOrEmpty(contactFieldName) ||
                           string.IsNullOrEmpty(registerFieldName) ||
                           string.IsNullOrEmpty(expireFieldName))
                        {
                            MessageBox.Show("One or more form fields are missing in the template PDF.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Set the values for the fields
                        fields[lastnameFieldName].SetValue(last_name);
                        fields[firstnameFieldName].SetValue(first_name);
                        fields[middlenameFieldName].SetValue(middle_name);
                        fields[birthdateFieldName].SetValue(birth_date);
                        fields[statusFieldName].SetValue(status);
                        fields[addressFieldName].SetValue(address);
                        fields[guardianFieldName].SetValue(Guardian);
                        fields[relationshipFieldName].SetValue(Relationship);
                        fields[address1FieldName].SetValue(address1);
                        fields[contactFieldName].SetValue(Contact_Number);
                        fields[registerFieldName].SetValue(Registered_On);
                        fields[expireFieldName].SetValue(Expires_On);
                        fields[idnumFieldName].SetValue(id_num);

                        if (image != null)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                                byte[] imageBytes = memoryStream.ToArray();

                                // Adjust the x, y, width, and height values as needed
                                float x = 15;  // X position
                                float y = 45; // Y position
                                float width = 52; // Width of the image
                                float height = 175; // Height of the image

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


        private void btn_openclose_Click(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox3.Image = null;
        }
    }
}