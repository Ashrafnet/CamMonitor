using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using CamStudent;
using CamMonitor.Properties;

namespace CamMonitor
{
    public partial class mdiMain : Form
    {
        private string  SelNodemnu;
        public mdiMain()
        {
            InitializeComponent();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            frmCameraNew x = new frmCameraNew();
            if (x.ShowDialog() == DialogResult.Cancel) return;
             LoadCams(); BuildMainMenu();

        }
        void BuilCamMenu()
        {
            try
            {
                //ToolStripItem toolItem = new ToolStripMenuItem("Media", global::CamMonitor.Properties.Resources.picfolder);
                //ToolStripItem picItems = new ToolStripMenuItem("Snapshots", global::CamMonitor.Properties.Resources.picfolder);
                //ToolStripItem picItems = new ToolStripMenuItem("Videos", global::CamMonitor.Properties.Resources.vfolder);                

                //mnuCamera.Items.Insert(5, toolItem);

            }
            catch
            {
            }
        }
        private void BuildMainMenu()
        {
            try
            {
                BuilCamMenu();
                
                string[] cams = Settings.Cameras.GetCameras;
                TreeNode Cams = new TreeNode();
                Cams.SelectedImageIndex = 2;
                Cams.ImageIndex  = 2;
                treeView1.Nodes.Clear();
                foreach (string CamName in cams)
                {
                    Settings.Cameras x = new CamMonitor.Settings.Cameras(CamName);
                    if (x.URL.Length > 0 && x.CamType > 0 && x.GetSnapPath.Length > 0 && x.GetVideoPath.Length > 0)
                    {
                        TreeNode Cam = new TreeNode();
                        Cam.Text = CamName;
                        Cam.ContextMenuStrip = mnuCamera;
                        Cam.Tag = CamName ;
                        Cam.ToolTipText  = x.URL+ "\n"+ x.Desc;                        
                        Cam.SelectedImageIndex = 0;
                        Cam.ImageIndex = 0;
                        Cams.Nodes.Add(Cam); 
                    }
                }

                
                Cams.Text = "All Cameras";
                Cams.Tag = "allcams";
                Cams.ToolTipText = "View All Cameras";
                Cams.Expand();
                treeView1.Nodes.Add(Cams);
                TreeNode ViewMode = new TreeNode("View Mode", 2, 2);
                TreeNode View1 = new TreeNode("Small Size", 1, 1); View1.Tag = "small";
                TreeNode View2 = new TreeNode("Medium Size", 1, 1); View2.Tag = "medium";
                TreeNode View3 = new TreeNode("Big Size", 1, 1); View3.Tag = "Big";
                TreeNode View4 = new TreeNode("Fill Screen", 1, 1); View4.Tag = "fill";
                ViewMode.Nodes.Add(View1); ViewMode.Nodes.Add(View2); ViewMode.Nodes.Add(View3); ViewMode.Nodes.Add(View4);
                ViewMode.Expand();
                ViewMode.Tag = "Views";
                treeView1.Nodes.Add(ViewMode);
            }
            catch
            {

            }
        }
        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
                // TODO: Add code here to open the file.
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
                // TODO: Add code here to save the current contents of the form to a file.
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1.Icon = null;
            Application.Exit();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Use System.Windows.Forms.Clipboard to insert the selected text or images into the clipboard
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Use System.Windows.Forms.Clipboard to insert the selected text or images into the clipboard
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Use System.Windows.Forms.Clipboard.GetText() or System.Windows.Forms.GetData to retrieve information from the clipboard.
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void mdiMain_Load(object sender, EventArgs e)
        {
            OriginalParent = this;
            LoadCams();
            BuildMainMenu();
                      

            notifyIcon1.Icon = Icon;
            int x = Settings.General.StartUpType;
            switch (x)
            {
                case 1://play
                    OperateAllCameras(1);
                    break;
                case 2://play and record
                    OperateAllCameras(1);
                    OperateAllCameras(3);
                    break;
                
            }
            
        }

