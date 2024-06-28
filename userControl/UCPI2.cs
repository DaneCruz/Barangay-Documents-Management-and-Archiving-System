using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BARANGAY.userControl
{
    public partial class UCPI2 : UserControl
    {
        public UCPI2()
        {
            InitializeComponent();
        }

        private void guna2TextBox5_TextChanged(object sender, EventArgs e)
        {

        }
        private void addUserControl(UserControl userControl, UCPI2 uCPI2)
        {
            userControl.Dock = DockStyle.Fill;
            uCPI2.Controls.Clear();
            uCPI2.Controls.Add(userControl);
            userControl.BringToFront();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FmWebCamera f = new FmWebCamera();
            f.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
