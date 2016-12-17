using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CamStudent
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
             textBox1.Text  =clsFunctions.GetPicPath1;
             textBox2.Text = clsFunctions.GetPicPath2;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            clsFunctions.GetPicPath1 = textBox1.Text;
            clsFunctions.GetPicPath2 = textBox2.Text;

            if (clsFunctions.GetPicPath1 != textBox1.Text || clsFunctions.GetPicPath2 != textBox2.Text)
                MessageBox.Show("Œÿ√ «À‰«¡ «·Õ›Ÿ", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                Close();
        }
    }
}