using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Windows.Forms;


namespace CamMonitor.Settings
{
    public class General
    {
        public static string ConFile = System.Windows.Forms.Application.StartupPath + "\\conf.ini";
        private static string _GetPicPath1;
        private static string _GetPicPath2;

        public static string GetVideoPath
        {
            get
            {
                IniFile config = new IniFile(General.ConFile);
                string x = config.IniReadValue("Paths", "VideoPath");
                
                if (x == "")
                    _GetPicPath1 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\CamMonitor\Video";
                else
                    _GetPicPath1 = x;
                return _GetPicPath1;
            }
            set
            {
                _GetPicPath1 = value;
                IniFile config = new IniFile(General.ConFile);
                config.IniWriteValue("Paths", "VideoPath", _GetPicPath1);
            }
        }
        public static int  CutTop
        {
            get
            {
                IniFile config = new IniFile(System.Windows.Forms.Application.StartupPath + "\\conf.ini");
                string x = config.IniReadValue("Paths", "CutTop");

                if (x == "")
                    return 1;
                else

                    return int.Parse(x);
            }
            set
            {
                IniFile config = new IniFile(System.Windows.Forms.Application.StartupPath + "\\conf.ini");
                config.IniWriteValue("Paths", "CutTop", value.ToString());
            }
        }
        public static int  CutLeft
        {
            get
            {
                IniFile config = new IniFile(System.Windows.Forms.Application.StartupPath + "\\conf.ini");
                string x = config.IniReadValue("Paths", "CutLeft");

                if (x == "")
                    return 1;
                else
                    
                return int.Parse(x) ;
            }
            set
            {                
                IniFile config = new IniFile(System.Windows.Forms.Application.StartupPath + "\\conf.ini");
                config.IniWriteValue("Paths", "CutLeft", value.ToString() );
            }
        }

