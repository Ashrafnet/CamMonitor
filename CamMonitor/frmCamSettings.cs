using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CamMonitor
{
    public partial class frmCamSettings : Form
    {
        private string _Name;
        public frmCamSettings(string CamName)
        {
            InitializeComponent();
            _Name = CamName;
            LoadCamInfo();
        }
        public frmCamSettings()
        {
            InitializeComponent();            
        }
        private void button2_Click(object sender, EventArgs e)
        {
            SaveCamInfo();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Enabled = checkBox1.Checked;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (txtVideoFolder.Text.Length > 0)
                folderBrowserDialog1.SelectedPath = txtVideoFolder.Text;
            if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
                txtVideoFolder.Text = folderBrowserDialog1.SelectedPath;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (txtSnapFolder.Text.Length > 0)
                folderBrowserDialog1.SelectedPath = txtSnapFolder.Text;
            if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
                txtSnapFolder.Text = folderBrowserDialog1.SelectedPath;
        }

        private void frmCamSettings_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;

            
        }

        void SaveCamInfo()
        {
            try
            {
                
                if (textBox1.Text.Length < 1)
                {
                    MessageBox.Show("you must Set The Camera Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign ^ MessageBoxOptions.RtlReading);
                    return;
                }
                if (txtVideoFolder.Text.Length < 1)
                {
                    MessageBox.Show("you must Set The Video Folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign ^ MessageBoxOptions.RtlReading);
                    return;
                }
                if (txtSnapFolder.Text.Length < 1)
                {
                    MessageBox.Show("you must Set The Snapsot Folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign ^ MessageBoxOptions.RtlReading);
                    return;
                }
                Settings.Cameras c = new CamMonitor.Settings.Cameras(_Name);
                c.Auth = checkBox2.Checked;
                c.EnableMotionDetect = checkBox1.Checked;
                c.EnableAlertBySMS = checkBox4.Checked;
                c.EnableAlertByEmail = checkBox3.Checked;
                c.URL = textBox1.Text + "";
                c.Desc = textBox2.Text;
                c.GetVideoPath=txtVideoFolder.Text;
                c.GetSnapPath = txtSnapFolder.Text;
                c.UserName = textBox3.Text;
                if (textBox4.Text.Length > 0)
                    c.Password = (textBox4.Text);
                else
                    c.Password = "";
               
                
                          
                

                Close();
            }
            catch (Exception er)
            {
                string x = er.Message;
            }
        }
        void LoadCamInfo()
        {
            try
            {
                Settings.Cameras c = new CamMonitor.Settings.Cameras(_Name);
                
                Text = _Name + " Properties";
                textBox1.Text = c.URL;
                textBox2.Text = c.Desc ;

                txtSnapFolder.Text = c.GetSnapPath;
                txtVideoFolder.Text = c.GetVideoPath;
                checkBox2.Checked = c.Auth;
                checkBox1.Checked = c.EnableMotionDetect;
                checkBox3.Checked = c.EnableAlertByEmail;
                checkBox4.Checked = c.EnableAlertBySMS;
                textBox3.Text = c.UserName;
                textBox4.Text = c.Password ;

                
            }
            catch
            {
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            panel2.Enabled = checkBox2.Checked;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form x = new Form();
            x.MinimizeBox = false;
            x.MaximizeBox = false;
            x.ShowInTaskbar = false;
            x.StartPosition = FormStartPosition.CenterParent;
            x.FormBorderStyle=FormBorderStyle.FixedDialog;
            ctrMailConfig c = new ctrMailConfig(true );
            c.Dock = DockStyle.Fill;
            x.Controls.Add(c);
            x.Size = new Size(420, 435);
            x.Text = "E-Mail Config";
            x.ShowDialog();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            button3.Enabled = checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            button5.Enabled = checkBox4.Checked;
        }
    }
}