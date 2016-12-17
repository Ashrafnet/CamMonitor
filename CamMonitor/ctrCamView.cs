using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using motion;
using VideoSource;
using System.IO;
using System.Diagnostics;
using System.Net;

namespace CamMonitor
{
    public partial class ctrCamView : UserControl
    {
        private string _IP, _Name, __VideoPath, _VideoPath, _SnapPath, _UserName, _Pass;
        private int _CamType, _CamWidth, _CamHeigh, _RecordType;
        private bool IsPlaying;
        private   Camera camera;
        private bool isPressed;
        Point buffer;
                 

        private IMotionDetector detector = new MotionDetector3Optimized();
        //private int detectorType = 4;
        private int intervalsToSave = 0;
        private int intervalsToSaveSnap = 1;
        private int intervalsToSaveSnapMail = 10;
        private bool DidMailSent = false;
        private DateTime LastTimeSnaped=DateTime.Now;
        private DateTime LastTimeSnapedMail = DateTime.Now;
        private AVIWriter writer = null;
        private bool saveOnMotion = false  ;
        private Bitmap lastFrame = null;

        // image width and height
        private int width = -1, height = -1;

        // alarm level
        private double alarmLevel = 0.005;
        //Alarm Event
        public event EventHandler Alarm;

        public ctrCamView(string CameraName)
        {            
            InitializeComponent();
            _Name = CameraName;
            EnableBar(IsPlaying);
            LoadCamInfo();
            
        }

        
        public string Cameraname
        {
            get { return _Name ; }
        }

        bool EnableMotionDetector
        {
            get {

                Settings.Cameras x = new CamMonitor.Settings.Cameras(_Name);
                return  x.EnableMotionDetect;            
            }            
        }
        string _SnapPath_root;
       public  void LoadCamInfo()
        {
            try
            {
                Settings.Cameras c = new CamMonitor.Settings.Cameras(_Name);
                toolStripStatusLabel3.Text = _Name;
                _IP = c.URL;
                _VideoPath = c.GetVideoPath;
                __VideoPath = c.GetVideoPath;
                _VideoPath += "\\" + _Name + "\\" + DateTime.Now.ToString("MMMM") + "\\" + DateTime.Now.ToString("dd") + "\\";
                _SnapPath_root= _SnapPath = c.GetSnapPath;
                _SnapPath += "\\" + _Name + "\\" + DateTime.Now.ToString("MMMM") + "\\" + DateTime.Now.ToString("dd") + "\\";
                _CamType = c.CamType;
                _UserName = c.UserName;
                _Pass = c.Password;
                _CamWidth = c.CamWidth;
                _CamHeigh = c.CamHeigh;
                _RecordType = Settings.General.RecordType ;
                intervalsToSaveSnap = Settings.General.SnapPictureEvery;
                
                LoadSnapsFoldersMenu(); LoadVideoFoldersMenu();
            }
            catch
            {
            }
        }
        // LastFrame property
        public Bitmap LastFrame
        {
            get {
                lastFrame = cameraWindow.Camera.LastFrame;
                return lastFrame; }
        }
        private void ctrCamView_Load(object sender, EventArgs e)
        {
            Size = new Size(_CamWidth, _CamHeigh);
        }

