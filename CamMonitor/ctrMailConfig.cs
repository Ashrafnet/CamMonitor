using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace CamMonitor
{
    public partial class ctrMailConfig : UserControl
    {
        public ctrMailConfig()
        {
            InitializeComponent();
            
        }

        public ctrMailConfig(bool LoadInForm)
        {
            InitializeComponent();
            if (LoadInForm)
            {
                button1.Visible = true;
                button2.Visible = true;
            }
            

        }

        private void btnTestSmtp_Click(object sender, EventArgs e)
        {
           int x= clsMail.TestMailSettings(txtMailServer.Text ,int.Parse(txtMailServerPort.Text ),txtMailUser.Text ,txtMailPassword.Text , txtMailSender.Text , txtMailRecipient.Text , "Testing E-Mail Settings", "<html><body><h2>This Message from CamMonitor -UCAS.<br>If you see this message it Means Test was OK. </H2></body></html>", "", "");
           if (x > 0)
               MessageBox.Show("Successfully Sent Mail", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
               MessageBox.Show("Error While Sending Mail.\n\nNote:\n\tTry Change your Settings.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public bool SaveConfig()
        {
            try
            {
                Settings.MailConfig.MailFrom = txtMailSender.Text;
                Settings.MailConfig.MailTo = txtMailRecipient.Text;
                Settings.MailConfig.ServerName = txtMailServer.Text;
                Settings.MailConfig.PortNo = int.Parse(txtMailServerPort.Text);
                Settings.MailConfig.UserName = txtMailUser.Text;
                Settings.MailConfig.MailFrom = txtMailSender.Text ;
                Settings.MailConfig.Password = txtMailPassword.Text ;

                Settings.MailConfig.MailSubject = textBox2.Text;
                Settings.MailConfig.MailBody = textBox1.Text;
                return true;
            }
            catch
            {
                return false;
            }
        }

        void LoadConfig()
        {
            txtMailSender.Text = Settings.MailConfig.MailFrom;
            txtMailRecipient.Text = Settings.MailConfig.MailTo;
            txtMailServer.Text = Settings.MailConfig.ServerName;
            txtMailServerPort.Text = Settings.MailConfig.PortNo+"";
            txtMailUser.Text = Settings.MailConfig.UserName;
            txtMailSender.Text  = Settings.MailConfig.MailFrom;
            txtMailPassword.Text = Settings.MailConfig.Password;
            textBox2 .Text = Settings.MailConfig.MailSubject;
            textBox1.Text = Settings.MailConfig.MailBody;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (SaveConfig())
            { Control controlAll = (Control)sender;
            ((Form)controlAll.Parent.Parent ).Close();
            }
            else
                MessageBox.Show("Error While Save Settings.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private void ctrMailConfig_Load(object sender, EventArgs e)
        {
            LoadConfig();
        }
    }
}
