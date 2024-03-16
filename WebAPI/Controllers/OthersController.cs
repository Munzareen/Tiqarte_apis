using BusinesEntities;
using DAL;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class OthersController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [Route("api/User/OTP")]
        [HttpPost]
        public HttpResponseMessage Generate_OTP(string mobile)
        {
            char[] charArr = "0123456789".ToCharArray();
            string strrandom = string.Empty;
            Random objran = new Random();
            for (int i = 0; i < 6; i++)
            {
                //It will not allow Repetation of Characters
                int pos = objran.Next(1, charArr.Length);
                if (!strrandom.Contains(charArr.GetValue(pos).ToString())) strrandom += charArr.GetValue(pos);
                else i--;
            }
            string res = SendSMS(mobile, strrandom);
            //HttpContext.Current.Session["otp"] = strrandom;

            //return this.Request.CreateResponse(HttpStatusCode.OK, res);
            return this.Request.CreateResponse(HttpStatusCode.OK, new { message = "otp generated", otp = strrandom });
        }

        public static string SendSMS(string mobile, string message)
        {
            String msg = HttpUtility.UrlEncode(message);

            //string uri = "http://my.ezeee.pk/sendsms_url.html?";
            //String strPost = "Username=03210000128&Password=03210000128&From=SAFEER GOLD&To=0" + (Convert.ToInt64(mobile)).ToString() + "&Message=" + message;
            string uri = "http://www.outreach.pk/api/sendsms.php/sendsms/url";
            String strPost = "id=rchalnaseeb&pass=naseeb720&msg=" + message + " is Your Al-Naseeb OTP, please don't share it with anyone.&to=92" + (Convert.ToInt64(mobile)).ToString() + "&mask=Al Naseeb&type=xml&lang=English";
            string strResponce = ReadHtmlPage(uri, strPost);

            return strResponce;
        }

        public static string GetResponse(string smsURL)
        {
            try
            {
                WebClient objWebClient = new WebClient();
                System.IO.StreamReader reader = new System.IO.StreamReader(objWebClient.OpenRead(smsURL));
                string ResultHTML = reader.ReadToEnd();
                return ResultHTML;
            }
            catch (Exception)
            {
                return "Fail";
            }
        }

        public static String ReadHtmlPage(string url, string strPost)
        {
            String result = "";//, simpleHTMLGet = ""; int myPos1;

            System.IO.StreamWriter myWriter = null;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
            objRequest.Method = "POST";
            objRequest.ContentLength = Encoding.UTF8.GetByteCount(strPost);
            objRequest.ContentType = "application/x-www-form-urlencoded";
            try
            {
                myWriter = new System.IO.StreamWriter(objRequest.GetRequestStream()); myWriter.Write(strPost);
            }
            catch (Exception e) { return e.Message; }
            finally { myWriter.Close(); }

            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            using (System.IO.StreamReader sr = new System.IO.StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd(); // Close and clean up the StreamReader
                                         //MessageBox.Show(result);
                sr.Close();
            }
            return result;
        }

        [Route("api/user/resendOTP")]
        [HttpGet]
        public HttpResponseMessage ResendOTP(string userID)
        {
            var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var UserManager = new UserManager<ApplicationUser>(userStore);
            //Return userId and authentication code
            return Request.CreateResponse(HttpStatusCode.OK, new { message = "Successfully Updated" }, userID);
        }

        [HttpGet]
        [Route("api/GetUserClaims")]
        public AccountModel GetUserClaims()
        {
            var identityClaims = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;
            AccountModel model = new AccountModel()
            {
                UserName = identityClaims.FindFirst("Username").Value,
                Email = identityClaims.FindFirst("Email").Value,
                FirstName = identityClaims.FindFirst("FirstName").Value,
                LastName = identityClaims.FindFirst("LastName").Value,
                LoggedOn = identityClaims.FindFirst("LoggedOn").Value
            };
            return model;
        }

        [HttpPost]
        [Route("api/user/defineRole")]
        public async Task<IHttpActionResult> PostRoleAsync([FromBody] RootObject postData)
        {
            ApplicationDbContext _applicationDbContext = new ApplicationDbContext();

            if (postData.action == "Create")
            {
                if (_applicationDbContext.Roles.Any(c => c.Name == postData.category))
                {
                    return Ok("Role Already Exist");
                }
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_applicationDbContext));
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_applicationDbContext));
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = postData.category;
                roleManager.Create(role);
            }


            if (_applicationDbContext.MenuMaster.Any(c => c.User_Roll == postData.category))
            {

                _applicationDbContext.MenuMaster.RemoveRange(_applicationDbContext.MenuMaster.Where(x => x.User_Roll == postData.category));
                _applicationDbContext.SaveChanges();
                _applicationDbContext.UserPermissions.RemoveRange(_applicationDbContext.UserPermissions.Where(x => x.RoleName == postData.category));
                _applicationDbContext.SaveChanges();

                foreach (var item in postData.mainmenus)
                {
                    MenuMaster menuMaster = new MenuMaster();
                    menuMaster.PageName = item.name;
                    menuMaster.icion = item.icon;
                    menuMaster.MenuID = item.name;
                    menuMaster.Parent_MenuID = "*";
                    menuMaster.User_Roll = postData.category;
                    menuMaster.USE_YN = "Y";
                    menuMaster.IsActive = true;
                    menuMaster.ControllerName = item.components.Select(c => c.controller).FirstOrDefault();
                    menuMaster.ActionName = item.components.Select(c => c.action).FirstOrDefault();
                    _applicationDbContext.MenuMaster.Add(menuMaster);

                    int b = _applicationDbContext.SaveChanges();

                    if (item.components.Count > 0)
                    {
                        if (b > 0)
                        {
                            //string connectionString = _configuration.GetConnectionString("DefaultConnection");
                            string commandText = "INSERT INTO[dbo].[MenuMasters]([PageName] ,[Parent_MenuID] ,[User_Roll] ,[ActionName] ,[ControllerName] ,[CreatedDate] ,[IsActive] ,[icion])" +
                                " VALUES(@PageName,@Parent_MenuID,@User_Roll,@ActionName,@ControllerName,@CreatedDate,@IsActive,@icion)";

                            using (SqlConnection con = new SqlConnection(DALHelper.ConnectionString))
                            {
                                con.Open();
                                SqlCommand cmd = new SqlCommand(commandText, con);
                                cmd.Parameters.Add("@PageName", SqlDbType.NVarChar);
                                cmd.Parameters.Add("@Parent_MenuID", SqlDbType.NVarChar);
                                cmd.Parameters.Add("@User_Roll", SqlDbType.NVarChar);
                                cmd.Parameters.Add("@ActionName", SqlDbType.NVarChar);
                                cmd.Parameters.Add("@ControllerName", SqlDbType.NVarChar);
                                cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime);
                                cmd.Parameters.Add("@IsActive", SqlDbType.Bit);
                                cmd.Parameters.Add("@icion", SqlDbType.NVarChar);
                                cmd.CommandType = CommandType.Text;

                                for (int j = 0; j < item.components.Count; j++)
                                {
                                    cmd.Parameters["@PageName"].Value = item.components[j].name;
                                    cmd.Parameters["@Parent_MenuID"].Value = item.name;
                                    cmd.Parameters["@User_Roll"].Value = postData.category;
                                    cmd.Parameters["@ActionName"].Value = item.components[j].action;
                                    cmd.Parameters["@ControllerName"].Value = item.components[j].controller;
                                    cmd.Parameters["@CreatedDate"].Value = DateTime.Now;
                                    cmd.Parameters["@IsActive"].Value = true;
                                    cmd.Parameters["@icion"].Value = item.components[j].icon;
                                    try
                                    {
                                        cmd.ExecuteNonQuery();
                                    }
                                    catch (Exception)
                                    {
                                        throw;
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (var item2 in postData.mainmenus)
                {
                    UserPermission userPermissions = new UserPermission();

                    string RoutName = "/" + item2.components.Select(c => c.controller).FirstOrDefault() + "/" + item2.components.Select(c => c.action).FirstOrDefault();
                    userPermissions.RoleName = postData.category;
                    userPermissions.Route = RoutName;
                    _applicationDbContext.UserPermissions.Add(userPermissions);
                    string commandTextRoute = "INSERT INTO[dbo].[UserPermissions]([RoleName] ,[Route] ,[IsActive],[Permission])" +
                " VALUES(@RoleName,@Route,@IsActive,@Permission)";
                    using (SqlConnection con = new SqlConnection(DALHelper.ConnectionString))
                    {
                        con.Open();
                        SqlCommand cmd2 = new SqlCommand(commandTextRoute, con);
                        cmd2.Parameters.Add("@RoleName", SqlDbType.NVarChar);
                        cmd2.Parameters.Add("@Route", SqlDbType.NVarChar);
                        cmd2.Parameters.Add("@IsActive", SqlDbType.Bit);
                        cmd2.Parameters.Add("@Permission", SqlDbType.NVarChar);
                        cmd2.CommandType = CommandType.Text;
                        for (int k = 0; k < item2.components.Count; k++)
                        {
                            string Route = "/" + item2.components[k].controller + "/" + item2.components[k].action;
                            cmd2.Parameters["@RoleName"].Value = postData.category;
                            cmd2.Parameters["@Route"].Value = Route;
                            cmd2.Parameters["@IsActive"].Value = true;
                            if (item2.components[k].Permission == "")
                            {
                                cmd2.Parameters["@Permission"].Value = "None";
                            }
                            else
                            {
                                cmd2.Parameters["@Permission"].Value = item2.components[k].Permission;
                            }

                            try
                            {
                                cmd2.ExecuteNonQuery();
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        }
                        //       // test.Add(myArry);
                        //   }
                        //   if (con.State == ConnectionState.Open)
                        //   {
                        //       con.Close();
                        //   }
                    }
                }
                return Ok("Update");
            }
            else
            {
                foreach (var item in postData.mainmenus)
                {
                    MenuMaster menuMaster = new MenuMaster();
                    menuMaster.PageName = item.name;
                    menuMaster.icion = item.icon;
                    menuMaster.MenuID = item.name;
                    menuMaster.Parent_MenuID = "*";
                    menuMaster.User_Roll = postData.category;
                    menuMaster.USE_YN = "Y";
                    menuMaster.IsActive = true;
                    menuMaster.ControllerName = item.components.Select(c => c.controller).FirstOrDefault();
                    menuMaster.ActionName = item.components.Select(c => c.action).FirstOrDefault();
                    _applicationDbContext.MenuMaster.Add(menuMaster);

                    int b = _applicationDbContext.SaveChanges();

                    if (item.components.Count > 1)
                    {
                        if (b > 0)
                        {
                            //string connectionString = _configuration.GetConnectionString("DefaultConnection");
                            string commandText = "INSERT INTO[dbo].[MenuMasters]([PageName] ,[Parent_MenuID] ,[User_Roll] ,[ActionName] ,[ControllerName] ,[CreatedDate] ,[IsActive] ,[icion])" +
                                " VALUES(@PageName,@Parent_MenuID,@User_Roll,@ActionName,@ControllerName,@CreatedDate,@IsActive,@icion)";

                            using (SqlConnection con = new SqlConnection(DALHelper.ConnectionString))
                            {
                                con.Open();
                                SqlCommand cmd = new SqlCommand(commandText, con);
                                cmd.Parameters.Add("@PageName", SqlDbType.NVarChar);
                                cmd.Parameters.Add("@Parent_MenuID", SqlDbType.NVarChar);
                                cmd.Parameters.Add("@User_Roll", SqlDbType.NVarChar);
                                cmd.Parameters.Add("@ActionName", SqlDbType.NVarChar);
                                cmd.Parameters.Add("@ControllerName", SqlDbType.NVarChar);
                                cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime);
                                cmd.Parameters.Add("@IsActive", SqlDbType.Bit);
                                cmd.Parameters.Add("@icion", SqlDbType.NVarChar);
                                cmd.CommandType = CommandType.Text;

                                for (int j = 0; j < item.components.Count; j++)
                                {

                                    cmd.Parameters["@PageName"].Value = item.components[j].name;
                                    cmd.Parameters["@Parent_MenuID"].Value = item.name;
                                    cmd.Parameters["@User_Roll"].Value = postData.category;
                                    cmd.Parameters["@ActionName"].Value = item.components[j].action;
                                    cmd.Parameters["@ControllerName"].Value = item.components[j].controller;
                                    cmd.Parameters["@CreatedDate"].Value = DateTime.Now;
                                    cmd.Parameters["@IsActive"].Value = true;
                                    cmd.Parameters["@icion"].Value = item.components[j].icon;
                                    try
                                    {
                                        cmd.ExecuteNonQuery();
                                    }
                                    catch (Exception)
                                    {
                                        throw;
                                    }
                                }
                            }
                        }
                    }

                }
                foreach (var item2 in postData.mainmenus)
                {
                    UserPermission userPermissions = new UserPermission();

                    string RoutName = "/" + item2.components.Select(c => c.controller).FirstOrDefault() + "/" + item2.components.Select(c => c.action).FirstOrDefault();

                    userPermissions.RoleName = postData.category;
                    userPermissions.Route = RoutName;

                    _applicationDbContext.UserPermissions.Add(userPermissions);
                    string commandTextRoute = "INSERT INTO[dbo].[UserPermissions]([RoleName] ,[Route] ,[IsActive],[Permission])" +
                " VALUES(@RoleName,@Route,@IsActive,@Permission)";
                    using (SqlConnection con = new SqlConnection(DALHelper.ConnectionString))
                    {
                        con.Open();
                        SqlCommand cmd2 = new SqlCommand(commandTextRoute, con);
                        cmd2.Parameters.Add("@RoleName", SqlDbType.NVarChar);
                        cmd2.Parameters.Add("@Route", SqlDbType.NVarChar);
                        cmd2.Parameters.Add("@IsActive", SqlDbType.Bit);
                        cmd2.Parameters.Add("@Permission", SqlDbType.NVarChar);
                        cmd2.CommandType = CommandType.Text;
                        for (int k = 0; k < item2.components.Count; k++)
                        {
                            string Route = "/" + item2.components[k].controller + "/" + item2.components[k].action;
                            cmd2.Parameters["@RoleName"].Value = postData.category;
                            cmd2.Parameters["@Route"].Value = Route;
                            cmd2.Parameters["@IsActive"].Value = true;
                            if (item2.components[k].Permission == "")
                            {
                                cmd2.Parameters["@Permission"].Value = "None";
                            }
                            else
                            {
                                cmd2.Parameters["@Permission"].Value = item2.components[k].Permission;
                            }

                            try
                            {
                                cmd2.ExecuteNonQuery();
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        }
                        //       // test.Add(myArry);
                        //   }
                        //   if (con.State == ConnectionState.Open)
                        //   {
                        //       con.Close();
                        //   }
                    }
                }
                return Ok("Save");
            }
            //return BadRequest("Invalid");
            //return Ok("Successfull");
        }

        [HttpGet]
        [Route("api/user/UseroleList")]
        public HttpResponseMessage GetList1(string name)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var res = db.MenuMaster.Where(c => c.User_Roll == name).ToList();
            return this.Request.CreateResponse(HttpStatusCode.OK, res);
        }

        [HttpGet]
        [Route("api/user/RoleEdit")]
        public HttpResponseMessage RoleEdit(string name)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var res = db.UserPermissions.Where(c => c.RoleName == name).Select(c => new { c.Route, c.Permission }).ToList();
            return this.Request.CreateResponse(HttpStatusCode.OK, res);
        }

        [HttpGet]
        [Route("api/user/UseroleList1")]
        public HttpResponseMessage GetList(string name)
        {
            SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
            SqlCommand cmd = new SqlCommand("GetUserRoleList", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
            var jsonResult = new StringBuilder();
            try
            {
                con.Open();
                var reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    jsonResult.Append("[]");
                }
                else
                {
                    while (reader.Read())
                    {
                        jsonResult.Append(reader.GetValue(0).ToString());
                    }

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
            var json = JsonConvert.DeserializeObject(jsonResult.ToString());
            return this.Request.CreateResponse(HttpStatusCode.OK, json);
        }

        [HttpGet]
        [Route("api/user/RolePermission")]
        public HttpResponseMessage RolePermission(string name)
        {
            ApplicationDbContext _applicationDbContext = new ApplicationDbContext();
            var res = _applicationDbContext.UserPermissions.Where(c => c.RoleName == name).Select(c => c.Route).ToList();
            return this.Request.CreateResponse(HttpStatusCode.OK, res);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("api/ForAdminRole")]
        public string ForAdminRole()
        {
            return "for admin role";
        }

        [HttpGet]
        [Authorize(Roles = "Author")]
        [Route("api/ForAuthorRole")]
        public string ForAuthorRole()
        {
            return "For author role";
        }

        [HttpGet]
        [Authorize(Roles = "Author,Reader")]
        [Route("api/ForAuthorOrReader")]
        public string ForAuthorOrReader()
        {
            return "For author/reader role";
        }

        public string FAzi()
        {
            return "For author/reader role";
        }

        [Route("Logout")]
        [Authorize]
        public IHttpActionResult Logout()
        {
            return Ok("Success");
        }

    }
}
