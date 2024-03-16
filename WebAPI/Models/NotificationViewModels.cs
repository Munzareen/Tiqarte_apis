using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class NotificationRequest
    {
        public string NotificationTitle { get; set; }
        public int NotificationTemplateId { get; set; }
        public int NotificationUserGroupId { get; set; }
        public string NotificationText { get; set; }
        public int FrequencyId { get; set; }
        public int TriggerId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public DateTime ScheduledTime { get; set; }
    }

    public class UpdateNotificationRequest
    {
        public int Id { get; set; }
        public string NotificationTitle { get; set; }
        public int NotificationTemplateId { get; set; }
        public int NotificationUserGroupId { get; set; }
        public string NotificationText { get; set; }
        public int FrequencyId { get; set; }
        public int TriggerId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public DateTime ScheduledTime { get; set; }
    }

    public partial class NotificationTemplateRequest
    {
        public string TemplateTitle { get; set; }
        public string TemplateDescription { get; set; }
    }

    public partial class UpdateNotificationTemplateRequest
    {
        public int Id { get; set; }
        public string TemplateTitle { get; set; }
        public string TemplateDescription { get; set; }
    }

    public partial class NotificationUserGroupRequest
    {
        public string UserGroupTitle { get; set; }
        public string Location { get; set; }
        public int UserTypeId { get; set; }
        public string Criteria { get; set; }
    }

    public partial class UpdateNotificationUserGroupRequest
    {
        public int Id { get; set; }
        public string UserGroupTitle { get; set; }
        public string Location { get; set; }
        public int UserTypeId { get; set; }
        public string Criteria { get; set; }
    }

    public class UserNotificationsRequest
    {
        public int PromotorId { get; set; }
        public int UserId { get; set; }
        public string NotificationHeader { get; set; }
        public string NotificationText { get; set; }
        public string NotificationType { get; set; }
    }

    public class UpdateUserNotificationsRequest
    {
        public int Id { get; set; }
        public string NotificationHeader { get; set; }
        public string NotificationText { get; set; }
        public string NotificationType { get; set; }
    }
}