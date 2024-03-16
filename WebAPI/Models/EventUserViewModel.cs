using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class EventUserRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; } //1 Admin, 2 Moderator
        public int EventId { get; set; }
        public bool isPOSUser { get; set; }
        public bool isReportOrders { get; set; }
    }

    public class UpdateEventUserRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; } //1 Admin, 2 Moderator
        public int EventId { get; set; }
        public bool isPOSUser { get; set; }
        public bool isReportOrders { get; set; }
    }
}