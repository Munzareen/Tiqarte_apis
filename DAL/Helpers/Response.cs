using BusinesEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Helpers
{
    public class response
    {
        public string Status { get; set; }
        public string Filename { get; set; }
        public string ImagePath { get; set; }
        public double ? Weight { get; set; }
        public double? Quantity { get; set; }
        public string TagNo { get; set; }

        public List<string> Images { get; set; }
    }
}
