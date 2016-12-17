using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dshow;
using dshow.Core;
using System.Xml;

namespace CamMonitor
{
    public partial class frmCameraNew : Form
    {
        FilterCollection filters;
        private int CurrPic = 0;
        private int _CamType;
        public frmCameraNew()
        {
            InitializeComponent();
            LoadLocalCameras();
        }

       
        void LoadLocalCameras()
        {
            try
            {
                filters = new FilterCollection(FilterCategory.VideoInputDevice);

                if (filters.Count == 0)
                    throw new ApplicationException();

                // add all devices to combo
                foreach (Filter filter in filters)
                {
                    deviceCombo.Items.Add(filter.Name);
                }
            }
            catch (ApplicationException)
            {
                deviceCombo.Items.Add("No local capture devices");
                deviceCombo.Enabled = false;                
            }

            deviceCombo.SelectedIndex = 0;
        }

        string  GetCamAddress()
        {
            if (radioButton1.Checked)
            {
                _CamType = 1;
                try
                {
                    return filters[deviceCombo.SelectedIndex].MonikerString;
                }
                catch
                {

                    return "";
                }
            }

            if (radioButton2.Checked)
            {

                try
                {
                    string x = comboBox1.Text;
                    if (radioButton3.Checked && comboBox1.Text.Length > 1)//Axis Camera
                    {
                        _CamType = 2;
                        x.Replace("/axis-cgi/mjpg/video.cgi", "");
                        if (x.StartsWith("http://"))
                            return x + "/axis-cgi/mjpg/video.cgi";
                        else
                            return "http://" + x + "/axis-cgi/mjpg/video.cgi";
                    }
                    else if (radioButton4.Checked && comboBox1.Text.Length > 1)
                    {
                        _CamType = 3;
                        x.Replace("/axis-cgi/mjpg/video.cgi", "");
                        if (x.StartsWith("http://"))
                            return x;
                        else
                            return "http://" + x;
                    } else if (radioButton5.Checked && comboBox1.Text.Length > 1)
                    {
                        _CamType = 3;
                        x.Replace("/image/jpeg.cgi", "");
                        if (x.StartsWith("http://"))
                            return x + "/image/jpeg.cgi";
                        else
                            return "http://" + x + "/image/jpeg.cgi";
                    }
                    else
                        return "";
                    
                }
                catch
                {

                    return "";
                }
            }
            return "";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void radioButton4_Click(object sender, EventArgs e)
        {
            lblInfo.Text = "Type Camera's IP Or Camera's URL ";
            panel5.Visible = false;
        }

        private void radioButton3_Click(object sender, EventArgs e)
        {
            lblInfo.Text = "Type Axis Camera's IP Or URL ";
            panel5.Visible = true ;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Enabled = radioButton2.Checked ;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            deviceCombo .Enabled = radioButton1.Checked;
            LoadLocalCameras();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SortPanels();
            switch (CurrPic)
            {
                    
                case 0:
                    if (GetCamAddress().Length < 1)
                    {
                        panel2.Visible = true;
                        MessageBox.Show("you must Set The Camera Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign ^ MessageBoxOptions.RtlReading);
                        return;
                    }
                    CurrPic++;
                    panel3.Visible = true;
                    button3.Enabled = true;
                    button2.Text = "Save";
                    break;
                case 1:
                    SaveCam();
                    break;

            }
        }

        void SaveCam()
        {
            try
            {
                panel3.Visible = true;
                if (textBox1.Text .Length < 1)
                {
                    MessageBox.Show("you must Set The Camera Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign ^ MessageBoxOptions.RtlReading);
                    return;
                }
                if (txtVideoFolder.Text.Length < 1)
                {
                    MessageBox.Show("you must Set The Video Folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign ^ MessageBoxOptions.RtlReading);
                    return;
                }
                if (txtSnapFolder.Text.Length < 1)
                {
                    MessageBox.Show("you must Set The Snapshot Folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign ^ MessageBoxOptions.RtlReading);
                    return;
                }
                IniFile xx = new IniFile(System.Windows.Forms.Application.StartupPath + "\\conf.ini");
                string CamName = textBox1.Text;
                string cams = xx.IniReadValue("Cameras", "Names");
                char[] ch ={ ';' ,','};
                string[] c= cams.Split(ch );
                foreach (string  var in c )
                {
                    if (var.Trim().ToUpper() == CamName.Trim().ToUpper())
                    {
                        MessageBox.Show("The name '" + var + "' already exist.please change it.", "Exist Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //SortPanels();
                        textBox1.Focus();
                        textBox1.SelectAll();
                        return;
                    }
                }
                
                    if(cams.Length <1)
                        xx.IniWriteValue("Cameras", "Names",  CamName);
                    else
                        xx.IniWriteValue("Cameras", "Names", cams + ";" + CamName);

                    if (int.Parse(textBox5.Text) > 10 && int.Parse(textBox6.Text) > 10 && radioButton3.Checked)
                    xx.IniWriteValue(CamName, "URL", GetCamAddress() + "?resolution="+textBox5.Text +"x"+textBox6.Text +"");
                else
                    xx.IniWriteValue(CamName, "URL", GetCamAddress());
                xx.IniWriteValue(CamName, "Desc", textBox2.Text);
                xx.IniWriteValue(CamName, "VideoFolder", txtVideoFolder.Text);
                xx.IniWriteValue(CamName, "SnapFolder", txtSnapFolder.Text);
                xx.IniWriteValue(CamName, "CamType", _CamType+"");
                xx.IniWriteValue(CamName, "Auth", checkBox2.Checked + "");
                xx.IniWriteValue(CamName, "UserName", textBox3.Text  + "");
                xx.IniWriteValue(CamName, "MotionDetect",  "False");
                xx.IniWriteValue(CamName, "FrameWidth", textBox5.Text );
                xx.IniWriteValue(CamName, "FrameHeight", textBox6.Text);
                xx.IniWriteValue(CamName, "FrameType", comboBox2.SelectedIndex+"");


                if (textBox4.Text.Length > 0)
                    xx.IniWriteValue(CamName, "Pass", Encryption.Encrypt(textBox4.Text) + "");
                else
                    xx.IniWriteValue(CamName, "Pass",  "");

                this.DialogResult = DialogResult.OK;
                Close();
            }
            catch(Exception er)
            {
                string x = er.Message;
            }
        }
        private void frmCameraNew_Load(object sender, EventArgs e)
        {
            SortPanels();
            panel2.Visible = true;
            LoadDefualtalues();
        }
        void LoadDefualtalues()
        {
            try
            {
                txtVideoFolder.Text = Settings.General.GetVideoPath;
                txtSnapFolder.Text = Settings.General.GetSnapPath;
            }
            catch
            {

            }
        }
        void SortPanels()
        {
            
            panel3.Location = panel2.Location;
            panel3.Visible = false;
            panel2.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SortPanels();
            switch (CurrPic)
            {
                case 1:
                    CurrPic--;
                    panel2.Visible = true;
                    button3.Enabled = false;
                    button2.Text = "Next";
                    break;
               

            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if(txtVideoFolder.Text.Length >0)
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

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            panel4.Enabled = checkBox2.Checked;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    textBox5.Text = "320";
                    textBox6.Text = "240";
                    break;
                case 1:
                    textBox5.Text = "640";
                    textBox6.Text = "480";
                    break;
                default:
                    break;
            }
        }
    }
}