        bool checkCamPassword(IVideoSource source)
        {

            try
            {
                var req = (HttpWebRequest)WebRequest.Create(source.VideoSource);

                // set login and password
                if ((source.Login != null) && (source.Password != null) && (source.Login != ""))
                    req.Credentials = new NetworkCredential(source.Login, source.Password);

                // get response
                req.GetResponse();
                return true;
            }
            catch(Exception er)
            {
                return false;
            }
        }
        // Open video source
        private bool OpenVideoSource(IVideoSource source)
        {
            try
            {
                // close previous file
                CloseFile();

                if (!checkCamPassword(source))
                {
                    toolStripStatusLabel3.Text = "Invalid Username Or Password.";
                    
                    return false ;
                }
                // set busy cursor
                this.Cursor = Cursors.WaitCursor;

                

                // enable/disable motion alarm
                if (detector != null)
                {

                    detector.MotionLevelCalculation = true;
                }

                // create camera
                if (EnableMotionDetector)
                {
                    camera = new Camera(source, detector);
                }
                else
                {
                    camera = new Camera(source);
                }
                //camera.Width = 640;
                //camera.Height  = 460;
                // start camera
                camera.Start();


                // attach camera to camera window
                cameraWindow.Camera = camera;



                // set event handlers
                camera.NewFrame += new EventHandler(camera_NewFrame);
                camera.Alarm += new EventHandler(camera_Alarm);

                // start timer
                timer.Start();

                this.Cursor = Cursors.Default;
                return true;
            }
            catch (Exception er)
            {
                toolStripStatusLabel3.Text = er.Message;
                return false;
            }
        }

        
        void camera_NewFrame(object sender, EventArgs e)
        {

            if (_RecordType == 0)//video recording..
            {
                RecordInVideoFile();
            }
            else if (_RecordType == 1)//Snapshot recording..
            {
                RecordInSnapFiles();
            }
        }

