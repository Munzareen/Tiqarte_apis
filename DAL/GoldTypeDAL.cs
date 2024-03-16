using BusinesEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
  public  class GoldTypeDAL
    {
      public void InsertGoldType(GoldType gType,SqlConnection con,SqlTransaction trans)
      {
          SqlCommand cmd = new SqlCommand("spGoldTypeInsert", con);
    
          cmd.CommandType = CommandType.StoredProcedure;

          cmd.Parameters.Add(new SqlParameter("@AdvNo", gType.AdvNo));
          cmd.Parameters.Add(new SqlParameter("@OrderNo", gType.OrderNo));
          cmd.Parameters.Add(new SqlParameter("@Method", gType.Method));
          cmd.Parameters.Add(new SqlParameter("@TGold", gType.TGold));
          cmd.Parameters.Add(new SqlParameter("@Waste", gType.Waste));
          cmd.Parameters.Add(new SqlParameter("@Kaat", gType.Kaat));
          cmd.Parameters.Add(new SqlParameter("@Purity", gType.Purity));
          cmd.Parameters.Add(new SqlParameter("@Karrat", gType.Karrat));
          cmd.Parameters.Add(new SqlParameter("@RattiPlus", gType.RattiPlus));
          cmd.Parameters.Add(new SqlParameter("@RattiMinus", gType.RattiMinus));

 
          try
          {
              cmd.Transaction = trans;
           
              cmd.ExecuteNonQuery();
          
              try
              {
                 
              }
              catch (Exception ex)
              {
                  throw ex;
              }
          }
          finally
          {
          }
      }

      public void InsertGoldType(List<GoldType> gType,int orderno, SqlConnection con, SqlTransaction trans)
      {
          SqlCommand cmd = new SqlCommand("spGoldTypeInsert", con);

          cmd.CommandType = CommandType.StoredProcedure;

          cmd.Parameters.Add(new SqlParameter("@AdvNo", SqlDbType.Int));
          cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int));
          cmd.Parameters.Add(new SqlParameter("@Method", SqlDbType.NVarChar));
          cmd.Parameters.Add(new SqlParameter("@TGold", SqlDbType.Float));
          cmd.Parameters.Add(new SqlParameter("@Waste", SqlDbType.Float));
          cmd.Parameters.Add(new SqlParameter("@Kaat", SqlDbType.Float));
          cmd.Parameters.Add(new SqlParameter("@Purity", SqlDbType.Float));
          cmd.Parameters.Add(new SqlParameter("@Karrat", SqlDbType.Float));
          cmd.Parameters.Add(new SqlParameter("@RattiPlus", SqlDbType.Float));
          cmd.Parameters.Add(new SqlParameter("@RattiMinus", SqlDbType.Float));
          cmd.Parameters.Add(new SqlParameter("@Pure", SqlDbType.Float));

          try
          {
              cmd.Transaction = trans;
              try
              {
                  foreach (var item in gType)
                  {
                      cmd.Parameters["@AdvNo"].Value = item.AdvNo;
                      cmd.Parameters["@OrderNo"].Value = orderno;
                      cmd.Parameters["@Method"].Value = item.Method;
                      cmd.Parameters["@TGold"].Value = item.TGold;
                      cmd.Parameters["@Waste"].Value = item.Waste;
                      cmd.Parameters["@Kaat"].Value = item.Kaat;
                      cmd.Parameters["@Purity"].Value = item.Purity;
                      cmd.Parameters["@Karrat"].Value = item.Karrat;
                      cmd.Parameters["@RattiPlus"].Value = item.RattiPlus;
                      cmd.Parameters["@RattiMinus"].Value = item.RattiMinus;
                      cmd.Parameters["@Pure"].Value = item.gold;
                     
                      cmd.ExecuteNonQuery();                      
                  }
              }
              catch (Exception ex)
              {
                  throw ex;
              }
          }
          finally
          {
          }
      }


      public void InsertGoldTypeforAdvance(List<GoldType> gType, int advNo, SqlConnection con, SqlTransaction trans)
      {
          SqlCommand sqlCommand = new SqlCommand("spGoldTypeInsert", con);
          sqlCommand.CommandType = CommandType.StoredProcedure;
          sqlCommand.Parameters.Add(new SqlParameter("@AdvNo", SqlDbType.Int));
          sqlCommand.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int));
          sqlCommand.Parameters.Add(new SqlParameter("@Method", SqlDbType.NVarChar));
          sqlCommand.Parameters.Add(new SqlParameter("@TGold", SqlDbType.Float));
          sqlCommand.Parameters.Add(new SqlParameter("@Waste", SqlDbType.Float));
          sqlCommand.Parameters.Add(new SqlParameter("@Kaat", SqlDbType.Float));
          sqlCommand.Parameters.Add(new SqlParameter("@Purity", SqlDbType.Float));
          sqlCommand.Parameters.Add(new SqlParameter("@Karrat", SqlDbType.Float));
          sqlCommand.Parameters.Add(new SqlParameter("@RattiPlus", SqlDbType.Float));
          sqlCommand.Parameters.Add(new SqlParameter("@RattiMinus", SqlDbType.Float));
          sqlCommand.Parameters.Add(new SqlParameter("@Pure", SqlDbType.Float));
          sqlCommand.Transaction = trans;
          try
          {
              foreach (GoldType goldType in gType)
              {
                  sqlCommand.Parameters["@AdvNo"].Value = (object)advNo;
                  sqlCommand.Parameters["@OrderNo"].Value = (object)goldType.OrderNo;
                  sqlCommand.Parameters["@Method"].Value = (object)goldType.Method;
                  sqlCommand.Parameters["@TGold"].Value = (object)goldType.TGold;
                  sqlCommand.Parameters["@Waste"].Value = (object)goldType.Waste;
                  sqlCommand.Parameters["@Kaat"].Value = (object)goldType.Kaat;
                  sqlCommand.Parameters["@Purity"].Value = (object)goldType.Purity;
                  sqlCommand.Parameters["@Karrat"].Value = (object)goldType.Karrat;
                  sqlCommand.Parameters["@RattiPlus"].Value = (object)goldType.RattiPlus;
                  sqlCommand.Parameters["@RattiMinus"].Value = (object)goldType.RattiMinus;
                  sqlCommand.Parameters["@Pure"].Value = (object)goldType.gold;
                  sqlCommand.ExecuteNonQuery();
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }
    }
}