        void LoadCams()
        {
            try
            {
                //flowLayoutPanel1.Controls.Clear();
                string[] cams = Settings.Cameras.GetCameras;
                foreach (string  CamName in cams)
                {
                    Settings.Cameras x = new CamMonitor.Settings.Cameras(CamName);
                    if (x.URL.Length > 0 && x.CamType > 0 && x.GetSnapPath.Length > 0 && x.GetVideoPath.Length > 0)
                    {

                        ctrCamView cc = new ctrCamView(CamName);
                        cc.Tag = CamName;
                        cc.Name = CamName;
                        if (!IsCamAdded(CamName))
                            flowLayoutPanel1.Controls.Add(cc);                        
                    }
                }
                //remove undefine cams
                for( int i=0 ;i<flowLayoutPanel1.Controls.Count ;i++)
                {
                    if(Settings.Cameras.GetCamerasString.IndexOf("" + flowLayoutPanel1.Controls[i].Name )<0)
                    {
                        ((ctrCamView)flowLayoutPanel1.Controls[i]).StopPlay();
                        flowLayoutPanel1.Controls.Remove(flowLayoutPanel1.Controls[i]);
                    }
                }
            }
            catch
            {
            }
        }

        bool IsCamAdded(string CamName)
        {
            for (int i = 0; i < flowLayoutPanel1.Controls.Count ; i++)
            {
                if (((ctrCamView)flowLayoutPanel1.Controls[i]).Cameraname == CamName)
                    return true;
            }
            return false;
        }
        void LoadCamWindow()
        {
            Form x = new Form();
            x.MaximizeBox = false;
            x.MdiParent = this;
            //x.FormBorderStyle = FormBorderStyle.Fixed3D;
            ctrCamView cc = new ctrCamView("");
            cc.Dock = DockStyle.Fill;
            x.Controls.Add(cc);

            x.Show();
        }

