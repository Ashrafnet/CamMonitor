using System;
using System.Data;
using System.Configuration;
using System.Net.Mail;
using System.Net;




namespace CamMonitor
{
    /// <summary>
    /// Use This Class To Send Mailes
    /// </summary>
    public class clsMail
    {
        public clsMail()
        {

        }
        static public int TestMailSettings(string ServerName, int ServerPort, string UserName, string Pass, string MailFrom, string MailTo, string MailSubject, string MSGBody, string CC, string AttachedFiles)
        {
            try
            {


                MailMessage x = new MailMessage(MailFrom, MailTo, MailSubject, MSGBody);
                x.IsBodyHtml = true;
                if (CC.Length > 0)
                {
                    MailAddress cc = new MailAddress(CC);                    
                    x.CC.Add(  cc);
                }
                if (AttachedFiles.Length > 1)
                {                    
                    char[] chr ={ ',', ';' };
                    string[] afs = AttachedFiles.Split(chr);
                    foreach (string af in afs)
                    {
                        if (af.Length > 0)
                        {
                            Attachment data = new Attachment(af, System.Net.Mime.MediaTypeNames.Application.Octet);
                            x.Attachments.Add(data);
                        }
                    }
                }
                SmtpClient client = new SmtpClient(ServerName, ServerPort);

                if (UserName.Length > 0)
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(UserName, Pass);
                }
                else
                    client.UseDefaultCredentials = true;

                try
                {
                    client.Send(x);
                    return 1;
                }
                catch (Exception ex)
                {
                    return 0;
                }

            }
            catch (Exception er)
            {
                string x = er.Message;
                return 0;
            }
        }
        static public int SendMail(string MailTo, string MailSubject, string MSGBody, string CC, string AttachedFiles)
        {
            try
            {

                return TestMailSettings(GetMailServer(), GetServerPort(), Settings.MailConfig.UserName, Settings.MailConfig.Password, GetFromMail(), MailTo, MailSubject, MSGBody, CC, AttachedFiles);
            }
            catch
            {
                return 0;
            }
        }
        private static int  GetServerPort()
        {
            try
            {
                return Settings.MailConfig.PortNo;
            }
            catch
            {
                return 25;
            }
        }
        private static string GetMailServer()
        {
            try
            {
                return Settings.MailConfig.ServerName;
            }
            catch
            {
                return "ccastemail";
            }
        }
        private static string GetFromMail()
        {
            try
            {


                return Settings.MailConfig.MailFrom;
            }
            catch
            {
                return "info@ccast.edu.ps";
            }

        }

        static void LogMsg(string MsgBody, string MsgSubject, string MsgFrom, string MsgTo)
        {
            try
            {
                


            }
            catch (Exception er)
            {
                throw er;
                //MessageBox.Show(er.Message, "Œÿ√ «À‰«¡ «·«—”«·", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}