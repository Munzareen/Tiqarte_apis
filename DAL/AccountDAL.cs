using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using BusinesEntities;


namespace DAL
{
    public class AccountDAL
    {
        ParentAccount p;
        ChildAccount c;
        ChildAccount chld = new ChildAccount();
        string creatParent = "AddParentAccount";
        string creatChild = "AddChildAccount";
        //SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        public ChildAccount GetAccount(int headcode, string parent, string childname, SqlConnection con, SqlTransaction trans)
        {
            ChildAccount c1 = new ChildAccount();
            c1.ChildName = childname;
            c1.HeadCode = headcode;
            c1.ParentCode = this.GetParentCode(c1.HeadCode.ToString(), parent, con, trans);
            c1.ChildCode = this.GetChildCode(c1.ParentCode, childname, con, trans);
            c1.Balance = 0;// this.GetCashInHandBalance(childname, con, trans);
            if (!(this.isChildExist(c1.ChildCode, con, trans)))
            {
                c1 = null;
            }
            return c1;
        }

        public string GetParentCode(string sgCode, string pName, SqlConnection con, SqlTransaction trans)
        {
            string selectsql = "select ParentCode from ParentAccounts where ParentName='" + pName + "'and HeadCode='" + sgCode + "'";
            SqlCommand cmd = new SqlCommand(selectsql, con);
            cmd.Transaction = trans;
            string str = "";
            SqlDataReader dr = null;
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    str = dr["ParentCode"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dr.Close();
            }
            return str;
        }

