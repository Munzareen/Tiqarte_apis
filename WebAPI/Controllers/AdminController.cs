using BusinesEntities;
using CsQuery.ExtensionMethods;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class AdminDashboardEvent
    {
        int id { get; set; }
        string eventName { get; set; }
        int ticketsold { get; set; }

        public class AdminController : ApiController
        {
            ApplicationDbContext db = new ApplicationDbContext();

            [HttpGet]
            [Route("admin/getEventsForDashboard")]
            public IHttpActionResult GetEventsForDashboard()
            {
                List<Event> lst = new List<Event>();
                lst = db.Event.Where(a => a.isActive == true).ToList();
                foreach (Event ve in lst)
                {
                    ve.PreEventImages = db.EventImages.Where(e => e.eventid == ve.EventId && e.Type == 0).Select(i => i.url).ToArray();
                    ve.PostEventImages = db.EventImages.Where(e => e.eventid == ve.EventId && e.Type == 1).Select(i => i.url).ToArray();
                }

                var lista = new List<AdminDashboardEvent>();
                foreach (Event ve in lst)
                    lista.Add(new AdminDashboardEvent
                    {
                        id = ve.EventId,
                        eventName = ve.Name,
                        ticketsold = 1
                    });

                return Json(lista);
            }

            [HttpPost]
            [Route("admin/adminlogin")]
            [AllowAnonymous]
            public IHttpActionResult AdminLogin()
            {
                var email = HttpContext.Current.Request.Params["email"];
                var password = HttpContext.Current.Request.Params["password"];

                var res = db.Users.Where(c => c.Email == email && c.Password == password).FirstOrDefault();
                if (res == null)
                    return Json("Invalid username or password");
                else

                    return Json(GetToken(res));
            }

            [HttpPost]
            [Route("api/addPromotorContacts")]
            public IHttpActionResult AddPromotorContacts(AddPromotorContactsViewModels model)
            {
                var Propotor = new PropotorContacts
                {
                    PromotorId = model.PromotorId,
                    CustomerService = model.CustomerService,
                    WhatsAppNumber = model.WhatsAppNumber,
                    WebsiteAddress = model.WebsiteAddress,
                    FacebookId = model.FacebookId,
                    TwitterId = model.TwitterId,
                    InstagramId = model.InstagramId,
                };
                db.PropotorContact.Add(Propotor);
                db.SaveChanges();
                return Json(Propotor);
            }

            [HttpGet]
            [Route("api/getPromotorContacts")]
            public IHttpActionResult GetPromotorContacts(int PromotorId)
            {
                var data = db.PropotorContact.FirstOrDefault(a => a.PromotorId == PromotorId);
                return Json(data);
            }

            [HttpPost]
            [Route("api/CreateContactUsLog")]
            public IHttpActionResult CreateContactUsLog(ContactUsLogsRequest model)
            {
                var cUs = db.ContactUsLogs.Add(new ContactUsLogs
                {
                    PromotorId = model.PromotorId,
                    Name = model.Name,
                    EmailAddress = model.EmailAddress,
                    Municipality = model.Municipality,
                    PhoneNumber = model.PhoneNumber,
                    Message = model.Message,
                    CreatedDateTime = DateTime.Now,
                    isRead = false,
                    isAnswered = false,
                });

                db.SaveChanges();
                return Json(cUs);
            }

            [HttpGet]
            [Route("api/GetContactUsLogs")]
            public IHttpActionResult GetContactUsLogs(int PromotorId)
            {
                var data = db.ContactUsLogs.Where(a => a.PromotorId == PromotorId).AsEnumerable();
                return Json(data);
            }

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
                permClaims.Add(new Claim("name", applicationUser.FirstName + " " + applicationUser.LastName));
                permClaims.Add(new Claim("profilepictureurl", applicationUser.ImageUrl));

                //Create Security Token object by giving required parameters    
                var token = new JwtSecurityToken(issuer, //Issure    
                                issuer,  //Audience    
                                permClaims,
                                expires: DateTime.Now.AddDays(1),
                                signingCredentials: credentials);
                var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
                return new { data = jwt_token };
            }

        }
    }
}