        private void toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Tag + "")
            {
                case "start":
                    OperateAllCameras(1);
                    
                    break;
                case "stop":
                    OperateAllCameras(2);
                    break;
                case "record":
                    OperateAllCameras(3);
                    break;
                case "snap":
                    OperateAllCameras(4);
                    break;
                case "settings":
                    //StartRecord();
                    break;
                case "refresh":
                    BuildMainMenu();
                    LoadCams();
                    break; 
                case "full":
                    FullScreen();
                    break;
                case "showhide":
                    splitContainer1.Panel1Collapsed = !splitContainer1.Panel1Collapsed;
                    toolStripButton4.Checked = !splitContainer1.Panel1Collapsed;
                    break;

            }
        }
        Control OriginalParent = null; Form f = null;
        void FullScreen()
        {
            if (panel1.Parent == OriginalParent)
            {
                f = new Form();
                f.FormBorderStyle = FormBorderStyle.None;
                f.WindowState = FormWindowState.Maximized;
                toolStripButton3.Image = Resources.exitFullScreenHS;
                toolStripButton3.Text = "Exit Full Screen.";
                toolStrip.Hide();

                panel1.Parent = f;
                timer1.Enabled = true;
                f.ShowDialog();
                timer1.Enabled = false;
                toolStripButton3.Image = Resources.FullScreenHS;
                toolStripButton3.Text = "Full Screen.";
                
            }
            else
            {
                panel1.Parent = OriginalParent;
                panel1.BringToFront();
                f.Close();
                timer1.Enabled = false;
            }
            
        }
        void OperateAllCameras(int OprationNo)
        {
            try
            {
                for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
                {
                    if (flowLayoutPanel1.Controls[i] != null)
                    {
                        switch (OprationNo)
                        {
                            case 1://Start Play
                                ((ctrCamView)flowLayoutPanel1.Controls[i]).StartPlay();
                                break;
                            case 2://Stop Play
                                ((ctrCamView)flowLayoutPanel1.Controls[i]).StopPlay();
                                break;
                            case 3://Record
                                ((ctrCamView)flowLayoutPanel1.Controls[i]).Record();
                                recordOnMotionToolStripMenuItem.Checked = helpToolStripButton.Checked;
                                break;
                            case 4://SnapShout
                                ((ctrCamView)flowLayoutPanel1.Controls[i]).GetSnapShot();
                                break;
                            case 5://Applay Settings
                                ((ctrCamView)flowLayoutPanel1.Controls[i]).LoadCamInfo();
                                break;
                        }

                    }
                }
            }
            catch
            {
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmOptions x = new frmOptions();
            x.ShowDialog();
            OperateAllCameras(5);
        }

        private void mdiMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.ApplicationExitCall)
            {
                e.Cancel = true;
                this.Hide();
            }
            else
            {
                OperateAllCameras(2);
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show();
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            switch (e.ClickedItem.Tag+"")
	        {
		        case "exit":
                    ExitToolsStripMenuItem_Click(null, null);
                    break;
                case "record":
                    OperateAllCameras(3);
                    break;
                case "show":
                    Show();
                    this.WindowState = FormWindowState.Normal;
                    this.Activate();
                    break;
	        }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            switch (e.Node.Tag+"")
            {

                case "Views":
                    break;
                case "Big":
                    ResizeCams(1);
                    break;
                case "medium":
                    ResizeCams(2);
                    break;
                case "small":
                    ResizeCams(3);
                    break;
                case "fill":
                    ResizeCams(4);
                    break;
                case "allcams":
                    for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
                    {
                        flowLayoutPanel1.Controls[i].Visible = true ;                        
                    }
                    break;
                default :
                    for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
                    {
                        flowLayoutPanel1.Controls[i].Visible = false;
                        if (((ctrCamView ) flowLayoutPanel1.Controls[i]).Tag+"" == e.Node.Tag + "")
                        {
                            flowLayoutPanel1.Controls[i].Visible = true;
                        }
                        
                    }
                    break;
            }
        }
        void ResizeCams(int size)
        {
            try
            {
               
                int w, h;
                w = (flowLayoutPanel1.Width / size)-23+size ;
                h  = w;
                if (size == 4) w += 5;
                for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
                {
                   
                        flowLayoutPanel1.Controls[i].Size = new Size(w, h);
                }
            }
            catch
            {

            }
        }

        private void mnuCamera_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Tag+"")
            {
                case "play":
                    ((ctrCamView)flowLayoutPanel1.Controls[SelNodemnu]).StartPlay();
                    break;
                case "stop":
                    ((ctrCamView)flowLayoutPanel1.Controls[SelNodemnu]).StopPlay();
                    break;
                case "del":
                    DelCamera(SelNodemnu);
                    break;
                case "rename":
                    break;
                case "prop":
                    frmCamSettings x = new frmCamSettings(SelNodemnu);
                    x.ShowDialog();
                    ((ctrCamView)flowLayoutPanel1.Controls[SelNodemnu]).LoadCamInfo();
                    break;
            }
        }

        private void DelCamera(string SelNodemnu)
        {
            try
            {
                string x = "Are you sure you want to delete '" + SelNodemnu + "'?\n\nNote:\n\tNo way to undo this action.";
                mnuCamera.Hide();
                if(MessageBox.Show(x, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.No)return ;

                if (Settings.Cameras.DelCamera(SelNodemnu))
                {
                    BuildMainMenu();
                    LoadCams();
                }
                

            }
            catch
            {
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            SelNodemnu= e.Node.Tag+"";
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout x = new frmAbout();
            x.ShowDialog();
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (MousePosition.Y < 3)
                toolStrip.Visible = true;
            else if(MousePosition.Y  > 30)
                toolStrip.Visible = false ;
        }
    }
}
