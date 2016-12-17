using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using CamMonitor;


namespace CamStudent
{
    class clsFunctions
    {
        private static string _GetPicPath1;
        private static string _GetPicPath2;

       

	public static  string  GetPicPath1
	{

         
        get {
            IniFile config = new IniFile(System.Windows.Forms.Application.StartupPath  + "\\conf.ini");
            string x = config.IniReadValue("Path", "PicPath1");
            //string x = Registry.CurrentUser.GetValue("PicPath1", "").ToString();
            if (x == "")
                _GetPicPath1 = @"\\10.10.40.15\Student_Pictures$";
            else
                _GetPicPath1 = x;
                return _GetPicPath1;
        }
            set { _GetPicPath1 = value;
            //Registry.CurrentUser.SetValue("PicPath1", _GetPicPath1);
            IniFile config = new IniFile(System.Windows.Forms.Application.StartupPath + "\\conf.ini");
            config.IniWriteValue ("Path", "PicPath1",_GetPicPath1 );
            }
	}

    public static string GetPicPath2
        {
            get
            {
                IniFile config = new IniFile(System.Windows.Forms.Application.StartupPath + "\\conf.ini");
                string x = config.IniReadValue("Path", "PicPath2");
                if (x == "")
                    _GetPicPath2 = @"\\10.10.40.50\Student_Pictures$";
                else
                    _GetPicPath2 = x;
                return _GetPicPath2;
            }
            set
            {
                _GetPicPath2 = value;
                IniFile config = new IniFile(System.Windows.Forms.Application.StartupPath + "\\conf.ini");
                config.IniWriteValue("Path", "PicPath2", _GetPicPath2);
            }
        }
	
    }

   
}
