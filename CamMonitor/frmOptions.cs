using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace CamMonitor
{
    public partial class frmOptions : Form
    {
        public frmOptions()
        {
            InitializeComponent();
        }

        private void frmOptions_Load(object sender, EventArgs e)
        {
            LoadInfo();
        }
        void LoadInfo()
        {
            try
            {
                checkBox1.Checked=Settings.General.StartupWithWindows;
                checkBox2.Checked = Settings.General.StartMainWindowInTry;
                comboBox1.SelectedIndex = Settings.General.StartUpType;
                comboBox2.SelectedIndex = Settings.General.RecordType;

                txtVideoFolder.Text = Settings.General.GetVideoPath;
                txtSnapFolder.Text = Settings.General.GetSnapPath;
                numericUpDown1.Value=Settings.General.SnapPictureEvery ;

               checkBox3.Checked = Settings.General.UseVideoCompression ;
               checkBox4.Checked = Settings.General.EnableEmailNotify;
            }
            catch
            {
            }
        }
        bool  Apply()
        {
            try
            {
                Settings.General.SetStartUpWithWindows(checkBox1.Checked);
                Settings.General.StartMainWindowInTry = checkBox2.Checked;
                Settings.General.StartUpType = comboBox1.SelectedIndex;
                Settings.General.RecordType = comboBox2.SelectedIndex;
                Settings.General.GetVideoPath = txtVideoFolder.Text;
                Settings.General.GetSnapPath = txtSnapFolder.Text;
                Settings.General.SnapPictureEvery = int.Parse(numericUpDown1.Value+"");
                Settings.General.UseVideoCompression = checkBox3.Checked;
                Settings.General.EnableEmailNotify = checkBox4.Checked;
                ctrVideoCompression1.Apply();
                ctrExptionProxyPassing1.SaveInfo();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(Apply())
                Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Apply())
                button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Enabled  = checkBox1.Checked;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            ctrVideoCompression1.Enabled = checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            ctrMailConfig1.Enabled = checkBox4.Checked ;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string SysPath=System.Environment.GetFolderPath(Environment.SpecialFolder.System);

            string fPath = SysPath + "\\rundll32.exe ";
            string fArg = SysPath + "\\shell32.dll,Control_RunDLL " + SysPath + "\\inetcpl.cpl,Internet Options";
            Process.Start(fPath, fArg);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 1)
                panel1.Visible = true;
            else
                panel1.Visible = false;
        }
    }
}