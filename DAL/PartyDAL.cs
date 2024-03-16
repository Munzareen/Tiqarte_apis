using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using BusinesEntities;

namespace DAL
{
    public class PartyDAL
    {
        public void AddParty(AccountMaster accountMaster)
        {
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            SqlCommand cmd = new SqlCommand("AddAccount", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Name", accountMaster.Name));
            cmd.Parameters.Add(new SqlParameter("@LastName", accountMaster.LastName));
            cmd.Parameters.Add(new SqlParameter("@Email", accountMaster.Email));
            cmd.Parameters.Add(new SqlParameter("@Address", accountMaster.Address));
            cmd.Parameters.Add(new SqlParameter("@Reference", accountMaster.Reference));
            cmd.Parameters.Add(new SqlParameter("@ChildCode", accountMaster.ChildCode));
            cmd.Parameters.Add(new SqlParameter("@Phone", accountMaster.Phone));
          
            //add new
            //cmd.Parameters.Add(new SqlParameter("@House", accountMaster.House));
            //cmd.Parameters.Add(new SqlParameter("@Block", accountMaster.Block));
            //cmd.Parameters.Add(new SqlParameter("@Street", accountMaster.Street));
            //cmd.Parameters.Add(new SqlParameter("@Near", accountMaster.Near));
         


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
                    con.Close();
            }
        }
        public string GetHawala(string query)
        {
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            string Sr = "";
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                if (dr.Read())
                {
                    if (dr["Hawala"] == DBNull.Value)
                        Sr = "";
                    else
                        Sr = Convert.ToString(dr["Hawala"]);
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
            return Sr;
        }
    }
}