        public void  RecordInSnapFiles()
        {
            try
            {
                TimeSpan span = DateTime.Now.Subtract(LastTimeSnaped);

                //MessageBox.Show(span.TotalSeconds + "");
                
                if (span.TotalSeconds < intervalsToSaveSnap) return;
                //if (saveOnMotion == true)
                if ((intervalsToSave != 0) && (saveOnMotion == true))
                {
                    Camera camera = cameraWindow.Camera;
                    camera.Lock();                    
                    lastFrame = camera.LastFrame;
                    
                  string  _SnapPathx =_SnapPath_root + "\\" + _Name + "\\" + DateTime.Now.ToString("MMMM") + "\\" + DateTime.Now.ToString("dd") + "\\";

                     _SnapPathx =_SnapPathx + "AutoSnap\\H_" + DateTime.Now.Hour + "\\";
                   if(!Directory.Exists(_SnapPathx)) Directory.CreateDirectory(_SnapPathx);
                    DateTime date = DateTime.Now;
                    String fileName = _SnapPathx + String.Format("{0}-{1}-{2} {3}-{4}-{5}.jpg",
                        date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);

                    lastFrame.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    camera.Unlock();
                    LastTimeSnaped = DateTime.Now;
                    toolStripStatusLabel1.Text = "Recording AutoSnapShot..";

                }

            }
            catch (Exception er)
            {
                MakeMoreFreeSpaceForPics(1024 * 1024);
                //MessageBox.Show(this, "Unable to GetSnapShot: " + er.Message);
                toolStripStatusLabel1.Text = "Error While AutoSnapShot.";

            }
            finally
            {
                camera.Unlock();
            }
        }
        void RecordInVideoFile()
        {
            
            if ((intervalsToSave != 0) && (saveOnMotion == true))
            {
                
                // lets save the frame
                if (writer == null)
                {
                    // create file name
                    DateTime date = DateTime.Now;
                    string fileName = _VideoPath + String.Format("{0}-{1}-{2} {3}-{4}-{5}.avi",
                        date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);

                    try
                    {
                        
                        // create AVI writer
                        if (Settings.General.UseVideoCompression)
                            writer = new AVIWriter(Settings.General.VideoCompressionCodec + "");
                        else
                            writer = new AVIWriter();
                        Directory.CreateDirectory(_VideoPath);                        
                        // open AVI file
                        writer.Open(fileName, cameraWindow.Camera.Width, cameraWindow.Camera.Height);
                    }
                    catch (ApplicationException ex)
                    {
                        if (writer != null)
                        {
                            writer.Dispose();
                            writer = null;
                        }
                    }
                }
                else
                {
                    string x=Path.GetFileName(writer.FileName  + "");
                    x=x.Substring(0,x.IndexOf(" "));
                    DateTime FDate = DateTime.Parse(x );
                    if (FDate.Day != DateTime.Now.Day)
                    {
                        writer.Close();
                        writer.Dispose();
                        writer = null;
                    }
                }
                toolStripStatusLabel1.Text = "Recording..";
                // save the frame
                Camera camera = cameraWindow.Camera;


                try
                {
                    camera.Lock();
                    writer.AddFrame(camera.LastFrame);
                    camera.Unlock();
                }
                catch (ApplicationException er)
                {
                    MakeMoreFreeSpace(1024*1024);
                    //MessageBox.Show(er.Message);
                }
                finally
                {
                    camera.Unlock();
                }
                
            }
        }
 void MakeMoreFreeSpaceForPics(long RequiredSpace)
        {
            try
            {
                string RootPath = Path.GetPathRoot(_SnapPath_root);
                DriveInfo x = new DriveInfo(RootPath);
                long freespace= x.AvailableFreeSpace;
                if (RequiredSpace < freespace) return;


                string[] files = Directory.GetFiles(_SnapPath_root, "*.*", SearchOption.AllDirectories);
                if (files.Length <= 1) return;
                for (int i = 0; i < files.Length; i++)
                {
                    string lastWriteTime = File.GetCreationTime (files[i]).ToString("yyyyMMddHHmmss");
                    files[i] = lastWriteTime + files[i];
                }
                Array.Sort(files);
                
                string Oldest = Path.GetFileName(files[0].Substring(15));
                try
                {
                    File.Delete(Oldest);
                    MakeMoreFreeSpace(RequiredSpace);
                }
                catch
                {

                }
            }
            catch
            {

            }
        }
        void MakeMoreFreeSpace(long RequiredSpace)
        {
            try
            {
                string RootPath= Path.GetPathRoot(_VideoPath);
                DriveInfo x = new DriveInfo(RootPath);
                long freespace= x.AvailableFreeSpace;
                if (RequiredSpace < freespace) return;
                

                string[] files = Directory.GetFiles(__VideoPath, "*.*", SearchOption.AllDirectories);
                if (files.Length <= 1) return;
                for (int i = 0; i < files.Length; i++)
                {
                    string lastWriteTime = File.GetCreationTime (files[i]).ToString("yyyyMMddHHmmss");
                    files[i] = lastWriteTime + files[i];
                }
                Array.Sort(files);
                
                string Oldest = Path.GetFileName(files[0].Substring(15));
                try
                {
                    File.Delete(Oldest);
                    MakeMoreFreeSpace(RequiredSpace);
                }
                catch
                {

                }
            }
            catch
            {

            }
        }
        void camera_Alarm(object sender, EventArgs e)
        {
            // save movie for 5 seconds after motion stops
            intervalsToSave = (int)(5 * (1000 / timer.Interval));
            SendMailNotify();
        }