        public static int SnapPictureEvery
        {
            get
            {
                IniFile config = new IniFile(ConFile );
                string x = config.IniReadValue("General", "SnapPictureEvery");

                if (x == "")
                    return 1;
                else

                    return int.Parse(x);
            }
            set
            {
                IniFile config = new IniFile(ConFile);
                config.IniWriteValue("General", "SnapPictureEvery", value.ToString());
            }
        }
        public static string GetSnapPath
        {
            get
            {
                IniFile config = new IniFile(System.Windows.Forms.Application.StartupPath + "\\conf.ini");
                string x = config.IniReadValue("Paths", "SnapPath");
                
                if (x == "")
                    _GetPicPath2 =  Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\CamMonitor\Snapshot";
                else
                    _GetPicPath2 = x;
                return _GetPicPath2;
            }
            set
            {
                _GetPicPath2 = value;
                IniFile config = new IniFile(System.Windows.Forms.Application.StartupPath + "\\conf.ini");
                config.IniWriteValue("Paths", "SnapPath", _GetPicPath2);
            }
        }
        public static string Password
        {
            get
            {
                
                return "#4123$asdfghsjakd";
            }
           
        }
        public static bool  StartupWithWindows
        {

            get
            {
                RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                if (rkApp.GetValue("CamMonitor") == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }


            }           
        }
        public static int StartUpType
        {
            get
            {
                string x="0";
                try
                {
                    IniFile config = new IniFile(General.ConFile);
                     x = config.IniReadValue("General", "StartType");
                    return int.Parse("0" + x);
                }
                catch
                {
                    return (0);
                }

            }
            set
            {
                try
                {
                    IniFile config = new IniFile(General.ConFile);
                    config.IniWriteValue("General", "StartType", value.ToString());
                }
                catch
                {

                }
            }
        }
        public static int RecordType
        {
            get
            {
                try
                {
                    IniFile config = new IniFile(General.ConFile);
                    string x = config.IniReadValue("General", "RecordType");
                    return int.Parse("0" + x);
                }
                catch {
                    return 0;
                }

            }
            set
            {
                try
                {
                    IniFile config = new IniFile(General.ConFile);
                    config.IniWriteValue("General", "RecordType", value.ToString());
                }
                catch
                {

                }
            }
        }
        public static string VideoCompressionCodec
        {
            get
            {
                IniFile config = new IniFile(ConFile);
                string x = config.IniReadValue("General", "VideoCompressionCodec");                
                return x;
            }
            set
            {                
                IniFile config = new IniFile(ConFile);
                config.IniWriteValue("General", "VideoCompressionCodec", value );
            }
        }
        public static string VideoCompressionCodecName
        {
            get
            {
                IniFile config = new IniFile(ConFile);
                string x = config.IniReadValue("General", "VideoCompressionCodecName");
                return x;
            }
            set
            {
                IniFile config = new IniFile(ConFile);
                config.IniWriteValue("General", "VideoCompressionCodecName", value);
            }
        }
        public static bool UseVideoCompression
        {
            get
            {
                IniFile config = new IniFile(General.ConFile);
                string x = config.IniReadValue("General", "UseVideoCompression");
                return bool.Parse(x);

            }
            set
            {
                try
                {
                    IniFile config = new IniFile(General.ConFile);
                    config.IniWriteValue("General", "UseVideoCompression", value.ToString());
                }
                catch
                {

                }
            }
        }
        public static bool EnableEmailNotify
        {
            get
            {
                IniFile config = new IniFile(General.ConFile);
                string x = config.IniReadValue("General", "EnableEmailNotify");
                return bool.Parse(x);

            }
            set
            {
                try
                {
                    IniFile config = new IniFile(General.ConFile);
                    config.IniWriteValue("General", "EnableEmailNotify", value.ToString());
                }
                catch
                {

                }
            }
        }
        public static bool StartMainWindowInTry
        {
            get
            {
                IniFile config = new IniFile(General.ConFile);
                string x = config.IniReadValue("General", "StartInTry");                
                return bool.Parse(x);
               
            }
            set
            {
                try
                {
                    IniFile config = new IniFile(General.ConFile);
                    config.IniWriteValue("General", "StartInTry", value.ToString());                    
                }
                catch
                {
                    
                }
            }
        }
        public static  void SetStartUpWithWindows(bool StartWithWindows)
        {
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (StartWithWindows)
            {
                // Add the value in the registry so that the application runs at startup
                rkApp.SetValue("CamMonitor", Application.ExecutablePath.ToString());
            }
            else
            {
                // Remove the value from the registry so that the application doesn't start
                rkApp.DeleteValue("CamMonitor", false);
            }

        }

    }
    public class MailConfig
    {
        public static string ServerName
        {
            get
            {
                IniFile config = new IniFile(General.ConFile);
                string x = config.IniReadValue("MailConfig", "ServerName");

                return x;
            }
            set
            {
                IniFile config = new IniFile(General.ConFile);
                config.IniWriteValue("MailConfig", "ServerName", value);
            }
        }
        public static string MailFrom
        {
            get
            {
                IniFile config = new IniFile(General.ConFile);
                string x = config.IniReadValue("MailConfig", "MailFrom");

                return x;
            }
            set
            {
                IniFile config = new IniFile(General.ConFile);
                config.IniWriteValue("MailConfig", "MailFrom", value);
            }
        }
        public static string MailTo
        {
            get
            {
                IniFile config = new IniFile(General.ConFile);
                string x = config.IniReadValue("MailConfig", "MailTo");

                return x;
            }
            set
            {
                IniFile config = new IniFile(General.ConFile);
                config.IniWriteValue("MailConfig", "MailTo", value);
            }
        }

        public static string MailSubject
        {
            get
            {
                IniFile config = new IniFile(General.ConFile);
                string x = config.IniReadValue("MailConfig", "MailSubject");

                return x;
            }
            set
            {
                IniFile config = new IniFile(General.ConFile);
                config.IniWriteValue("MailConfig", "MailSubject", value);
            }
        }

