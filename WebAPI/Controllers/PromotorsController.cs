using BusinesEntities;
using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class PromotorsController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        [Route("admin/addEventPromotor")]
        public IHttpActionResult AddEventPromotor(EventPromotorRequest model)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var EventPromotor = db.EventPromotors.Add(new EventPromotor
                {
                    PromotoId = PromotoID,
                    Name = model.Name,
                    Email = model.Email,
                    Telephone = model.Telephone,
                    ImageUrl = model.ImageUrl,
                    PaymentGateway = model.PaymentGateway,
                    SecretKey = model.SecretKey,
                    APIKey = model.APIKey,
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
        [Route("admin/editEventPromotor")]
        public IHttpActionResult EditEventPromotor(UpdateEventPromotorRequest model)
        {
            var _dc = db.EventPromotors.FirstOrDefault(a => a.Id == model.Id);
            if (_dc != null)
            {
                _dc.Name = model.Name;
                _dc.Email = model.Email;
                _dc.Telephone = model.Telephone;
                _dc.ImageUrl = model.ImageUrl;
                _dc.PaymentGateway = model.PaymentGateway;
                _dc.SecretKey = model.SecretKey;
                _dc.APIKey = model.APIKey;
            }

            db.SaveChanges();
            return Json(_dc);
        }

        [HttpDelete]
        [Route("admin/deleteEventPromotor")]
        public IHttpActionResult DeleteEventPromotor(int Id)
        {
            var _dc = db.EventPromotors.FirstOrDefault(a => a.Id == Id);
            if (_dc != null)
                _dc.isActive = false;

            db.SaveChanges();
            return Json(_dc);
        }

        [HttpGet]
        [Route("admin/getEventPromotorById")]
        public IHttpActionResult GetEventPromotorById(int Id)
        {
            var data = db.EventPromotors.FirstOrDefault(a => a.Id == Id && a.isActive == true);
            return Json(data);
        }

        [HttpGet]
        [Route("admin/getAllEventPromotorByPromotor")]
        public IHttpActionResult GetAllEventPromotorByPromotor(int PromotorId)
        {
            var data = db.EventPromotors.Where(a => a.PromotoId == PromotorId && a.isActive == true).ToList();
            return Json(data);
        }

        [HttpGet]
        [Route("admin/getAllEventPromotor")]
        public IHttpActionResult GetAllEventPromotor()
        {
            var data = db.EventPromotors.Where(a => a.isActive == true).ToList();
            return Json(data);
        }
    }
}