        void SendMailNotify()
        {
            try
            {
                TimeSpan span = DateTime.Now.Subtract(LastTimeSnapedMail);
                if(DidMailSent)
                if (span.TotalMinutes < intervalsToSaveSnapMail) return;
                Settings.Cameras cam=new CamMonitor.Settings.Cameras(_Name );
                if (!Settings.General.EnableEmailNotify) return;
                if (!cam.EnableAlertByEmail) return;

                
                DateTime date = DateTime.Now;
                    String fileName = _SnapPath + String.Format("{0}-{1}-{2} {3}-{4}-{5}.jpg",
                        date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
                    camera.LastFrame.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);

                    clsMail.SendMail(Settings.MailConfig.MailTo, Settings.MailConfig.MailSubject, Settings.MailConfig.MailBody.Replace("%CameraName%",_Name ), "", fileName);

                    LastTimeSnapedMail = DateTime.Now;
                    DidMailSent = true;
            }
            catch
            {
            }
        }
        // Close current file
        private void CloseFile()
        {
            Camera camera = cameraWindow.Camera;

            if (camera != null)
            {
                // detach camera from camera window
                cameraWindow.Camera = null;

                // signal camera to stop
                camera.SignalToStop();
                camera.Stop();
                //// wait for the camera
                //camera.WaitForStop();

                camera = null;
                IsPlaying = false;

                if (detector != null)
                    detector.Reset();
            }

            if (writer != null)
            {
                writer.Dispose();
                writer = null;
            }
            intervalsToSave = 0;
            if(saveOnMotion)
                toolStripStatusLabel1.Text = "Monitor..";
            else
                toolStripStatusLabel1.Text = "View..";
            //cameraWindow.BackColor = SystemColors.ButtonFace;
            Graphics.FromHwnd(cameraWindow.Handle).Clear(SystemColors.ButtonFace );
            Rectangle rec = new Rectangle();
            rec.Size = new Size(cameraWindow.Size.Width-2, cameraWindow.Size.Height-2);
            rec.X = 0; rec.Y = 0;
            Graphics.FromHwnd(cameraWindow.Handle).DrawRectangle(Pens.Black, rec);

            DidMailSent = false;
        }
       public  void StartPlay()
        {
            try
            {
                LoadCamInfo();
                if (IsPlaying) return;
                
                // create video source
                switch (_CamType)
                {
                    case 1://Local Device
                        CaptureDevice CD = new CaptureDevice();
                        CD.Login = _UserName;
                        CD.Password = _Pass;
                        CD.VideoSource = _IP;
                        // open it
                      IsPlaying=  OpenVideoSource(CD);
                        break;
                    case 2://Axis Cam
                        MJPEGStream mjpegSource = new MJPEGStream();
                        mjpegSource.Login = _UserName;
                        mjpegSource.Password = _Pass;
                        mjpegSource.VideoSource = _IP;
                        // open it
                     IsPlaying=   OpenVideoSource(mjpegSource);
                        break;
                    case 3://Local Device
                        JPEGStream JS = new JPEGStream();
                        JS.Login = _UserName;
                        JS.Password = _Pass;
                        JS.VideoSource = _IP;
                        
                        // open it
                     IsPlaying=   OpenVideoSource(JS);
                        break;

                    case 4://Open File Movie
                        VideoFileSource VF = new VideoFileSource();
                        VF.Login = _UserName;
                        VF.Password = _Pass;
                        VF.VideoSource = _IP;

                        // open it
                    IsPlaying=    OpenVideoSource(VF);
                        break;

                }

                if(IsPlaying)
                toolStripStatusLabel1.Text = "Playing";                

                //toolStripButton2.Enabled = IsPlaying ;
            }
            catch (ArgumentException ArgEx)
            {
                IsPlaying = false;
                //MessageBox.Show(ArgEx.Message, "Error");
                toolStripStatusLabel2.Text = ""+ ArgEx.Message;
            }
            catch (Exception Ex)
            {
                //MessageBox.Show(Ex.Message, "Error");
                toolStripStatusLabel2.Text = ""+Ex.Message ;
            }
            finally
            {
                toolStripButton1.Enabled = false;
                EnableBar(IsPlaying);
            }
        }

        void EnableBar(bool Enabaled)
        {
            foreach (ToolStripItem  var in toolStrip1.Items)
            {

                var.Enabled = Enabaled;
            }
            
                toolStripButton1.Enabled = Enabled;
                toolStripButton5.Enabled = !Enabaled;
                toolStripComboBox1.Enabled = true;
                toolStripComboBox2.Enabled = true;
            

        }
        public void StopPlay()
        {
            try
            {
                
                CloseFile();
                toolStripStatusLabel1.Text = "Stoped";
                
                
            }
            catch { }
            finally
            {
                EnableBar(IsPlaying);
                toolStripButton1.Enabled = true;
            }
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // descrease save counter
            if (intervalsToSave > 0)
            {
                if ((--intervalsToSave == 0))
                {
                    if (writer != null)
                    {
                        writer.Dispose();
                        writer = null;
                        statusStrip1.Items[0].Text = "Monitoring..";
                    }
                }
            }      
            
            

        }

