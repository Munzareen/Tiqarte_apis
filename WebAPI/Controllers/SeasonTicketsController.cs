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
    public class SeasonTicketsController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        [Route("admin/getEventSchedules")]
        public IHttpActionResult GetEventSchedules()
        {
            try
            {
                Dictionary<string, List<object>> keyValuePairs = new Dictionary<string, List<object>>();
                var claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotorId = Convert.ToInt32(claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var venus = db.EventTickets.Where(a => a.PromotorId == PromotorId && a.isActive == true).ToList();
                foreach (var item in venus)
                {
                    var ticketDetails = db.TicketDetails.FirstOrDefault(a => a.InitialTicketID == item.Id);
                    if (ticketDetails != null)
                    {
                        var events = db.Event.FirstOrDefault(a => a.EventId == ticketDetails.EventId && a.isActive == true);
                        if (events != null)
                        {
                            var newEvent = new
                            {
                                events.EventId,
                                events.Name,
                                events.EventDate,
                                events.Location,
                                events.City
                            };


                            if (keyValuePairs.ContainsKey(item.Venue))
                            {
                                var list = new List<object>();
                                keyValuePairs.TryGetValue(item.Venue, out list);
                                list.Add(newEvent);
                            }
                            else
                            {
                                List<object> list = new List<object>();
                                list.Add(newEvent);
                                keyValuePairs.Add(item.Venue, list);
                            }
                        }
                    }
                }

                return Json(keyValuePairs);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" });
            }
        }

        [HttpPost]
        [Route("admin/createSeasonTicket")]
        public IHttpActionResult CreateSeasonTicket(SeasonTicketRequest request)
        {
            try
            {
                var claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotorId = Convert.ToInt32(claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);
                var seasonTickets = db.SeasonTickets.Add(new SeasonTicket
                {
                    Name = request.Name,
                    EventId = request.EventId,
                    CreatedTime = DateTime.Now,
                    isActive = true,
                    PromotorId = PromotorId
                });
                db.SaveChanges();
                return Json(seasonTickets);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" });
            }
        }

        [HttpPut]
        [Route("admin/updateSeasonTicket")]
        public IHttpActionResult UpdateSeasonTicket(UpdateSeasonTicketRequest request)
        {
            try
            {
                var claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotorId = Convert.ToInt32(claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);
                var seasonTicket = db.SeasonTickets.FirstOrDefault(a => a.Id == request.Id);
                seasonTicket.Name = request.Name;
                seasonTicket.EventId = request.EventId;

                db.SaveChanges();
                return Json(seasonTicket);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" });
            }
        }

        [HttpDelete]
        [Route("admin/deleteupdateSeasonTicket")]
        public IHttpActionResult DeleteSeasonTicket(int Id)
        {
            try
            {
                var claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotorId = Convert.ToInt32(claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);
                var seasonTicket = db.SeasonTickets.FirstOrDefault(a => a.Id == Id);
                seasonTicket.isActive = false;

                db.SaveChanges();
                return Json(seasonTicket);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("admin/getSeasonTickets")]
        public IHttpActionResult GetSeasonTickets()
        {
            try
            {
                var claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotorId = Convert.ToInt32(claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var Events = db.SeasonTickets.Where(a => a.PromotorId == PromotorId && a.isActive == true).ToList();
                return Json(Events);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" });
            }
        }

    }
}
