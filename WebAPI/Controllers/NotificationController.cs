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
    public class NotificationController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        [Route("admin/createnotification")]
        public IHttpActionResult CreateNotification(NotificationRequest dto)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var scheduledReport = db.Notifications.Add(new Notifications
                {
                    PromotorId = PromotoId,
                    NotificationTitle = dto.NotificationTitle,
                    NotificationTemplateId = dto.NotificationTemplateId,
                    NotificationUserGroupId = dto.NotificationUserGroupId,
                    NotificationText = dto.NotificationText,
                    ScheduledDate = dto.ScheduledDate,
                    ScheduledTime = dto.ScheduledTime,
                    FrequencyId = dto.FrequencyId,
                    TriggerId = dto.TriggerId,
                    isActive = true,
                    CreationTime = DateTime.Now,
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
        [Route("admin/updatenotification")]
        public IHttpActionResult UpdateNotification(UpdateNotificationRequest dto)
        {
            try
            {
                var scheduledReport = db.Notifications.FirstOrDefault(a => a.Id == dto.Id);
                if (scheduledReport != null)
                {
                    scheduledReport.NotificationTitle = dto.NotificationTitle;
                    scheduledReport.NotificationTemplateId = dto.NotificationTemplateId;
                    scheduledReport.NotificationUserGroupId = dto.NotificationUserGroupId;
                    scheduledReport.NotificationText = dto.NotificationText;
                    scheduledReport.ScheduledDate = dto.ScheduledDate;
                    scheduledReport.ScheduledTime = dto.ScheduledTime;
                    scheduledReport.FrequencyId = dto.FrequencyId;
                    scheduledReport.TriggerId = dto.TriggerId;
                };
                db.SaveChanges();
                return Json(scheduledReport);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpDelete]
        [Route("admin/deletenotification")]
        public IHttpActionResult DeleteNotification(int id)
        {
            try
            {
                var scheduledReport = db.Notifications.FirstOrDefault(a => a.Id == id);
                if (scheduledReport != null)
                {
                    scheduledReport.isActive = false;
                };
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
        [Route("admin/getnotificationbyid")]
        public IHttpActionResult GetNotificationById(int id)
        {
            try
            {
                var scheduledReport = db.Notifications.FirstOrDefault(a => a.Id == id);
                return Json(scheduledReport);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("admin/getnotificationbypromotor")]
        public IHttpActionResult GetNotificationByPromotor()
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);


                var scheduledReport = db.Notifications.Where(a => a.PromotorId == PromotoId);
                return Json(scheduledReport);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }




        [HttpPost]
        [Route("admin/createusergroup")]
        public IHttpActionResult CreateUserGroup(NotificationUserGroupRequest dto)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var scheduledReport = db.NotificationUserGroup.Add(new NotificationUserGroup
                {
                    PromotorId = PromotoId,
                    UserGroupTitle = dto.UserGroupTitle,
                    Location = dto.Location,
                    Criteria = dto.Criteria,
                    UserTypeId = dto.UserTypeId,
                    isActive = true,
                    CreationTime = DateTime.Now,
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
        [Route("admin/updateusergroup")]
        public IHttpActionResult UpdateUserGroup(UpdateNotificationUserGroupRequest dto)
        {
            try
            {
                var scheduledReport = db.NotificationUserGroup.FirstOrDefault(a => a.Id == dto.Id);
                if (scheduledReport != null)
                {
                    scheduledReport.UserGroupTitle = dto.UserGroupTitle;
                    scheduledReport.Location = dto.Location;
                    scheduledReport.Criteria = dto.Criteria;
                    scheduledReport.UserTypeId = dto.UserTypeId;
                };
                db.SaveChanges();
                return Json(scheduledReport);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpDelete]
        [Route("admin/deleteusergroup")]
        public IHttpActionResult DeleteUserGroup(int id)
        {
            try
            {
                var scheduledReport = db.NotificationUserGroup.FirstOrDefault(a => a.Id == id);
                if (scheduledReport != null)
                {
                    scheduledReport.isActive = false;
                };
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
        [Route("admin/getusergroupbyid")]
        public IHttpActionResult GetUserGroupById(int id)
        {
            try
            {
                var scheduledReport = db.NotificationUserGroup.FirstOrDefault(a => a.Id == id);
                return Json(scheduledReport);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("admin/getusergroupbypromotor")]
        public IHttpActionResult GetUserGroupByPromotor()
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var scheduledReport = db.NotificationUserGroup.Where(a => a.PromotorId == PromotoId);
                return Json(scheduledReport);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }




        [HttpPost]
        [Route("admin/createtemplate")]
        public IHttpActionResult CreateTemplate(NotificationTemplateRequest dto)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var scheduledReport = db.NotificationTemplate.Add(new NotificationTemplate
                {
                    PromotorId = PromotoId,
                    TemplateDescription = dto.TemplateDescription,
                    TemplateTitle = dto.TemplateTitle,
                    isActive = true,
                    CreationTime = DateTime.Now,
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
        [Route("admin/updatetemplate")]
        public IHttpActionResult UpdateTemplate(UpdateNotificationTemplateRequest dto)
        {
            try
            {
                var scheduledReport = db.NotificationTemplate.FirstOrDefault(a => a.Id == dto.Id);
                if (scheduledReport != null)
                {
                    scheduledReport.TemplateTitle = dto.TemplateTitle;
                    scheduledReport.TemplateDescription = dto.TemplateDescription;
                };
                db.SaveChanges();
                return Json(scheduledReport);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpDelete]
        [Route("admin/deletetemplate")]
        public IHttpActionResult DeleteTemplate(int id)
        {
            try
            {
                var scheduledReport = db.NotificationTemplate.FirstOrDefault(a => a.Id == id);
                if (scheduledReport != null)
                {
                    scheduledReport.isActive = false;
                };
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
        [Route("admin/gettemplatebyid")]
        public IHttpActionResult GetTemplateById(int id)
        {
            try
            {
                var scheduledReport = db.NotificationTemplate.FirstOrDefault(a => a.Id == id);
                return Json(scheduledReport);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("admin/gettemplatebypromotor")]
        public IHttpActionResult GetTemplateByPromotor()
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var scheduledReport = db.NotificationTemplate.Where(a => a.PromotorId == PromotoId);
                return Json(scheduledReport);
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




        //====================================================\\



        [HttpPost]
        [Route("api/addUserNotification")]
        public IHttpActionResult AddUserNotification(UserNotificationsRequest dto)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var UserId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);
                var PromotoId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("promotoid", StringComparison.OrdinalIgnoreCase))?.Value);

                var scheduledReport = db.UserNotifications.Add(new UserNotifications
                {
                    PromotorId = PromotoId,
                    NotificationHeader = dto.NotificationHeader,
                    NotificationType = dto.NotificationType,
                    NotificationText = dto.NotificationText,
                    UserId = UserId,
                    isRead = false,
                    isActive = true,
                    CreationTime = DateTime.Now,
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
        [Route("api/updateUserNotification")]
        public IHttpActionResult UpdateUserNotification(UpdateUserNotificationsRequest dto)
        {
            try
            {
                var scheduledReport = db.UserNotifications.FirstOrDefault(a => a.Id == dto.Id);
                if (scheduledReport != null)
                {
                    scheduledReport.NotificationHeader = dto.NotificationHeader;
                    scheduledReport.NotificationText = dto.NotificationText;
                    scheduledReport.NotificationType = dto.NotificationType;
                };
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
        [Route("api/readUserNotification")]
        public IHttpActionResult ReadUserNotification(int Id)
        {
            try
            {
                var scheduledReport = db.UserNotifications.FirstOrDefault(a => a.Id == Id);
                if (scheduledReport != null)
                {
                    scheduledReport.isRead = true;
                };
                db.SaveChanges();
                return Json(scheduledReport);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpDelete]
        [Route("api/deleteUserNotification")]
        public IHttpActionResult DeleteUserNotification(int id)
        {
            try
            {
                var scheduledReport = db.UserNotifications.FirstOrDefault(a => a.Id == id);
                if (scheduledReport != null)
                {
                    scheduledReport.isActive = false;
                };
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
        [Route("api/getUserNotificationById")]
        public IHttpActionResult GetUserNotificationById(int id)
        {
            try
            {
                var scheduledReport = db.UserNotifications.FirstOrDefault(a => a.Id == id);
                return Json(scheduledReport);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("api/getUserNotificationByUserId")]
        public IHttpActionResult GetUserNotificationByUserId(int UserId)
        {
            try
            {
                var scheduledReport = db.UserNotifications.Where(a => a.UserId == UserId).Select(a => new { a.NotificationHeader, a.NotificationText, a.NotificationType, a.isRead, a.CreationTime, a.iconURL });
                return Json(scheduledReport);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        public int AddUserNotificationLocal(UserNotificationsRequest dto)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var UserId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);
                var PromotoId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("promotoid", StringComparison.OrdinalIgnoreCase))?.Value);

                var scheduledReport = db.UserNotifications.Add(new UserNotifications
                {
                    PromotorId = PromotoId,
                    NotificationHeader = dto.NotificationHeader,
                    NotificationType = dto.NotificationType,
                    NotificationText = dto.NotificationText,
                    UserId = UserId,
                    isRead = false,
                    isActive = true,
                    CreationTime = DateTime.Now,
                });
                return db.SaveChanges();
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                throw;
            }
        }

    }
}
