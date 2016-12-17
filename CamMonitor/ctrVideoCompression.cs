using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using dshow;

namespace CamMonitor
{
    public partial class ctrVideoCompression : UserControl
    {
        public ctrVideoCompression()
        {
            InitializeComponent();
        }

        private void ctrVideoCompression_Load(object sender, EventArgs e)
        {
            FilterCollection filters;
            filters = new FilterCollection(dshow.Core.FilterCategory.VideoCompressorCategory);

            if (filters.Count > 0)
            {
                
                // add all devices to combo
                foreach (Filter filter in filters)
                {
                    try
                    {
                        
                        if (filter.MonikerString.Substring(filter.MonikerString.IndexOf("\\")).Length ==5)                            
                            comboBox1.Items.Add(new ItemData(filter.Name,filter.MonikerString));
                        
                        
                    }
                    catch { }
                }
                comboBox1.DisplayMember = "Display";
                comboBox1.ValueMember = "Value";	
                comboBox1.Enabled = true;

            }
            else
            {
                comboBox1.Enabled = false ;
                comboBox1.Text = "Codecs didn't found.";
            }

            LoadInfo();
        }
        void LoadInfo()
        {
            comboBox1.Text = Settings.General.VideoCompressionCodecName;
        }
        public bool Apply()
        {
            try
            {
                Settings.General.VideoCompressionCodec = ((ItemData)(comboBox1.SelectedItem)).Value.Substring(((ItemData)(comboBox1.SelectedItem)).Value.IndexOf("\\") + 1);
                Settings.General.VideoCompressionCodecName = ((ItemData)(comboBox1.SelectedItem)).Display;
                return true;
            }
            catch {
                return false;
            }
        }
    }
}
