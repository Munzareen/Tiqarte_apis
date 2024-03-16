using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using WebAPI.Models;
using Evernote.EDAM.Type;

namespace WebAPI.Controllers
{
    public class CustomersController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [Route("admin/getCustomers")]
        [HttpGet]
        public IHttpActionResult GetCustomers()
        {
            var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            var PromotorId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

            var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var manager = new UserManager<ApplicationUser>(userStore);
            try
            {
                var users = db.Users.Where(a => a.PromotorId == PromotorId && a.isActive == true).ToList();
                if (users == null) return Json(PromotorId);

                List<object> lstUsers = new List<object>();

                foreach (var u in users)
                {
                    var response = new
                    {
                        u.UserId,
                        User = u.FirstName + " " + u.LastName,
                        u.Email,
                        u.Notes,
                        u.CreationDate,
                        u.LastUpdate
                    };
                    lstUsers.Add(response);
                }

                return Json(lstUsers);
            }
            catch (Exception ex)
            {
                return Json("Failed: " + ex.Message);
            }
        }

        [Route("admin/getCustomersDetails")]
        [HttpGet]
        public IHttpActionResult GetCustomersDetails(int UserId)
        {
            var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            var PromotorId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

            var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var manager = new UserManager<ApplicationUser>(userStore);
            try
            {
                List<object> lstOrdersViewModel = new List<object>();
                BillingDetailsModelView billingAddress = null;
                ApplicationUser u = db.Users.FirstOrDefault(a => a.PromotorId == PromotorId && a.UserId == UserId);
                if (u == null) return Json(PromotorId);

                var billingDetails = db.ShopCheckOut.FirstOrDefault(a => a.UserId == UserId);

                var details = new
                {
                    u.UserId,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    u.DOB,
                    u.Password,
                };

                var notes = new
                {
                    u.Notes,
                };

                if (billingDetails != null)
                {
                    billingAddress = new BillingDetailsModelView
                    {
                        BillingName = billingDetails.CustomerName,
                        BillingEmail = billingDetails.CustomerEmail,
                        BillingCountry = billingDetails.BillingCountry,
                        BillingAddressLine1 = billingDetails.AddressLine1,
                        BillingAddressLine2 = billingDetails.AddressLine2,
                        BillingTown = billingDetails.City,
                        BillingPostalCode = billingDetails.PostalCode,
                    };
                }
                else
                {
                    var billingCustomerDetails = db.CustomerBillingDetails.FirstOrDefault(a => a.UserId == UserId);
                    if (billingCustomerDetails != null)
                    {
                        billingAddress = new BillingDetailsModelView
                        {
                            BillingName = billingCustomerDetails.BillingName,
                            BillingEmail = billingCustomerDetails.BillingEmail,
                            BillingCountry = billingCustomerDetails.BillingCountry,
                            BillingAddressLine1 = billingCustomerDetails.BillingAddressLine1,
                            BillingAddressLine2 = billingCustomerDetails.BillingAddressLine2,
                            BillingTown = billingCustomerDetails.BillingTown,
                            BillingPostalCode = billingCustomerDetails.BillingPostalCode,
                        };
                    }
                }



                var ShopCheckOut = db.ShopCheckOut.Where(a => a.PromotorId == PromotorId && a.isActive == true && a.isHide == false).ToList();
                foreach (var sco in ShopCheckOut)
                {
                    string EventName = "";
                    decimal TotalAmount = 0;
                    var CheckOutProducts = db.CheckOutProducts.Where(a => a.CheckOutId == sco.Id).ToList();
                    foreach (var cop in CheckOutProducts)
                    {
                        var AddToCart = db.AddToCart.Where(a => a.Id == cop.AddToCartId).ToList();
                        foreach (var atc in AddToCart)
                        {
                            var Product = db.ShopProduct.FirstOrDefault(a => a.Id == atc.ProductId);
                            var Event = db.Event.FirstOrDefault(a => a.EventId == Product.EventId && a.isActive == true);
                            TotalAmount += atc.Quantity * Product.Price;
                            EventName = Event != null ? Event.Name : "N/A";
                        }
                    }

                    lstOrdersViewModel.Add(new
                    {
                        sco.Id,
                        sco.OrderNo,
                        EventName,
                        TotalAmount,
                        Status = "Unpaid",
                        sco.PurchaseDate,
                    });
                }


                var response = new
                {
                    details,
                    billingAddress,
                    notes = u.Notes == null ? null : notes,
                    lstOrdersViewModel
                };

                return Json(response);
            }
            catch (Exception ex)
            {
                return Json("Failed: " + ex.Message);
            }
        }

