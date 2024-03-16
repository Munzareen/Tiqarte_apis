using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinesEntities
{
    public class OnBoarding
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public string ImageUrl { get; set; }
        public string Heading { get; set; }
        public string Description { get; set; }
        public bool isActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
