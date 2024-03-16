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
    public class PolicyController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        [Route("api/addPolicy")]
        public IHttpActionResult AddPolicy()
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var PolicyType = HttpContext.Current.Request.Params["policyType"];
                var PolicyHeading = HttpContext.Current.Request.Params["policyHeading"];
                var PolicyDetails = HttpContext.Current.Request.Params["policyDetails"];
                var _Policies = db.Policies.Add(
                   new Policies
                   {
                       PolicyType = PolicyType,
                       PolicyHeading = PolicyHeading,
                       PolicyDetails = PolicyDetails,
                       PromotorId = PromotoId,
                       isActive = true,
                       CreatedDate = DateTime.Now,
                   });
                db.SaveChanges();
                return Json(_Policies);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpPut]
        [Route("api/editPolicy")]
        public IHttpActionResult EditPolicy(Policies policies)
        {
            try
            {
                var _Policies = db.Policies.FirstOrDefault(a => a.Id == policies.Id);
                if (_Policies == null)
                    return Json("No Record Found");
                else
                {
                    _Policies.PolicyDetails = policies.PolicyDetails;
                    _Policies.PolicyHeading = policies.PolicyHeading;
                    _Policies.PolicyType = policies.PolicyType;
                }

                db.SaveChanges();
                return Json(_Policies);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("api/getAllPolicies")]
        public IHttpActionResult GetPolicies()
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var _Policies = db.Policies.Where(a => a.PromotorId == PromotoId).ToList();
                if (_Policies == null)
                    return Json("No Record Found");

                return Json(_Policies);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpDelete]
        [Route("api/deletePolicyById")]
        public IHttpActionResult DeletePolicyById(int Id)
        {
            try
            {
                var _Policies = db.Policies.FirstOrDefault(a => a.Id == Id);
                if (_Policies == null)
                    return Json("No Record Found");

                _Policies.isActive = false;
                db.SaveChanges();

                return Json("Record Deleted");
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("api/getPolicyById")]
        public IHttpActionResult GetPolicyById(int Id)
        {
            try
            {
                var _Policies = db.Policies.FirstOrDefault(a => a.Id == Id);
                if (_Policies == null)
                    return Json("No Record Found");

                return Json(_Policies);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        //-------------------------------- Customer --------------------------------//

        [HttpGet]
        [Route("api/getAllPolicyTypes")]
        public IHttpActionResult GetAllPolicyTypes(int PromotorId)
        {
            try
            {
                var _Policies = db.Policies.Where(a => a.PromotorId == PromotorId && a.isActive == true).Select(a => a.PolicyType).Distinct().ToList();
                if (_Policies == null)
                    return Json("No Record Found");

                return Json(_Policies);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("api/getPolicyByType")]
        public IHttpActionResult GetPolicyByType(int PromotorId, string Type)
        {
            try
            {
                var _Policies = db.Policies.Where(a => a.PolicyType == Type && a.PromotorId == PromotorId && a.isActive == true).Select(a => new
                {
                    a.PolicyType,
                    a.PolicyHeading,
                    a.PolicyDetails
                }).ToList();
                if (_Policies == null)
                    return Json("No Record Found");

                return Json(_Policies);
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
}
