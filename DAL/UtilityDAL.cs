using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Security.Principal;
using Microsoft.SqlServer.Management;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.IO;
using System.Configuration;
using BusinesEntities;
using DAL;
using System.Text.RegularExpressions;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Management;

namespace DAL
{
    public class UtilityDAL
    {
        public static string GetProcessorID()
        {
            string cpuInfo = string.Empty;


            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                if (cpuInfo == "")
                {
                    //Get only the first CPU's ID
                    cpuInfo = mo.Properties["processorID"].Value.ToString();
                    break;
                }
            }
            return cpuInfo;
        }
        public int GetMaxNo(string query)
        {
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            int maxNo = 0;
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                if (dr.Read())
                {
                    if (dr["MaxNo"] == DBNull.Value)
                        maxNo = 0;
                    else
                        maxNo = Convert.ToInt32(dr["MaxNo"]);
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            return maxNo;
        }

        public double GetMale(string query)
        {
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            double maxNo = 0;
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                if (dr.Read())
                {
                    if (dr["Male"] == DBNull.Value)
                        maxNo = 0;
                    else
                        maxNo = Convert.ToDouble(dr["Male"]);
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            return maxNo;
        }

        public bool isExist(string query)
        {
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            SqlCommand cmd = new SqlCommand(query, con);

            bool bFlag = false;
            SqlDataReader dr = null;
            try
            {
                con.Open();
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                    bFlag = true;
                dr.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            return bFlag;
        }

        public bool CheckPassward(string query)
        {
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            SqlCommand cmd = new SqlCommand("Select Passward from Login where UserId=(select max(UserId) from Login)", con);

            bool bFlag = false;
            SqlDataReader dr = null;
            try
            {
                con.Open();
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    string pass = dr["Passward"].ToString();
                    if (pass.ToLower() == query.ToLower())
                        bFlag = true;                    
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            return bFlag;
        }

        public void SavePassword( string password)
        {
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            SqlCommand cmd = new SqlCommand("update Login set Passward=@password where UserId=(select max(UserId) from Login)", con);
            cmd.Parameters.AddWithValue("@password", password);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
        BackgroundWorker bk;

        public void BackupDatabase(String databaseName, String userName,
            String password,/* String serverName,*/ String destinationPath, BackgroundWorker bk)
        {
            this.bk = bk;
            // this.lblProgress = lblProgress;
            Backup sqlBackup = new Backup();
            sqlBackup.PercentComplete += new PercentCompleteEventHandler(sqlBackup_PercentComplete);
            sqlBackup.Action = BackupActionType.Database;
            sqlBackup.BackupSetDescription = "ArchiveDataBase:" +
                                             DateTime.Now.ToShortDateString();
            sqlBackup.BackupSetName = "Archive";

            sqlBackup.Database = databaseName;
            //sqlBackup.Database = "JewlDB";
            BackupDeviceItem deviceItem = new BackupDeviceItem(destinationPath, DeviceType.File);
            SqlConnection sqlCon = new SqlConnection(DALHelper.ConnectionString);
            ServerConnection connection = new ServerConnection(sqlCon);
            connection.StatementTimeout = 0;
            Server sqlServer = new Server(connection);

            Microsoft.SqlServer.Management.Smo.Database db = sqlServer.Databases[databaseName];

            sqlBackup.Initialize = true;
            sqlBackup.Checksum = true;
            sqlBackup.ContinueAfterError = true;

            sqlBackup.Devices.Add(deviceItem);
            sqlBackup.Incremental = false;

            sqlBackup.ExpirationDate = DateTime.Now.AddDays(3);
            sqlBackup.LogTruncation = BackupTruncateLogType.Truncate;

            sqlBackup.FormatMedia = false;
            sqlBackup.PercentCompleteNotification = 1;
            sqlBackup.SqlBackup(sqlServer);


            //MessageBox.Show(sqlBackup.PercentCompleteNotification.ToString());
        }

        private void sqlBackup_PercentComplete(object sender, PercentCompleteEventArgs e)
        {
            bk.ReportProgress(e.Percent);
            //MessageBox.Show(e.Percent.ToString());
        }

        public void CreateDatabase()
        {
            int e = DatabaseExists("select case when db_id('SaleemCasting') is not null then 1 else 0 end as Exist");
            if (e == 0)
            {
                SqlConnection conM = new SqlConnection(DALHelper.ConnectionStringMaster);
                string script = File.ReadAllText(DALHelper.ScriptPath + "\\script.sql");

                // split script on GO command
                IEnumerable<string> commandStrings = Regex.Split(script, @"^\s*GO\s*$",
                                         RegexOptions.Multiline | RegexOptions.IgnoreCase);
                conM.Open();
                foreach (string commandString in commandStrings)
                {
                    if (commandString.Trim() != "")
                    {
                        using (var command = new SqlCommand(commandString, conM))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
                conM.Close();
                SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
                con.Open();
                using (var command = new SqlCommand("RefreshDB", con))
                {
                    command.ExecuteNonQuery();
                }
                con.Close();
            }
        }

        public int DatabaseExists(string query)
        {

            SqlConnection con = new SqlConnection(DALHelper.ConnectionStringMaster);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            //cmd.Parameters.Add("@RateDate", SqlDbType.DateTime).Value = dt;
            int a = 0;
            SqlDataReader dr = null;
            try
            {
                con.Open();
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    if (Convert.ToInt32(dr["Exist"]) == 0)
                        a = 0;
                    else
                        a = Convert.ToInt32(dr["Exist"]);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            return a;
        }

        public void VerifyReports(string reportPath, ReportDocument report)
        {
            System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder();
            builder.ConnectionString = DALHelper.ConnectionString;
            string nombre = WindowsIdentity.GetCurrent().Name.ToString().Split('\\')[0];
            TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
            TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
            ConnectionInfo crConnectionInfo = new ConnectionInfo();
            Tables CrTables;
            //cryRpt.SetDatabaseLogon(string.Empty,string.Empty, nombre + "\\sqlexpress","trupp");
            report.Load(reportPath);

            crConnectionInfo.ServerName = builder.DataSource;//nombre + "\\sqlexpress";
            crConnectionInfo.IntegratedSecurity = builder.IntegratedSecurity;
            //crConnectionInfo.UserID = builder.UserID;
            //crConnectionInfo.Password = builder.Password;
            crConnectionInfo.DatabaseName = builder.InitialCatalog;


            //}
            //else
            //{
            //    crConnectionInfo.ServerName = nombre;
            //    crConnectionInfo.IntegratedSecurity = false;
            //    crConnectionInfo.UserID = "sa";
            //    crConnectionInfo.Password = "123";
            //    crConnectionInfo.DatabaseName = "JewlDB";
            //}
            CrTables = report.Database.Tables;
            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
            {
                crtableLogoninfo = CrTable.LogOnInfo;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                CrTable.ApplyLogOnInfo(crtableLogoninfo);
            }
            if (report.Subreports.Count > 0)
            {
                CrTables = report.Subreports[0].Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }
            }
            if (builder.DataSource.Contains("sqlexpress") || builder.DataSource.Contains("SQLEXPRESS"))
            {
                //if (builder.IntegratedSecurity != true)
                //report.SetDatabaseLogon(builder.UserID, builder.Password);
            }
            else
            {
                if (builder.IntegratedSecurity != true)
                    report.SetDatabaseLogon(builder.UserID, builder.Password);
            }
            //http://stackoverflow.com/questions/3315283/crystal-reports-not-chaging-the-server-programatically
            //SELECT SERVERPROPERTY('productversion')'Version', SERVERPROPERTY ('productlevel')'ServicePack', SERVERPROPERTY ('edition')'Edition'
            //http://stackoverflow.com/questions/141154/how-can-i-determine-installed-sql-server-instances-and-their-versions
        }
    }
}
