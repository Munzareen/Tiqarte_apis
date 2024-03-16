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
    public class ScheduledReportsController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        [Route("admin/createscheduledreport")]
        public IHttpActionResult CreateScheduledReport(ScheduledReportsRequest dto)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var scheduledReport = db.ScheduledReports.Add(new ScheduledReports
                {
                    PromotorId = PromotoId,
                    ScheduledReportName = dto.ScheduledReportName,
                    ReportId = dto.ReportId,
                    DayofWeek = dto.DayofWeek,
                    ReportScheduled = dto.ReportScheduled,
                    EmailAddress = dto.EmailAddress,
                    isActive = true,
                    CreationTime = DateTime.Now,
                    ReportScheduledTime = dto.ReportScheduledTime,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                });
                db.SaveChanges();
                return Json(scheduledReport);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpPut]
        [Route("admin/updatescheduledreport")]
        public IHttpActionResult UpdateScheduledReport(ScheduledReportsEditRequest dto)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var scheduledReport = db.ScheduledReports.FirstOrDefault(a => a.Id == dto.Id);
                if (scheduledReport != null)
                {
                    scheduledReport.ScheduledReportName = dto.ScheduledReportName;
                    scheduledReport.ReportId = dto.ReportId;
                    scheduledReport.DayofWeek = dto.DayofWeek;
                    scheduledReport.ReportScheduled = dto.ReportScheduled;
                    scheduledReport.EmailAddress = dto.EmailAddress;
                    scheduledReport.ReportScheduledTime = dto.ReportScheduledTime;
                    scheduledReport.StartDate = dto.StartDate;
                    scheduledReport.EndDate = dto.EndDate;
                }
                db.SaveChanges();
                return Json(scheduledReport);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("admin/getscheduledreports")]
        public IHttpActionResult GetScheduledReports()
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var ScheduledReports = db.ScheduledReports.Where(a => a.PromotorId == PromotoId).ToList();
                if (ScheduledReports == null)
                    return Json("No Record Found");

                return Json(ScheduledReports);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("admin/getscheduledreportbyid")]
        public IHttpActionResult GetScheduledReportById(int Id)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var ScheduledReports = db.ScheduledReports.FirstOrDefault(a => a.PromotorId == PromotoId && a.Id == Id);
                if (ScheduledReports == null)
                    return Json("No Record Found");

                return Json(ScheduledReports);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpDelete]
        [Route("admin/deletescheduledreport")]
        public IHttpActionResult DeleteScheduledReport(int Id)
        {
            try
            {
                var ScheduledReports = db.ScheduledReports.FirstOrDefault(a => a.Id == Id);
                if (ScheduledReports == null)
                    return Json("No Record Found");

                ScheduledReports.isActive = false;
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
