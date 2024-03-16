using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Helpers
{
    public class StockHelper
    {
        public string GenrateTagNo(int id, string str)
        {
            //int i = 0;

            //ADORecordSetHelper Rstemp = new ADORecordSetHelper("");
            string selectItemName = "Select stk.ItemId ,stk.TagNo,(Select Abrivation from Items Where ItemId = stk.ItemId)'Abrivation' from Stocks stk  " +
                                       "Where stk.ItemId =" + id;
            // string selectItemName = "SelectMaxStockId";
            //  string selectAbri = "select Abrivation from Item where ItemId= " + id;

            string abrivation = "";
            string TagNo;
            int no = 0;


            string tag;
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            SqlCommand cmd = new SqlCommand(selectItemName, con);
            //cmd.CommandType = CommandType.StoredProcedure;
            // SqlCommand cmdAbri = new SqlCommand(selectAbri, con);
            SqlDataReader dr = null;
            try
            {
                con.Open();
                dr = cmd.ExecuteReader();

                if (dr.HasRows == true)
                {
                    if (dr.Read())
                    {
                        do
                        {
                            tag = dr["TagNo"].ToString();
                            abrivation = dr["Abrivation"].ToString();
                            if (no < Convert.ToInt32(tag.Remove(0, abrivation.Length)))
                                no = Convert.ToInt32(tag.Remove(0, abrivation.Length));
                        }
                        while (dr.Read());
                        no += 1;

                    }
                }

                dr.Close();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                con.Close();
            }
            if (no > 9999)
                tag = string.Format("{0:00000}", no);
            else
                if (abrivation == "")
            {
                tag = str + "00001";
            }
            else
            {
                tag = string.Format("{0:00000}", no);
            }
            //  tag=
            TagNo = abrivation.ToString() + tag.ToString();
            return TagNo;

            //Rstemp.Close();
            //a = StringsHelper.Format(a, "0000");
            //return Abrivaton + a;


        }
    }
}
