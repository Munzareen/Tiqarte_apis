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
    public class VouchersDAL
    {
        //public string CreatVNO(string VType)
        //{
        //    string qury = "select VNO from Vouchers where VNO like  '" + VType + "%' order by VNO";
        //    SqlConnection con1 = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand(qury, con1);
        //    string str = "";
        //    string vno = "";
        //    SqlDataReader dr = null;
        //    int no = 1;
        //    try
        //    {
        //        con1.Open();
        //        dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            do
        //            {
        //                vno = dr["VNO"].ToString();
        //                if (no < Convert.ToInt32(vno.Remove(0, VType.Length)))
        //                    no = Convert.ToInt32(vno.Remove(0, VType.Length));
        //            }
        //            while (dr.Read());
        //            no += 1;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (con1.State == ConnectionState.Open)
        //            con1.Close();
        //    }
        //    str = VType + no.ToString();
        //    return str;

        //}
        public string CreateVNO(string VType, SqlConnection con, SqlTransaction tran)
        {
            string qury = "select VoucheNo from Vouchers where VoucheNo like'" + VType + "%' order by VoucheNo";
            //SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            SqlCommand cmd = new SqlCommand(qury, con);
            cmd.Transaction = tran;
            string str = "";
            string vno = "";
            SqlDataReader dr;
            int no = 1;
            try
            {
               // con.Open();
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    do
                    {
                        vno = dr["VoucheNo"].ToString();
                        if (no < Convert.ToInt32(vno.Remove(0, VType.Length)))
                            no = Convert.ToInt32(vno.Remove(0, VType.Length));
                    }
                    while (dr.Read());
                    no += 1;

                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
            finally
            {
                //if (con.State == ConnectionState.Open)
                //    con.Close();
            }
            str = VType + no.ToString();
            dr.Close();
            return str;


        }
        //public void AddVoucherj(Voucher v, SqlConnection con, SqlTransaction tran)
        //{
        //    string query = "AddVoucher";
        //    //SqlConnection con = new SqlConnection(DBAccess.GetConnection);
        //    SqlCommand cmd = new SqlCommand(query, con);
        //    cmd.Transaction = tran;

        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.Add(new SqlParameter("@VNO", v.VoucheNo));
        //    cmd.Parameters.Add(new SqlParameter("@AccountCode", v.AccountCode.ChildCode));
        //    cmd.Parameters.Add(new SqlParameter("@AccountName", v.AccountCode.ChildName));
        //    cmd.Parameters.Add(new SqlParameter("@ParentCode", v.AccountCode.ParentCode));
        //    cmd.Parameters.Add(new SqlParameter("@HeadCode", v.AccountCode.HeadCode));
        //    if (v.Dr < 0)
        //    {
        //        v.Dr = -v.Dr;
        //    }
        //    cmd.Parameters.Add(new SqlParameter("@Dr", v.Dr));
        //    if (v.Cr < 0)
        //    {
        //        v.Cr = -v.Cr;
        //    }
        //    cmd.Parameters.Add(new SqlParameter("@Cr", v.Cr));
        //    cmd.Parameters.Add(new SqlParameter("@Type", v.Type));
        //    if (v.SaleNO!=0)
        //        cmd.Parameters.Add(new SqlParameter("@SaleNO", v.SaleNO));
        //    else
        //    {
        //        //    cmd .Parameters.Add ("@SaleNo",SqlDbType .Int );
        //        // cmd .Parameters ["@saleNo"].Value =DBNull.Value;
        //    }
        //    if (v.OrderNO!=0)
        //        cmd.Parameters.Add(new SqlParameter("@OrderNO", v.OrderNO));
        //    else
        //    {
        //        cmd.Parameters.Add("@OrderNo", SqlDbType.Int);
        //        cmd.Parameters["@OrderNo"].Value = DBNull.Value;
        //    }
        //    if (v.RID.HasValue)
        //        cmd.Parameters.Add(new SqlParameter("@RID", v.RID));
        //    else
        //    {
        //        cmd.Parameters.Add("@RID", SqlDbType.Int);
        //        cmd.Parameters["@RID"].Value = DBNull.Value;
        //    }
        //    cmd.Parameters.Add(new SqlParameter("@DDate", v.DDate));
        //    cmd.Parameters.Add(new SqlParameter("@Description", v.Description));
        //    if (v.GoldDr == null)
        //    {
        //        v.GoldDr = 0;
        //        cmd.Parameters.Add(new SqlParameter("@GoldDr", v.GoldDr));
        //    }
        //    else
        //        cmd.Parameters.Add(new SqlParameter("@GoldDr", v.GoldDr));
        //    if (v.GoldCr == null)
        //    {
        //        v.GoldCr = 0;
        //        cmd.Parameters.Add(new SqlParameter("@GoldCr", v.GoldCr));
        //    }
        //    else
        //        cmd.Parameters.Add(new SqlParameter("@GoldCr", v.GoldCr));
        //    try
        //    {
        //        //con.Open();
        //        cmd.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    finally
        //    {
        //        //if (con.State == ConnectionState.Open)
        //        //    con.Close();
        //    }
        //}

        //public string CreatWGVNO(string VType, SqlConnection con, SqlTransaction tran)
        //{
        //    string qury = "select VNO from GoldDetail where VNO like'" + VType + "%' order by VNO";
        //    //SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand(qury, con);
        //    cmd.Transaction = tran;
        //    string str = "";
        //    string vno = "";
        //    SqlDataReader dr;
        //    int no = 1;
        //    try
        //    {
        //        //con.Open();
        //        dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            do
        //            {
        //                vno = dr["VNO"].ToString();
        //                if (no < Convert.ToInt32(vno.Remove(0, VType.Length)))
        //                    no = Convert.ToInt32(vno.Remove(0, VType.Length));
        //            }
        //            while (dr.Read());
        //            no += 1;

        //        }
        //        dr.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        tran.Rollback();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        //if (con.State == ConnectionState.Open)
        //        //    con.Close();
        //    }
        //    str = VType + no.ToString();
        //    dr.Close();
        //    return str;


        //}

        //public string GetMaxVoucherNo()
        //{
        //    string querry = "Select MAX(VNO) as [MaxVNO] from Vouchers";
        //    SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand(querry, con);
        //    cmd.CommandType = CommandType.Text;
        //    string VNo = "";
        //    try
        //    {
        //        con.Open();
        //        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);


        //        //SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
        //        if (dr.Read())
        //        {
        //            if (dr["MaxVNO"] == DBNull.Value)
        //                VNo = "";
        //            else
        //                VNo = dr["MaxVNO"].ToString(); ;
        //        }

        //        dr.Close();

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (con.State == ConnectionState.Open)
        //            con.Close();
        //    }
        //    return VNo;
        //}
        //public void AddVoucher(Voucher v, SqlConnection con, SqlTransaction tran)
        //{
        //    string query = "AddVoucher";
        //    //SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand(query, con);
        //    cmd.Transaction = tran;
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.Add(new SqlParameter("@VNO", v.VNO));
        //    cmd.Parameters.Add(new SqlParameter("@AccountCode", v.AccountCode.ChildCode));
        //    cmd.Parameters.Add(new SqlParameter("@AccountName", v.AccountCode.ChildName));
        //    cmd.Parameters.Add(new SqlParameter("@ParentCode", v.AccountCode.ParentCode));
        //    //cmd.Parameters.Add(new SqlParameter("@GroupCode", v.AccountCode.GroupCode));
        //    //cmd.Parameters.Add(new SqlParameter("@SubGroupCode", v.AccountCode.SubGroupCode));
        //    cmd.Parameters.Add(new SqlParameter("@HeadCode", v.AccountCode.HeadCode));
        //    cmd.Parameters.Add(new SqlParameter("@Dr", v.Dr));
        //    cmd.Parameters.Add(new SqlParameter("@Cr", v.Cr));
        //    if (v.SNO.HasValue)
        //        cmd.Parameters.Add(new SqlParameter("@SaleNO", v.SNO));
        //    else
        //    {
        //        cmd.Parameters.Add("@SaleNo", SqlDbType.Int);
        //        cmd.Parameters["@saleNo"].Value = DBNull.Value;
        //    }
        //    if (v.OrderNo.HasValue)
        //        cmd.Parameters.Add(new SqlParameter("@OrderNo", v.OrderNo));
        //    else
        //    {
        //        cmd.Parameters.Add("@OrderNo", SqlDbType.Int);
        //        cmd.Parameters["@OrderNo"].Value = DBNull.Value;
        //    }
        //    if (v.RID.HasValue)
        //        cmd.Parameters.Add(new SqlParameter("@RID", v.RID));
        //    else
        //    {
        //        cmd.Parameters.Add("@RID", SqlDbType.Int);
        //        cmd.Parameters["@RID"].Value = DBNull.Value;
        //    }


        //    cmd.Parameters.Add(new SqlParameter("@DDate", v.DDate));
        //    cmd.Parameters.Add(new SqlParameter("@Description", v.Description));
        //    if (v.GoldDr == null)
        //    {
        //        v.GoldDr = 0;
        //        cmd.Parameters.Add(new SqlParameter("@GoldDr", v.GoldDr));
        //    }
        //    else
        //        cmd.Parameters.Add(new SqlParameter("@GoldDr", v.GoldDr));
        //    if (v.GoldCr == null)
        //    {
        //        v.GoldCr = 0;
        //        cmd.Parameters.Add(new SqlParameter("@GoldDr", v.GoldDr));
        //    }
        //    else
        //        cmd.Parameters.Add(new SqlParameter("@GoldCr", v.GoldCr));
        //    try
        //    {
        //        //con.Open();
        //        cmd.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        //trans.Rollback();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        //if (con.State == ConnectionState.Open)
        //        //    con.Close();
        //    }
        //}

        //public void AddVoucher(Voucher v)
        //{
        //    string query = "AddVoucher";
        //    SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand(query, con);

        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.Add(new SqlParameter("@VNO", v.VNO));
        //    cmd.Parameters.Add(new SqlParameter("@AccountCode", v.AccountCode.ChildCode));
        //    cmd.Parameters.Add(new SqlParameter("@AccountName", v.AccountCode.ChildName));
        //    cmd.Parameters.Add(new SqlParameter("@ParentCode", v.AccountCode.ParentCode));
        //    cmd.Parameters.Add(new SqlParameter("@HeadCode", v.AccountCode.HeadCode));
        //    cmd.Parameters.Add(new SqlParameter("@Dr", v.Dr));
        //    cmd.Parameters.Add(new SqlParameter("@Cr", v.Cr));
        //    if (v.SNO.HasValue)
        //        cmd.Parameters.Add(new SqlParameter("@SaleNO", v.SNO));
        //    else
        //    {
        //        cmd.Parameters.Add("@SaleNo", SqlDbType.Int);
        //        cmd.Parameters["@saleNo"].Value = DBNull.Value;
        //    }
        //    if (v.OrderNo.HasValue)
        //        cmd.Parameters.Add(new SqlParameter("@OrderNo", v.OrderNo));
        //    else
        //    {
        //        cmd.Parameters.Add("@OrderNo", SqlDbType.Int);
        //        cmd.Parameters["@OrderNo"].Value = DBNull.Value;
        //    }
        //    if (v.RID.HasValue)
        //        cmd.Parameters.Add(new SqlParameter("@RID", v.RID));
        //    else
        //    {
        //        cmd.Parameters.Add("@RID", SqlDbType.Int);
        //        cmd.Parameters["@RID"].Value = DBNull.Value;
        //    }


        //    cmd.Parameters.Add(new SqlParameter("@DDate", v.DDate));
        //    cmd.Parameters.Add(new SqlParameter("@Description", v.Description));
        //    if (v.GoldDr == null)
        //    {
        //        v.GoldDr = 0;
        //        cmd.Parameters.Add(new SqlParameter("@GoldDr", v.GoldDr));
        //    }
        //    else
        //        cmd.Parameters.Add(new SqlParameter("@GoldDr", v.GoldDr));
        //    if (v.GoldCr == null)
        //    {
        //        v.GoldCr = 0;
        //        cmd.Parameters.Add(new SqlParameter("@GoldDr", v.GoldDr));
        //    }
        //    else
        //        cmd.Parameters.Add(new SqlParameter("@GoldCr", v.GoldCr));
        //    try
        //    {
        //        con.Open();
        //        cmd.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (con.State == ConnectionState.Open)
        //            con.Close();
        //    }
        //}

        //public void DeleteVoucher(string vno)
        //{

        //    string deleteCustomer = "Delete from Vouchers where VNO='" + vno + "'";
        //    SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmdDelete = new SqlCommand(deleteCustomer, con);
        //    cmdDelete.CommandType = CommandType.Text;
        //    try
        //    {
        //        con.Open();

        //        SqlTransaction tran = con.BeginTransaction();
        //        cmdDelete.Transaction = tran;


        //        try
        //        {
        //            cmdDelete.ExecuteNonQuery();

        //            tran.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            tran.Rollback();
        //            throw ex;
        //        }
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //}

        //public void DeleteVoucher(string vno, SqlConnection con, SqlTransaction tran)
        //{

        //    string deleteCustomer = "Delete from Vouchers where VNO='" + vno + "'";
        //    //SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmdDelete = new SqlCommand(deleteCustomer, con);
        //    cmdDelete.Transaction = tran;
        //    cmdDelete.CommandType = CommandType.Text;
        //    try
        //    {
        //        //con.Open();

        //        //SqlTransaction tran = con.BeginTransaction();
        //        //cmdDelete.Transaction = trans;


        //        try
        //        {
        //            cmdDelete.ExecuteNonQuery();

        //            //tran.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            //tran.Rollback();
        //            throw ex;
        //        }
        //    }
        //    finally
        //    {
        //        //con.Close();
        //    }
        //}

        //public double GetCash(string CPV, string ccode)
        //{
        //    string query = "select * from Vouchers where VNO ='" + CPV + "'" + "and AccountCode ='" + ccode + "'";
        //    SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand(query, con);
        //    cmd.CommandType = CommandType.Text;
        //    double Cash = 0;
        //    try
        //    {
        //        con.Open();
        //        SqlDataReader dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            Cash = Convert.ToSingle(dr["Dr"]);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (con.State == ConnectionState.Open)
        //            con.Close();
        //    }
        //    return Cash;
        //}

        //public double GetCashPayment(string CPV, string ccode)
        //{
        //    string query = "select * from Vouchers where VNO ='" + CPV + "'" + "and AccountCode ='" + ccode + "'";
        //    SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand(query, con);
        //    cmd.CommandType = CommandType.Text;
        //    double Cash = 0;
        //    try
        //    {
        //        con.Open();
        //        SqlDataReader dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            Cash = Convert.ToSingle(dr["Cr"]);
        //        }
        //        dr.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {

        //        if (con.State == ConnectionState.Open)
        //            con.Close();
        //    }
        //    return Cash;
        //}

        //public double GetCashPayment(string CPV, string ccode, SqlConnection con, SqlTransaction tran)
        //{
        //    string query = "select * from Vouchers where VNO ='" + CPV + "'" + "and AccountCode ='" + ccode + "'";
        //    //SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand(query, con);
        //    cmd.Transaction = tran;
        //    cmd.CommandType = CommandType.Text;
        //    double Cash = 0;
        //    try
        //    {
        //        //con.Open();
        //        SqlDataReader dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            Cash = Convert.ToSingle(dr["Cr"]);
        //        }
        //        dr.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {

        //        //if (con.State == ConnectionState.Open)
        //        //    con.Close();
        //    }
        //    return Cash;
        //}

        //public List<Voucher> GetVoucher()
        //{
        //    //string selectSql = "select * from Vouchers where ChildName='" + name + "'";
        //    SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand("GetVoucher", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    List<Voucher> c = null;
        //    SqlDataReader dr = null;
        //    try
        //    {
        //        con.Open();
        //        dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            c = new List<Voucher>();
        //            if (c == null) c = new List<Voucher>();
        //            do
        //            {
        //                Voucher v = new Voucher();
        //                v.DDate = Convert.ToDateTime(dr["DDate"]);
        //                v.VNO = dr["VNO"].ToString();
        //                v.AccountCode = new ChildAccount();
        //                v.AccountCode.ChildName = dr["AccountName"].ToString();
        //                v.Dr = Convert.ToSingle(dr["Dr"]);
        //                v.Cr = Convert.ToSingle(dr["Cr"]);
        //                v.Description = dr["Description"].ToString();
        //                c.Add(v);
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
        //        if (con.State == ConnectionState.Open)
        //            con.Close();
        //    }
        //    return c;
        //}

        //public List<Voucher> GetVoucher(string query)
        //{
        //    //string selectSql = "select * from Vouchers where ChildName='" + name + "'";
        //    SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand(query, con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    List<Voucher> c = null;
        //    SqlDataReader dr = null;
        //    try
        //    {
        //        con.Open();
        //        dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            c = new List<Voucher>();
        //            if (c == null) c = new List<Voucher>();
        //            do
        //            {
        //                Voucher v = new Voucher();
        //                v.DDate = Convert.ToDateTime(dr["DDate"]);
        //                v.VNO = dr["VNO"].ToString();
        //                v.AccountCode = new ChildAccount();
        //                v.AccountCode.ChildName = dr["AccountName"].ToString();
        //                v.Dr = Convert.ToSingle(dr["Dr"]);
        //                v.Cr = Convert.ToSingle(dr["Cr"]);
        //                v.GoldDr = Convert.ToSingle(dr["GoldDr"]);
        //                v.GoldCr = Convert.ToSingle(dr["GoldCr"]);
        //                v.Description = dr["Description"].ToString();
        //                c.Add(v);
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
        //        if (con.State == ConnectionState.Open)
        //            con.Close();
        //    }
        //    return c;
        //}

        //public List<Voucher> GetVoucherForUpdate(string vno, int a)
        //{
        //    //string selectSql = "select * from Vouchers where ChildName='" + name + "'";
        //    SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand("GetReceiptVoucher", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.Add("@Con", SqlDbType.Int).Value = a;
        //    cmd.Parameters.Add("@VNO", SqlDbType.NVarChar).Value = vno;
        //    List<Voucher> c = null;
        //    SqlDataReader dr = null;
        //    try
        //    {
        //        con.Open();
        //        dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            c = new List<Voucher>();
        //            if (c == null) c = new List<Voucher>();
        //            do
        //            {
        //                Voucher v = new Voucher();
        //                v.DDate = Convert.ToDateTime(dr["DDate"]);
        //                v.VNO = dr["VNO"].ToString();
        //                v.AccountCode = new ChildAccount();
        //                v.AccountCode.ChildCode = dr["AccountCode"].ToString();
        //                v.AccountCode.ChildName = dr["AccountName"].ToString();
        //                v.Dr = Convert.ToSingle(dr["Dr"]);
        //                v.Cr = Convert.ToSingle(dr["Cr"]);
        //                v.GoldDr = Convert.ToSingle(dr["GoldDr"]);
        //                v.GoldCr = Convert.ToSingle(dr["GoldCr"]);
        //                v.Description = dr["Description"].ToString();
        //                c.Add(v);
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
        //        if (con.State == ConnectionState.Open)
        //            con.Close();
        //    }
        //    return c;
        //}

        //public List<Voucher> GetPaymentVoucher()
        //{
        //    // string selectSql = "select * from Vouchers where VNo Like 'CPV%' and AccountName not like 'Cash In%' and convert (varchar , DDate ,112) = convert (varchar ,'"+DateTime.Today+"',112)";
        //    SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand("GetPaymentVoucher", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    List<Voucher> c = null;
        //    SqlDataReader dr = null;
        //    try
        //    {
        //        con.Open();
        //        dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            c = new List<Voucher>();
        //            if (c == null) c = new List<Voucher>();
        //            do
        //            {
        //                Voucher v = new Voucher();
        //                v.DDate = Convert.ToDateTime(dr["DDate"]);
        //                v.VNO = dr["VNO"].ToString();
        //                v.AccountCode = new ChildAccount();
        //                v.AccountCode.ChildName = dr["AccountName"].ToString();
        //                v.Dr = Convert.ToSingle(dr["Dr"]);
        //                v.Cr = Convert.ToSingle(dr["Cr"]);
        //                v.Description = dr["Description"].ToString();
        //                c.Add(v);
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
        //        if (con.State == ConnectionState.Open)
        //            con.Close();
        //    }
        //    return c;
        //}

        //public List<Voucher> GetPaymentVoucherForUpdate(string vno)
        //{
        //    //string selectSql = "select * from Vouchers where VNo='" + vno + "' and AccountName not like 'Cash In%'";
        //    SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand(vno, con);
        //    cmd.CommandType = CommandType.Text;
        //    //cmd.Parameters.Add("@VNO", SqlDbType.NVarChar).Value = vno;
        //    List<Voucher> c = null;
        //    SqlDataReader dr = null;
        //    try
        //    {
        //        con.Open();
        //        dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            c = new List<Voucher>();
        //            if (c == null) c = new List<Voucher>();
        //            do
        //            {
        //                Voucher v = new Voucher();
        //                v.DDate = Convert.ToDateTime(dr["DDate"]);
        //                v.VNO = dr["VNO"].ToString();
        //                v.AccountCode = new ChildAccount();
        //                v.AccountCode.ChildCode = dr["AccountCode"].ToString();
        //                v.AccountCode.ChildName = dr["AccountName"].ToString();
        //                v.Dr = Convert.ToSingle(dr["Dr"]);
        //                v.Cr = Convert.ToSingle(dr["Cr"]);
        //                v.GoldDr = Convert.ToSingle(dr["GoldDr"]);
        //                v.GoldCr = Convert.ToSingle(dr["GoldCr"]);
        //                v.Description = dr["Description"].ToString();
        //                c.Add(v);
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
        //        if (con.State == ConnectionState.Open)
        //            con.Close();
        //    }
        //    return c;
        //}

        //public List<Voucher> GetBankReceiptVoucher()
        //{
        //    //string selectSql = "select * from Vouchers where ChildName='" + name + "'";
        //    SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand("GetBankReceiptVoucher", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    List<Voucher> c = null;
        //    SqlDataReader dr = null;
        //    try
        //    {
        //        con.Open();
        //        dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            c = new List<Voucher>();
        //            if (c == null) c = new List<Voucher>();
        //            do
        //            {
        //                Voucher v = new Voucher();
        //                v.DDate = Convert.ToDateTime(dr["DDate"]);
        //                v.VNO = dr["VNO"].ToString();
        //                v.AccountCode = new ChildAccount();
        //                v.AccountCode.ChildName = dr["AccountName"].ToString();
        //                v.Dr = Convert.ToSingle(dr["Dr"]);
        //                v.Cr = Convert.ToSingle(dr["Cr"]);
        //                v.Description = dr["Description"].ToString();
        //                c.Add(v);
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
        //        if (con.State == ConnectionState.Open)
        //            con.Close();
        //    }
        //    return c;
        //}

        //public List<Voucher> GetBankVoucherForUpdate(string vno)
        //{
        //    //string selectSql = "select * from Vouchers where ChildName='" + name + "'";
        //    SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand("GetBankVoucherForUpdate", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.Add("@VNO", SqlDbType.NVarChar).Value = vno;
        //    List<Voucher> c = null;
        //    SqlDataReader dr = null;
        //    try
        //    {
        //        con.Open();
        //        dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            c = new List<Voucher>();
        //            if (c == null) c = new List<Voucher>();
        //            do
        //            {
        //                Voucher v = new Voucher();
        //                v.DDate = Convert.ToDateTime(dr["DDate"]);
        //                v.VNO = dr["VNO"].ToString();
        //                v.AccountCode = new ChildAccount();
        //                v.AccountCode.ChildCode = dr["AccountCode"].ToString();
        //                v.AccountCode.ChildName = dr["AccountName"].ToString();
        //                v.Dr = Convert.ToSingle(dr["Dr"]);
        //                v.Cr = Convert.ToSingle(dr["Cr"]);
        //                v.Description = dr["Description"].ToString();
        //                c.Add(v);
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
        //        if (con.State == ConnectionState.Open)
        //            con.Close();
        //    }
        //    return c;
        //}

        //public Voucher GetBankVoucherForUp(string vno)
        //{
        //    //string selectSql = "select * from Vouchers where ChildName='" + name + "'";
        //    SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand("GetBankVoucherForUp", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.Add("@VNO", SqlDbType.NVarChar).Value = vno;
        //    Voucher v = null;
        //    SqlDataReader dr = null;
        //    try
        //    {
        //        con.Open();
        //        dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            //c = new List<Voucher>();
        //            //if (c == null) c = new List<Voucher>();
        //            //do
        //            //{
        //            v = new Voucher();
        //            v.DDate = Convert.ToDateTime(dr["DDate"]);
        //            v.VNO = dr["VNO"].ToString();
        //            v.AccountCode = new ChildAccount();
        //            v.AccountCode.ChildCode = dr["AccountCode"].ToString();
        //            v.AccountCode.ChildName = dr["AccountName"].ToString();
        //            v.Dr = Convert.ToSingle(dr["Dr"]);
        //            v.Cr = Convert.ToSingle(dr["Cr"]);
        //            v.Description = dr["Description"].ToString();

        //            //}
        //            //while (dr.Read());
        //        }
        //        dr.Close();

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (con.State == ConnectionState.Open)
        //            con.Close();
        //    }
        //    return v;
        //}

        //public List<Voucher> GetBankPaymentVoucher()
        //{
        //    //string selectSql = "select * from Vouchers where ChildName='" + name + "'";
        //    SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand("GetBankPaymentVoucher", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    List<Voucher> c = null;
        //    SqlDataReader dr = null;
        //    try
        //    {
        //        con.Open();
        //        dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            c = new List<Voucher>();
        //            if (c == null) c = new List<Voucher>();
        //            do
        //            {
        //                Voucher v = new Voucher();
        //                v.DDate = Convert.ToDateTime(dr["DDate"]);
        //                v.VNO = dr["VNO"].ToString();
        //                v.AccountCode = new ChildAccount();
        //                v.AccountCode.ChildName = dr["AccountName"].ToString();
        //                v.Dr = Convert.ToSingle(dr["Dr"]);
        //                v.Cr = Convert.ToSingle(dr["Cr"]);
        //                v.Description = dr["Description"].ToString();
        //                c.Add(v);
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
        //        if (con.State == ConnectionState.Open)
        //            con.Close();
        //    }
        //    return c;
        //}

        //public List<Voucher> GetBankPaymentVoucherForUpdate(string vno)
        //{
        //    //string selectSql = "select * from Vouchers where ChildName='" + name + "'";
        //    SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand("GetBankPaymentVoucherForUpdate", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.Add("@VNO", SqlDbType.NVarChar).Value = vno;
        //    List<Voucher> c = null;
        //    SqlDataReader dr = null;
        //    try
        //    {
        //        con.Open();
        //        dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            c = new List<Voucher>();
        //            if (c == null) c = new List<Voucher>();
        //            do
        //            {
        //                Voucher v = new Voucher();
        //                v.DDate = Convert.ToDateTime(dr["DDate"]);
        //                v.VNO = dr["VNO"].ToString();
        //                v.AccountCode = new ChildAccount();
        //                v.AccountCode.ChildCode = dr["AccountCode"].ToString();
        //                v.AccountCode.ChildName = dr["AccountName"].ToString();
        //                v.Dr = Convert.ToSingle(dr["Dr"]);
        //                v.Cr = Convert.ToSingle(dr["Cr"]);
        //                v.Description = dr["Description"].ToString();
        //                c.Add(v);
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
        //        if (con.State == ConnectionState.Open)
        //            con.Close();
        //    }
        //    return c;
        //}

        //public Voucher GetBankPaymentVoucherForUp(string vno)
        //{
        //    //string selectSql = "select * from Vouchers where ChildName='" + name + "'";
        //    SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand("GetBankPaymentForUp", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.Add("@VNO", SqlDbType.NVarChar).Value = vno;
        //    Voucher v = null;
        //    SqlDataReader dr = null;
        //    try
        //    {
        //        con.Open();
        //        dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            //c = new List<Voucher>();
        //            //if (c == null) c = new List<Voucher>();
        //            //do
        //            //{
        //            v = new Voucher();
        //            v.DDate = Convert.ToDateTime(dr["DDate"]);
        //            v.VNO = dr["VNO"].ToString();
        //            v.AccountCode = new ChildAccount();
        //            v.AccountCode.ChildCode = dr["AccountCode"].ToString();
        //            v.AccountCode.ChildName = dr["AccountName"].ToString();
        //            v.Dr = Convert.ToSingle(dr["Dr"]);
        //            v.Cr = Convert.ToSingle(dr["Cr"]);
        //            v.Description = dr["Description"].ToString();

        //            //}
        //            //while (dr.Read());
        //        }
        //        dr.Close();

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (con.State == ConnectionState.Open)
        //            con.Close();
        //    }
        //    return v;
        //}

        //public List<Voucher> GetJournalVoucher()
        //{
        //    //string selectSql = "select * from Vouchers where ChildName='" + name + "'";
        //    SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand("GetJournalVoucher", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    List<Voucher> c = null;
        //    SqlDataReader dr = null;
        //    try
        //    {
        //        con.Open();
        //        dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            c = new List<Voucher>();
        //            if (c == null) c = new List<Voucher>();
        //            do
        //            {
        //                Voucher v = new Voucher();
        //                v.DDate = Convert.ToDateTime(dr["DDate"]);
        //                v.VNO = dr["VNO"].ToString();
        //                v.AccountCode = new ChildAccount();
        //                v.AccountCode.ChildName = dr["AccountName"].ToString();
        //                v.Dr = Convert.ToSingle(dr["Dr"]);
        //                v.Cr = Convert.ToSingle(dr["Cr"]);
        //                v.Description = dr["Description"].ToString();
        //                c.Add(v);
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
        //        if (con.State == ConnectionState.Open)
        //            con.Close();
        //    }
        //    return c;
        //}

        //public List<Voucher> GetJournalVoucherForUpdate(string vno)
        //{
        //    //string selectSql = "select * from Vouchers where ChildName='" + name + "'";
        //    SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand("GetJournalVoucherForUpdate", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.Add("@VNO", SqlDbType.NVarChar).Value = vno;
        //    List<Voucher> c = null;
        //    SqlDataReader dr = null;
        //    try
        //    {
        //        con.Open();
        //        dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            c = new List<Voucher>();
        //            if (c == null) c = new List<Voucher>();
        //            do
        //            {
        //                Voucher v = new Voucher();
        //                v.DDate = Convert.ToDateTime(dr["DDate"]);
        //                v.VNO = dr["VNO"].ToString();
        //                v.AccountCode = new ChildAccount();
        //                v.AccountCode.ChildCode = dr["AccountCode"].ToString();
        //                v.AccountCode.ChildName = dr["AccountName"].ToString();
        //                v.Dr = Convert.ToSingle(dr["Dr"]);
        //                v.Cr = Convert.ToSingle(dr["Cr"]);
        //                v.Description = dr["Description"].ToString();
        //                c.Add(v);
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
        //        if (con.State == ConnectionState.Open)
        //            con.Close();
        //    }
        //    return c;
        //}

        //public string GetVoucherGeneral(string query)
        //{
        //    //string selectSql = "select * from Vouchers where ChildName='" + name + "'";
        //    SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand(query, con);
        //    cmd.CommandType = CommandType.Text;
        //    string v = null;
        //    SqlDataReader dr = null;
        //    try
        //    {
        //        con.Open();
        //        dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            //c = new Voucher();
        //            //if (c == null) c = new Voucher();
        //            //do
        //            //{  v = new Voucher();
        //            do
        //                v = dr["VNO"].ToString();
        //            while (dr.Read());
        //            //c.Add(v);
        //            //}
        //            //while (dr.Read());
        //        }
        //        dr.Close();

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (con.State == ConnectionState.Open)
        //            con.Close();
        //    }
        //    return v;
        //}
        //public string GetVoucherGeneral(string query, SqlConnection con, SqlTransaction tran)
        //{
        //    //string selectSql = "select * from Vouchers where ChildName='" + name + "'";
        //    //SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand(query, con);
        //    cmd.Transaction = tran;
        //    cmd.CommandType = CommandType.Text;
        //    string v = "";
        //    SqlDataReader dr = null;
        //    try
        //    {
        //        //con.Open();
        //        dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            //c = new Voucher();
        //            //if (c == null) c = new Voucher();
        //            //do
        //            //{  v = new Voucher();
        //            do
        //                v = dr["VNO"].ToString();
        //            while (dr.Read());
        //            //c.Add(v);
        //            //}
        //            //while (dr.Read());
        //        }
        //        dr.Close();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        //if (con.State == ConnectionState.Open)
        //        //    con.Close();
        //    }
        //    return v;
        //}

        //public List<Voucher> GetVoucherGold()
        //{
        //    //string selectSql = "select * from Vouchers where ChildName='" + name + "'";
        //    SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand("GetVoucherGold", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    List<Voucher> c = null;
        //    SqlDataReader dr = null;
        //    try
        //    {
        //        con.Open();
        //        dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            c = new List<Voucher>();
        //            if (c == null) c = new List<Voucher>();
        //            do
        //            {
        //                Voucher v = new Voucher();
        //                v.DDate = Convert.ToDateTime(dr["DDate"]);
        //                v.VNO = dr["VNO"].ToString();
        //                v.AccountCode = new ChildAccount();
        //                v.AccountCode.ChildName = dr["AccountName"].ToString();
        //                v.GoldDr = Convert.ToSingle(dr["GoldDr"]);
        //                v.GoldCr = Convert.ToSingle(dr["GoldCr"]);
        //                v.Description = dr["Description"].ToString();
        //                c.Add(v);
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
        //        if (con.State == ConnectionState.Open)
        //            con.Close();
        //    }
        //    return c;
        //}

        //public List<Voucher> GetPaymentVoucherGold()
        //{
        //    // string selectSql = "select * from Vouchers where VNo Like 'CPV%' and AccountName not like 'Cash In%' and convert (varchar , DDate ,112) = convert (varchar ,'"+DateTime.Today+"',112)";
        //    SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        //    SqlCommand cmd = new SqlCommand("GetPaymentVoucherGold", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    List<Voucher> c = null;
        //    SqlDataReader dr = null;
        //    try
        //    {
        //        con.Open();
        //        dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            c = new List<Voucher>();
        //            if (c == null) c = new List<Voucher>();
        //            do
        //            {
        //                Voucher v = new Voucher();
        //                v.DDate = Convert.ToDateTime(dr["DDate"]);
        //                v.VNO = dr["VNO"].ToString();
        //                v.AccountCode = new ChildAccount();
        //                v.AccountCode.ChildName = dr["AccountName"].ToString();
        //                v.GoldDr = Convert.ToSingle(dr["GoldDr"]);
        //                v.GoldCr = Convert.ToSingle(dr["GoldCr"]);
        //                v.Description = dr["Description"].ToString();
        //                c.Add(v);
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
        //        if (con.State == ConnectionState.Open)
        //            con.Close();
        //    }
        //    return c;
        //}
    }
}
