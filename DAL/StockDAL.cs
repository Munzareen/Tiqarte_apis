using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinesEntities;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{
    public class StockDAL
    {
        public void AddOrder(Stock stock, out int labSr, SqlConnection con, SqlTransaction trans)
        {
            //SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            SqlCommand cmd = new SqlCommand("AddStock", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlCommand cmd1 = new SqlCommand("AddStockDetail", con);
            cmd1.CommandType = CommandType.StoredProcedure;


            //cmd.Parameters.Add(new SqlParameter());
            Party party = new Party();
            cmd.Parameters.Add(new SqlParameter("@AccountCode", stock.Party.AccountCode));
            cmd.Parameters.Add(new SqlParameter("@PartyBillNo", stock.Party.PartyBillNo));
            cmd.Parameters.Add(new SqlParameter("@CastingWeight", stock.CastingWeight));
            cmd.Parameters.Add(new SqlParameter("@Waste", stock.Waste));
            cmd.Parameters.Add(new SqlParameter("@TotalWeight", stock.TotalWeight));
            cmd.Parameters.Add(new SqlParameter("@MaleNikla", stock.MaleNikla));
            cmd.Parameters.Add(new SqlParameter("@Pure", stock.Pure));
            cmd.Parameters.Add(new SqlParameter("@Advance", stock.Advance));
            cmd.Parameters.Add(new SqlParameter("@TotalPure", stock.TotalPure));
            cmd.Parameters.Add(new SqlParameter("@MaleType", stock.MaleType));
            cmd.Parameters.Add(new SqlParameter("@MaleCode", stock.MaleCode));
            cmd.Parameters.Add(new SqlParameter("@Mazdoori", stock.Mazdoori));
            cmd.Parameters.Add(new SqlParameter("@SabqaMazdoori", stock.SabqaMazdoori));
            cmd.Parameters.Add(new SqlParameter("@TotalMazdoori", stock.TotalMazdoori));
            cmd.Parameters.Add(new SqlParameter("@WasteDiscount", stock.WasteDiscuont));
            cmd.Parameters.Add(new SqlParameter("@MazdooriDiscount", stock.MazdooriDiscount));
            cmd.Parameters.Add(new SqlParameter("@GoldRate", stock.GoldRate));
            cmd.Parameters.Add(new SqlParameter("@RatePerGram", stock.RatePerGram));
            cmd.Parameters.Add(new SqlParameter("@OrderDate", stock.OrderDate));
            cmd.Parameters.Add(new SqlParameter("@PaymentMethod", stock.PaymentMethod));
            cmd.Parameters.Add(new SqlParameter("@MazdooriBaqaya", stock.MazdooriBaqaya));
            cmd.Parameters.Add(new SqlParameter("@SonaMazdooriBaqaya", stock.SonaMazdooriBaqaya));
            cmd.Parameters.Add(new SqlParameter("@SonaSonaBaqaya", stock.SonaSonaBaqaya));
            cmd.Parameters.Add(new SqlParameter("@SonaSonaBaqaya2", stock.SonaSonaBaqaya2));
            cmd.Parameters.Add(new SqlParameter("@CashBaqaya", stock.CashBaqaya));
            cmd.Parameters.Add(new SqlParameter("@SonaSonaWasolKeya", stock.SonaSonaWasolKeya));
            cmd.Parameters.Add(new SqlParameter("@SonaSonaWapisDeya", stock.SonaSonaWapisDeya));
            cmd.Parameters.Add(new SqlParameter("@SonaSonaCashWasolKeya", stock.SonaSonaCashWasolKeya));

            cmd.Parameters.Add(new SqlParameter("@SMMazdooriWasol", stock.SMMazdooriWasol));
            cmd.Parameters.Add(new SqlParameter("@SMMazdooriWapis", stock.SMMazdooriWapis));
            cmd.Parameters.Add(new SqlParameter("@SMSonaWasolKeya", stock.SMSonaWasolKeya));
            cmd.Parameters.Add(new SqlParameter("@SMSonaWapisDeya", stock.SMSonaWapisDeya));
            cmd.Parameters.Add(new SqlParameter("@CashWasolKeya", stock.CashWasolKeya));
            cmd.Parameters.Add(new SqlParameter("@CashWapisDeya", stock.CashWapisDeya));
            cmd.Parameters.Add(new SqlParameter("@Remarks", stock.Remarks));



            cmd1.Parameters.Add(new SqlParameter("@SrNo", SqlDbType.Int));
            cmd1.Parameters.Add(new SqlParameter("@OItemId", SqlDbType.NVarChar));
            cmd1.Parameters.Add(new SqlParameter("@PartyBillNo", SqlDbType.Int));
            cmd1.Parameters.Add(new SqlParameter("@CastingWeight", SqlDbType.Float));
            cmd1.Parameters.Add(new SqlParameter("@Waste", SqlDbType.Float));
            cmd1.Parameters.Add(new SqlParameter("@TotalWeight", SqlDbType.Float));
            cmd1.Parameters.Add(new SqlParameter("@MaleNikla", SqlDbType.Float));
            cmd1.Parameters.Add(new SqlParameter("@Pure", SqlDbType.Float));
            cmd1.Parameters.Add(new SqlParameter("@Advance", SqlDbType.Float));
            cmd1.Parameters.Add(new SqlParameter("@TotalPure", SqlDbType.Float));
            cmd1.Parameters.Add(new SqlParameter("@MaleType", SqlDbType.NVarChar));
            cmd1.Parameters.Add(new SqlParameter("@MaleCode", SqlDbType.Float));
            cmd1.Parameters.Add(new SqlParameter("@Mazdoori", SqlDbType.Float));
            cmd1.Parameters.Add(new SqlParameter("@SabqaMazdoori", SqlDbType.Float));
            cmd1.Parameters.Add(new SqlParameter("@TotalMazdoori", SqlDbType.Float));
            cmd1.Parameters.Add(new SqlParameter("@WasteDiscount", SqlDbType.Float));
            cmd1.Parameters.Add(new SqlParameter("@MazdooriDiscount", SqlDbType.Float));
            cmd1.Parameters.Add(new SqlParameter("@GoldRate", SqlDbType.Float));
            cmd1.Parameters.Add(new SqlParameter("@RatePerGram", SqlDbType.Float));
            cmd1.Parameters.Add(new SqlParameter("@OrderDate", SqlDbType.DateTime));
            cmd1.Parameters.Add(new SqlParameter("@PaymentMethod", SqlDbType.NVarChar));
            cmd1.Parameters.Add(new SqlParameter("@Remarks", SqlDbType.NVarChar));

            try
            {
                //con.Open();
                cmd.Transaction = trans;
                cmd1.Transaction = trans;
                //cmd.ExecuteNonQuery();
                var lbSr = cmd.ExecuteScalar();
                labSr = int.Parse(lbSr.ToString());
                int i=1;
                foreach (var item in stock.StockLineItem)
                {
                    cmd1.Parameters["@SrNo"].Value=labSr;
                    cmd1.Parameters["@OItemId"].Value=labSr+"-"+i;
                    cmd1.Parameters["@PartyBillNo"].Value=item.Stock.Party.PartyBillNo;
                    cmd1.Parameters["@CastingWeight"].Value=item.Stock.CastingWeight;
                    cmd1.Parameters["@Waste"].Value=item.Stock.Waste;
                    cmd1.Parameters["@TotalWeight"].Value=item.Stock.TotalWeight;
                    cmd1.Parameters["@MaleNikla"].Value=item.Stock.MaleNikla;
                    cmd1.Parameters["@Pure"].Value=item.Stock.Pure;
                    cmd1.Parameters["@Advance"].Value=item.Stock.Advance;
                    cmd1.Parameters["@TotalPure"].Value=item.Stock.TotalPure;
                    cmd1.Parameters["@MaleType"].Value=item.Stock.MaleType;
                    cmd1.Parameters["@MaleCode"].Value=item.Stock.MaleCode;
                    cmd1.Parameters["@Mazdoori"].Value=item.Stock.Mazdoori;
                    cmd1.Parameters["@SabqaMazdoori"].Value=item.Stock.SabqaMazdoori;
                    cmd1.Parameters["@TotalMazdoori"].Value=item.Stock.TotalMazdoori;
                    cmd1.Parameters["@WasteDiscount"].Value=item.Stock.WasteDiscuont;
                    cmd1.Parameters["@MazdooriDiscount"].Value=item.Stock.MazdooriDiscount;
                    cmd1.Parameters["@GoldRate"].Value=item.Stock.GoldRate;
                    cmd1.Parameters["@RatePerGram"].Value=item.Stock.RatePerGram;
                    cmd1.Parameters["@OrderDate"].Value=item.Stock.OrderDate;
                    cmd1.Parameters["@PaymentMethod"].Value = item.Stock.PaymentMethod == null ? "" : item.Stock.PaymentMethod;
                    cmd1.Parameters["@Remarks"].Value = item.Stock.Remarks;
                    cmd1.ExecuteNonQuery();
                    i++;
                }

            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                //if (con.State == ConnectionState.Open)
                //    con.Close();                
            }

        }

        public List<Stock> GetOldParchi(Party p)
        {
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            SqlCommand cmd = new SqlCommand("select * from Stock s inner join Party p on p.AccountCode=s.AccountCode where s.AccountCode='" + p.AccountCode + "' order by s.OrderDate,s.SrNo ", con);
            cmd.CommandType = CommandType.Text;
            List<Stock> stockList = null;
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    stockList = new List<Stock>();
                    do
                    {
                        Stock stock = new Stock();
                        stock.SrNo = Convert.ToInt32(dr["SrNo"]);
                        stock.Party.Name = dr["Name"].ToString();
                        stock.Party.AccountCode = dr["AccountCode"].ToString();
                        stock.Party.PartyBillNo = Convert.ToInt32(dr["PartyBillNo"]);
                        stock.Remarks = dr["Remarks"].ToString();
                        stock.CastingWeight = Convert.ToDouble(dr["CastingWeight"]);
                        stock.Waste = Convert.ToDouble(dr["Waste"]);
                        stock.TotalWeight = Convert.ToDouble(dr["TotalWeight"]);
                        stock.MaleNikla = Convert.ToDouble(dr["MaleNikla"]);
                        stock.Pure = Convert.ToDouble(dr["Pure"]);
                        stock.Advance = Convert.ToDouble(dr["Advance"]);
                        stock.TotalPure = Convert.ToDouble(dr["TotalPure"]);
                        stock.MaleType = dr["MaleType"].ToString();
                        stock.MaleCode = Convert.ToDouble(dr["MaleCode"]);
                        stock.Mazdoori = Convert.ToInt32(dr["Mazdoori"]);
                        stock.SabqaMazdoori = Convert.ToInt32(dr["SabqaMazdoori"]);
                        stock.TotalMazdoori = Convert.ToInt32(dr["TotalMazdoori"]);
                        stock.WasteDiscuont = Convert.ToDouble(dr["WasteDiscount"]);
                        stock.MazdooriDiscount = Convert.ToDouble(dr["MazdooriDiscount"]);
                        stock.GoldRate = Convert.ToDouble(dr["GoldRate"]);
                        stock.RatePerGram = Convert.ToDouble(dr["RatePerGram"]);
                        stock.OrderDate = Convert.ToDateTime(dr["OrderDate"]);
                        stock.PaymentMethod = dr["PaymentMethod"].ToString();
                        stock.MazdooriBaqaya = Convert.ToInt32(dr["MazdooriBaqaya"]);
                        stock.SonaMazdooriBaqaya = Convert.ToDouble(dr["SonaMazdooriBaqaya"]);
                        stock.SonaSonaWasolKeya = Convert.ToDouble(dr["SonaSonaWasolKeya"]);
                        stock.SonaSonaWapisDeya = Convert.ToDouble(dr["SonaSonaWapisDeya"]);
                        stock.SonaSonaBaqaya2 = Convert.ToDouble(dr["SonaSonaBaqaya2"]);
                        stock.SonaSonaCashWasolKeya = Convert.ToDouble(dr["SonaSonaCashWasolKeya"]);
                        stock.SonaSonaBaqaya = Convert.ToDouble(dr["SonaSonaBaqaya"]);
                        stock.SMSonaWasolKeya = Convert.ToDouble(dr["SMSonaWasolKeya"]);
                        stock.SMSonaWapisDeya = Convert.ToDouble(dr["SMSonaWapisDeya"]);
                        stock.SMMazdooriWasol = Convert.ToInt32(dr["SMMazdooriWasol"]);
                        stock.SMMazdooriWapis = Convert.ToInt32(dr["SMMazdooriWapis"]);
                        stock.CashWasolKeya = Convert.ToInt32(dr["CashWasolKeya"]);
                        stock.CashWapisDeya = Convert.ToInt32(dr["CashWapisDeya"]);
                        stock.CashBaqaya = Convert.ToInt32(dr["CashBaqaya"]);
                        stock.ParchiFormate = stock.OrderDate.ToString("d") + "   " + stock.SrNo.ToString();
                        stockList.Add(stock);
                    } while (dr.Read());
                }
                dr.Close();
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
            return stockList;
        }

        public DataSet GetParchi(DateTime DateF, DateTime DateT, string accountCode)
        {
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("GetParchi", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@DateF", DateF));
                cmd.Parameters.Add(new SqlParameter("@DateT", DateT));
                cmd.Parameters.Add(new SqlParameter("@AccountCode", accountCode));
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds, "Parchi");
                //using (SqlDataReader dr=cmd.ExecuteReader())
                //{
                //    DataTable dt = new DataTable("Parchi");
                //    dt.Load(dr);
                //    ds.Tables.Add(dt);
                //}

                con.Close();
                return ds;
            }
            catch
            {
                return null;
            }
        }

        public Stock GetTotalStockDetail()
        {
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            SqlCommand cmd = new SqlCommand("GetStockDetail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            Stock stk = null;
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    stk = new Stock();
                    stk.CastingWeight = Convert.ToDouble(dr["TotalWeight"]);
                    stk.Waste = Convert.ToDouble(dr["TotalWaste"]);
                    stk.Mazdoori = Convert.ToDouble(dr["KhalisMazdoori"]);
                    stk.TotalMazdoori = Convert.ToDouble(dr["TotalMazdoori"]);
                    stk.Pure = Convert.ToDouble(dr["TotalKhalis"]);

                }
                dr.Close();
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
            return stk;
        }

        public DataSet ShowAllRecord()
        {
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter("ShowAllRecord", con);
                ad.Fill(ds, "ShowAllRecord");
                con.Close();
                return ds;
            }
            catch
            {
                return null;
            }


        }

        public DataSet MaxOrder()
        {
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter("MaxOrder", con);
                ad.Fill(ds, "MaxOrder");
                con.Close();
                return ds;
            }
            catch
            {
                return null;
            }
        }

        public DataSet MaxWaste()
        {
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter("MaxWaste", con);
                ad.Fill(ds, "MaxWaste");
                con.Close();
                return ds;
            }
            catch
            {
                return null;
            }
        }

        public DataSet LainaList()
        {
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter("LainaList", con);
                ad.Fill(ds, "LainaList");
                con.Close();
                return ds;
            }
            catch
            {
                return null;
            }
        }

        public DataSet DainaList()
        {
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter("DainaList", con);
                ad.Fill(ds, "DainaList");
                con.Close();
                return ds;
            }
            catch
            {
                return null;
            }
        }

        public DataSet ExpenseGoldList()
        {
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter("ExpenseGoldList", con);
                ad.Fill(ds, "ExpenseGoldList");
                con.Close();
                return ds;
            }
            catch
            {
                return null;
            }
        }

        public DataSet ExpenseCashList()
        {
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter("ExpenseCashList", con);
                ad.Fill(ds, "ExpenseCashList");
                con.Close();
                return ds;
            }
            catch
            {
                return null;
            }
        }

        public DataSet GoldList()
        {
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter("GoldList", con);
                ad.Fill(ds, "GoldList");
                con.Close();
                return ds;
            }
            catch
            {
                return null;
            }
        }
        public DataSet CashList()
        {
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter("CashList", con);
                ad.Fill(ds, "CashList");
                con.Close();
                return ds;
            }
            catch
            {
                return null;
            }
        }
        public void AddSTK(AdvanceLineItem stk)
        {
            string query = "insert into STK Values(@Gold,@Cash,@Date)";
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add("@Gold", SqlDbType.Decimal, 18).Value = stk.Gold;
            cmd.Parameters.AddWithValue("@Cash", stk.Cash);
            cmd.Parameters.AddWithValue("@Date", stk.VDate);
            cmd.CommandType = CommandType.Text;
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

        public AdvanceLineItem ShowStock()
        {
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            SqlCommand cmd = new SqlCommand("ShowStock", con);
            cmd.CommandType = CommandType.StoredProcedure;
            AdvanceLineItem stk = null;
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                if (dr.Read())
                {
                    if (stk == null) stk = new AdvanceLineItem();
                    stk.Gold = Convert.ToDouble(dr["Gold"]);
                    stk.Cash = Convert.ToInt32(dr["Cash"]);
                    stk.VDate = Convert.ToDateTime(dr["Date"]);
                }
                dr.Close();
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
            return stk;
        }

        public Voucher ShowLainaDaina()
        {
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            SqlCommand cmd = new SqlCommand(@"select p.AccountCode, ((select isnull(sum(Dr),0) from Vouchers where AccountCode=p.AccountCode)-(select isnull(sum(Cr),0) from Vouchers  where AccountCode=p.AccountCode)) as CBalance,
                                           ((select isnull(sum(GoldDr),0) from Vouchers where AccountCode=p.AccountCode)-(select isnull(sum(GoldCr),0) from Vouchers  where AccountCode=p.AccountCode)) as GBalance 
                                               from Party p group by p.AccountCode", con);
            cmd.CommandType = CommandType.Text;
            Voucher v = null;
            long CashLaina = 0, CashDaina = 0;
            double GoldLaina = 0, GoldDaina = 0;
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    if (v == null)
                        v = new Voucher();
                    do
                    {
                        long CBalance = Convert.ToInt32(dr["CBalance"]);
                        double GBalance = Convert.ToDouble(dr["GBalance"]);
                        if (CBalance < 0)
                            CashDaina += (CBalance * -1);
                        else
                            CashLaina += CBalance;
                        if (GBalance < 0)
                            GoldDaina += GBalance * -1;
                        else
                            GoldLaina += GBalance;

                    } while (dr.Read());

                    v.Cr = CashLaina;
                    v.Dr = CashDaina;
                    v.GoldCr = GoldLaina;
                    v.GoldDr = GoldDaina;
                }
                dr.Close();
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
            return v;
        }

    }
}
