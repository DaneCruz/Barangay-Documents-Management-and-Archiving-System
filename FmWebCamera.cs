using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

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

                if (_capture == null || _capture.Ptr == IntPtr.Zero) // Check if capture object is valid
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
                var frame = _capture.QueryFrame()?.ToImage<Bgr, byte>();
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
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during streaming: {ex.Message}", "Streaming Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void stream_onoff_Click(object sender, EventArgs e)
        {
            if (!_streaming)
            {
                try
                {
                    Application.Idle += streaming; // Start streaming
                    stream_onoff.Text = "Stop Streaming";
                    stream_onoff.ForeColor = Color.White;
                    stream_onoff.BackColor = Color.Red;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error starting streaming: {ex.Message}", "Streaming Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                Application.Idle -= streaming; // Stop streaming
                stream_onoff.Text = "Start Streaming";
                stream_onoff.ForeColor = Color.Black;
                stream_onoff.BackColor = Color.Gainsboro;
            }
            _streaming = !_streaming;
        }

        private void button_capture_Click(object sender, EventArgs e)
        {
            try
            {
                using (var frame = _capture.QueryFrame()?.ToImage<Bgr, byte>())
                {
                    if (frame != null)
                    {
                        var bmp = frame.Bitmap;
                        pictureCapture.Image = bmp;
                    }
                    else
                    {
                        MessageBox.Show("Failed to capture image. Please try again.", "Capture Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during image capture: {ex.Message}", "Capture Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void button4_Click_1(object sender, EventArgs e)
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

        private void button_save_Click_1(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Save Your Photo",
                Filter = "JPEG Image|*.jpg",
                InitialDirectory = @"C:\Users\user\source\repos\Project - Copy\ID Images" // Set your desired folder path here
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Save the image from the picture box
                if (pictureCapture.Image != null)
                {
                    pictureCapture.Image.Save(saveFileDialog.FileName, ImageFormat.Jpeg);
                    MessageBox.Show("Picture Saved Successfully!");
                }
                else
                {
                    MessageBox.Show("No image to save.", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}