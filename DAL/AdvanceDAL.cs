using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinesEntities;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace DAL
{
    public class AdvanceDAL
    {
        int transno;
        //Advance advance = new Advance();

        //public void ManageAdvance(Advance advance, out int ad, SqlConnection con, SqlTransaction tran)
        //{
        //    //SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand("ManageAdvance", con);
        //    SqlCommand cmdstk = new SqlCommand("ManageAdvanceDetail", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    SqlParameter parm = new SqlParameter("@TransNum", SqlDbType.Int);
        //    parm.Direction = ParameterDirection.Output; // This is important!
        //    cmd.Parameters.Add(parm);
        //    cmdstk.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.Add(new SqlParameter("@TransId", advance.TransId));
        //    cmd.Parameters.Add(new SqlParameter("@TDate", advance.TDate));
        //    cmd.Parameters.Add(new SqlParameter("@Hawala", advance.Hawala));
        //    cmd.Parameters.Add(new SqlParameter("@Method", advance.Method));
        //    cmd.Parameters.Add(new SqlParameter("@TGold", advance.TGold));
        //    cmd.Parameters.Add(new SqlParameter("@Waste", advance.Waste));
        //    cmd.Parameters.Add(new SqlParameter("@Kaat", advance.Kaat));
        //    cmd.Parameters.Add(new SqlParameter("@Purity", advance.Purity));
        //    cmd.Parameters.Add(new SqlParameter("@Karrat", advance.Karrat));
        //    cmd.Parameters.Add(new SqlParameter("@RattiPlus", advance.RattiPlus));
        //    cmd.Parameters.Add(new SqlParameter("@RattiMinus", advance.RattiMinus));

        //    cmd.Parameters.Add(new SqlParameter("@AccountCode", advance.Party.AccountCode));
        //    cmd.Parameters.Add(new SqlParameter("@DeleteThisRecord", advance.DeleteThisRecord == false ? advance.DeleteThisRecord = null : advance.DeleteThisRecord = true));
        //    cmdstk.Parameters.Add("@TransId", SqlDbType.Int);
        //    cmdstk.Parameters.Add("@ItemId", SqlDbType.NVarChar);
        //    cmdstk.Parameters.Add("@Gold", SqlDbType.Float);
        //    cmdstk.Parameters.Add("@Cash", SqlDbType.Float);
        //    cmdstk.Parameters.Add("@Status", SqlDbType.NVarChar);
        //    //cmdstk.Parameters.Add("@AccountCode", SqlDbType.NVarChar);


        //    try
        //    {
        //        cmd.Transaction = tran;
        //        cmdstk.Transaction = tran;
        //        cmd.ExecuteNonQuery();
        //        transno = (int)parm.Value;
        //        ad = transno;
        //        try
        //        {
        //            int i = 1;
        //            foreach (AdvanceLineItem ali in advance.AdvanceLineItems)
        //            {
        //                if (advance.TransId != 0)
        //                {
        //                    cmdstk.Parameters["@TransId"].Value = advance.TransId;
        //                    ali.ItemId = advance.TransId + "-" + i;
        //                    cmdstk.Parameters["@ItemId"].Value = ali.ItemId;
        //                }
        //                else
        //                {
        //                    cmdstk.Parameters["@TransId"].Value = transno;
        //                    ali.ItemId = transno + "-" + i;
        //                    cmdstk.Parameters["@ItemId"].Value = ali.ItemId;
        //                }
        //                i++;
        //                cmdstk.Parameters["@Gold"].Value = ali.Gold;
        //                cmdstk.Parameters["@Cash"].Value = ali.Cash;
        //                cmdstk.Parameters["@Status"].Value = ali.Status;
        //                //cmdstk.Parameters["@AccountCode"].Value = ali.AccountCode;
        //                cmdstk.ExecuteNonQuery();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    finally
        //    {
        //    }
        //}

        //public void ManageAdvance(Advance advance, SqlConnection con, SqlTransaction tran)
        //{
        //    //SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand("ManageAdvance1", con);
        //    SqlCommand cmdstk = new SqlCommand("ManageAdvanceDetail", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    //SqlParameter parm = new SqlParameter("@TransNum", SqlDbType.Int);
        //    //parm.Direction = ParameterDirection.Output; // This is important!
        //    //cmd.Parameters.Add(parm);
        //    cmdstk.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.Add(new SqlParameter("@TransId", advance.TransId));
        //    cmd.Parameters.Add(new SqlParameter("@TDate", advance.TDate));
        //    cmd.Parameters.Add(new SqlParameter("@Hawala", advance.Hawala));
        //    cmd.Parameters.Add(new SqlParameter("@AccountCode", advance.Party.AccountCode));
        //    cmd.Parameters.Add(new SqlParameter("@DeleteThisRecord", advance.DeleteThisRecord == false ? advance.DeleteThisRecord = null : advance.DeleteThisRecord = true));
        //    cmdstk.Parameters.Add("@TransId", SqlDbType.Int);
        //    cmdstk.Parameters.Add("@ItemId", SqlDbType.NVarChar);
        //    cmdstk.Parameters.Add("@Gold", SqlDbType.Float);
        //    cmdstk.Parameters.Add("@Cash", SqlDbType.Float);
        //    cmdstk.Parameters.Add("@Status", SqlDbType.NVarChar);
        //    //cmdstk.Parameters.Add("@AccountCode", SqlDbType.NVarChar);


        //    try
        //    {
        //        cmd.Transaction = tran;
        //        cmdstk.Transaction = tran;
        //        cmd.ExecuteNonQuery();

        //        try
        //        {
        //            int i = 1;
        //            foreach (AdvanceLineItem ali in advance.AdvanceLineItems)
        //            {
        //                if (advance.TransId != 0)
        //                {
        //                    cmdstk.Parameters["@TransId"].Value = advance.TransId;
        //                    ali.ItemId = advance.TransId + "-" + i;
        //                    cmdstk.Parameters["@ItemId"].Value = ali.ItemId;
        //                }
        //                else
        //                {
        //                    cmdstk.Parameters["@TransId"].Value = 0;
        //                    //ali.ItemId = transno + "-" + i;
        //                    cmdstk.Parameters["@ItemId"].Value = "";
        //                }
        //                i++;
        //                cmdstk.Parameters["@Gold"].Value = ali.Gold;
        //                cmdstk.Parameters["@Cash"].Value = ali.Cash;
        //                cmdstk.Parameters["@Status"].Value = ali.Status;
        //                //cmdstk.Parameters["@AccountCode"].Value = ali.AccountCode;
        //                cmdstk.ExecuteNonQuery();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    finally
        //    {
        //    }
        //}

        //public List<Advance> GetAllCutomerRecord(string query)
        //{

        //    SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand(query, con);// SampleNo=" + sampleNo + " and 
        //    cmd.CommandType = CommandType.Text;
        //    List<Advance> records = null;
        //    List<Advance> records2 = new List<Advance>();

        //    try
        //    {
        //        con.Open();
        //        SqlDataReader dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            records = new List<Advance>();

        //            if (records == null) records = new List<Advance>();
        //            bool t = false;
        //            do
        //            {
        //                Advance ad = new Advance();
        //                Advance ad2 = new Advance();
        //                ad.TransId = dr["TransId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TransId"]);
        //                ad.TDate = dr["TDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(dr["TDate"]);
        //                ad.Hawala = dr["Hawala"].ToString();
        //                ad.Party = new Party();
        //                ad.Party.AccountCode = dr["AccountCode"].ToString();
        //                ad.Party.Name = dr["Name"].ToString();
        //                ad.Party.PCode = Convert.ToInt32(dr["PCode"]);

        //                ad2.TransId = dr["TransId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TransId"]);
        //                ad2.TDate = dr["TDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(dr["TDate"]);
        //                ad2.Hawala = dr["Hawala"].ToString();
        //                ad2.Party = new Party();
        //                ad2.Party.AccountCode = dr["AccountCode"].ToString();
        //                ad2.Party.Name = dr["Name"].ToString();
        //                ad2.Party.PCode = Convert.ToInt32(dr["PCode"]);


        //                if (GetAllIds("select isnull(Id, 0) as Id from Vouchers Where AccountCode='" + ad.Party.AccountCode + "' and SaleNO=" + ad.TransId + "") != null)
        //                {
        //                    foreach (int strTag in GetAllIds("select isnull(Id, 0) as Id from Vouchers Where AccountCode='" + ad.Party.AccountCode + "' and SaleNO=" + ad.TransId + ""))
        //                    {
        //                        ad.AddLineItems(GetRecordByIds("Select ad.* from Vouchers ad where ad.Id = '" + strTag + "'"));
        //                    }
        //                    if (t == false)
        //                    {
        //                        foreach (int strTag in GetAllIds("select isnull(Id, 0) as Id from Vouchers Where AccountCode='" + ad.Party.AccountCode + "' and SaleNO is null"))
        //                        {
        //                            ad2.TransId = 0;
        //                            ad2.Hawala = "";
        //                            ad2.AddLineItems(GetRecordByIds("Select ad.* from Vouchers ad where ad.Id = '" + strTag + "'"));
        //                        }
        //                        t = true;
        //                        records2.Add(ad2);
        //                    }


        //                }
        //                records.Add(ad);
        //            }
        //            while (dr.Read());
        //            records.AddRange(records2);
        //        }
        //        dr.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (con.State == ConnectionState.Open) con.Close();
        //    }

        //    return records;

        //}

        //public List<Advance> GetAdvanceRecord(string accountCode)
        //{

        //    SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand("GetAdvanceRecord", con);// SampleNo=" + sampleNo + " and 
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountCode;
        //    List<Advance> records = null;

        //    try
        //    {
        //        con.Open();
        //        SqlDataReader dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            records = new List<Advance>();

        //            if (records == null) records = new List<Advance>();
        //            do
        //            {
        //                Advance ad = new Advance();
        //                ad.TransId = dr["TransId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TransId"]);
        //                ad.TDate = dr["TDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(dr["TDate"]);
        //                ad.Hawala = dr["Hawala"].ToString();
        //                ad.Party = new Party();
        //                ad.Party.AccountCode = dr["AccountCode"].ToString();
        //                ad.Voucher.DDate = Convert.ToDateTime(dr["DDate"]);
        //                ad.Voucher.GoldDr = Convert.ToDouble(dr["GoldDr"]);
        //                if (ad.Voucher.GoldDr == 0)
        //                    ad.Voucher.GoldDr = Convert.ToDouble(dr["GoldCr"]);
        //                ad.Voucher.Dr = Convert.ToDouble(dr["Dr"]);
        //                if (ad.Voucher.Dr == 0)
        //                    ad.Voucher.Dr = Convert.ToDouble(dr["Cr"]);

        //                ad.Voucher.Description = dr["Description"].ToString();
        //                ad.TotalCashPaid = Convert.ToInt32(dr["TotalCashPaid"].ToString());
        //                ad.TotalCashReceived = Convert.ToInt32(dr["TotalCashReceived"].ToString());
        //                ad.TotalGoldPaid = Convert.ToDouble(dr["TotalGoldPaid"].ToString());
        //                ad.TotalGoldReceived = Convert.ToDouble(dr["TotalGoldReceived"].ToString());
        //                records.Add(ad);
        //            }
        //            while (dr.Read());


        //        }
        //        dr.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (con.State == ConnectionState.Open) con.Close();
        //    }

        //    return records;

        //}

        //public Voucher GetPartyAdvance(string accountCode)
        //{

        //    SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand("GetPartyAdvance", con);// SampleNo=" + sampleNo + " and 
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountCode;
        //    Voucher v = null;

        //    try
        //    {
        //        con.Open();
        //        SqlDataReader dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            if (v == null) v = new Voucher();
        //            v.GoldDr = Convert.ToDouble(dr["TotalGold"].ToString());
        //            v.Dr = Convert.ToDouble(dr["TotalCash"].ToString());
        //        }
        //        dr.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (con.State == ConnectionState.Open) con.Close();
        //    }

        //    return v;

        //}

        //public AdvanceLineItem GetRecordByItemIds(string query)
        //{
        //    SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand(query, con);
        //    AdvanceLineItem adi = null;

        //    try
        //    {
        //        con.Open();
        //        SqlDataReader dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            adi = new AdvanceLineItem();
        //            adi.ItemId = dr["ItemId"].ToString();
        //            adi.Gold = Convert.ToDouble(dr["Gold"]);
        //            adi.Cash = Convert.ToDouble(dr["Cash"]);
        //            adi.Status = dr["Status"].ToString();
        //        }
        //        dr.Close();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (con.State == ConnectionState.Open) con.Close();
        //    }

        //    return adi;

        //}

        //public List<string> GetAllItemIds(string query)
        //{
        //    string getRecord = query;
        //    SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand(getRecord, con);
        //    cmd.CommandType = CommandType.Text;
        //    List<string> records = null;

        //    try
        //    {
        //        con.Open();
        //        SqlDataReader dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            if (records == null) records = new List<string>();

        //            do
        //            {
        //                string str = dr["ItemId"].ToString();
        //                records.Add(str);
        //            }

        //            while (dr.Read());

        //        }
        //        dr.Close();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (con.State == ConnectionState.Open) con.Close();
        //    }
        //    if (records != null) records.TrimExcess();
        //    return records;
        //}

        //public List<RattiIn> GetAllRattiIn(string query)
        //{
        //    SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand(query, con);
        //    cmd.CommandType = CommandType.Text;
        //    List<RattiIn> records = null;

        //    try
        //    {
        //        con.Open();
        //        SqlDataReader dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            records = new List<RattiIn>();
        //            if (records == null) records = new List<RattiIn>();

        //            do
        //            {
        //                RattiIn ad = new RattiIn();
        //                ad.MaleCode = dr["MaleCode"] == DBNull.Value ? 0 : Convert.ToDouble(dr["MaleCode"]);
        //                ad.MaleRattiIn = dr["MaleRattiIn"] == DBNull.Value ? 0 : Convert.ToDouble(dr["MaleRattiIn"]);
        //                records.Add(ad);
        //            }
        //            while (dr.Read());
        //        }
        //        dr.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (con.State == ConnectionState.Open) con.Close();
        //    }

        //    return records;

        //}

        //public List<RattiOut> GetAllRattiOut(string query)
        //{
        //    SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand(query, con);
        //    cmd.CommandType = CommandType.Text;
        //    List<RattiOut> records = null;

        //    try
        //    {
        //        con.Open();
        //        SqlDataReader dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            records = new List<RattiOut>();
        //            if (records == null) records = new List<RattiOut>();

        //            do
        //            {
        //                RattiOut ad = new RattiOut();
        //                ad.MaleCodeRattiOut = dr["MaleCodeRattiOut"] == DBNull.Value ? 0 : Convert.ToDouble(dr["MaleCodeRattiOut"]);
        //                ad.MaleRattiOut = dr["MaleRattiOut"] == DBNull.Value ? 0 : Convert.ToDouble(dr["MaleRattiOut"]);
        //                records.Add(ad);
        //            }
        //            while (dr.Read());
        //        }
        //        dr.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (con.State == ConnectionState.Open) con.Close();
        //    }

        //    return records;
        //}


        //public AdvanceLineItem GetRecordByIds(string query)
        //{
        //    SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand(query, con);
        //    AdvanceLineItem adi = null;

        //    try
        //    {
        //        con.Open();
        //        SqlDataReader dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            adi = new AdvanceLineItem();
        //            adi.VDate = Convert.ToDateTime(dr["DDate"]);
        //            adi.ItemId = "";
        //            if (Convert.ToDouble(dr["GoldDr"]) != 0)
        //            {
        //                adi.Gold = Convert.ToDouble(dr["GoldDr"]);
        //                adi.Cash = 0;// Convert.ToDouble(dr["Cash"]);
        //                adi.Status = "Paid";
        //            }
        //            if (Convert.ToDouble(dr["GoldCr"]) != 0)
        //            {
        //                adi.Gold = Convert.ToDouble(dr["GoldCr"]);
        //                adi.Cash = 0;// Convert.ToDouble(dr["Cash"]);
        //                adi.Status = "Received";
        //            }
        //            if (Convert.ToDouble(dr["Dr"]) != 0)
        //            {
        //                adi.Gold = 0;// Convert.ToDouble(dr["Dr"]);
        //                adi.Cash = Convert.ToDouble(dr["Dr"]);
        //                adi.Status = "Paid";
        //            }
        //            if (Convert.ToDouble(dr["Cr"]) != 0)
        //            {
        //                adi.Gold = 0;// Convert.ToDouble(dr["GoldDr"]);
        //                adi.Cash = Convert.ToDouble(dr["Cr"]);
        //                adi.Status = "Received";
        //            }
        //        }
        //        dr.Close();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (con.State == ConnectionState.Open) con.Close();
        //    }

        //    return adi;

        //}

        public List<int> GetAllIds(string query)
        {
            string getRecord = query;
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            SqlCommand cmd = new SqlCommand(getRecord, con);
            cmd.CommandType = CommandType.Text;
            List<int> records = null;

            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    if (records == null) records = new List<int>();

                    do
                    {
                        int str = Convert.ToInt32(dr["Id"]);
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

        public static string GetMethod(int id)
        {
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            
            SqlCommand cmd = new SqlCommand("Select Method from AdvanceMaster where TransId="+id, con);
            string method = "";
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    method = dr["Method"].ToString();
                }
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
            return method;
        }
    }
}
