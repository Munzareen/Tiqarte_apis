using BusinesEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class DashboardController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        [Route("admin/getdashboardTilesDashboard24H")]
        public IHttpActionResult GetdashboardTiles24H(int? EventId)
        {
            try
            {
                DateTime startDate = DateTime.Now.AddDays(-1);
                DateTime endDate = DateTime.Now;

                decimal BookingFee = 0;
                decimal TicketSales = 0;
                decimal ShopSales = 0;
                int TicketsSold = 0;
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var addtocart = db.AddToCart.Where(a => a.CreatedDate.Value >= startDate && a.CreatedDate.Value <= endDate && a.PromotorId == PromotoID).ToList();
                foreach (var item in addtocart)
                {
                    var prod = db.ShopProduct.Where(a => a.Id == item.ProductId);

                    if (EventId != null)
                        prod = prod.Where(a => a.EventId == EventId);
                    
                    if (prod != null)
                        ShopSales += prod.FirstOrDefault().Price * item.Quantity;
                }

                var ticketbooking = db.TicketBookings.Where(a => a.CreatedDate >= startDate && a.CreatedDate <= endDate && a.PromotorId == PromotoID);

                if (EventId != null)
                    ticketbooking = ticketbooking.Where(a => a.EventId == EventId);
                
                foreach (var item in ticketbooking)
                {
                    var ticketDetails = db.TicketDetails.FirstOrDefault(a => a.Id == item.TicketId);
                    if (ticketDetails != null)
                    {
                        TicketSales += ticketDetails.TicketPrice * item.TicketCount;
                        BookingFee += ticketDetails.BookingFee * item.TicketCount;
                    }
                    TicketsSold += item.TicketCount;
                }

                DashboardTilesViewModel model = new DashboardTilesViewModel();
                model.Revenue = BookingFee + TicketSales + ShopSales;
                model.BookingFee = BookingFee;
                model.Refunds = 0;
                model.TicketSales = TicketSales;
                model.Orders = db.ShopCheckOut.Where(a => a.PurchaseDate >= startDate && a.PurchaseDate <= endDate && a.PromotorId == PromotoID).AsEnumerable().Count();
                model.TicketsSold = TicketsSold;
                model.Customers = db.Users.Where(a => a.UserTypeId == 0 && a.CreationDate >= startDate && a.CreationDate <= endDate && a.PromotorId == PromotoID).AsEnumerable().Count();
                model.ShopSales = ShopSales;

                return Json(model);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("admin/getdashboardTilesDashboard7D")]
        public IHttpActionResult GetdashboardTiles7D(int? EventId)
        {
            try
            {
                DateTime startDate = DateTime.Now.AddDays(-7);
                DateTime endDate = DateTime.Now;

                decimal BookingFee = 0;
                decimal TicketSales = 0;
                decimal ShopSales = 0;
                int TicketsSold = 0;
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var addtocart = db.AddToCart.Where(a => a.CreatedDate.Value >= startDate && a.CreatedDate.Value <= endDate && a.PromotorId == PromotoID).ToList();
                foreach (var item in addtocart)
                {
                    var prod = db.ShopProduct.Where(a => a.Id == item.ProductId);

                    if (EventId != null)
                        prod = prod.Where(a => a.EventId == EventId);

                    if (prod != null)
                        ShopSales += prod.FirstOrDefault().Price * item.Quantity;
                }

                var ticketbooking = db.TicketBookings.Where(a => a.CreatedDate >= startDate && a.CreatedDate <= endDate && a.PromotorId == PromotoID);

                if (EventId != null)
                    ticketbooking = ticketbooking.Where(a => a.EventId == EventId);

                foreach (var item in ticketbooking)
                {
                    var ticketDetails = db.TicketDetails.FirstOrDefault(a => a.Id == item.TicketId);
                    if (ticketDetails != null)
                    {
                        TicketSales += ticketDetails.TicketPrice * item.TicketCount;
                        BookingFee += ticketDetails.BookingFee * item.TicketCount;
                    }
                    TicketsSold += item.TicketCount;
                }

                DashboardTilesViewModel model = new DashboardTilesViewModel();
                model.Revenue = BookingFee + TicketSales + ShopSales;
                model.BookingFee = BookingFee;
                model.Refunds = 0;
                model.TicketSales = TicketSales;
                model.Orders = db.ShopCheckOut.Where(a => a.PurchaseDate >= startDate && a.PurchaseDate <= endDate && a.PromotorId == PromotoID).AsEnumerable().Count();
                model.TicketsSold = TicketsSold;
                model.Customers = db.Users.Where(a => a.UserTypeId == 0 && a.CreationDate >= startDate && a.CreationDate <= endDate && a.PromotorId == PromotoID).AsEnumerable().Count();
                model.ShopSales = ShopSales;

                return Json(model);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("admin/getdashboardTilesDashboard1M")]
        public IHttpActionResult GetdashboardTiles1M(int? EventId)
        {
            try
            {
                DateTime startDate = DateTime.Now.AddDays(-30);
                DateTime endDate = DateTime.Now;

                decimal BookingFee = 0;
                decimal TicketSales = 0;
                decimal ShopSales = 0;
                int TicketsSold = 0;
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var addtocart = db.AddToCart.Where(a => a.CreatedDate.Value >= startDate && a.CreatedDate.Value <= endDate && a.PromotorId == PromotoID).ToList();
                foreach (var item in addtocart)
                {
                    var prod = db.ShopProduct.Where(a => a.Id == item.ProductId);

                    if (EventId != null)
                        prod = prod.Where(a => a.EventId == EventId);

                    if (prod != null)
                        ShopSales += prod.FirstOrDefault().Price * item.Quantity;
                }

                var ticketbooking = db.TicketBookings.Where(a => a.CreatedDate >= startDate && a.CreatedDate <= endDate && a.PromotorId == PromotoID);

                if (EventId != null)
                    ticketbooking = ticketbooking.Where(a => a.EventId == EventId);

                foreach (var item in ticketbooking)
                {
                    var ticketDetails = db.TicketDetails.FirstOrDefault(a => a.Id == item.TicketId);
                    if (ticketDetails != null)
                    {
                        TicketSales += ticketDetails.TicketPrice * item.TicketCount;
                        BookingFee += ticketDetails.BookingFee * item.TicketCount;
                    }
                    TicketsSold += item.TicketCount;
                }

                DashboardTilesViewModel model = new DashboardTilesViewModel();
                model.Revenue = BookingFee + TicketSales + ShopSales;
                model.BookingFee = BookingFee;
                model.Refunds = 0;
                model.TicketSales = TicketSales;
                model.Orders = db.ShopCheckOut.Where(a => a.PurchaseDate >= startDate && a.PurchaseDate <= endDate && a.PromotorId == PromotoID).AsEnumerable().Count();
                model.TicketsSold = TicketsSold;
                model.Customers = db.Users.Where(a => a.UserTypeId == 0 && a.CreationDate >= startDate && a.CreationDate <= endDate && a.PromotorId == PromotoID).AsEnumerable().Count();
                model.ShopSales = ShopSales;

                return Json(model);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("admin/getdashboardTilesDashboard6M")]
        public IHttpActionResult GetdashboardTiles6M(int? EventId)
        {
            try
            {
                DateTime startDate = DateTime.Now.AddDays(-180);
                DateTime endDate = DateTime.Now;

                decimal BookingFee = 0;
                decimal TicketSales = 0;
                decimal ShopSales = 0;
                int TicketsSold = 0;
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var addtocart = db.AddToCart.Where(a => a.CreatedDate.Value >= startDate && a.CreatedDate.Value <= endDate && a.PromotorId == PromotoID).ToList();
                foreach (var item in addtocart)
                {
                    var prod = db.ShopProduct.Where(a => a.Id == item.ProductId);

                    if (EventId != null)
                        prod = prod.Where(a => a.EventId == EventId);

                    if (prod != null)
                        ShopSales += prod.FirstOrDefault().Price * item.Quantity;
                }

                var ticketbooking = db.TicketBookings.Where(a => a.CreatedDate >= startDate && a.CreatedDate <= endDate && a.PromotorId == PromotoID);

                if (EventId != null)
                    ticketbooking = ticketbooking.Where(a => a.EventId == EventId);

                foreach (var item in ticketbooking)
                {
                    var ticketDetails = db.TicketDetails.FirstOrDefault(a => a.Id == item.TicketId);
                    if (ticketDetails != null)
                    {
                        TicketSales += ticketDetails.TicketPrice * item.TicketCount;
                        BookingFee += ticketDetails.BookingFee * item.TicketCount;
                    }
                    TicketsSold += item.TicketCount;
                }

                DashboardTilesViewModel model = new DashboardTilesViewModel();
                model.Revenue = BookingFee + TicketSales + ShopSales;
                model.BookingFee = BookingFee;
                model.Refunds = 0;
                model.TicketSales = TicketSales;
                model.Orders = db.ShopCheckOut.Where(a => a.PurchaseDate >= startDate && a.PurchaseDate <= endDate && a.PromotorId == PromotoID).AsEnumerable().Count();
                model.TicketsSold = TicketsSold;
                model.Customers = db.Users.Where(a => a.UserTypeId == 0 && a.CreationDate >= startDate && a.CreationDate <= endDate && a.PromotorId == PromotoID).AsEnumerable().Count();
                model.ShopSales = ShopSales;

                return Json(model);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("admin/getdashboardTilesDashboard1Y")]
        public IHttpActionResult GetdashboardTiles1Y(int? EventId)
        {
            try
            {
                DateTime startDate = DateTime.Now.AddDays(-180);
                DateTime endDate = DateTime.Now;

                decimal BookingFee = 0;
                decimal TicketSales = 0;
                decimal ShopSales = 0;
                int TicketsSold = 0;
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var addtocart = db.AddToCart.Where(a => a.CreatedDate.Value >= startDate && a.CreatedDate.Value <= endDate && a.PromotorId == PromotoID).ToList();
                foreach (var item in addtocart)
                {
                    var prod = db.ShopProduct.Where(a => a.Id == item.ProductId);

                    if (EventId != null)
                        prod = prod.Where(a => a.EventId == EventId);

                    if (prod != null)
                        ShopSales += prod.FirstOrDefault().Price * item.Quantity;
                }

                var ticketbooking = db.TicketBookings.Where(a => a.CreatedDate >= startDate && a.CreatedDate <= endDate && a.PromotorId == PromotoID);

                if (EventId != null)
                    ticketbooking = ticketbooking.Where(a => a.EventId == EventId);

                foreach (var item in ticketbooking)
                {
                    var ticketDetails = db.TicketDetails.FirstOrDefault(a => a.Id == item.TicketId);
                    if (ticketDetails != null)
                    {
                        TicketSales += ticketDetails.TicketPrice * item.TicketCount;
                        BookingFee += ticketDetails.BookingFee * item.TicketCount;
                    }
                    TicketsSold += item.TicketCount;
                }

                DashboardTilesViewModel model = new DashboardTilesViewModel();
                model.Revenue = BookingFee + TicketSales + ShopSales;
                model.BookingFee = BookingFee;
                model.Refunds = 0;
                model.TicketSales = TicketSales;
                model.Orders = db.ShopCheckOut.Where(a => a.PurchaseDate >= startDate && a.PurchaseDate <= endDate && a.PromotorId == PromotoID).AsEnumerable().Count();
                model.TicketsSold = TicketsSold;
                model.Customers = db.Users.Where(a => a.UserTypeId == 0 && a.CreationDate >= startDate && a.CreationDate <= endDate && a.PromotorId == PromotoID).AsEnumerable().Count();
                model.ShopSales = ShopSales;

                return Json(model);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("admin/getdashboardTilesDashboardNew")]
        public IHttpActionResult GetdashboardTilesNew(DateTime startDate, DateTime endDate, int? EventId)
        {
            try
            {
                decimal BookingFee = 0;
                decimal TicketSales = 0;
                decimal ShopSales = 0;
                int TicketsSold = 0;
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var addtocart = db.AddToCart.Where(a => a.CreatedDate.Value >= startDate && a.CreatedDate.Value <= endDate && a.PromotorId == PromotoID).ToList();
                foreach (var item in addtocart)
                {
                    var prod = db.ShopProduct.Where(a => a.Id == item.ProductId);

                    if (EventId != null)
                        prod = prod.Where(a => a.EventId == EventId);

                    if (prod != null)
                        ShopSales += prod.FirstOrDefault().Price * item.Quantity;
                }

                var ticketbooking = db.TicketBookings.Where(a => a.CreatedDate >= startDate && a.CreatedDate <= endDate && a.PromotorId == PromotoID);

                if (EventId != null)
                    ticketbooking = ticketbooking.Where(a => a.EventId == EventId);

                foreach (var item in ticketbooking)
                {
                    var ticketDetails = db.TicketDetails.FirstOrDefault(a => a.Id == item.TicketId);
                    if (ticketDetails != null)
                    {
                        TicketSales += ticketDetails.TicketPrice * item.TicketCount;
                        BookingFee += ticketDetails.BookingFee * item.TicketCount;
                    }
                    TicketsSold += item.TicketCount;
                }

                DashboardTilesViewModel model = new DashboardTilesViewModel();
                model.Revenue = BookingFee + TicketSales + ShopSales;
                model.BookingFee = BookingFee;
                model.Refunds = 0;
                model.TicketSales = TicketSales;
                model.Orders = db.ShopCheckOut.Where(a => a.PurchaseDate >= startDate && a.PurchaseDate <= endDate && a.PromotorId == PromotoID).AsEnumerable().Count();
                model.TicketsSold = TicketsSold;
                model.Customers = db.Users.Where(a => a.UserTypeId == 0 && a.CreationDate >= startDate && a.CreationDate <= endDate && a.PromotorId == PromotoID).AsEnumerable().Count();
                model.ShopSales = ShopSales;

                return Json(model);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("admin/getCustomersDashboardNew")]
        public IHttpActionResult getCustomersNew(DateTime startDate, DateTime endDate, int? EventId)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var users = db.Users.Where(a => a.UserTypeId == 0 && a.CreationDate >= startDate && a.CreationDate <= endDate && a.PromotorId == PromotoID).AsEnumerable();
                if (EventId != null)
                {
                    users = users.Where(a=> a.eve)
                }

                CustomersViewModel model = new CustomersViewModel();
                model.Total = users.Count();
                model.Male = users.Where(a => a.Gender == "Male" && DateTime.Parse(a.DOB).Year < 2015).Count();
                model.Female = users.Where(a => a.Gender == "Female" && DateTime.Parse(a.DOB).Year < 2015).Count();
                model.Child = users.Where(a => DateTime.Parse(a.DOB).Year >= 2015).Count();
                return Json(model);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }



























        [HttpGet]
        [Route("admin/getdashboardTilesDashboard")]
        public IHttpActionResult GetdashboardTiles(DateTime startDate, DateTime endDate)
        {
            try
            {
                decimal BookingFee = 0;
                decimal TicketSales = 0;
                decimal ShopSales = 0;
                int TicketsSold = 0;
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var addtocart = db.AddToCart.Where(a => a.CreatedDate.Value >= startDate && a.CreatedDate.Value <= endDate && a.PromotorId == PromotoID).ToList();
                foreach (var item in addtocart)
                {
                    var prod = db.ShopProduct.FirstOrDefault(a => a.Id == item.ProductId);
                    if (prod != null)
                        ShopSales += prod.Price * item.Quantity;
                }

                var ticketbooking = db.TicketBookings.Where(a => a.CreatedDate >= startDate && a.CreatedDate <= endDate && a.PromotorId == PromotoID);
                foreach (var item in ticketbooking)
                {
                    var ticketDetails = db.TicketDetails.FirstOrDefault(a => a.Id == item.TicketId);
                    if (ticketDetails != null)
                    {
                        TicketSales += ticketDetails.TicketPrice * item.TicketCount;
                        BookingFee += ticketDetails.BookingFee * item.TicketCount;
                    }
                    TicketsSold += item.TicketCount;
                }

                DashboardTilesViewModel model = new DashboardTilesViewModel();
                model.Revenue = BookingFee + TicketSales + ShopSales;
                model.BookingFee = BookingFee;
                model.Refunds = 0;
                model.TicketSales = TicketSales;
                model.Orders = db.ShopCheckOut.Where(a => a.PurchaseDate >= startDate && a.PurchaseDate <= endDate && a.PromotorId == PromotoID).AsEnumerable().Count();
                model.TicketsSold = TicketsSold;
                model.Customers = db.Users.Where(a => a.UserTypeId == 0 && a.CreationDate >= startDate && a.CreationDate <= endDate && a.PromotorId == PromotoID).AsEnumerable().Count();
                model.ShopSales = ShopSales;

                return Json(model);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("admin/getPopularEventsDashboard")]
        public IHttpActionResult GetPopularEvents(DateTime startDate, DateTime endDate)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);


                List<PopularEventsViewModel> model = new List<PopularEventsViewModel>();
                var eventIds = db.TicketBookings.Where(a => a.CreatedDate >= startDate && a.CreatedDate <= endDate && a.PromotorId == PromotoID).Select(a => a.EventId).Distinct().ToList();
                foreach (var item in eventIds)
                {
                    var events = db.Event.FirstOrDefault(a => a.EventId == item && a.isActive == true);
                    if (events != null)
                    {
                        model.Add(new PopularEventsViewModel
                        {
                            EventName = events.Name,
                            TicketsSold = db.TicketBookings.Where(a => a.EventId == item).Sum(a => a.TicketCount)
                        }); ;
                    }
                }

                return Json(model);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("admin/getCustomersDashboard")]
        public IHttpActionResult getCustomers(DateTime startDate, DateTime endDate)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var users = db.Users.Where(a => a.UserTypeId == 0 && a.CreationDate >= startDate && a.CreationDate <= endDate && a.PromotorId == PromotoID).AsEnumerable();
                CustomersViewModel model = new CustomersViewModel();
                model.Total = users.Count();
                model.Male = users.Where(a => a.Gender == "Male" && DateTime.Parse(a.DOB).Year < 2015).Count();
                model.Female = users.Where(a => a.Gender == "Female" && DateTime.Parse(a.DOB).Year < 2015).Count();
                model.Child = users.Where(a => DateTime.Parse(a.DOB).Year >= 2015).Count();
                return Json(model);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        public void AddDBLogs(string Logs)
        {
            db.AppLogs.Add(new AppLogs
            {
                Logs = Logs
            });
            db.SaveChanges();
        }
    }

    public class DashboardTilesViewModel
    {
        public decimal Revenue { get; set; }
        public decimal BookingFee { get; set; }
        public decimal Refunds { get; set; }
        public decimal TicketSales { get; set; }
        public int Orders { get; set; }
        public int TicketsSold { get; set; }
        public int Customers { get; set; }
        public decimal ShopSales { get; set; }
    }

    public class PopularEventsViewModel
    {
        public string EventName { get; set; }
        public int TicketsSold { get; set; }
    }
    
    public class CustomersViewModel
    {
        public int Total { get; set; }
        public int Male { get; set; }
        public int Female { get; set; }
        public int Child { get; set; }
    }
}
