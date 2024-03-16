using BusinesEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using WebAPI.Models;
using WebAPI.Services.Emailing.Models;
using WebAPI.Services.Emailing;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Web.UI.WebControls;
using System.IO;
using System.Net.Http;
using System.Net;
using WebAPI.Services;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Evernote.EDAM.Type;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Owin.Security.Providers.Orcid.Message;

namespace WebAPI.Controllers
{
    public class SuperAdminController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        [Route("api/superadmin/login")]
        [HttpGet]
        public IHttpActionResult Login()
        {
            var email = HttpContext.Current.Request.Params["email"];
            var password = HttpContext.Current.Request.Params["password"];

            var login = db.SuperAdminUsers.FirstOrDefault(a => a.Email == email && a.Password == password);
            if (login == null)
            {
                var response = new
                {
                    isSuccess = false,
                    message = "Invalid Username or Password",
                    data = ""
                };
                return Json(response);
            }
            else
            {
                var response = new
                {
                    isSuccess = true,
                    message = "Login Successful",
                    data = GetToken(login)
                };
                return Json(response);
            }
        }

        [HttpGet]
        public Object GetToken(SuperAdminUser applicationUser)
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
            permClaims.Add(new Claim("email", applicationUser.Email));
            permClaims.Add(new Claim("ImageUrl", applicationUser.ImageUrl == null ? "" : applicationUser.ImageUrl));

            //Create Security Token object by giving required parameters    
            var token = new JwtSecurityToken(issuer, //Issure    
                            issuer,  //Audience    
                            permClaims,
                            expires: DateTime.Now.AddDays(30),
                            signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
            return new { data = jwt_token };
        }

        //=============================================================================================\\ GET, CREATE, UPDATE PROMOTOR

