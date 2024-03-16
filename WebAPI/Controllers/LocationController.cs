using BusinesEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class LocationController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        [Route("api/addLocation")]
        public IHttpActionResult AddLocation(string LocationName)
        {
            try
            {
                var _Policies = db.Locations.Add(
                   new Locations
                   {
                       LocationName = LocationName,
                       isActive = true,
                   });
                db.SaveChanges();
                return Json(_Policies);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" });
            }
        }

        [HttpPut]
        [Route("api/editLocation")]
        public IHttpActionResult EditLocation(int Id, string LocationName)
        {
            try
            {
                var _Policies = db.Locations.FirstOrDefault(a => a.Id == Id);
                if (_Policies == null)
                    return Json("No Record Found");
                else
                {
                    _Policies.LocationName = LocationName;
                }

                db.SaveChanges();
                return Json(_Policies);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("api/getAllLocations")]
        public IHttpActionResult GetAllLocations()
        {
            try
            {
                var _Policies = db.Locations.ToList();
                if (_Policies == null)
                    return Json("No Record Found");

                return Json(_Policies);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" });
            }
        }

        [HttpDelete]
        [Route("api/deleteLocation")]
        public IHttpActionResult DeleteLocation(int Id)
        {
            try
            {
                var _Policies = db.Locations.FirstOrDefault(a => a.Id == Id);
                if (_Policies == null)
                    return Json("No Record Found");

                _Policies.isActive = false;
                db.SaveChanges();

                return Json("Record Deleted");
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("api/getLocationsById")]
        public IHttpActionResult GetLocationsById(int Id)
        {
            try
            {
                var _Policies = db.Locations.FirstOrDefault(a => a.Id == Id);
                if (_Policies == null)
                    return Json("No Record Found");

                return Json(_Policies);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" });
            }
        }
    }
}