        [Route("admin/CreateCustomer")]
        [HttpPost]
        public IHttpActionResult CreateCustomer(CreateCustomer customer)
        {
            var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            var PromotorId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

            var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var manager = new UserManager<ApplicationUser>(userStore);
            try
            {
                var res = db.Users.Where(c => c.Email == customer.Email).Select(c => c.Email).SingleOrDefault();
                if (res == customer.Email)
                {
                    var response1 = new
                    {
                        isSuccess = false,
                        message = "Email Already Exsit"
                    };

                    return Json(response1);
                }

                var users = db.Users.Count();
                var user = new ApplicationUser()
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    UserName = customer.Email,
                    Email = customer.Email,
                    Password = customer.Password,
                    Gender = customer.Gender,
                    DOB = customer.DOB,
                    isProfileCompleted = false,
                    isVerified = false,
                    UserId = users + 1,
                    UserTypeId = 0,
                    CreationDate = DateTime.Now,
                    PromotorId = PromotorId,
                    Notes = customer.Notes,
                };

                manager.PasswordValidator = new PasswordValidator
                {
                    RequiredLength = 3
                };
                IdentityResult result = manager.Create(user, customer.Password);

                var billingDetails = db.CustomerBillingDetails.Add(new BusinesEntities.CustomerBillingDetails
                {
                    BillingAddressLine1 = customer.BillingAddressLine1,
                    BillingAddressLine2 = customer.BillingAddressLine2,
                    BillingCountry = customer.BillingCountry,
                    BillingEmail = customer.BillingEmail,
                    BillingName = customer.BillingName,
                    BillingPhone = customer.BillingPhone,
                    BillingPostalCode = customer.BillingPostalCode,
                    BillingTown = customer.BillingTown,
                    UserId = user.UserId
                });
                db.SaveChanges();

                var response = new
                {
                    isSuccess = true,
                    message = "Successfully Save"
                };

                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    isSuccess = false,
                    message = ex.Message
                };

