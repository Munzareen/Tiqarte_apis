using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Management;
using BusinesEntities;

namespace DAL
{
    public static class DateDAL
    {
        public static void AddDates(DateTime s)
        {
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            SqlCommand cmd = new SqlCommand("insert into Dates (CurrentDate) values ('" + s + "')", con);
            cmd.CommandType = CommandType.Text;

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        public static DateTime GetMaxDate()
        {

            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            SqlCommand cmd = new SqlCommand("select MAX(currentdate) as [mx] from Dates", con);
            cmd.CommandType = CommandType.Text;
            //cmd.Parameters.Add("@RateDate", SqlDbType.DateTime).Value = dt;
            DateTime d = DateTime.Parse("1/1/0001");
            SqlDataReader dr = null;
            try
            {
                con.Open();
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    if (dr["mx"] == DBNull.Value)
                        d = DateTime.Parse("1/1/0001");
                    else
                        d = Convert.ToDateTime(dr["mx"]);
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


            return d;

        }
        public static DateTime GetDate(string query)
        {

            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            //cmd.Parameters.Add("@RateDate", SqlDbType.DateTime).Value = dt;
            DateTime d = DateTime.Parse("1/1/0001");
            SqlDataReader dr = null;
            try
            {
                con.Open();
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    if (dr["Date"] == DBNull.Value)
                        d = DateTime.Parse("1/1/0001");
                    else
                        d = Convert.ToDateTime(dr["Date"]);
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
            return d;
        }
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
    }
}
