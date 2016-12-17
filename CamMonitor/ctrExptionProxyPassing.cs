using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace CamMonitor
{
    public partial class ctrExptionProxyPassing : UserControl
    {
        public ctrExptionProxyPassing()
        {
            InitializeComponent();
            LoadInfo();
        }
        string KeySettings = @"Software\Microsoft\Windows\CurrentVersion\Internet Settings\";
        void LoadInfo()
        {
            try
            {
                textBox1.Text = Registry.CurrentUser.OpenSubKey(KeySettings).GetValue("ProxyOverride") + "";
            }
            catch(Exception er)
            {
                //MessageBox.Show(er.Message );
            }
        }

        public  bool  SaveInfo()
        {
            try
            {
                Registry.CurrentUser.OpenSubKey(KeySettings,true ).SetValue("ProxyOverride", textBox1.Text);
                return true;
            }
            catch (Exception er)
            {
                //MessageBox.Show(er.Message );
                return false;
            }
        }

        private void ctrExptionProxyPassing_Load(object sender, EventArgs e)
        {

        }
    }
}
