using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace BusinesEntities
{
    public class EventUsers
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; } //1 Admin, 2 Moderator
        public int EventId { get; set; }
        public bool isPOSUser { get; set; }
        public bool isReportOrders { get; set; }
        public bool isActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