        void SendToMail()
        {
            Settings.Cameras x = new CamMonitor.Settings.Cameras(_Name);
            if (x.EnableAlertByEmail)
                clsMail.SendMail(Settings.MailConfig.MailTo, Settings.MailConfig.MailSubject.Replace("%CameraName%", _Name),"<html><body>"+ Settings.MailConfig.MailBody.Replace("%CameraName%", _Name) + "</html></body>", "", GetSnapShot());
        }
        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Tag+"")
            {
                case "play":
                    StartPlay();                    
                    break;
                case "stop":
                    StopPlay();
                    break;
                case "record":
                    Record();
                    break;
                case "snap":
                    GetSnapShot();
                    break;
                case "fullscreen":
                    ShowCamInWindow(true );
                    break;
                case "swindow":
                    ShowCamInWindow(false );
                    break;
                case "settings":
                    frmCamSettings x = new frmCamSettings(_Name );
                    x.ShowDialog();
                    LoadCamInfo();
                    break;

            }
        }

        private void ShowCamInWindow(bool IsFullScreen)
        {
            try
            {
                if (!IsPlaying) return;
                Form x = new Form();                
                CameraWindow c = new CameraWindow();
                c.Camera = camera;
                
                c.KeyDown += new KeyEventHandler(c_KeyDown);
                c.DoubleClick += new EventHandler(c_DoubleClick);
                c.Dock = DockStyle.Fill;

                if (IsFullScreen)
                {
                    x.FormBorderStyle = FormBorderStyle.None;
                    x.WindowState = FormWindowState.Maximized;
                    x.Controls.Add(c );
                }
                else
                {
                    x.Controls.Add(c );

                }

                ((CameraWindow)x.Controls[0]).Camera.Start();
                //c.Camera.Start();
                //StopPlay();
                x.DoubleClick += new EventHandler(x_DoubleClick);
                x.FormClosing += new FormClosingEventHandler(x_FormClosed);
                x.ShowInTaskbar = false;
                x.ShowDialog();

                x.Dispose();
            }
            catch
            {
            }
        }

        void x_DoubleClick(object sender, EventArgs e)
        {
            
            
        }

       

        void c_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                ((Form)((CameraWindow)sender).Parent).Close();
                
            }
            catch(Exception er)

            {
                string err = er.Message;
            }
        }

        void x_FormClosed(object sender, FormClosingEventArgs e)
        {
            //try
            //{
                //((CameraWindow)((Form)sender).Controls[0]).Camera.Stop();
            //}
            //catch
            //{
            //}
        }
        void LoadVideoFoldersMenu()
        {
            try
            {
                Settings.Cameras c = new CamMonitor.Settings.Cameras(_Name);
                string spath = c.GetVideoPath + "\\" + _Name;
                toolStripComboBox2.DropDownItems.Clear();
                toolStripComboBox2.Tag = _VideoPath;
                foreach (string var in Directory.GetDirectories(spath))
                {
                    ToolStripItem x = new ToolStripMenuItem();
                    x.Tag = var;
                    DirectoryInfo d = new DirectoryInfo(var);
                    x.Text = d.Name;
                    x.Image = global::CamMonitor.Properties.Resources.folder;
                    x.Click += new EventHandler(x_Click);
                    x.ToolTipText = "Open Recorded Video Folder (" + x.Text + ")";
                    toolStripComboBox2.DropDownItems.Add(x);
                }
                if (toolStripComboBox2.DropDownItems.Count < 1)
                    toolStripComboBox2.Visible = false;
                else
                    toolStripComboBox2.Visible = true;
            }
            catch
            {
                toolStripComboBox2.Visible = false;
            }
        }
        void LoadSnapsFoldersMenu()
        {
            try
            {
                Settings.Cameras c = new CamMonitor.Settings.Cameras(_Name);
                string spath = c.GetSnapPath + "\\" + _Name;
                toolStripComboBox1.DropDownItems.Clear();
                toolStripComboBox1.Tag = _SnapPath;
                foreach (string var in Directory.GetDirectories(spath))
                {
                    ToolStripItem x = new ToolStripMenuItem();
                    x.Tag = var;
                    DirectoryInfo d = new DirectoryInfo(var);
                    x.Text = d.Name;
                    x.Image = global::CamMonitor.Properties.Resources.folder;
                    x.Click += new EventHandler(x_Click);
                    x.ToolTipText = "Open Snapsot Folder ("+x.Text+")";
                    toolStripComboBox1.DropDownItems.Add(x);
                }
                if (toolStripComboBox1.DropDownItems.Count < 1)
                    toolStripComboBox1.Visible = false;
                else
                    toolStripComboBox1.Visible = true;
            }
            catch
            {
                toolStripComboBox1.Visible = false;
            }
        }

        void x_Click(object sender, EventArgs e)
        {
            string x = ((ToolStripItem)sender ).Tag + "";
            if(Directory.Exists(x))
                Process.Start("explorer", x );
            
        }
        void c_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    c_DoubleClick(sender, e);
                   // ((Form)sender).Close();
                }
            }
            catch
            {

            }
        }
       public  string  GetSnapShot()
        {
            try
            {
                lastFrame =  cameraWindow.Camera.LastFrame;
                Directory.CreateDirectory(_SnapPath);
                DateTime date = DateTime.Now;
                String fileName = _SnapPath + String.Format("{0}-{1}-{2} {3}-{4}-{5}.jpg",
                    date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
                
                lastFrame.Save(fileName , System.Drawing.Imaging.ImageFormat.Jpeg);
                FileInfo f = new FileInfo(fileName);
                toolStripStatusLabel2.Text = f.Name;
                toolStripStatusLabel2.IsLink = true;
                toolStripStatusLabel2.Tag = fileName;
                toolStripStatusLabel2.Click += new EventHandler(toolStripStatusLabel2_Click);
                return fileName;
               
            }
            catch(Exception er)
            {
                //MessageBox.Show(this, "Unable to GetSnapShot: " + er.Message);
                toolStripStatusLabel2.Text = "Unable to GetSnapShot";
                return "";
            }
        }

        void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {
            if (toolStripStatusLabel2.IsLink)
            {
                FileInfo x = new FileInfo(toolStripStatusLabel2.Tag+"");
                Process.Start("explorer", x.Directory + @",/select," + toolStripStatusLabel2.Tag + "");
            }
        }
        public void Record()
        {
            try
            {
                if (saveOnMotion)
                {
                    if(IsPlaying)
                        toolStripStatusLabel1.Text = "View..";
                    saveOnMotion = false;
                    toolStripButton3.Checked = saveOnMotion;
                    
                }
                else
                {
                    if (IsPlaying)
                        toolStripStatusLabel1.Text = "Monitoring..";
                    saveOnMotion = true;
                    toolStripButton3.Checked = saveOnMotion;
                }
            }
            catch (Exception ex)
            {
                toolStripButton3.Checked = saveOnMotion;                
                toolStripStatusLabel2.Text = "Unable to start/stop recording";
            }
        }

        private void cameraWindow_DoubleClick(object sender, EventArgs e)
        {
            ShowCamInWindow(true);
        }

        private void statusStrip1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void toolStripStatusLabel1_TextChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "";
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isPressed) return;
            Size += new Size(e.X - buffer.X, e.Y - buffer.Y);
            Settings.Cameras x = new CamMonitor.Settings.Cameras(_Name);
            x.CamHeigh = Size.Height;
            x.CamWidth  = Size.Width;            
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            isPressed = true;
            buffer = e.Location;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isPressed = false;
        }

        

        
    }
}
