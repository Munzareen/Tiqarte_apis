using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class SARolesRequest
    {
        public int PromotorId { get; set; }
        public string RoleName { get; set; }
        public string PlanName { get; set; }
        public string GiveAccess { get; set; }
        public bool Create { get; set; }
        public bool Update { get; set; }
        public bool Read { get; set; }
        public bool Delete { get; set; }
    }

    public class SARolesUpdate
    {
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public string RoleName { get; set; }
        public string PlanName { get; set; }
        public string GiveAccess { get; set; }
        public bool Create { get; set; }
        public bool Update { get; set; }
        public bool Read { get; set; }
        public bool Delete { get; set; }
    }
}