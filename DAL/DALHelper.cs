using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;


namespace DAL
{
    public class DALHelper
    {
        public static string ConnectionStringMaster { get { return ConfigurationManager.ConnectionStrings["DBConnectionMaster"].ConnectionString.ToString(); } }
        public static string ConnectionString { get { return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString(); } }
        public static string ReportsPath { get { return ConfigurationManager.AppSettings["ReportsPath"].ToString(); } }
        public static string ScriptPath { get { return ConfigurationManager.AppSettings["ScriptPath"].ToString(); } }

    }
}
