using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;


namespace WebAPI.Services
{
    public class MessageHelper
    {
        public static void SendSMS(String message, string mobile, string lang, string mask)
        {
            try
            {
                message =  System.Web.HttpUtility.UrlEncode(message);
                string uri = "http://outreach.pk/api/sendsms.php/sendsms/url";
                //string NewMsg = mobile.Remove(0, 1);
                String strPost = "id=rchzulfiqarjew&pass=jeweller41&msg=" + message + "&to=92" + (Convert.ToInt64(mobile)).ToString() + "&mask=" + mask + " &type=xml&lang=" + lang;
                string res = readHtmlPage(uri, strPost);
            }

            catch (Exception ex)
            {
                string es = ex.Message.ToString();
            }
        }
        public static String readHtmlPage(string url, string strPost)
        {
            String result = "";//, simpleHTMLGet = ""; int myPos1;

            StreamWriter myWriter = null;
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
            objRequest.Method = "POST";
            objRequest.ContentLength = Encoding.UTF8.GetByteCount(strPost);
            objRequest.ContentType = "application/x-www-form-urlencoded";
            try
            {
                myWriter = new StreamWriter(objRequest.GetRequestStream()); myWriter.Write(strPost);
            }
            catch (Exception e) { return e.Message; }
            finally { myWriter.Close(); }

            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd(); // Close and clean up the StreamReader
                //MessageBox.Show(result);
                sr.Close();
            }
            return result;
        }
        public static double GetSMSCount()
        {
            int myPos1; string result = "";
            string uri = "http://outreach.pk/api/sendsms.php/balance/status";
            String strPost = "id=rchzulfiqarjew&pass=jeweller41";
            string res = readHtmlPage(uri, strPost);
            myPos1 = Convert.ToInt32(res.IndexOf("<response>"));
            result = res.Substring(myPos1 + 10, 6);
            double totaolmsg = Convert.ToDouble(result) / 0.30;
            //string results = totaolmsg.ToString();
            return totaolmsg;
        }
    }
}