        [Route("api/superadmin/createPromotors")]
        [HttpPost]
        public IHttpActionResult CreatePromotors(CreatePromotorRequest model)
        {
            try
            {
                var promotor = db.Promotors.Add(new Promotor
                {
                    FullName = model.PromotorRequest.FullName,
                    NickName = model.PromotorRequest.NickName,
                    EmailAddress = model.PromotorRequest.EmailAddress,
                    RoleId = model.PromotorRequest.RoleId,
                    StatusId = model.PromotorRequest.StatusId,
                    Password = model.PromotorRequest.Password,
                    SerialNumber = model.PromotorRequest.SerialNumber,
                    CreatedAt = DateTime.Now,
                });
                db.SaveChanges();

                var promotorCompanyDetail = db.PromotorCompanyDetails.Add(new PromotorCompanyDetail
                {
                    PromotorId = promotor.Id,
                    MunicipalityId = model.PromotorCompanyDetailRequest.MunicipalityId,
                    CityId = model.PromotorCompanyDetailRequest.CityId,
                    CompanyEmailAddress = model.PromotorCompanyDetailRequest.CompanyEmailAddress,
                    BillingAddress = model.PromotorCompanyDetailRequest.BillingAddress,
                    AddressLine1 = model.PromotorCompanyDetailRequest.AddressLine1,
                    AddressLine2 = model.PromotorCompanyDetailRequest.AddressLine2,
                    AddressLine3 = model.PromotorCompanyDetailRequest.AddressLine3,
                    Telephone = model.PromotorCompanyDetailRequest.Telephone,
                    TaxIdentificationNumber = model.PromotorCompanyDetailRequest.TaxIdentificationNumber,
                    PostalCode = model.PromotorCompanyDetailRequest.PostalCode,
                    CopyRights = model.PromotorCompanyDetailRequest.CopyRights,
                    LegalDisclaimer = model.PromotorCompanyDetailRequest.LegalDisclaimer,
                    FacebookLine = model.PromotorCompanyDetailRequest.FacebookLine,
                    InstaLink = model.PromotorCompanyDetailRequest.InstaLink,
                    LinkedinLink = model.PromotorCompanyDetailRequest.LinkedinLink,
                    TwitterLink = model.PromotorCompanyDetailRequest.TwitterLink,
                    isActive = true,
                    CreatedAt = DateTime.Now,
                });
                db.SaveChanges();

                var promotorCheckOut = db.PromotorCheckOut.Add(new PromotorCheckOut
                {
                    PromotorId = promotor.Id,
                    ReservationDurationMinutes = model.PromotorCheckOutRequest.ReservationDurationMinutes,
                    DisableGuestCheckout = model.PromotorCheckOutRequest.DisableGuestCheckout,
                    AskMarketingOption = model.PromotorCheckOutRequest.AskMarketingOption,
                    MarketingOptionLabel = model.PromotorCheckOutRequest.MarketingOptionLabel,
                    CopyForTopCheckout = model.PromotorCheckOutRequest.CopyForTopCheckout,
                    isActive = true,
                    CreatedAt = DateTime.Now,
                });
                db.SaveChanges();

                var promotorEvents = db.PromotorEvents.Add(new PromotorEvents
                {
                    PromotorId = promotor.Id,
                    EventSlug = model.PromotorEventsRequest.EventSlug,
                    EventTypeId = model.PromotorEventsRequest.EventTypeId,
                    ShowRemainingTicketCount = model.PromotorEventsRequest.ShowRemainingTicketCount,
                    isActive = true,
                    CreatedAt = DateTime.Now,
                });
                db.SaveChanges();

                var promotorConfirmationEmail = db.PromotorConfirmationEmail.Add(new PromotorConfirmationEmail
                {
                    PromotorId = promotor.Id,
                    To = model.PromotorConfirmationEmailRequest.To,
                    BCC = model.PromotorConfirmationEmailRequest.BCC,
                    CC = model.PromotorConfirmationEmailRequest.CC,
                    Write = model.PromotorConfirmationEmailRequest.Write,
                    isActive = true,
                    CreatedAt = DateTime.Now,
                });
                db.SaveChanges();

                var promotorFormatting = db.PromotorFormatting.Add(new PromotorFormatting
                {
                    PromotorId = promotor.Id,
                    isActive = true,
                    CreatedAt = DateTime.Now,
                    DateFormatId = model.PromotorFormattingRequest.DateFormatId,
                    TimeFormat = model.PromotorFormattingRequest.TimeFormat,
                    APIDomain = model.PromotorFormattingRequest.APIDomain,
                });
                db.SaveChanges();

                if (model.PromotorCommissionStructureRequest != null)
                {
                    var promotorCommissionStructure = db.PromotorCommissionStructure.Add(new PromotorCommissionStructure
                    {
                        PromotorId = promotor.Id,
                        isActive = true,
                        CreatedAt = DateTime.Now,
                        ContractDuration = model.PromotorCommissionStructureRequest.ContractDuration,
                        CurrencyId = model.PromotorCommissionStructureRequest.CurrencyId,
                        CommissionPercentage = model.PromotorCommissionStructureRequest.CommissionPercentage,
                        OnEachEntry = model.PromotorCommissionStructureRequest.OnEachEntry,
                        OnEachProduct = model.PromotorCommissionStructureRequest.OnEachProduct,
                        FixAmount = model.PromotorCommissionStructureRequest.FixAmount,
                        TransactionAmount = model.PromotorCommissionStructureRequest.TransactionAmount,
                        CommissionCap = model.PromotorCommissionStructureRequest.CommissionCap,
                        TaxTypeId = model.PromotorCommissionStructureRequest.TaxTypeId,
                        TaxPercentage = model.PromotorCommissionStructureRequest.TaxPercentage,
                    });
                    db.SaveChanges();
                }

                if (model.PromotorSubscriptionRequest != null)
                {
                    var promotorSubscription = db.PromotorSubscription.Add(new PromotorSubscription
                    {
                        PromotorId = promotor.Id,
                        isActive = true,
                        CreatedAt = DateTime.Now,
                        SubscriptionChargesId = model.PromotorSubscriptionRequest.SubscriptionChargesId,
                        NumberofMonths = model.PromotorSubscriptionRequest.NumberofMonths,
                        Amount = model.PromotorSubscriptionRequest.Amount,
                        CurrencyId = model.PromotorSubscriptionRequest.CurrencyId,
                    });
                    db.SaveChanges();
                }

                var promotorSupportSubscription = db.PromotorSupportSubscription.Add(new PromotorSupportSubscription
                {
                    PromotorId = promotor.Id,
                    isActive = true,
                    CreatedAt = DateTime.Now,
                    SubscriptionChargesId = model.PromotorSupportSubscriptionRequest.SubscriptionChargesId,
                    NumberofMonths = model.PromotorSupportSubscriptionRequest.NumberofMonths,
                    Amount = model.PromotorSupportSubscriptionRequest.Amount,
                    CurrencyId = model.PromotorSupportSubscriptionRequest.CurrencyId,
                });
                db.SaveChanges();

                var response = new
                {
                    isSuccess = true,
                    message = "Promotor Added Successfully",
                    data = promotor
                };
                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    isSuccess = false,
                    message = "Promotor Not Saved: " + ex.Message,
                    data = ""
                };
                return Json(response);
            }
        }

        [Route("api/superadmin/updatePromotors")]
        [HttpPost]
        public IHttpActionResult UpdatePromotors(UpdatePromotorRequest model)
        {
            try
            {
                var promotor = db.Promotors.FirstOrDefault(a => a.Id == model.PromotorRequest.Id);
                if (promotor == null)
                {
                    promotor.FullName = model.PromotorRequest.FullName;
                    promotor.NickName = model.PromotorRequest.NickName;
                    promotor.EmailAddress = model.PromotorRequest.EmailAddress;
                    promotor.RoleId = model.PromotorRequest.RoleId;
                    promotor.StatusId = model.PromotorRequest.StatusId;
                    promotor.Password = model.PromotorRequest.Password;
                    promotor.SerialNumber = model.PromotorRequest.SerialNumber;
                };
                db.SaveChanges();

                var promotorCompanyDetail = db.PromotorCompanyDetails.FirstOrDefault(a => a.Id == model.PromotorRequest.Id);
                if (promotorCompanyDetail == null)
                {
                    promotorCompanyDetail.MunicipalityId = model.PromotorCompanyDetailRequest.MunicipalityId;
                    promotorCompanyDetail.CityId = model.PromotorCompanyDetailRequest.CityId;
                    promotorCompanyDetail.CompanyEmailAddress = model.PromotorCompanyDetailRequest.CompanyEmailAddress;
                    promotorCompanyDetail.BillingAddress = model.PromotorCompanyDetailRequest.BillingAddress;
                    promotorCompanyDetail.AddressLine1 = model.PromotorCompanyDetailRequest.AddressLine1;
                    promotorCompanyDetail.AddressLine2 = model.PromotorCompanyDetailRequest.AddressLine2;
                    promotorCompanyDetail.AddressLine3 = model.PromotorCompanyDetailRequest.AddressLine3;
                    promotorCompanyDetail.Telephone = model.PromotorCompanyDetailRequest.Telephone;
                    promotorCompanyDetail.TaxIdentificationNumber = model.PromotorCompanyDetailRequest.TaxIdentificationNumber;
                    promotorCompanyDetail.PostalCode = model.PromotorCompanyDetailRequest.PostalCode;
                    promotorCompanyDetail.CopyRights = model.PromotorCompanyDetailRequest.CopyRights;
                    promotorCompanyDetail.LegalDisclaimer = model.PromotorCompanyDetailRequest.LegalDisclaimer;
                    promotorCompanyDetail.FacebookLine = model.PromotorCompanyDetailRequest.FacebookLine;
                    promotorCompanyDetail.InstaLink = model.PromotorCompanyDetailRequest.InstaLink;
                    promotorCompanyDetail.LinkedinLink = model.PromotorCompanyDetailRequest.LinkedinLink;
                    promotorCompanyDetail.TwitterLink = model.PromotorCompanyDetailRequest.TwitterLink;
                };
                db.SaveChanges();

                var promotorCheckOut = db.PromotorCheckOut.FirstOrDefault(a => a.Id == model.PromotorRequest.Id);
                if (promotorCheckOut == null)
                {
                    promotorCheckOut.ReservationDurationMinutes = model.PromotorCheckOutRequest.ReservationDurationMinutes;
                    promotorCheckOut.DisableGuestCheckout = model.PromotorCheckOutRequest.DisableGuestCheckout;
                    promotorCheckOut.AskMarketingOption = model.PromotorCheckOutRequest.AskMarketingOption;
                    promotorCheckOut.MarketingOptionLabel = model.PromotorCheckOutRequest.MarketingOptionLabel;
                    promotorCheckOut.CopyForTopCheckout = model.PromotorCheckOutRequest.CopyForTopCheckout;
                };
                db.SaveChanges();

                var promotorEvents = db.PromotorEvents.FirstOrDefault(a => a.Id == model.PromotorRequest.Id);
                if (promotorEvents == null)
                {
                    promotorEvents.EventSlug = model.PromotorEventsRequest.EventSlug;
                    promotorEvents.EventTypeId = model.PromotorEventsRequest.EventTypeId;
                    promotorEvents.ShowRemainingTicketCount = model.PromotorEventsRequest.ShowRemainingTicketCount;
                };
                db.SaveChanges();

                var promotorConfirmationEmail = db.PromotorConfirmationEmail.FirstOrDefault(a => a.Id == model.PromotorRequest.Id);
                if (promotorConfirmationEmail == null)
                {
                    promotorConfirmationEmail.To = model.PromotorConfirmationEmailRequest.To;
                    promotorConfirmationEmail.BCC = model.PromotorConfirmationEmailRequest.BCC;
                    promotorConfirmationEmail.CC = model.PromotorConfirmationEmailRequest.CC;
                    promotorConfirmationEmail.Write = model.PromotorConfirmationEmailRequest.Write;
                };
                db.SaveChanges();

                var promotorFormatting = db.PromotorFormatting.FirstOrDefault(a => a.Id == model.PromotorRequest.Id);
                if (promotorFormatting == null)
                {
                    promotorFormatting.DateFormatId = model.PromotorFormattingRequest.DateFormatId;
                    promotorFormatting.TimeFormat = model.PromotorFormattingRequest.TimeFormat;
                    promotorFormatting.APIDomain = model.PromotorFormattingRequest.APIDomain;
                };
                db.SaveChanges();

                if (model.PromotorCommissionStructureRequest != null)
                {
                    var promotorCommissionStructure = db.PromotorCommissionStructure.FirstOrDefault(a => a.Id == model.PromotorRequest.Id);
                    if (promotorCommissionStructure == null)
                    {
                        promotorCommissionStructure.ContractDuration = model.PromotorCommissionStructureRequest.ContractDuration;
                        promotorCommissionStructure.CurrencyId = model.PromotorCommissionStructureRequest.CurrencyId;
                        promotorCommissionStructure.CommissionPercentage = model.PromotorCommissionStructureRequest.CommissionPercentage;
                        promotorCommissionStructure.OnEachEntry = model.PromotorCommissionStructureRequest.OnEachEntry;
                        promotorCommissionStructure.OnEachProduct = model.PromotorCommissionStructureRequest.OnEachProduct;
                        promotorCommissionStructure.FixAmount = model.PromotorCommissionStructureRequest.FixAmount;
                        promotorCommissionStructure.TransactionAmount = model.PromotorCommissionStructureRequest.TransactionAmount;
                        promotorCommissionStructure.CommissionCap = model.PromotorCommissionStructureRequest.CommissionCap;
                        promotorCommissionStructure.TaxTypeId = model.PromotorCommissionStructureRequest.TaxTypeId;
                        promotorCommissionStructure.TaxPercentage = model.PromotorCommissionStructureRequest.TaxPercentage;
                    };
                    db.SaveChanges();
                }

                if (model.PromotorSubscriptionRequest != null)
                {
                    var promotorSubscription = db.PromotorSubscription.FirstOrDefault(a => a.Id == model.PromotorRequest.Id);
                    if (promotorSubscription == null)
                    {
                        promotorSubscription.SubscriptionChargesId = model.PromotorSubscriptionRequest.SubscriptionChargesId;
                        promotorSubscription.NumberofMonths = model.PromotorSubscriptionRequest.NumberofMonths;
                        promotorSubscription.Amount = model.PromotorSubscriptionRequest.Amount;
                        promotorSubscription.CurrencyId = model.PromotorSubscriptionRequest.CurrencyId;
                    };
                    db.SaveChanges();
                }

                var promotorSupportSubscription = db.PromotorSupportSubscription.FirstOrDefault(a => a.Id == model.PromotorRequest.Id);
                if (promotorSupportSubscription == null)
                {
                    promotorSupportSubscription.SubscriptionChargesId = model.PromotorSupportSubscriptionRequest.SubscriptionChargesId;
                    promotorSupportSubscription.NumberofMonths = model.PromotorSupportSubscriptionRequest.NumberofMonths;
                    promotorSupportSubscription.Amount = model.PromotorSupportSubscriptionRequest.Amount;
                    promotorSupportSubscription.CurrencyId = model.PromotorSupportSubscriptionRequest.CurrencyId;
                };
                db.SaveChanges();

                var response = new
                {
                    isSuccess = true,
                    message = "Promotor Added Successfully",
                    data = promotor
                };
                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    isSuccess = false,
                    message = "Promotor Not Saved: " + ex.Message,
                    data = ""
                };
                return Json(response);
            }
        }

        [Route("api/superadmin/getPromotors")]
        [HttpGet]
        public IHttpActionResult GetPromotors(string SearchText)
        {
            var users = db.Promotors.Where(a => a.StatusId == 1).AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
                users = users.Where(a => a.FullName.ToLower().Contains(SearchText.ToLower()) || a.NickName.ToLower().Contains(SearchText.ToLower()) || a.EmailAddress.ToLower().Contains(SearchText.ToLower()));

            if (User != null)
            {
                var selectedColumns = users.Select(a => new
                {
                    Id = a.Id,
                    Serial = a.SerialNumber,
                    Promoter = a.FullName,
                    Email = a.EmailAddress,
                    Role = Enum.GetName(typeof(PromotorRole), a.RoleId),
                    Status = Enum.GetName(typeof(PromotorStatus), a.StatusId),
                    TotalPayed = 0,
                    Events = 1,
                    RegistrationDate = a.CreatedAt.ToShortDateString(),
                    CommissionRevenue = 0
                }).ToList();


                var response = new
                {
                    isSuccess = true,
                    message = selectedColumns.Count + " Records Found",
                    data = selectedColumns
                };
                return Json(response);
            }
            else
            {
                var response = new
                {
                    isSuccess = false,
                    message = "0 Records Found",
                    data = ""
                };
                return Json(response);
            }

        }

        //=============================================================================================\\ GET, CREATE, UPDATE PROMOTOR USER

        [Route("api/superadmin/createPromotorUser")]
        [HttpPost]
        public IHttpActionResult CreatePromotorUser(PromotorUser model)
        {
            try
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var manager = new UserManager<ApplicationUser>(userStore);
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var users = db.Users.Count();
                    var res = db.Users.Where(c => c.Email == model.Email).FirstOrDefault();


                    var user = new ApplicationUser()
                    {
                        UserName = model.UserName,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        State = model.State,
                        City = model.City,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        Role = model.Role,
                        isActive = model.State == "active",
                        Password = model.Password,
                        CreationDate = DateTime.Now,
                        FullName = model.FirstName + " " + model.LastName,
                        EmailConfirmed = true,
                        Gender = "Male",
                        isProfileCompleted = true,
                        isVerified = true,
                        PromotorId = model.PromotorId,
                        UserId = users + 1,
                        UserTypeId = 0,
                    };
                    manager.PasswordValidator = new PasswordValidator
                    {
                        RequiredLength = 3
                    };
                    IdentityResult result = manager.Create(user);

                    var response = new
                    {
                        isSuccess = true,
                        message = "Promotor Added Successfully",
                        data = user
                    };

                    return Json(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    isSuccess = false,
                    message = "Promotor Not Saved: " + ex.Message,
                    data = ""
                };
                return Json(response);
            }
        }

        [Route("api/superadmin/getPromotorUsers")]
        [HttpGet]
        public IHttpActionResult GetPromotorUsers(string SearchText)
        {
            var users = (from u in db.Users
                         join p in db.Promotors on u.PromotorId equals p.Id into p_join
                         from p in p_join.DefaultIfEmpty()
                         select new { u, p }
                         ).ToList();

            if (!string.IsNullOrWhiteSpace(SearchText))
                users = users.Where(a => a.u.FullName.ToLower().Contains(SearchText.ToLower()) || a.u.NickName.ToLower().Contains(SearchText.ToLower()) || a.u.Email.ToLower().Contains(SearchText.ToLower())).ToList();

            if (User != null)
            {
                var selectedColumns = users.Select(a => new
                {
                    Id = a.u.Id,
                    Promoter = a.p.FullName,
                    Role = a.u.Role,
                    Username = a.u.UserName,
                    Email = a.u.Email,
                    Phone = a.u.PhoneNumber,
                    Status = a.u.isActive
                }).ToList();

                var response = new
                {
                    isSuccess = true,
                    message = selectedColumns.Count + " Records Found",
                    data = selectedColumns
                };
                return Json(response);
            }
            else
            {
                var response = new
                {
                    isSuccess = false,
                    message = "0 Records Found",
                    data = ""
                };
                return Json(response);
            }

        }

        //=============================================================================================\\ SUSPEND, ACTIVE, DELETE, REACTIVE, UNSUSPEND

        [Route("api/superadmin/suspendPromoter")]
        [HttpPost]
        public IHttpActionResult SuspendPromoter(int PromoterId, DateTime StartDate, DateTime EndDate)
        {
            var promoter = db.Promotors.FirstOrDefault(a => a.Id == PromoterId);
            if (promoter == null)
            {
                var response = new
                {
                    isSuccess = false,
                    message = "Promoter Not Found",
                    data = ""
                };
                return Json(response);
            }
            else
            {
                promoter.StatusId = 2;
                promoter.SuspendedFrom = StartDate;
                promoter.SuspendedTo = EndDate;
                db.SaveChanges();

                var response = new
                {
                    isSuccess = true,
                    message = "Suspended",
                    data = ""
                };
                return Json(response);
            }
        }

        [Route("api/superadmin/deletePromoter")]
        [HttpPost]
        public IHttpActionResult DeletePromoter(int PromoterId)
        {
            var promoter = db.Promotors.FirstOrDefault(a => a.Id == PromoterId);
            if (promoter == null)
            {
                var response = new
                {
                    isSuccess = false,
                    message = "Promoter Not Found",
                    data = ""
                };
                return Json(response);
            }
            else
            {
                promoter.StatusId = 3;
                db.SaveChanges();

                var response = new
                {
                    isSuccess = true,
                    message = "Deleted",
                    data = ""
                };
                return Json(response);
            }
        }

        [Route("api/superadmin/unSuspendPromoter")]
        [HttpPost]
        public IHttpActionResult UnSuspendPromoter(int PromoterId)
        {
            var promoter = db.Promotors.FirstOrDefault(a => a.Id == PromoterId);
            if (promoter == null)
            {
                var response = new
                {
                    isSuccess = false,
                    message = "Promoter Not Found",
                    data = ""
                };
                return Json(response);
            }
            else
            {
                promoter.StatusId = 1;
                promoter.SuspendedFrom = null;
                promoter.SuspendedTo = null;
                db.SaveChanges();

                var response = new
                {
                    isSuccess = true,
                    message = "Unsuspended",
                    data = ""
                };
                return Json(response);
            }
        }

        //=============================================================================================\\ CONTENT MANAGER

        [HttpPost]
        [Route("api/superadmin/createPromotorBranding")]
        [AllowAnonymous]
        public IHttpActionResult createPromotorBranding(UILayoutRequestModel layout)
        {
            UILayout model = new UILayout
            {
                PromotorId = layout.PromotorId,
                LogoURL = layout.LogoURL,
                DarkLogoURL = layout.DarkLogoURL,
                FaviconURL = layout.FaviconURL,
                LogoLink = layout.LogoLink,
                TicketBgForOrderURL = layout.TicketBgForOrderURL,
                FrontImageForSessionTicketURL = layout.FrontImageForSessionTicketURL,
                CheckBgURL = layout.CheckBgURL,
                OrderCompletionImageURL = layout.OrderCompletionImageURL,
                PrimaryColor = layout.PrimaryColor,
                SecondaryColor = layout.SecondaryColor,
                AdditionalColors = string.Join(",", layout.AdditionalColors),
                CreatedDate = DateTime.Now,
                Description = layout.Description,
                isActive = true,
                StickOnNumber = layout.StickOnNumber,
                Title = layout.Title,
            };

            var layOut = db.UILayout.Add(model);
            db.SaveChanges();

            return Json(layOut);
        }

        [HttpGet]
        [Route("api/superadmin/getPromotorBranding")]
        [AllowAnonymous]
        public IHttpActionResult GetUILayout(int PromotorId)
        {
            List<UILayoutRequestModel> model = new List<UILayoutRequestModel>();
            var layout = db.UILayout.Where(ui => ui.PromotorId == PromotorId);
            if (layout == null) { return Json(new { result = "No Record Found" }); }

            foreach (var item in layout)
            {
                model.Add(new UILayoutRequestModel
                {
                    PromotorId = PromotorId,
                    PrimaryColor = item.PrimaryColor,
                    SecondaryColor = item.SecondaryColor,
                    AdditionalColors = item.AdditionalColors.Split(',').ToArray(),
                    LogoURL = item.LogoURL,
                    DarkLogoURL = item.DarkLogoURL,
                    FaviconURL = item.FaviconURL,
                    LogoLink = item.LogoLink,
                    TicketBgForOrderURL = item.TicketBgForOrderURL,
                    FrontImageForSessionTicketURL = item.FrontImageForSessionTicketURL,
                    CheckBgURL = item.CheckBgURL,
                    OrderCompletionImageURL = item.OrderCompletionImageURL,
                    Title = item.Title,
                    StickOnNumber = item.StickOnNumber,
                    Description = item.Description
                });
            }

            return Json(model);
        }

        //=============================================================================================\\ VENUE MANAGER

        [Route("api/superadmin/createVenueMaps")]
        [HttpPost]
        public IHttpActionResult AddEventVenue(EventVenueRequestSA model)
        {
            try
            {
                var EventVenue = db.EventVenues.Add(new BusinesEntities.EventVenue
                {
                    PromotorId = model.PromotorId,
                    Name = model.Name,
                    Location = model.Location,
                    Status = model.Status,
                    TotalBlocks = model.TotalBlocks,
                    TotalAvailableSeats = model.TotalAvailableSeats,
                    TotalCapacity = model.TotalCapacity,
                    ImageURL = model.ImageURL,
                    isActive = true,
                    CreatedDate = DateTime.Now,
                });

                db.SaveChanges();
                return Json("Record Added Successfully");
            }
            catch (Exception)
            {
                return BadRequest("Error");
            }
        }

        [HttpPut]
        [Route("api/superadmin/editEventVenue")]
        public IHttpActionResult EditEventVenue(UpdateEventVenueRequestSA model)
        {
            var _dc = db.EventVenues.FirstOrDefault(a => a.Id == model.Id);
            if (_dc != null)
            {
                _dc.Name = model.Name;
                _dc.Location = model.Location;
                _dc.Status = model.Status;
                _dc.TotalBlocks = model.TotalBlocks;
                _dc.TotalAvailableSeats = model.TotalAvailableSeats;
                _dc.TotalCapacity = model.TotalCapacity;
                _dc.ImageURL = model.ImageURL;
            }

            db.SaveChanges();
            return Json(_dc);
        }

        [HttpGet]
        [Route("api/superadmin/getAllEventVenueByPromotor")]
        public IHttpActionResult GetAllEventVenueByPromotor(int PromotorId)
        {
            var data = db.EventVenues.Where(a => a.PromotorId == PromotorId && a.isActive == true).ToList();
            return Json(data);
        }

        [HttpDelete]
        [Route("api/superadmin/deleteEventVenue")]
        public IHttpActionResult DeleteEventVenue(int Id)
        {
            var _dc = db.EventVenues.FirstOrDefault(a => a.Id == Id);
            if (_dc != null)
                _dc.isActive = false;

            db.SaveChanges();
            return Json(_dc);
        }

        [HttpGet]
        [Route("api/superadmin/getEventVenueById")]
        public IHttpActionResult GetEventVenueById(int Id)
        {
            var data = db.EventVenues.FirstOrDefault(a => a.Id == Id && a.isActive == true);
            return Json(data);
        }

        //=============================================================================================\\ ROLES MANAGER

        [Route("api/superadmin/createRole")]
        [HttpPost]
        public IHttpActionResult CreateRole(SARolesRequest model)
        {
            try
            {
                var EventVenue = db.SARoles.Add(new Roles
                {
                    PromotorId = model.PromotorId,
                    Create = model.Create,
                    Delete = model.Delete,
                    GiveAccess = model.GiveAccess,
                    PlanName = model.PlanName,
                    Read = model.Read,
                    RoleName = model.RoleName,
                    Update = model.Update,
                    isActive = true,
                    CreateDate = DateTime.Now,
                });

                db.SaveChanges();
                return Json("Record Added Successfully");
            }
            catch (Exception)
            {
                return BadRequest("Error");
            }
        }

        [Route("api/superadmin/updateRole")]
        [HttpPost]
        public IHttpActionResult UpdateRole(SARolesUpdate model)
        {
            try
            {
                var EventVenue = db.SARoles.FirstOrDefault(a => a.Id == model.Id);
                if (EventVenue != null)
                {
                    EventVenue.PromotorId = model.PromotorId;
                    EventVenue.Create = model.Create;
                    EventVenue.Delete = model.Delete;
                    EventVenue.GiveAccess = model.GiveAccess;
                    EventVenue.PlanName = model.PlanName;
                    EventVenue.Read = model.Read;
                    EventVenue.RoleName = model.RoleName;
                    EventVenue.Update = model.Update;
                };

                db.SaveChanges();
                return Json("Record Updated Successfully");
            }
            catch (Exception)
            {
                return BadRequest("Error");
            }
        }

        [HttpDelete]
        [Route("api/superadmin/deleteRole")]
        public IHttpActionResult DeleteRole(int Id)
        {
            var _dc = db.SARoles.FirstOrDefault(a => a.Id == Id);
            if (_dc != null)
                _dc.isActive = false;

            db.SaveChanges();
            return Json(_dc);
        }

        [HttpGet]
        [Route("api/superadmin/getAllRolesByPromotor")]
        public IHttpActionResult GetAllRolesByPromotor(int PromotorId)
        {
            var data = db.SARoles.Where(a => a.PromotorId == PromotorId && a.isActive == true).ToList();
            return Json(data);
        }

        [HttpGet]
        [Route("api/superadmin/getRoleById")]
        public IHttpActionResult GetRoleById(int Id)
        {
            var data = db.SARoles.FirstOrDefault(a => a.Id == Id && a.isActive == true);
            return Json(data);
        }

        //=============================================================================================\\ PAYMENTS MANAGER

        [Route("api/superadmin/createPromotorPayment")]
        [HttpPost]
        public IHttpActionResult CreatePromotorPayment(PromotorPaymentRequest model)
        {
            try
            {
                var EventVenue = db.PromotorPayments.Add(new PromotorPayment
                {
                    PromotorId = model.PromotorId,
                    Amount = model.Amount,
                    Currency = model.Currency,
                    Date = model.Date,
                    Description = model.Description,
                    InvoiceNumber = model.InvoiceNumber,
                    PromotorName = model.PromotorName,
                    Status = model.Status,
                    isActive = true,
                    CreatedDate = DateTime.Now,
                });

                db.SaveChanges();
                return Json("Record Added Successfully");
            }
            catch (Exception)
            {
                return BadRequest("Error");
            }
        }

        [Route("api/superadmin/updatePromotorPayment")]
        [HttpPost]
        public IHttpActionResult UpdatePromotorPayment(PromotorPaymentUpdate model)
        {
            try
            {
                var EventVenue = db.PromotorPayments.FirstOrDefault(a => a.Id == model.Id);
                if (EventVenue != null)
                {
                    EventVenue.PromotorId = model.PromotorId;
                    EventVenue.Amount = model.Amount;
                    EventVenue.Currency = model.Currency;
                    EventVenue.Date = model.Date;
                    EventVenue.Description = model.Description;
                    EventVenue.InvoiceNumber = model.InvoiceNumber;
                    EventVenue.PromotorName = model.PromotorName;
                    EventVenue.Status = model.Status;
                };

                db.SaveChanges();
                return Json("Record Updated Successfully");
            }
            catch (Exception)
            {
                return BadRequest("Error");
            }
        }

        [HttpDelete]
        [Route("api/superadmin/deletePromotorPayment")]
        public IHttpActionResult DeletePromotorPayment(int Id)
        {
            var _dc = db.PromotorPayments.FirstOrDefault(a => a.Id == Id);
            if (_dc != null)
                _dc.isActive = false;

            db.SaveChanges();
            return Json(_dc);
        }

        [HttpGet]
        [Route("api/superadmin/getAllPromotorPaymentByPromotor")]
        public IHttpActionResult GetAllPromotorPaymentByPromotor(int PromotorId)
        {
            var data = db.PromotorPayments.Where(a => a.PromotorId == PromotorId && a.isActive == true).ToList();
            return Json(data);
        }

        [HttpGet]
        [Route("api/superadmin/getPromotorPaymentById")]
        public IHttpActionResult GetPromotorPaymentById(int Id)
        {
            var data = db.PromotorPayments.FirstOrDefault(a => a.Id == Id && a.isActive == true);
            return Json(data);
        }

    }
}