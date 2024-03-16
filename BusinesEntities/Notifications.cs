using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinesEntities
{
    public partial class Notifications
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public string NotificationTitle { get; set; }
        public int NotificationTemplateId { get; set; }
        public int NotificationUserGroupId { get; set; }
        public string NotificationText { get; set; }
        public DateTime ScheduledDate { get; set; }
        public DateTime ScheduledTime { get; set; }
        public int FrequencyId { get; set; }
        public int TriggerId { get; set; }
        public bool isActive { get; set; }
        public DateTime CreationTime { get; set; }
    }

    public partial class NotificationTemplate
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public string TemplateTitle { get; set; }
        public string TemplateDescription { get; set; }
        public bool isActive { get; set; }
        public DateTime CreationTime { get; set; }
    }

    public partial class NotificationUserGroup
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public string UserGroupTitle { get; set; }
        public string Location { get; set; }
        public int UserTypeId { get; set; }
        public string Criteria { get; set; }
        public bool isActive { get; set; }
        public DateTime CreationTime { get; set; }
    }

    public class UserNotifications
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public int UserId { get; set; }
        public string NotificationHeader { get; set; }
        public string NotificationText { get; set; }
        public string NotificationType { get; set; }
        public string iconURL { get; set; }
        public bool isRead { get; set; }
        public bool isActive { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
