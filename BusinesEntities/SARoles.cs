using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinesEntities
{
    public class Roles
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public string RoleName { get; set; }
        public string PlanName { get; set; }
        public string GiveAccess { get; set; }
        public bool Create { get; set; }
        public bool Update { get; set; }
        public bool Read { get; set; }
        public bool Delete { get; set; }
        public bool isActive { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
