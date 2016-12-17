using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CamStudent
{
    public partial class frmPicExist : Form
    {
        private string _ImagePath;
        public frmPicExist(string ImagePath1,string ImagePath2,string std_No)
        {
            InitializeComponent();
            try
            {
                label2.Text = label2.Text.Replace("%NO%", std_No);
                try
                {
                    pictureBox1.Load(ImagePath1);
                    _ImagePath = ImagePath1;
                }
                catch
                {
                    pictureBox1.Load(ImagePath2);
                    _ImagePath = ImagePath2;
                }
            }
            catch
            {

            }
        }

        private void frmPicExist_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //WebCam_Capture.WebCamCapture.DisplayFileProperties(_ImagePath);            
        }
    }
}