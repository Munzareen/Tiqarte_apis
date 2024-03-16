using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BusinesEntities
{
    public class HeadAccount
    {
        [Key]
        public int HeadAccountId { get; set; }
        public int HeadCode { get; set; }
        public string HeadName { get; set; }
    }
}