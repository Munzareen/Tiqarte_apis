using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinesEntities
{
    public partial class Policies
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public string PolicyType { get; set; }
        public string PolicyHeading { get; set; }
        public string PolicyDetails { get; set; }
        public bool isActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
