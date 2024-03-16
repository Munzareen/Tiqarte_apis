using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinesEntities;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace DAL
{
   public class OrderDAL
    {
       int goldRate ;
      // Advance adv = new Advance();
       public List<Advance> GetStockBySockLabSrNo(string query)
       {

           SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
           SqlCommand cmd = new SqlCommand(query, con);// SampleNo=" + sampleNo + " and 
           cmd.CommandType = CommandType.Text;
           List<Advance> records = null;

           try
           {
               con.Open();
               SqlDataReader dr = cmd.ExecuteReader();
               if (dr.Read())
               {
                   records = new List<Advance>();
                   if (records == null) records = new List<Advance>();

                   do
                   {
                       Advance ad = new Advance();
                       ad.TransId = dr["TransId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TransId"]);
                       //ad.TDate = dr["TDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(dr["TDate"]);
                       //ad.Hawala = dr["Hawala"].ToString();
                       ad.Party = new Party();
                       ad.Party.AccountCode = dr["AccountCode"].ToString();
                       ad.Party.Name = dr["Name"].ToString();
                       ad.Party.PCode = Convert.ToInt32(dr["PCode"]);
                       if (ad.TransId > 0)
                       {
                           foreach (string strTag in GetAllItemIds("select ItemId from AdvanceDetail Where TransId=" + ad.TransId))
                           {
                               ad.AddLineItems(GetRecordByItemIds("Select ad.* from AdvanceDetail ad where ad.ItemId = '" + strTag + "'"));
                           }
                       }
                       records.Add(ad);
                   }
                   while (dr.Read());
               }
               dr.Close();
           }
           catch (Exception ex)
           {
               throw ex;
           }
           finally
           {
               if (con.State == ConnectionState.Open) con.Close();
           }

           return records;

       }

       public List<string> GetAllItemIds(string query)
       {
           string getRecord = query;
           SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
           SqlCommand cmd = new SqlCommand(getRecord, con);
           cmd.CommandType = CommandType.Text;
           List<string> records = null;

           try
           {
               con.Open();
               SqlDataReader dr = cmd.ExecuteReader();
               if (dr.Read())
               {
                   if (records == null) records = new List<string>();

                   do
                   {
                       string str = dr["ItemId"].ToString();
                       records.Add(str);
                   }

                   while (dr.Read());

               }
               dr.Close();

           }
           catch (Exception ex)
           {

               throw ex;
           }
           finally
           {
               if (con.State == ConnectionState.Open) con.Close();
           }
           if (records != null) records.TrimExcess();
           return records;
       }

       public AdvanceLineItem GetRecordByItemIds(string query)
       {
           SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
           SqlCommand cmd = new SqlCommand(query, con);
           AdvanceLineItem adi = null;

           try
           {
               con.Open();
               SqlDataReader dr = cmd.ExecuteReader();
               if (dr.Read())
               {
                   adi = new AdvanceLineItem();
                   adi.ItemId = dr["ItemId"].ToString();
                   adi.Gold = Convert.ToDouble(dr["Gold"]);
                   adi.Cash = Convert.ToDouble(dr["Cash"]);
                   adi.Status = dr["Status"].ToString();
               }
               dr.Close();
           }
           catch (Exception ex)
           {

               throw ex;
           }
           finally
           {
               if (con.State == ConnectionState.Open) con.Close();
           }

           return adi;

       }

       public List<string> GetAllIds(string query)
       {
           string getRecord = query;
           SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
           SqlCommand cmd = new SqlCommand(getRecord, con);
           cmd.CommandType = CommandType.Text;
           List<string> records = null;

           try
           {
               con.Open();
               SqlDataReader dr = cmd.ExecuteReader();
               if (dr.Read())
               {
                   if (records == null) records = new List<string>();

                   do
                   {
                       string str = dr["ItemId"].ToString();
                       records.Add(str);
                   }

                   while (dr.Read());

               }
               dr.Close();

           }
           catch (Exception ex)
           {

               throw ex;
           }
           finally
           {
               if (con.State == ConnectionState.Open) con.Close();
           }
           if (records != null) records.TrimExcess();
           return records;


       }

       public int GetMaxGoldRate()
       {
           string querry = "Select GoldRate as [MaxRate] from Stock where SrNo=(select Max(SrNo) from Stock)";
           SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
           SqlCommand cmd = new SqlCommand(querry, con);
           cmd.CommandType = CommandType.Text;
           int goldRate = 0;
           try
           {
               con.Open();
               SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);


               //SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
               if (dr.Read())
               {
                   if (dr["MaxRate"] == DBNull.Value)
                       goldRate = 0;
                   else
                       goldRate = Convert.ToInt32(dr["MaxRate"]);
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
           return goldRate;
       }

       public static double GetRattiIn(double maleCode)
       {
           string querry = "select * from MaleRattiIn where MaleCode=@maleCode";
           SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
           SqlCommand cmd = new SqlCommand(querry, con);
           cmd.CommandType = CommandType.Text;
           cmd.Parameters.Add(new SqlParameter("@maleCode", maleCode));
           double goldRate = 0;
           try
           {
               con.Open();
               SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);


               //SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
               if (dr.Read())
               {
                   if (dr["MaleRattiIn"] == DBNull.Value)
                       goldRate = 0;
                   else
                       goldRate = Math.Round(Convert.ToDouble(dr["MaleRattiIn"]),3);
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
           return goldRate;
       }

       public static double GetRattiOut(double maleCode)
       {
           string querry = "select * from MaleRattiOut where MaleCodeRattiOut=@maleCode";
           SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
           SqlCommand cmd = new SqlCommand(querry, con);
           cmd.CommandType = CommandType.Text;
           cmd.Parameters.Add(new SqlParameter("@maleCode", maleCode));
           double goldRate = 0;
           try
           {
               con.Open();
               SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);


               //SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
               if (dr.Read())
               {
                   if (dr["MaleRattiOut"] == DBNull.Value)
                       goldRate = 0;
                   else
                       goldRate = Math.Round(Convert.ToDouble(dr["MaleRattiOut"]),3);
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
           return goldRate;
       }
       public Stock GetOrderDetail( string accountCode,string partyBillNo)
       {
           string query = "select * from Stock where AccountCode='" + accountCode + "' and PartyBillNo='" + partyBillNo + "'";
           SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
           SqlCommand cmd = new SqlCommand(query, con);
           cmd.CommandType = CommandType.Text;
           Stock stk = null;
           try
           {
               con.Open();
               SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
               if (stk==null) stk = new Stock();
              
               if (dr.Read())
               {
                   
               }
           }
           catch (Exception)
           {              
               throw;
           }
           finally
           {
               if (con.State==ConnectionState.Open)
                 con.Close();               
           }

           return stk;
       }

    }
}
