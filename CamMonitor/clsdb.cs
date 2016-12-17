using System;
using System.Data;
using System.Configuration;
using System.Data.OleDb;
using System.IO;

/// <summary>
/// Summary description for DBSql
/// </summary>
public class clsDB
{
    public clsDB()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    private static  string _username, _pass, _servername;
//    private static string Constr = "Provider=MSDAORA;Data Source=ccast;Persist Security Info=True;Password=examdb2006;User ID=examdb";
    private static OleDbConnection Conn = new OleDbConnection();


    

    public static  string Password
    {
        get { return _pass ; }
        set { _pass  = value; }
    }

    public static string UserName
    {
        get { return _username ; }
        set { _username = value; }
    }
    public static string ServerName
    {
        get { return _servername; }
        set { _servername = value; }
    }
    

    public static  string  Constr
    {
        get { 
            string constr = "Provider=MSDAORA;Data Source=%servername%;Persist Security Info=True;Password=%password%;User ID=%username%";
            constr=constr.Replace("%username%", _username);
            constr = constr.Replace("%password%", _pass );
            constr = constr.Replace("%servername%", _servername);
            return constr; 
        
        }
        

    }
	


    private static void OpenConnection()
    {

        try
        {
            if (Conn.State != ConnectionState.Open)
            {

                Conn.ConnectionString = Constr;
                Conn.Open();
            }
        }
        catch (Exception er)
        {

            //MessageBox.Show(er.Message, "Œÿ√ «À‰«¡ «·« ’«·", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //Application.Exit();
            LogErr(er.Message);
        }
    }
    public static void CloseConnection()
    {
        Conn.Close();
    }
    public static int RunSql(string sql)
    {
        OpenConnection();
        try
        {
            string constr = Constr;

            OleDbCommand myCommand2 = new OleDbCommand(sql, Conn);

            //Conn.Open();
            myCommand2.ExecuteNonQuery();
            //Conn.Close();
            myCommand2.Dispose();
            return 1;
        }
        catch (Exception er)
        {
            string xx = er.Message;
            //Conn.Close();
            //LogErr(er.Message + "\n" + sql);
            return 0;

        }

    }
    public static void LogErr(string Er)
    {
        try
        {
            FileStream fs = new FileStream(@"c:\Error.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter m_streamWriter = new StreamWriter(fs);
            m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
            m_streamWriter.WriteLine("" + Er + "\n*********************************************************");
            m_streamWriter.Flush();
            m_streamWriter.Close();
        }
        catch { }
    }
    public static string ExcuteReturnedSQLByReader(string Sql)
    {
        string constr = Constr;
        string Model = "";
        try
        {
            OpenConnection();

            OleDbCommand cmd = Conn.CreateCommand();

            cmd.CommandText = Sql;
            OleDbDataReader reader = cmd.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
                Model = reader.GetValue(0) + "";
            reader.Close();

            cmd.Dispose();
        }
        catch (Exception er)
        {
            LogErr(er.Message);
            string e = er.Message;
            //Conn.Close();
            return "";
        }
        return Model;
    }
    public static string ExcuteReturnedFullSQLByReader(string Sql)
    {
        string constr = Constr;
        string Result = "";

        try
        {
            OpenConnection();

            OleDbCommand cmd = Conn.CreateCommand();

            cmd.CommandText = Sql;
            OleDbDataReader reader = cmd.ExecuteReader();

            reader.Read();
            Result = reader.GetValue(0) + "";

            reader.Close();

            cmd.Dispose();
        }
        catch (Exception er)
        {

            string e = er.Message;
            //Conn.Close();
            return "";
        }
        return Result;
    }
    public static OleDbDataReader ExcuteSQLByRef(string Sql)
    {
        string constr = Constr;

        try
        {
            OpenConnection();

            OleDbCommand cmd = Conn.CreateCommand();

            cmd.CommandText = Sql;
            return cmd.ExecuteReader();

        }
        catch (Exception er)
        {

            string e = er.Message;
            //Conn.Close();
            return null;
        }

    }
    public static DataTable ExcuteSQLByRefDataTable(string Sql)
    {
        string constr = Constr;

        try
        {
            OleDbDataAdapter cmd = new OleDbDataAdapter(Sql, constr);

            DataTable DT = new DataTable();

            cmd.Fill(DT);
            cmd.Dispose();
            return DT;

        }
        catch (Exception er)
        {

            string e = er.Message;
            //Conn.Close();
            return null;
        }

    }
    public static int ExcuteNonSQL_WebService(string Sql)
    {
        string constr = Constr;

        try
        {
            CamMonitor.DBService.dbMethods db = new CamMonitor.DBService.dbMethods();
            return db.ExcuteNonSQL(Sql);
            

        }
        catch (Exception er)
        {


            
            return 0;
        }

    }

    public static string  ExcuteReturnedSQLByReader_WebService(string Sql)
    {
        string constr = Constr;

        try
        {
            CamMonitor.DBService.dbMethods db = new CamMonitor.DBService.dbMethods();
            return db.ExcuteReturnedSQLByReader (Sql);


        }
        catch (Exception er)
        {



            return "";
        }

    }

    public static int ExcuteNonSQL(string Sql)
    {
        string constr = Constr;

        try
        {
            //LogErr(Sql);
            OpenConnection();

            OleDbCommand cmd = Conn.CreateCommand();
            cmd.CommandText = Sql;
            return cmd.ExecuteNonQuery();

        }
        catch (Exception er)
        {


            LogErr(er.Message);
            return 0;
        }

    }


}
