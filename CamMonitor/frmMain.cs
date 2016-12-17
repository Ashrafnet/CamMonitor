using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using CamStudent;

namespace CamMonitor
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            if (Settings.Cameras.GetCameras==null )
            {
                frmCameraNew x = new frmCameraNew();
                x.ShowDialog();
                //return;
            }
            ctrCamView cc = new ctrCamView("Local");

            Controls.Add(cc);
        }
        void LoadSmallImage()
        {

            try
            {

                Bitmap bit = ((ctrCamView)Controls[7]).LastFrame  ;
                int w, h;
                w = pictureBox2.Width; h = pictureBox2.Height;

                bit = bit.Clone(new Rectangle(new Point(int.Parse(numericUpDown1.Value+""),int.Parse( numericUpDown2.Value+"") ), new Size(w, h - 1)), PixelFormat.Format24bppRgb);
                pictureBox2.Image = bit;

                //Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                //Image myThumbnail = bit.GetThumbnailImage(207, 231, myCallback, IntPtr.Zero);
                //pictureBox2.Image = myThumbnail;
            }
            catch (Exception er)
            {
                string ers = er.Message;
            }
        }
        public bool ThumbnailCallback()
        {
            return true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            LoadSmallImage();
            Settings.General.CutLeft =int.Parse( numericUpDown1.Value.ToString());
            Settings.General.CutTop =int.Parse( numericUpDown2.Value.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmSave x = new frmSave();
            //LoadSmallImage();
            x.pictureBox1.Image = pictureBox2.Image;
            x.ShowDialog();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
              numericUpDown1.Value=decimal.Parse( Settings.General.CutLeft.ToString());
              numericUpDown2.Value =decimal.Parse( Settings.General.CutTop.ToString());
        }
    }
}