        public static string MailBody
        {
            get
            {
                IniFile config = new IniFile(General.ConFile);
                string x = config.IniReadValue("MailConfig", "MailBody");

                return x;
            }
            set
            {
                IniFile config = new IniFile(General.ConFile);
                config.IniWriteValue("MailConfig", "MailBody", value);
            }
        }

        public static string UserName
        {
            get
            {
                IniFile config = new IniFile(General.ConFile);
                string x = config.IniReadValue("MailConfig", "UserName");

                return x;
            }
            set
            {
                IniFile config = new IniFile(General.ConFile);
                config.IniWriteValue("MailConfig", "UserName", value);
            }
        }
        public static string Password
        {
            get
            {
                IniFile config = new IniFile(General.ConFile);
                string x = config.IniReadValue("MailConfig", "Password");

                return Encryption.Decrypt(x);
            }
            set
            {
                IniFile config = new IniFile(General.ConFile);
                config.IniWriteValue("MailConfig", "Password",Encryption.Encrypt( value));
            }
        }
        public static int PortNo
        {
            get
            {
                try
                {
                    IniFile config = new IniFile(General.ConFile);
                    string x = config.IniReadValue("MailConfig", "Port");

                    return int.Parse(x);
                }
                catch
                {
                    return 25;
                }
            }
            set
            {
                IniFile config = new IniFile(General.ConFile);
                config.IniWriteValue("MailConfig", "Port", value+"");
            }
        }
        
    }
    public class Cameras
    {
        private string _CamName;
        public Cameras(string CamName)
        {
            _CamName = CamName;
            
            
        }
               
