using BusinesEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class FAQController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        [Route("api/addFAQ")]
        public IHttpActionResult AddFAQ()
        {
            try
            {
                var FAQType = HttpContext.Current.Request.Params["FAQType"];
                var FAQQuestion = HttpContext.Current.Request.Params["FAQQuestion"];
                var FAQAnswer = HttpContext.Current.Request.Params["FAQAnswer"];
                var _FAQs = db.FAQs.Add(
                   new FAQs
                   {
                       FAQType = FAQType,
                       FAQQuestion = FAQQuestion,
                       FAQAnswer = FAQAnswer
                   });

                db.SaveChanges();
                return Json(_FAQs);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpPut]
        [Route("api/editFAQ")]
        public IHttpActionResult EditFAQ(FAQs FAQs)
        {
            try
            {
                var _FAQs = db.FAQs.FirstOrDefault(a => a.Id == FAQs.Id);
                if (_FAQs == null)
                    return Json("No Record Found");
                else
                {
                    _FAQs.FAQType = FAQs.FAQType;
                    _FAQs.FAQQuestion = FAQs.FAQQuestion;
                    _FAQs.FAQAnswer = FAQs.FAQAnswer;
                }

                db.SaveChanges();
                return Json(_FAQs);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("api/getAllFAQs")]
        public IHttpActionResult GetFAQs()
        {
            try
            {
                var _FAQs = db.FAQs.ToList();
                if (_FAQs == null)
                    return Json("No Record Found");

                return Json(_FAQs);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("api/getAllFAQTypes")]
        public IHttpActionResult GetAllFAQTypes()
        {
            try
            {
                var _FAQTypes = db.FAQs.Select(a => a.FAQType).Distinct().ToList();
                if (_FAQTypes == null)
                    return Json("No Record Found");

                return Json(_FAQTypes);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("api/getFAQByType")]
        public IHttpActionResult GetFAQByType(string Type)
        {
            try
            {
                var _FAQs = db.FAQs.Where(a => a.FAQType == Type).ToList();
                if (_FAQs == null)
                    return Json("No Record Found");

                return Json(_FAQs);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("api/searchFAQ")]
        public IHttpActionResult SearchFAQ(string SearchText)
        {
            try
            {
                var _FAQs = db.FAQs.Where(a => a.FAQType.Contains(SearchText) || a.FAQQuestion.Contains(SearchText) || a.FAQAnswer.Contains(SearchText)).ToList();
                if (_FAQs == null)
                    return Json("No Record Found");

                return Json(_FAQs);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("api/searchFAQByType")]
        public IHttpActionResult SearchFAQByType(string SearchText, string Type)
        {
            try
            {
                var _FAQs = db.FAQs.Where(a => a.FAQType == Type && a.FAQQuestion.Contains(SearchText)).ToList();
                if (_FAQs == null)
                    return Json("No Record Found");

                return Json(_FAQs);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("api/getFAQById")]
        public IHttpActionResult GetFAQById(int Id)
        {
            try
            {
                var _FAQs = db.FAQs.FirstOrDefault(a => a.Id == Id);
                if (_FAQs == null)
                    return Json("No Record Found");

                return Json(_FAQs);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpDelete]
        [Route("api/deleteFAQById")]
        public IHttpActionResult DeleteFAQById(int Id)
        {
            try
            {
                var _FAQs = db.FAQs.FirstOrDefault(a => a.Id == Id);
                if (_FAQs == null)
                    return Json("No Record Found");

                db.FAQs.Remove(_FAQs);
                db.SaveChanges();

                return Json("Record Deleted");
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
