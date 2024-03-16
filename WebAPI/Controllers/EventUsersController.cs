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
    public class EventUsersController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        [Route("admin/addEventUser")]
        public IHttpActionResult AddEventUser(EventUserRequest model)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var EventUser = db.EventUsers.Add(new BusinesEntities.EventUsers
                {
                    PromotorId = PromotoID,
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password,
                    RoleId = model.RoleId,
                    EventId = model.EventId,
                    isPOSUser = model.isPOSUser,
                    isReportOrders = model.isReportOrders,
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
        [Route("admin/editEventUser")]
        public IHttpActionResult EditEventUser(UpdateEventUserRequest model)
        {
            var _dc = db.EventUsers.FirstOrDefault(a => a.Id == model.Id);
            if (_dc != null)
            {
                _dc.Name = model.Name;
                _dc.Email = model.Email;
                _dc.Password = model.Password;
                _dc.RoleId = model.RoleId;
                _dc.EventId = model.EventId;
                _dc.isPOSUser = model.isPOSUser;
                _dc.isReportOrders = model.isReportOrders;
            }

            db.SaveChanges();
            return Json(_dc);
        }

        [HttpDelete]
        [Route("admin/deleteEventUser")]
        public IHttpActionResult DeleteEventUser(int Id)
        {
            var _dc = db.EventUsers.FirstOrDefault(a => a.Id == Id);
            if (_dc != null)
                _dc.isActive = false;

            db.SaveChanges();
            return Json(_dc);
        }

        [HttpGet]
        [Route("admin/getEventUserById")]
        public IHttpActionResult GetEventUserById(int Id)
        {
            var data = db.EventUsers.FirstOrDefault(a => a.Id == Id && a.isActive == true);
            return Json(data);
        }

        [HttpGet]
        [Route("admin/getAllEventUserByPromotor")]
        public IHttpActionResult GetAllEventUserByPromotor(int PromotorId)
        {
            var data = db.EventUsers.Where(a => a.PromotorId == PromotorId && a.isActive == true).ToList();
            return Json(data);
        }

        [HttpGet]
        [Route("admin/getAllEventUser")]
        public IHttpActionResult GetAllEventUser()
        {
            var data = db.EventUsers.Where(a => a.isActive == true).ToList();
            return Json(data);
        }

        [HttpGet]
        [Route("admin/getEventUsersBySearch")]
        public IHttpActionResult GetEventUsersBySearch(string SearchText)
        {
            var data = db.EventUsers.Where(a => (a.Name.Trim().ToLower().Contains(SearchText.Trim().ToLower()) || a.Email.Trim().ToLower().Contains(SearchText.Trim().ToLower()) || a.Email.Trim().ToLower().Contains(SearchText.Trim().ToLower())) && a.isActive == true).ToList();
            return Json(data);
        }

        [HttpGet]
        [Route("admin/getEventUsersByRole")]
        public IHttpActionResult GetEventUsersByRole(int RoleId)
        {
            var data = db.EventUsers.Where(a => a.RoleId == RoleId && a.isActive == true).ToList();
            return Json(data);
        }

    }
}
