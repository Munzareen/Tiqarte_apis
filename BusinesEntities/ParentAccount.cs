using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BusinesEntities
{
    public class ParentAccount
    {
        [Key]
        public int ParentAccountId { get; set; }
        
        public int HeadCode { get; set; }
        public string ParentCode { get; set; }
        public string ParentName { get; set; }
        //add old
        [NotMapped]
        public string GroupCode { get; set; }
        [NotMapped]
        public string SubGroupCode { get; set; }

    }
}