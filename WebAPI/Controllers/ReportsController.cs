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
    public class ReportsController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        [Route("admin/getReports")]
        public IHttpActionResult GetReports()
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var ScheduledReports = db.Reports.Where(a => a.PromotorId == PromotoId).ToList();
                if (ScheduledReports == null)
                    return Json("No Record Found");


                return Json(ScheduledReports);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("admin/getCheckInsReport")]
        public IHttpActionResult GetCheckInsReport(DateTime? startDate, DateTime? endDate, bool? isCheckedIn, string ticketType)
        {
            try
            {
                List<CheckInReport> lstReport = new List<CheckInReport>();
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                lstReport.Add(new CheckInReport
                {
                    OrderDate = DateTime.Now.AddDays(-11),
                    OrderNumber = 2565211,
                    isCheckedIn = true,
                    CheckedInCount = 1,
                    CheckedInTime = DateTime.Now,
                    TicketType = "VIP",
                    Block = 11,
                    Row = 12,
                    Seat = 13,
                    Scanee = "Test"
                });
                lstReport.Add(new CheckInReport
                {
                    OrderDate = DateTime.Now.AddDays(-5),
                    OrderNumber = 5543455,
                    isCheckedIn = true,
                    CheckedInCount = 10,
                    CheckedInTime = DateTime.Now,
                    TicketType = "Normal",
                    Block = 2,
                    Row = 5,
                    Seat = 22,
                    Scanee = "Test",

                });
                lstReport.Add(new CheckInReport
                {
                    OrderDate = DateTime.Now.AddDays(-2),
                    OrderNumber = 76534323,
                    isCheckedIn = false,
                    CheckedInCount = 6,
                    CheckedInTime = DateTime.Now,
                    TicketType = "VIP",
                    Block = 6,
                    Row = 33,
                    Seat = 2,
                    Scanee = "Test"
                });

                if (!string.IsNullOrWhiteSpace(ticketType))
                    lstReport = lstReport.Where(a => a.TicketType.ToLower() == ticketType.ToLower()).ToList();

                if (startDate != null)
                    lstReport = lstReport.Where(a => a.OrderDate.Date >= startDate.Value.Date).ToList();

                if (endDate != null)
                    lstReport = lstReport.Where(a => a.OrderDate.Date <= endDate.Value.Date).ToList();

                if (isCheckedIn != null)
                    lstReport = lstReport.Where(a => a.isCheckedIn == isCheckedIn).ToList();

                return Json(lstReport);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("admin/getRefundReport")]
        public IHttpActionResult GetRefundReport()
        {
            try
            {
                List<RefundReport> lstReport = new List<RefundReport>();
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotorID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var ShopCheckOut = db.ShopCheckOut.Where(a => a.PromotorId == PromotorID && a.isActive == true && a.isHide == false && a.isRefund == true).ToList();
                foreach (var sco in ShopCheckOut)
                {
                    decimal TotalAmount = 0;
                    var CheckOutProducts = db.CheckOutProducts.Where(a => a.CheckOutId == sco.Id).ToList();
                    foreach (var cop in CheckOutProducts)
                    {
                        var AddToCart = db.AddToCart.Where(a => a.Id == cop.AddToCartId).ToList();
                        foreach (var atc in AddToCart)
                        {
                            var Product = db.ShopProduct.FirstOrDefault(a => a.Id == atc.ProductId);
                            TotalAmount += atc.Quantity * Product.Price;
                        }
                    }

                    lstReport.Add(new RefundReport
                    {
                        Id = sco.Id,
                        OrderNo = sco.OrderNo,
                        Name = sco.CustomerName,
                        Address = sco.AddressLine1,
                        RefundAmount = TotalAmount,
                    });
                }

                return Json(lstReport);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("admin/getTicketTypeCountReport")]
        public IHttpActionResult GetTicketTypeCountReport()
        {
            try
            {
                Dictionary<string, int> keyValuePairs = new Dictionary<string, int>();
                List<TicketTypeCountReport> lstReport = new List<TicketTypeCountReport>();
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);
                var tickets = db.TicketDetails.Where(a => a.PromotorId == PromotoId).ToList();

                foreach (var ticket in tickets)
                {
                    var bookings = db.TicketBookings.Where(a => a.TicketId == ticket.Id && a.PromotorId == PromotoId).Sum(a => a.TicketCount);

                    if (keyValuePairs.ContainsKey(ticket.TicketType))
                    {
                        int count = 0;
                        keyValuePairs.TryGetValue(ticket.TicketType, out count);
                        count += bookings;
                        keyValuePairs[ticket.TicketType] = count;
                    }
                    else
                        keyValuePairs.Add(ticket.TicketType, bookings);
                }



                return Json(keyValuePairs);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("admin/getTicketsReport")]
        public IHttpActionResult GetTicketsReport()
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                return Json("No Record Found");
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" });
            }
        }
    }
}