        private static string _GetPicPath1;
        private static string _GetPicPath2;
        public static bool DelCamera(string CamName)
        {
            try
            {
                string x;
                x = Settings.Cameras.GetCamerasString.Replace(CamName, "");
                IniFile xx = new IniFile(General.ConFile);
                xx.IniWriteValue("Cameras", "Names", x);
                return true;
            }
            catch
            {
                return false;
            }
                
        }
        public static string GetCamerasString
        {
            get
            {
                IniFile config = new IniFile(System.Windows.Forms.Application.StartupPath + "\\conf.ini");
                string x = config.IniReadValue("Cameras", "Names");

                if (x == "")
                    return null;
                else
                {


                    return x;
                }
            }

        }
        public static string[] GetCameras
        {
            get
            {
                IniFile config = new IniFile(System.Windows.Forms.Application.StartupPath + "\\conf.ini");
                string x = config.IniReadValue("Cameras", "Names");

                if (x == "")
                    return null;
                else
                {
                    char[] c ={ ';' ,','};
                    
                    return  x.Split(c);
                }                
            }
            
        }
        public  string GetVideoPath
        {
            get
            {
                IniFile config = new IniFile(General.ConFile);
                string x = config.IniReadValue(_CamName , "VideoFolder");

                if (x == "")
                    _GetPicPath1 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\CamMonitor\Video\"+_CamName ;
                else
                    _GetPicPath1 = x;
                return _GetPicPath1;
            }
            set
            {
                _GetPicPath1 = value;
                IniFile config = new IniFile(General.ConFile);
                config.IniWriteValue(_CamName, "VideoFolder", _GetPicPath1);
            }
        }
        public string GetSnapPath
        {
            get
            {
                IniFile config = new IniFile(General.ConFile);
                string x = config.IniReadValue(_CamName, "SnapFolder");

                if (x == "")
                    _GetPicPath2 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\CamMonitor\Snapshot\" + _CamName ;
                else
                    _GetPicPath2 = x;
                return _GetPicPath2;
            }
            set
            {
                _GetPicPath2 = value;
                IniFile config = new IniFile(General.ConFile);
                config.IniWriteValue(_CamName, "SnapPath", _GetPicPath2);
            }
        }
        public string URL
        {
            get
            {
                IniFile config = new IniFile(General.ConFile);
                string x = config.IniReadValue(_CamName, "URL");
               
                return x ;
            }
            set
            {                
                IniFile config = new IniFile(General.ConFile);
                config.IniWriteValue(_CamName, "URL", value );
            }
        }
        public string Password
        {
            get
            {
                IniFile config = new IniFile(General.ConFile);
                string x = config.IniReadValue(_CamName, "Pass");
                var ss=  Encryption.Decrypt(x);
                return ss;
                
            }
            set
            {
                IniFile config = new IniFile(General.ConFile);
                config.IniWriteValue(_CamName, "Pass",Encryption.Encrypt(value));
            }
        }
        public string UserName
        {
            get
            {
                IniFile config = new IniFile(General.ConFile);
                string x = config.IniReadValue(_CamName, "UserName");

                return x;
            }
            set
            {
                IniFile config = new IniFile(General.ConFile);
                config.IniWriteValue(_CamName, "UserName", value);
            }
        }
        public bool  Auth
        {
            get
            {
                IniFile config = new IniFile(General.ConFile);
                string x = config.IniReadValue(_CamName, "Auth");

                return bool.Parse( x);
            }
            set
            {
                IniFile config = new IniFile(General.ConFile);
                config.IniWriteValue(_CamName, "Auth", value + "");
            }
        }
        public bool EnableAlertByEmail
        {
            get
            {
                IniFile config = new IniFile(General.ConFile);
                string x = config.IniReadValue(_CamName, "AlertByEmail");

                return bool.Parse(x);
            }
            set
            {
                IniFile config = new IniFile(General.ConFile);
                config.IniWriteValue(_CamName, "AlertByEmail", value + "");
            }
        }
        public bool EnableAlertBySMS
        {
            get
            {
                IniFile config = new IniFile(General.ConFile);
                string x = config.IniReadValue(_CamName, "AlertBySMS");

                return bool.Parse(x);
            }
            set
            {
                IniFile config = new IniFile(General.ConFile);
                config.IniWriteValue(_CamName, "AlertBySMS", value + "");
            }
        }
        public bool EnableMotionDetect
        {
            get
            {
                IniFile config = new IniFile(General.ConFile);
                string x = config.IniReadValue(_CamName, "MotionDetect");

                return bool.Parse(x);
            }
            set
            {
                IniFile config = new IniFile(General.ConFile);
                config.IniWriteValue(_CamName, "MotionDetect", value + "");
            }
        }
        public string Desc
        {
            get
            {
                IniFile config = new IniFile(General.ConFile);
                string x = config.IniReadValue(_CamName, "Desc");

                return x;
            }
            set
            {
                IniFile config = new IniFile(General.ConFile);
                config.IniWriteValue(_CamName, "Desc", value);
            }
        }
        public int CamWidth
        {
            get
            {
                try
                {
                    IniFile config = new IniFile(General.ConFile);
                    return int.Parse(config.IniReadValue(_CamName, "CamWidth"));
                }
                catch
                {

                    return 400;
                }

            }
            set
            {
                IniFile config = new IniFile(General.ConFile);
                config.IniWriteValue(_CamName, "CamWidth", value + "");
            }
        }
        public int CamHeigh
        {
            get
            {
                try
                {
                    IniFile config = new IniFile(General.ConFile);
                    return int.Parse(config.IniReadValue(_CamName, "CamHeigh"));
                }
                catch
                {

                    return 340;
                }

            }
            set
            {
                IniFile config = new IniFile(General.ConFile);
                config.IniWriteValue(_CamName, "CamHeigh", value + "");
            }
        }
        public int  CamType
        {
            get
            {
                try
                {
                    IniFile config = new IniFile(General.ConFile);
                    return int.Parse(config.IniReadValue(_CamName, "CamType"));
                }
                catch
                {

                    return -1;
                }
                
            }
            set
            {                
                IniFile config = new IniFile(General.ConFile);
                config.IniWriteValue(_CamName, "CamType", value+"" );
            }
        }
        
    }


}
