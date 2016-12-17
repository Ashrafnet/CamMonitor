using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CamMonitor;

namespace CamStudent
{
    public partial class frmLogin : Form
    {

        
        public frmLogin()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LogMeIn();
        }
         void LogMeIn()
        {
            clsDB.UserName = textBox1.Text;
            clsDB.Password = textBox2.Text;
            clsDB.ServerName = textBox3.Text;
            string SQl = "select sysdate from dual";
            try
            {
                
                string  x= clsDB.ExcuteReturnedSQLByReader(SQl);
                if (x.Length > 0)
                {
                    Hide();
                    frmMain fr= new frmMain();
                    fr.ShowDialog();
                    Close();
                }

                else
                {
                    MessageBox.Show("Œÿ√ ›Ì ﬂ·„… «·„—Ê— «Ê «”„ «·„” Œœ„", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox2.Focus();
                    textBox2.SelectAll(); return;
                }
                    

            }
            catch(Exception er)
            {
                MessageBox.Show(er.Message );
                //Hide();
                //frmMain fr = new frmMain();
                //fr.ShowDialog();
                //Close();
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                LogMeIn();
        }
       
    }
}