        public string GetChildCode(string pCode, string chName, SqlConnection con, SqlTransaction trans)
        {
            string selectsql = "select ChildCode from ChildAccounts where ChildName='" + chName + "'and ParentCode='" + pCode + "'";
            SqlCommand cmd = new SqlCommand(selectsql, con);
            cmd.Transaction = trans;
            string str = "";
            SqlDataReader dr = null;
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    str = dr["ChildCode"].ToString();
                }
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                dr.Close();
            }
            return str;
        }

        public bool isChildExist(string code, SqlConnection con, SqlTransaction trans)
        {
            string qurry = "select ChildCode from ChildAccounts where ChildCode='" + code + "'";
            SqlCommand cmd = new SqlCommand(qurry, con);
            cmd.Transaction = trans;
            bool bFlag = false;
            SqlDataReader dr = null;
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
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
            }
            return bFlag;
        }

        public string CreateAccount(int hcode, string pname, string chname, int type, int Shopid, double opCash, SqlConnection con, SqlTransaction trans)
        {
            p = new ParentAccount();
            p = this.GetParent(pname, hcode.ToString(), con, trans);
            if (p == null)
            {
                p = new ParentAccount();
                p.HeadCode = hcode;
                p.ParentName = pname;
                p.ParentCode = this.CreateParentCode(p.HeadCode);
                this.CreateParentAccount(p);
            }
          
                c = new ChildAccount();
                c.HeadCode = p.HeadCode;
                c.ParentCode = p.ParentCode;
                c.ChildName = chname;
                c.ChildCode = this.CreateChildCode(c.ParentCode, c.HeadCode,con,trans);
                c.Date = DateTime.Today;
                c.Status = "";
                c.Description = "";
                c.AccountType = type;
                c.Balance = 0;
                c.OpCash = opCash;
                c.OpGold = 0;
                this.CreateChildAccount(c, true,con,trans);
            
            return c.ChildCode;
        }
       
        public ParentAccount GetParent(string pName, string sgCode, SqlConnection con, SqlTransaction trans)
        {
            string selectSql = "select * from ParentAccounts where ParentName='" + pName + "'and HeadCode='" + sgCode + "'";
            SqlCommand cmd = new SqlCommand(selectSql, con);
            //by me 
         //   con.Open();
            cmd.Transaction = trans;
            ParentAccount p = null;
            SqlDataReader dr = null;
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    if (p == null) p = new ParentAccount();
                    p.HeadCode = Convert.ToInt32(dr["HeadCode"]);
                    p.ParentCode = dr["ParentCode"].ToString();
                    p.ParentName = dr["ParentName"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dr.Close();
           //     con.Close();
            }
            return p;
        }

        public string CreateParentCode(int headCode )
        {
            string selectSQL = "select ParentCode from ParentAccounts where HeadCode=" + headCode + " order by ParentName";
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            SqlCommand cmd = new SqlCommand(selectSQL, con);
        
            int maxCode = 1;
            string code;
            string s = "";
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    do
                    {
                        s = dr["ParentCode"].ToString();
                        int c = Convert.ToInt32(s.Remove(0, 3));
                        if (c > maxCode)
                            maxCode = c;
                    }
                    while (dr.Read());
                    maxCode = maxCode + 1;
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally {
                //con.Close(); 
            }
            code = string.Format("{0:000}", maxCode);
            code = headCode + "-" + code;
            return code;
        }

        public bool CreateParentAccount(ParentAccount p)
        {
            string selectSql = "select ParentName from ParentAccounts where ParentName='" + p.ParentName + "'and HeadCode=" + p.HeadCode;
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            SqlCommand cmd = new SqlCommand(creatParent, con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlCommand cmdname = new SqlCommand(selectSql, con);
            cmdname.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@HeadCode", p.HeadCode));
            cmd.Parameters.Add(new SqlParameter("@ParentCode", p.ParentCode));
            cmd.Parameters.Add(new SqlParameter("@ParentName", p.ParentName));
            SqlDataReader dr = null;
            bool bFlag = true;
            try
            {
                con.Open();
                dr = cmdname.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Close();
                    con.Close();
                    bFlag = false;
                    return bFlag;
                }
                else
                {
                    dr.Close();
                    cmd.ExecuteNonQuery();
                }
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

        public ChildAccount GetChild(string cName, string pCode, SqlConnection con, SqlTransaction trans)
        {
            string selectSql = "select * from ChildAccount where ChildName='" + cName + "'and ParentCode='" + pCode + "'";
            SqlCommand cmd = new SqlCommand(selectSql, con);
            cmd.Transaction = trans;
            ChildAccount c = null;
            SqlDataReader dr = null;
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    if (c == null) c = new ChildAccount();
                    c.HeadCode = Convert.ToInt32(dr["HeadCode"]);
                    c.ChildCode = dr["ChildCode"].ToString();
                    c.ChildName = dr["ChildName"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dr.Close();
            }
            return c;
        }

         public string CreateChildCode(string pCode, int headCode,SqlConnection con,SqlTransaction trans)
        {
            string selectSQL = "select * from ChildAccounts where ChildCode=(select  Max(ChildCode) from ChildAccounts where ParentCode='" + pCode + "' and HeadCode='" + headCode + "')";
            SqlCommand cmd = new SqlCommand(selectSQL, con);
            cmd.Transaction = trans;
            int maxCode = 1;
            string code;
            string sgcode;
            string c;
            try
            {
              //  con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    do
                    {
                        c = dr["ChildCode"].ToString();
                        int a = Convert.ToInt32(c.Remove(0, 6));
                        if (a > maxCode)
                            maxCode = a;
                    }
                    while (dr.Read());
                    maxCode = maxCode + 1;
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { //con.Close();
            }
            code = string.Format("{0:00000}", maxCode);
            sgcode = pCode + "-" + code;
            return sgcode;
        }

         public bool CreateChildAccount(ChildAccount c, bool b, SqlConnection con, SqlTransaction trans)
         {
             string selectSql = "select ChildName from ChildAccounts where ChildName='" + c.ChildName + "'and HeadCode=" + c.HeadCode;

             SqlCommand cmd = new SqlCommand(creatChild, con);
             cmd.CommandType = CommandType.StoredProcedure;
             cmd.Transaction = trans;
             SqlCommand cmdname = new SqlCommand(selectSql, con);
             cmdname.CommandType = CommandType.Text;
             cmdname.Transaction = trans;
             cmd.Parameters.Add(new SqlParameter("@HeadCode", c.HeadCode));
             cmd.Parameters.Add(new SqlParameter("@ParentCode", c.ParentCode));
             cmd.Parameters.Add(new SqlParameter("@ChildCode", c.ChildCode));
             cmd.Parameters.Add(new SqlParameter("@ChildName", c.ChildName));
             cmd.Parameters.Add(new SqlParameter("@Status", c.Status));
             cmd.Parameters.Add(new SqlParameter("@Type", c.AccountType));
             cmd.Parameters.Add(new SqlParameter("@Date", c.Date));
             cmd.Parameters.Add(new SqlParameter("@Description", c.Description));
             cmd.Parameters.Add(new SqlParameter("@OpeningCash", c.OpCash));
             cmd.Parameters.Add(new SqlParameter("@OpeningGold", c.OpGold));
             //SqlDataReader dr = null;
             bool bFlag = true;
             try
             {
                 //  con.Open();
                 cmd.ExecuteNonQuery();
             }
             catch (Exception ex)
             {
                 throw ex;
             }
             finally
             {
                 //if (con.State == ConnectionState.Open)
                 //    con.Close();
             }
             return bFlag;
         }
       

        public ChildAccount GetChildByCode(string ChildCode)
        {
            string selectSql = "select * from ChildAccount where ChildCode='" + ChildCode + "'";
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            SqlCommand cmd = new SqlCommand(selectSql, con);
            ChildAccount c = null;
            SqlDataReader dr = null;
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    if (c == null) c = new ChildAccount();
                    c.HeadCode = Convert.ToInt32(dr["HeadCode"]);
                    c.ParentCode = dr["ParentCode"].ToString();
                    c.ChildCode = dr["ChildCode"].ToString();
                    c.ChildName = dr["ChildName"].ToString();
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
            return c;
        }
        public DataTable getData(string ProcedureName)
        {
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            DataTable ds = new DataTable();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(ProcedureName, con);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                //if (ProcedureName == "OneMonthRec" || ProcedureName == "DabbiSona")
                //{
                //    cmd.Parameters.Add(new SqlParameter("@DFrom", from));
                //    cmd.Parameters.Add(new SqlParameter("@DTo", to));
                //}
                ad.Fill(ds);

                con.Close();
                return ds;
            }
            catch
            {
                //throw ex;
                return null;
            }
        }

        public ChildAccount GetChildByCode(string ChildCode, SqlConnection con, SqlTransaction trans)
        {
            string selectSql = "select * from ChildAccounts where ChildCode='" + ChildCode + "'";
            SqlCommand cmd = new SqlCommand(selectSql, con);
            cmd.Transaction = trans;
            ChildAccount c = null;
            SqlDataReader dr = null;
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    if (c == null) c = new ChildAccount();
                    c.HeadCode = Convert.ToInt32(dr["HeadCode"]);
                    c.ParentCode = dr["ParentCode"].ToString();
                    c.ChildCode = dr["ChildCode"].ToString();
                    c.ChildName = dr["ChildName"].ToString();
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return c;
        }

        public void DeleteAccount(string query)
        {
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            SqlCommand cmd = new SqlCommand(query, con);
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
                    cmd.Dispose();
                }
            }
        }

        public void UpdateParent(string pcode, string name)
        {
            string querry = "UpdateParent";
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            SqlCommand cmd = new SqlCommand(querry, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@oldParentCode", SqlDbType.NVarChar).Value = pcode;
            cmd.Parameters.Add(new SqlParameter("@ParentName", name));
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
                    cmd.Dispose();
                }
            }
        }

        public void UpdateChild(ChildAccount cha)
        {
            string querry = "UpdateChildAccount";
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            SqlCommand cmd = new SqlCommand(querry, con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.Add("@oldChildCode", SqlDbType.NVarChar).Value = ccode;
            //cmd.Parameters.Add(new SqlParameter("@ChildName", cha.ChildName));
            //cmd.Parameters.Add(new SqlParameter("@Type", cha.AccountType));
            //cmd.Parameters.Add(new SqlParameter("@Status", cha.Status));
            cmd.Parameters.Add(new SqlParameter("@OpeningCash", cha.OpCash));
            cmd.Parameters.Add(new SqlParameter("@OpeningGold", cha.OpGold));
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
                    cmd.Dispose();
                }
            }
        }

        public List<ParentAccount> GetParentByHeadCode(int hCode)
        {
            string selectsql = "select * from ParentAccount where HeadCode=" + hCode + " order by ParentName";
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            SqlCommand cmd = new SqlCommand(selectsql, con);
            List<ParentAccount> gl = null;
            ParentAccount g = null;
            SqlDataReader dr = null;
            try
            {
                con.Open();
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    gl = new List<ParentAccount>();
                    do
                    {
                        g = new ParentAccount();
                        g.HeadCode = Convert.ToInt32(dr["HeadCode"]);
                        g.ParentName = dr["ParentName"].ToString();
                        g.ParentCode = dr["ParentCode"].ToString();
                        gl.Add(g);
                    } while (dr.Read());
                    dr.Close();

                }
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
            return gl;
        }

        public List<ChildAccount> GetAllChildAccountsByPCode(string pCode)
        {
            string selectSql = "select * from ChildAccount where ParentCode='" + pCode + "' order by ChildName";
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            SqlCommand cmd = new SqlCommand(selectSql, con);
            List<ChildAccount> childs = null;
            SqlDataReader dr = null;
            try
            {
                con.Open();
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    childs = new List<ChildAccount>();
                    do
                    {
                        ChildAccount c = new ChildAccount();
                        c.HeadCode = Convert.ToInt32(dr["HeadCode"]);
                        c.ParentCode = dr["ParentCode"].ToString();
                        c.ChildCode = dr["ChildCode"].ToString();
                        c.ChildName = dr["ChildName"].ToString();
                        if (dr["OpeningCash"] == DBNull.Value)
                            c.OpCash = 0;
                        else
                            c.OpCash = Convert.ToDouble(dr["OpeningCash"]);
                        if (dr["OpenningGold"] == DBNull.Value)
                            c.OpGold = 0;
                        else
                            c.OpGold = Convert.ToDouble(dr["OpenningGold"]);
                        childs.Add(c);

                    } while (dr.Read());
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
            return childs;
        }

        public List<ChildAccount> GetAllChildAccounts()
        {
            string selectSql = "select * from ChildAccount order by ChildName";
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            SqlCommand cmd = new SqlCommand(selectSql, con);
            List<ChildAccount> childs = null;
            SqlDataReader dr = null;
            try
            {
                con.Open();
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    childs = new List<ChildAccount>();
                    do
                    {
                        ChildAccount c = new ChildAccount();
                        c.HeadCode = Convert.ToInt32(dr["HeadCode"]);
                        c.ParentCode = dr["ParentCode"].ToString();
                        c.ChildCode = dr["ChildCode"].ToString();
                        c.ChildName = dr["ChildName"].ToString();
                        if (dr["OpeningCash"] == DBNull.Value)
                            c.OpCash = 0;
                        else
                            c.OpCash = Convert.ToDouble(dr["OpeningCash"]);
                        if (dr["OpeningGold"] == DBNull.Value)
                            c.OpGold = 0;
                        else
                            c.OpGold = Convert.ToDouble(dr["OpeningGold"]);
                        childs.Add(c);

                    } while (dr.Read());
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
            return childs;
        }
    }
}
