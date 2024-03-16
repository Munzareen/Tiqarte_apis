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
    public class EventVenueController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        [Route("admin/addEventVenue")]
        public IHttpActionResult AddEventVenue(EventVenueRequest model)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var EventVenue = db.EventVenues.Add(new BusinesEntities.EventVenue
                {
                    PromotorId = PromotoID,
                    Name = model.Name,
                    Location = model.Location,
                    Address = model.Address,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    GoogleMapEmbedCode = model.GoogleMapEmbedCode,
                    BlockAlias = model.BlockAlias,
                    BlocksAlias = model.BlocksAlias,
                    RowAlias = model.RowAlias,
                    RowsAlias = model.RowsAlias,
                    SeatAlias = model.SeatAlias,
                    SeatsAlias = model.SeatsAlias,
                    TableAlias = model.TableAlias,
                    TablesAlias = model.TableAlias,
                    BasicStandardPlan = model.BasicStandardPlan,
                    Notes = model.Notes,
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
        [Route("admin/editEventVenue")]
        public IHttpActionResult EditEventVenue(UpdateEventVenueRequest model)
        {
            var _dc = db.EventVenues.FirstOrDefault(a => a.Id == model.Id);
            if (_dc != null)
            {
                _dc.Name = model.Name;
                _dc.Location = model.Location;
                _dc.Address = model.Address;
                _dc.Latitude = model.Latitude;
                _dc.Longitude = model.Longitude;
                _dc.GoogleMapEmbedCode = model.GoogleMapEmbedCode;
                _dc.BlockAlias = model.BlockAlias;
                _dc.BlocksAlias = model.BlocksAlias;
                _dc.RowAlias = model.RowAlias;
                _dc.RowsAlias = model.RowsAlias;
                _dc.SeatAlias = model.SeatAlias;
                _dc.SeatsAlias = model.SeatsAlias;
                _dc.TableAlias = model.TableAlias;
                _dc.TablesAlias = model.TablesAlias;
                _dc.BasicStandardPlan = model.BasicStandardPlan;
                _dc.Notes = model.Notes;
            }

            db.SaveChanges();
            return Json(_dc);
        }

        [HttpDelete]
        [Route("admin/deleteEventVenue")]
        public IHttpActionResult DeleteEventVenue(int Id)
        {
            var _dc = db.EventVenues.FirstOrDefault(a => a.Id == Id);
            if (_dc != null)
                _dc.isActive = false;

            db.SaveChanges();
            return Json(_dc);
        }

        [HttpGet]
        [Route("admin/getEventVenueById")]
        public IHttpActionResult GetEventVenueById(int Id)
        {
            var data = db.EventVenues.FirstOrDefault(a => a.Id == Id && a.isActive == true);
            return Json(data);
        }

        [HttpGet]
        [Route("admin/getAllEventVenueByPromotor")]
        public IHttpActionResult GetAllEventVenueByPromotor(int PromotorId)
        {
            var data = db.EventVenues.Where(a => a.PromotorId == PromotorId && a.isActive == true).ToList();
            return Json(data);
        }

        [HttpGet]
        [Route("admin/getAllEventVenue")]
        public IHttpActionResult GetAllEventVenue()
        {
            var data = db.EventVenues.Where(a => a.isActive == true).ToList();
            return Json(data);
        }
    }
}
