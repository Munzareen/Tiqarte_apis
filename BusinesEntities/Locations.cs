using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinesEntities
{
    public class Locations
    {
        [Key]
        public int Id { get; set; }
        public string LocationName { get; set; }
        public bool isActive { get; set; }
    }
}
