using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace CamStudent
{
    
    public partial class frmSave : Form
    {
        private Image MyImge;
        public frmSave()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string P1 = clsFunctions.GetPicPath1 + @"\";
                string P2 = clsFunctions.GetPicPath2 + @"\";

                

                P1 += textBox1.Text + ".jpg";
                P2 += textBox1.Text + ".jpg";
                if (System.IO.File.Exists(P1) || System.IO.File.Exists(P2))
                {
                    frmPicExist pic = new frmPicExist(P1,P2, textBox1.Text); 
                    DialogResult dr= pic.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        pictureBox1.Image.Save(P1, System.Drawing.Imaging.ImageFormat.Jpeg);
                        pictureBox1.Image.Save(P2, System.Drawing.Imaging.ImageFormat.Jpeg);
                        SetFlag();
                        Close();

                    }
                    return;
                }
                pictureBox1.Image.Save(P1, System.Drawing.Imaging.ImageFormat.Jpeg);
                pictureBox1.Image.Save(P2, System.Drawing.Imaging.ImageFormat.Jpeg);
                SetFlag();
                Close();
            }
            catch (Exception er)
            {
                MessageBox.Show("Œÿ√ «À‰«¡ Õ›Ÿ «·’Ê—… ﬁœ ·«  ﬂÊ‰ ·ﬂ ’·«ÕÌ… «÷«›… ’Ê— «·Ï „Ã·œ ’Ê— «·ÿ·»…" + "\n" + er.Message , "Œÿ√", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
        }

        void SetFlag()
        {
            string Sql = "update student_master set NO_PRINTED_CARD=-1 where STUDENT_NO="+textBox1.Text ;
            try
            {
                clsDB.ExcuteNonSQL(Sql);
            }
            catch { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GetInfo();
        }

        void GetInfo()
        {
            string Sql = "select STUDENT_A_NAME from  student_master m where m.STUDENT_NO=" + textBox1.Text;
            try
            {
                string tmp="";
                string  x= clsDB.ExcuteReturnedSQLByReader(Sql);
                if (x.Length  < 1)
                {
                    button1.Enabled = false ;
                    textBox2.Text = "·« ÌÊÃœ ÿ«·» »Â–« «·—ﬁ„";
                    textBox2.ForeColor = Color.Red;
                    textBox1.Focus();
                    textBox1.SelectAll();

                    return;
                }
                else
                {
                    tmp += x +"\n";
                    //tmp += x.Rows[0]["AFF_REQUEST_DATE"] + "";
                    textBox2.Text = tmp;
                    textBox2.ForeColor = Color.Green ;
                    button1.Enabled = true;
                    button1.Focus();
                }
            }
            catch
            {
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                GetInfo();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                Bitmap x = new Bitmap(MyImge);
                AdjustBrightnessMatrix(x, trackBar1.Value);
                pictureBox1.Image = x;
            }
            catch { }
        }
        public static void AdjustBrightnessMatrix(Bitmap img, int value)
        {
            if (value == 0) // No change, so just return
                return;

            float sb = (float)value / 255F;
            float[][] colorMatrixElements =
                  {
                        new float[] {1,  0,  0,  0, 0},
                        new float[] {0,  1,  0,  0, 0},
                        new float[] {0,  0,  1,  0, 0},
                        new float[] {0,  0,  0,  1, 0},
                        new float[] {sb, sb, sb, 1, 1}

                  };
            ColorMatrix cm = new ColorMatrix(colorMatrixElements);
            ImageAttributes imgattr = new ImageAttributes();
            Rectangle rc = new Rectangle(0, 0, img.Width, img.Height);
            Graphics g = Graphics.FromImage(img);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            imgattr.SetColorMatrix(cm);
            g.DrawImage(img, rc, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgattr);

            //Clean everything up
            imgattr.Dispose();
            g.Dispose();

        }

        private void frmSave_Load(object sender, EventArgs e)
        {
            MyImge = pictureBox1.Image;
        }


    }
}