                return Json(response);
            }
        }

        [Route("admin/UpdateCustomerProfile")]
        [HttpPost]
        public HttpResponseMessage UpdateCustomerProfile(UpdateCustomer model)
        {
            try
            {
                var u = db.Users.FirstOrDefault(a => a.UserId == model.UserId);
                if (u != null)
                {
                    u.UserName = model.Email;
                    u.Email = model.Email;
                    u.FirstName = model.FirstName;
                    u.LastName = model.LastName;
                    u.DOB = model.DOB;

                    db.SaveChanges();
                }

                var b = db.CustomerBillingDetails.FirstOrDefault(a => a.UserId == model.UserId);
                if (b != null)
                {
                    b.BillingPostalCode = model.BillingPostalCode;
                    b.BillingPhone = model.BillingPhone;
                    b.BillingEmail = model.BillingEmail;
                    b.BillingAddressLine1 = model.BillingAddressLine1;
                    b.BillingAddressLine2 = model.BillingAddressLine2;
                    b.BillingTown = model.BillingTown;
                    b.BillingCountry = model.BillingCountry;
                    b.BillingName = model.BillingName;
                    db.SaveChanges();
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, new { message = "Updated", user = u });
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Failed" });
            }
        }

        [Route("admin/DeleteCustomerOrder")]
        [HttpPost]
        public HttpResponseMessage DeleteCustomerOrder(int OrderNo)
        {
            try
            {
                var u = db.ShopCheckOut.FirstOrDefault(a => a.OrderNo == OrderNo);
                if (u != null)
                {
                    u.isActive = false;
                    db.SaveChanges();

                    return this.Request.CreateResponse(HttpStatusCode.OK, new { message = "Deleted", isSuccess = true });
                }
                else
                    return this.Request.CreateResponse(HttpStatusCode.OK, new { message = "Error", isSuccess = false });
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Failed" });
            }
        }

        [Route("admin/DeleteCustomer")]
        [HttpPost]
        public HttpResponseMessage DeleteCustomer(int UserId)
        {
            try
            {
                var u = db.Users.FirstOrDefault(a => a.UserId == UserId);
                if (u != null)
                {
                    u.isActive = false;
                    db.SaveChanges();

                    return this.Request.CreateResponse(HttpStatusCode.OK, new { message = "Deleted", isSuccess = true });
                }
                else
                    return this.Request.CreateResponse(HttpStatusCode.OK, new { message = "Error", isSuccess = false });
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Failed" });
            }
        }

        [Route("admin/UpdateCustomerDetails")]
        [HttpPost]
        public HttpResponseMessage UpdateCustomerDetails(UpdateCustomerDetails model)
        {
            try
            {
                var u = db.Users.FirstOrDefault(a => a.UserId == model.UserId);
                if (u != null)
                {
                    u.UserName = model.Email;
                    u.Email = model.Email;
                    u.FirstName = model.FirstName;
                    u.LastName = model.LastName;
                    u.DOB = model.DOB;

                    db.SaveChanges();
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, new { message = "Updated", user = u });
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Failed" });
            }
        }

        [Route("admin/UpdateCustomerBillingAddress")]
        [HttpPost]
        public HttpResponseMessage UpdateCustomerBillingAddress(UpdateCustomerBillingAddress model)
        {
            try
            {
                var b = db.CustomerBillingDetails.FirstOrDefault(a => a.UserId == model.UserId);
                if (b != null)
                {
                    b.BillingPostalCode = model.BillingPostalCode;
                    b.BillingPhone = model.BillingPhone;
                    b.BillingEmail = model.BillingEmail;
                    b.BillingAddressLine1 = model.BillingAddressLine1;
                    b.BillingAddressLine2 = model.BillingAddressLine2;
                    b.BillingTown = model.BillingTown;
                    b.BillingCountry = model.BillingCountry;
                    b.BillingName = model.BillingName;
                    db.SaveChanges();
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, new { message = "Updated", user = b });
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Failed" });
            }
        }

        [Route("admin/UpdateCustomerNotes")]
        [HttpPost]
        public HttpResponseMessage UpdateCustomerNotes(UpdateCustomerNotes model)
        {
            try
            {
                var u = db.Users.FirstOrDefault(a => a.UserId == model.UserId);
                if (u != null)
                {
                    u.Notes = model.Notes;
                    db.SaveChanges();
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, new { message = "Updated", user = u });
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Failed" });
            }
        }
    }
}


public class CreateCustomer
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string DOB { get; set; }
    public string Gender { get; set; }
    public string BillingName { get; set; }
    public string BillingCountry { get; set; }
    public string BillingAddressLine1 { get; set; }
    public string BillingAddressLine2 { get; set; }
    public string BillingTown { get; set; }
    public string BillingPostalCode { get; set; }
    public string BillingEmail { get; set; }
    public string BillingPhone { get; set; }
    public string Notes { get; set; }
}

public class UpdateCustomer
{
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string DOB { get; set; }
    public string BillingName { get; set; }
    public string BillingCountry { get; set; }
    public string BillingAddressLine1 { get; set; }
    public string BillingAddressLine2 { get; set; }
    public string BillingTown { get; set; }
    public string BillingPostalCode { get; set; }
    public string BillingEmail { get; set; }
    public string BillingPhone { get; set; }
    public string Notes { get; set; }
}

public class UpdateCustomerDetails
{
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string DOB { get; set; }
}

public class UpdateCustomerBillingAddress
{
    public int UserId { get; set; }
    public string BillingName { get; set; }
    public string BillingCountry { get; set; }
    public string BillingAddressLine1 { get; set; }
    public string BillingAddressLine2 { get; set; }
    public string BillingTown { get; set; }
    public string BillingPostalCode { get; set; }
    public string BillingEmail { get; set; }
    public string BillingPhone { get; set; }
}

public class UpdateCustomerNotes
{
    public int UserId { get; set; }
    public string Notes { get; set; }
}

public class BillingDetailsModelView
{
    public string BillingName { get; set; }
    public string BillingCountry { get; set; }
    public string BillingAddressLine1 { get; set; }
    public string BillingAddressLine2 { get; set; }
    public string BillingTown { get; set; }
    public string BillingPostalCode { get; set; }
    public string BillingEmail { get; set; }
    public string BillingPhone { get; set; }
}
