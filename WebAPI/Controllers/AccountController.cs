using BusinesEntities;
using DAL;
using Evernote.EDAM.Type;
using iTextSharp.text.pdf.security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Owin.Security.Providers.OpenIDBase.Infrastructure;
using Owin.Security.Providers.Orcid.Message;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Reflection.Emit;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Razor.Parser.SyntaxTree;
using System.Web.UI.WebControls;
using WebAPI.Models;
using WebAPI.Services;
using WebAPI.Services.Emailing;
using WebAPI.Services.Emailing.Models;
using ZXing.Aztec.Internal;
using static System.Net.Mime.MediaTypeNames;

namespace WebAPI.Controllers
{
    public class AccountController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        NotificationController notificationController = new NotificationController();

        [Route("api/user/login_social")]
        [HttpGet]
        [AllowAnonymous]
        public System.Web.Http.IHttpActionResult RegisterSocial(string Email, string FirstName, string LastName, string ImageUrl)
        {
            try
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var manager = new UserManager<ApplicationUser>(userStore);
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var users = db.Users.Count();
                    var res = db.Users.Where(c => c.Email == Email).FirstOrDefault();

                    if (res != null)
                    {
                        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity(res.UserName), new string[] { "user" });
                        return Json(GetToken(res));
                    }
                    else
                    {
                        var user = new ApplicationUser()
                        {
                            UserName = Email,
                            Email = Email,
                            isProfileCompleted = false,
                            isVerified = true,
                            FirstName = FirstName,
                            LastName = LastName,
                            ImageUrl = ImageUrl,
                            UserId = users + 1,
                            UserTypeId = 1,
                            CreationDate = DateTime.Now,
                        };
                        manager.PasswordValidator = new PasswordValidator
                        {
                            RequiredLength = 3
                        };
                        IdentityResult result = manager.Create(user);
                        notificationController.AddUserNotificationLocal(
                          new UserNotificationsRequest
                          {
                              NotificationHeader = "Account Setup Successful!",
                              NotificationText = "Your account setup was successful.",
                              NotificationType = "Account",
                              PromotorId = 1,
                              UserId = user.UserId
                          });

                        return Json(GetToken(user));
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(ex.InnerException.Message);
            }
        }

        [Route("api/login")]
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult AccessTokenLogin()
        {
            try
            {
                LoginObject loginObject = new LoginObject();
                string accessToken = HttpContext.Current.Request.Params["access_token"];
                string requestUrl = "https://www.googleapis.com/oauth2/v3/userinfo?access_token=" + accessToken;

                WebRequest request = WebRequest.Create(requestUrl);
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string responseJson = reader.ReadToEnd();
                        loginObject = JsonConvert.DeserializeObject<LoginObject>(responseJson);
                    }
                }

                var r = HttpContext.Current.Request.LogonUserIdentity.Name;
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var manager = new UserManager<ApplicationUser>(userStore);
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var users = db.Users.Count();
                    var res = db.Users.Where(c => c.Email == loginObject.email).FirstOrDefault();

                    if (res != null)
                    {
                        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity(res.UserName), new string[] { "user" });
                        return Json(GetToken(res));
                    }
                    else
                    {
                        var user = new ApplicationUser()
                        {
                            UserName = loginObject.email,
                            Email = loginObject.email,
                            isProfileCompleted = false,
                            isVerified = false,
                            FirstName = loginObject.given_name,
                            LastName = loginObject.family_name,
                            ImageUrl = loginObject.picture,
                            UserId = users + 1,
                            UserTypeId = 1
                        };
                        manager.PasswordValidator = new PasswordValidator
                        {
                            RequiredLength = 3
                        };
                        IdentityResult result = manager.Create(user);

                        return Json(GetToken(user));
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(ex.InnerException.Message);
            }
        }

        [Route("api/customer/login")]
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult CustomerLogin()
        {
            try
            {
                var email = HttpContext.Current.Request.Params["email"];
                var password = HttpContext.Current.Request.Params["password"];

                var res = db.Users.FirstOrDefault(c => c.Email == email && c.Password == password);
                if (res == null)
                    return Json("Invalid username or password");
                else
                {
                    if (res != null)
                    {
                        bool isVerified = false;
                        var otp = db.OTPTableTemp.FirstOrDefault(a => a.EmailAddress == email && a.IsCodeVerified == true);

                        if (otp != null)
                        {
                            isVerified = true;
                        }

                        var response = new
                        {
                            message = "User Exsit. Successful Login.",
                            res.isProfileCompleted,
                            isVerified,
                            token = GetToken(res),
                            user = res
                        };

                        if (!isVerified)
                        {
                            GenerateEmailOTPTemp(email);
                        }


                        return Json(response);
                    }
                }

                return Json(GetToken(res));
            }
            catch (Exception ex)
            {
                return Json(ex.InnerException.Message);
            }
        }

        //=========================================================================================================//

        [Route("api/user/register")]
        [HttpPost]
        public HttpResponseMessage RegisterAsync(string Email, string Password)
        {
            var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var manager = new UserManager<ApplicationUser>(userStore);
            try
            {
                var users = db.Users.Count();
                var user = new ApplicationUser()
                {
                    UserName = Email,
                    Email = Email,
                    Password = Password,
                    isProfileCompleted = false,
                    isVerified = false,
                    UserId = users + 1,
                    UserTypeId = 0,
                    CreationDate = DateTime.Now,
                };
                var res = db.Users.Where(c => c.Email == Email).Select(c => c.Email).SingleOrDefault();
                if (res == Email)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Email Already Exsit", res);
                }
                manager.PasswordValidator = new PasswordValidator
                {
                    RequiredLength = 3
                };
                IdentityResult result = manager.Create(user, Password);
                var res1 = db.Users.Where(c => c.Email == Email).Select(c => c.Email).SingleOrDefault();
                return Request.CreateResponse(HttpStatusCode.OK, "Successfully Save", res1);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            }
        }

        [Route("api/customer/register")]
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult CustomerRegister()
        {
            var Email = HttpContext.Current.Request.Params["email"];
            var Password = HttpContext.Current.Request.Params["password"];

            var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var manager = new UserManager<ApplicationUser>(userStore);
            try
            {
                var resu = (from u in db.Users
                            join o in db.OTPTableTemp on u.Email equals o.EmailAddress into o_join
                            from o in o_join.DefaultIfEmpty()
                            where u.Email == Email
                            select new { u, o }).FirstOrDefault();

                //var res = db.Users.FirstOrDefault(c => c.Email == Email);
                if (resu != null && resu.u != null)
                {
                    if (resu.u.Email == Email)
                    {
                        bool isVerified = false;
                        if (resu.o != null)
                        {
                            isVerified = true;
                        }

                        var response = new
                        {
                            message = "Email Already Exsit",
                            resu.u.isProfileCompleted,
                            isVerified
                        };
                        return Json(response);
                    }
                }


                var users = db.Users.Count();
                var user = new ApplicationUser()
                {
                    UserName = Email,
                    Email = Email,
                    Password = Password,
                    UserId = users + 1,
                    UserTypeId = 0,
                    CreationDate = DateTime.Now,
                    PromotorId = 1
                };

                manager.PasswordValidator = new PasswordValidator
                {
                    RequiredLength = 3
                };
                IdentityResult result = manager.Create(user, Password);
                GenerateEmailOTPTemp(Email);

                notificationController.AddUserNotificationLocal(
                  new UserNotificationsRequest
                  {
                      NotificationHeader = "Account Setup Successful!",
                      NotificationText = "Your account setup was successful.",
                      NotificationType = "Account",
                      PromotorId = 1,
                      UserId = user.UserId
                  }
                );

                return Json("User Created Successfully. OTP sent for Verification");
            }
            catch (Exception)
            {
                return Json(HttpStatusCode.BadRequest);
            }
        }

        //=========================================================================================================//

        [Route("api/user/updateProfile")]
        [HttpPost]
        public HttpResponseMessage UpdateProfile(ApplicationUser model)
        {
            var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            var UserId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

            try
            {
                object token = "";
                var u = db.Users.FirstOrDefault(a => a.UserId == UserId);
                if (u != null)
                {
                    u.UserName = model.Email;
                    u.Email = model.Email;
                    u.FullName = model.FullName;
                    u.FirstName = model.FirstName;
                    u.LastName = model.LastName;
                    u.Gender = model.Gender;
                    u.DOB = model.DOB;
                    u.NickName = model.NickName;
                    u.CountryCode = model.CountryCode;
                    u.PhoneNumber = model.PhoneNumber;
                    u.isProfileCompleted = true;
                    u.Location = model.Location;
                    u.State = model.State;
                    u.City = model.City;
                    u.ZipCode = model.ZipCode;

                    db.SaveChanges();

                    token = GetToken(u);
                }
                notificationController.AddUserNotificationLocal(
                         new UserNotificationsRequest
                         {
                             NotificationHeader = "Profile Update Successful!",
                             NotificationText = "Your account profile update was successful.",
                             NotificationType = "Account",
                             PromotorId = u.PromotorId,
                             UserId = u.UserId
                         });

                return this.Request.CreateResponse(HttpStatusCode.OK, new { message = "Updated", user = u, token = token });
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Failed" });
            }
        }

        [Route("api/customer/updateProfile")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage UpdateCustomerProfile()
        {
            var Email = HttpContext.Current.Request.Form["Email"];
            var file = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;


            if (file != null && !string.IsNullOrWhiteSpace(file.FileName))
            {
                var fileName = Path.GetFileName(file.FileName);
                try
                {
                    var blobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=tiqarteblob;AccountKey=n4JlNEkWc5KzFSPd02dkNKLJqbWjbxH8LHQqrk8zBAd4B8RUtnI9XJetMyo4wtbOJddQ++e93Emd+AStYPy3Vw==;EndpointSuffix=core.windows.net";
                    BlobService blobService = new BlobService(blobStorageConnectionString);
                    MemoryStream memoryStream = new MemoryStream();

                    using (Stream fileStream = file.InputStream)
                    {
                        fileStream.CopyTo(memoryStream);
                    }
                    memoryStream.Position = 0;
                    var URI = blobService.UploadFileBlobAsync("tiqarteblob", fileName, memoryStream).Result;

                    var u = db.Users.FirstOrDefault(a => a.Email == Email);
                    if (u != null)
                    {
                        u.ImageUrl = URI.AbsoluteUri;
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Failed" });
                }
            }



            var FirstName = HttpContext.Current.Request.Form["FirstName"];
            var LastName = HttpContext.Current.Request.Form["LastName"];
            try
            {
                object token = "";
                var u = db.Users.FirstOrDefault(a => a.Email == Email);
                if (u != null)
                {
                    u.UserName = Email;
                    u.Email = Email;
                    u.FullName = FirstName + " " + LastName;
                    u.FirstName = FirstName;
                    u.LastName = LastName;
                    u.Gender = HttpContext.Current.Request.Form["Gender"];
                    u.DOB = HttpContext.Current.Request.Form["DOB"];
                    u.NickName = HttpContext.Current.Request.Form["NickName"];
                    u.CountryCode = HttpContext.Current.Request.Form["CountryCode"];
                    u.PhoneNumber = HttpContext.Current.Request.Form["PhoneNumber"];
                    u.isProfileCompleted = true;
                    u.Location = HttpContext.Current.Request.Form["Location"];
                    u.State = HttpContext.Current.Request.Form["State"];
                    u.City = HttpContext.Current.Request.Form["City"];
                    u.ZipCode = HttpContext.Current.Request.Form["ZipCode"];
                    u.LastUpdate = DateTime.Now;
                    db.SaveChanges();

                    token = GetToken(u);
                }
                return this.Request.CreateResponse(HttpStatusCode.OK, new { message = "Updated", user = u, token = token });
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Failed" });
            }
        }

        [Route("api/user/updatePassword")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdatePassword(string userID, string newPass)
        {
            var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var UserManager = new UserManager<ApplicationUser>(userStore);
            ApplicationUser user = await UserManager.FindByIdAsync(userID);
            if (user == null)
            {
                return NotFound();
            }
            user.Password = newPass;
            user.PasswordHash = UserManager.PasswordHasher.HashPassword(newPass);
            var result = await UserManager.UpdateAsync(user);
            //Return userId and authentication code
            if (!result.Succeeded)
            {

                //throw exception......
            }
            return Ok();
            // Return userId and authentication code
        }

        [Route("api/user/forgotPassword")]
        [HttpGet]
        public HttpResponseMessage ForgotPassword(string sms, string email, string userID)
        {
            var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var UserManager = new UserManager<ApplicationUser>(userStore);
            //Return userId and authentication code
            return Request.CreateResponse(HttpStatusCode.OK, new { message = "Successfully Updated" }, userID);
        }

        [Route("api/customer/ChangePassword")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage ChangePassword()
        {
            var email = HttpContext.Current.Request.Form["Email"];
            var password = HttpContext.Current.Request.Form["Password"];
            try
            {
                var u = db.Users.FirstOrDefault(a => a.Email == email);
                if (u != null)
                {
                    u.Password = password;
                    db.SaveChanges();
                }
                return this.Request.CreateResponse(HttpStatusCode.OK, new { message = "Updated", user = u });
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Failed" });
            }
        }

        [Route("api/customer/uploadProfilePicture")]
        [HttpPost]
        public HttpResponseMessage UploadProfilePicture()
        {
            var file = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;
            var fileName = Path.GetFileName(file.FileName);
            try
            {
                var blobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=tiqarteblob;AccountKey=n4JlNEkWc5KzFSPd02dkNKLJqbWjbxH8LHQqrk8zBAd4B8RUtnI9XJetMyo4wtbOJddQ++e93Emd+AStYPy3Vw==;EndpointSuffix=core.windows.net";
                BlobService blobService = new BlobService(blobStorageConnectionString);
                MemoryStream memoryStream = new MemoryStream();

                using (Stream fileStream = file.InputStream)
                {
                    fileStream.CopyTo(memoryStream);
                }
                memoryStream.Position = 0;
                var URI = blobService.UploadFileBlobAsync("tiqarteblob", fileName, memoryStream).Result;

                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var UserId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);
                var u = db.Users.FirstOrDefault(a => a.UserId == UserId);
                if (u != null)
                {
                    u.ImageUrl = URI.AbsoluteUri;
                    db.SaveChanges();
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, new { message = "Updated", user = u });
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Failed" });
            }
        }

        [Route("api/customer/GetProfile")]
        [HttpGet]
        public IHttpActionResult GetProfile()
        {
            var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            var CustomerId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

            var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var manager = new UserManager<ApplicationUser>(userStore);
            try
            {
                ApplicationUser u = db.Users.FirstOrDefault(a => a.UserId == CustomerId);
                if (u == null) return Json(CustomerId);

                var response = new
                {
                    u.UserId,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    u.DOB,
                    u.Gender,
                    u.NickName,
                    u.CountryCode,
                    u.PhoneNumber,
                    u.Location,
                    u.State,
                    u.City,
                    u.ZipCode,
                };

                return Json(response);
            }
            catch (Exception ex)
            {
                return Json("Failed: " + ex.Message);
            }
        }

        //=========================================================================================================//

        [Route("api/customer/GenerateEmailOTP")]
        [HttpGet]
        public IHttpActionResult GenerateEmailOTP()
        {
            var otp = GenerateRandomOTP();
            var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            var UserId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);
            var u = db.Users.FirstOrDefault(a => a.UserId == UserId);

            string _emailtemplateHTML = "Dear Customer,<br><br>Thank you for registering with Us. We are excited to have you on board! To ensure the security of your account, we require you to confirm your verification by entering a One-Time Password: <b>{otpCode}</b>.<br><br>".Replace("{otpCode}", otp);

            var otpModel = new OTPTable
            {
                UserId = u.UserId,
                IsCodeVerified = false,
                OTPCode = int.Parse(otp),
                CreateDateTime = DateTime.Now,
                ExpirationTime = DateTime.UtcNow.AddMinutes(30)
            };

            db.OTPTable.Add(otpModel);
            db.SaveChanges();

            EmailInfo emailInfo = new EmailInfo()
            {
                ToEmails = u.Email,
                Subject = "Verify OTP",
                Body = _emailtemplateHTML,
                IsBodyHtml = true
            };
            var response = SMTPEmailSender.SendMail(emailInfo);
            return Json(response);
        }

        [Route("api/customer/VerifyEmailOTP")]
        [HttpGet]
        public IHttpActionResult VerifyEmailOTP(int otp)
        {
            var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            var UserId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);
            var u = db.Users.FirstOrDefault(a => a.UserId == UserId);

            var isCodeVerified = db.OTPTable.Where(a => a.OTPCode == otp && (a.UserId == u.UserId)).FirstOrDefault();

            if (isCodeVerified == null)
                return Json(new { isSuccess = false, Message = "OTP verification failed" });

            if (isCodeVerified != null && isCodeVerified.IsCodeVerified == true)
                return Json(new { isSuccess = false, Message = "Code is already verified." });
            else
            {
                var _user = db.Users.FirstOrDefault(a => a.UserId == UserId);
                _user.isVerified = true;
                isCodeVerified.IsCodeVerified = true;
                db.SaveChanges();
            }

            return Json(new { isSuccess = true, Message = "OTP verified." });
        }

        [Route("api/customer/GenerateEmailOTPTemp")]
        [HttpGet]
        [AllowAnonymous]
        public bool GenerateEmailOTPTemp(string emailAddress)
        {
            var otp = GenerateRandomOTP();
            string _emailtemplateHTML = "Dear Customer,<br><br>Thank you for registering with Us. We are excited to have you on board! To ensure the security of your account, we require you to confirm your verification by entering a One-Time Password: <b>{otpCode}</b>.<br><br>".Replace("{otpCode}", otp);

            var otpModel = new OTPTableTemp
            {
                EmailAddress = emailAddress,
                IsCodeVerified = false,
                OTPCode = int.Parse(otp),
                CreateDateTime = DateTime.Now,
                ExpirationTime = DateTime.UtcNow.AddMinutes(30)
            };

            db.OTPTableTemp.Add(otpModel);
            db.SaveChanges();

            EmailInfo emailInfo = new EmailInfo()
            {
                ToEmails = emailAddress,
                Subject = "Verify OTP",
                Body = _emailtemplateHTML,
                IsBodyHtml = true
            };
            var response = SMTPEmailSender.SendMail(emailInfo);
            return response;//Json(response);
        }

        [Route("api/customer/VerifyEmailOTPTemp")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult VerifyEmailOTPTemp(string emailAddress, int otp)
        {
            var isCodeVerified = db.OTPTableTemp.Where(a => a.OTPCode == otp && a.EmailAddress == emailAddress).FirstOrDefault();

            if (isCodeVerified == null)
                return Json(new { isSuccess = false, Message = "OTP verification failed" });

            if (isCodeVerified != null && isCodeVerified.IsCodeVerified == true)
                return Json(new { isSuccess = false, Message = "Code is already verified." });
            else
            {
                var _user = db.Users.FirstOrDefault(a => a.Email == emailAddress);
                if (_user != null)
                {
                    _user.isVerified = true;
                    isCodeVerified.IsCodeVerified = true;
                    db.SaveChanges();
                }
            }

            return Json(new { isSuccess = true, Message = "OTP verified." });
        }

        //=========================================================================================================//

        public string GenerateRandomOTP()
        {
            const string chars = "0123456789";
            var random = new Random();
            var otp = new string(Enumerable.Repeat(chars, 4) // Generate a 6-digit OTP
                .Select(s => s[random.Next(s.Length)])
                .ToArray());

            return otp;
        }

        public void SendEmailMain()
        {
            // SMTP server details
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587; // Replace with your SMTP port number
            string username = "saudbintariq2@gmail.com"; // Replace with your email username
            string password = "pucrwfdzrmhfqjms"; // Replace with your email password

            // Sender and recipient email addresses
            string fromEmail = "saudbintariq2@gmail.com";
            string toEmail = "saudbintariq@gmail.com";

            // Create a MailMessage object
            MailMessage message = new MailMessage(fromEmail, toEmail)
            {
                Subject = "Test Email",
                Body = "This is a test email sent using C#."
            };

            // Set the SMTP client configuration
            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            };

            try
            {
                // Send the email
                smtpClient.Send(message);
                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        [HttpGet]
        public Object GetToken(ApplicationUser applicationUser)
        {
            string key = ConfigurationManager.AppSettings["JwtKey"]; //Secret key which will be used later during validation    
            var issuer = ConfigurationManager.AppSettings["JwtIssuer"];  //normally this will be your site URL    

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Create a List of Claims, Keep claims name short    
            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            permClaims.Add(new Claim("userid", applicationUser.UserId.ToString()));
            permClaims.Add(new Claim("promotoid", applicationUser.PromotorId.ToString()));
            permClaims.Add(new Claim("name", applicationUser.FirstName + " " + applicationUser.LastName));
            permClaims.Add(new Claim("profilepictureurl", applicationUser.ImageUrl == null ? "" : applicationUser.ImageUrl));

            //Create Security Token object by giving required parameters    
            var token = new JwtSecurityToken(issuer, //Issure    
                            issuer,  //Audience    
                            permClaims,
                            expires: DateTime.Now.AddDays(30),
                            signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
            return new { data = jwt_token };
        }
    }

    public class RootObject
    {
        public string category { get; set; }
        public string action { get; set; }
        public List<Mainmenu> mainmenus { get; set; }
    }

    public class Mainmenu
    {
        public string name { get; set; }
        public string icon { get; set; }
        public List<Component> components { get; set; }
    }

    public class Component
    {
        public string name { get; set; }
        public string icon { get; set; }
        public string controller { get; set; }
        public string action { get; set; }
        public string Permission { get; set; }
    }

    public class LoginObject
    {
        public string sub { get; set; }
        public string name { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string picture { get; set; }
        public string email { get; set; }
        public bool email_verified { get; set; }
        public string locale { get; set; }
    }
}
