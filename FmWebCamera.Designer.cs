namespace BARANGAY
{
    partial class FmWebCamera
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.pictureCapture = new System.Windows.Forms.PictureBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button_quit = new System.Windows.Forms.Button();
            this.button_save = new System.Windows.Forms.Button();
            this.button_capture = new System.Windows.Forms.Button();
            this.stream_onoff = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.picturePreview = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureCapture)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturePreview)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(27, 116);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(1191, 551);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.pictureCapture);
            this.groupBox4.Location = new System.Drawing.Point(744, 46);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(422, 485);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "CAPTURED";
            // 
            // pictureCapture
            // 
            this.pictureCapture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureCapture.Location = new System.Drawing.Point(15, 27);
            this.pictureCapture.Margin = new System.Windows.Forms.Padding(4);
            this.pictureCapture.Name = "pictureCapture";
            this.pictureCapture.Size = new System.Drawing.Size(390, 446);
            this.pictureCapture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureCapture.TabIndex = 0;
            this.pictureCapture.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button_quit);
            this.groupBox3.Controls.Add(this.button_save);
            this.groupBox3.Controls.Add(this.button_capture);
            this.groupBox3.Controls.Add(this.stream_onoff);
            this.groupBox3.Location = new System.Drawing.Point(485, 46);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(222, 485);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            // 
            // button_quit
            // 
            this.button_quit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_quit.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_quit.Location = new System.Drawing.Point(26, 367);
            this.button_quit.Margin = new System.Windows.Forms.Padding(4);
            this.button_quit.Name = "button_quit";
            this.button_quit.Size = new System.Drawing.Size(159, 81);
            this.button_quit.TabIndex = 3;
            this.button_quit.Text = "Quit";
            this.button_quit.UseVisualStyleBackColor = true;
            this.button_quit.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // button_save
            // 
            this.button_save.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_save.Location = new System.Drawing.Point(26, 256);
            this.button_save.Margin = new System.Windows.Forms.Padding(4);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(159, 81);
            this.button_save.TabIndex = 2;
            this.button_save.Text = "Save";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click_1);
            // 
            // button_capture
            // 
            this.button_capture.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_capture.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_capture.Location = new System.Drawing.Point(26, 148);
            this.button_capture.Margin = new System.Windows.Forms.Padding(4);
            this.button_capture.Name = "button_capture";
            this.button_capture.Size = new System.Drawing.Size(159, 81);
            this.button_capture.TabIndex = 1;
            this.button_capture.Text = "Capture";
            this.button_capture.UseVisualStyleBackColor = true;
            this.button_capture.Click += new System.EventHandler(this.button_capture_Click);
            // 
            // stream_onoff
            // 
            this.stream_onoff.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stream_onoff.Location = new System.Drawing.Point(26, 38);
            this.stream_onoff.Margin = new System.Windows.Forms.Padding(4);
            this.stream_onoff.Name = "stream_onoff";
            this.stream_onoff.Size = new System.Drawing.Size(159, 81);
            this.stream_onoff.TabIndex = 0;
            this.stream_onoff.Text = "Stream on/off";
            this.stream_onoff.UseVisualStyleBackColor = true;
            this.stream_onoff.Click += new System.EventHandler(this.stream_onoff_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.picturePreview);
            this.groupBox2.Location = new System.Drawing.Point(24, 46);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(422, 485);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "STREAMING";
            // 
            // picturePreview
            // 
            this.picturePreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picturePreview.Location = new System.Drawing.Point(14, 23);
            this.picturePreview.Margin = new System.Windows.Forms.Padding(4);
            this.picturePreview.Name = "picturePreview";
            this.picturePreview.Size = new System.Drawing.Size(390, 446);
            this.picturePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picturePreview.TabIndex = 0;
            this.picturePreview.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(467, 57);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(210, 33);
            this.label1.TabIndex = 3;
            this.label1.Text = "Capture Photo";
            // 
            // FmWebCamera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1245, 724);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FmWebCamera";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Captured Photo";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FmWebCamera_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureCapture)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picturePreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.PictureBox pictureCapture;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button_quit;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.Button button_capture;
        private System.Windows.Forms.Button stream_onoff;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox picturePreview;
    }
}