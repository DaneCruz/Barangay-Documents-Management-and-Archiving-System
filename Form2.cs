using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV.Structure;
using Emgu.CV;

namespace BARANGAY
{
    public partial class FmWebCamera : Form
    {
        private bool _streaming;
        private Capture _capture;

        public FmWebCamera()
        {
            InitializeComponent();
        }

        private void FmWebCamera_Load(object sender, EventArgs e)
        {
            _streaming = false;
            try
            {
                // Initialize capture device with default camera (index 0)
                _capture = new Capture();

                if (_capture != null && _capture.Ptr != IntPtr.Zero) // Check if capture object is valid
                {
                    Application.Idle += streaming; // Start streaming frames
                }
                else
                {
                    MessageBox.Show("Failed to open webcam. Please make sure it is connected and try again.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing webcam capture: {ex.Message}", "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void streaming(object sender, EventArgs e)
        {
            try
            {
                // Capture a frame from the webcam
                using (var frame = _capture.QueryFrame()?.ToImage<Bgr, byte>())
                {
                    if (frame != null)
                    {
                        // Convert the frame to a Bitmap for display
                        var bmp = frame.Bitmap;

                        // Update PictureBox on UI thread
                        if (picturePreview.InvokeRequired)
                        {
                            picturePreview.Invoke(new Action(() => picturePreview.Image = bmp));
                        }
                        else
                        {
                            picturePreview.Image = bmp;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during streaming: {ex.Message}", "Streaming Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!_streaming)
            {
                try
                {
                    Application.Idle += streaming; // Start streaming
                    button1.Text = "Stop Streaming";
                    button1.ForeColor = Color.White;
                    button1.BackColor = Color.Red;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error starting streaming: {ex.Message}", "Streaming Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                Application.Idle -= streaming; // Stop streaming
                button1.Text = "Start Streaming";
                button1.ForeColor = Color.Black;
                button1.BackColor = Color.Gainsboro;
            }
            _streaming = !_streaming;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            picturePreview.Image = pictureCapture.Image;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Save Your Photo",
                Filter = "JPEG Image|*.jpg"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Save the image from the picture box
                if (pictureCapture.Image != null)
                {
                    pictureCapture.Image.Save(saveFileDialog.FileName + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    MessageBox.Show("Picture Saved Successfully!");
                }
                else
                {
                    MessageBox.Show("No image to save.", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Clean up resources when the form is closing
            if (_capture != null)
            {
                _capture.Dispose(); // Release the webcam
            }
            base.OnFormClosing(e);
        }
    }
}