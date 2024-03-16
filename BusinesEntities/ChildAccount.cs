using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BusinesEntities
{
    public class ChildAccount
    {
        [Key]
        public int ChildAccountId { get; set; }
        public double OpeningCash { get; set; }
        public double OpeningGold { get; set; }
        public string Status { get; set; }
        public int HeadCode { get; set; }
        public string ParentCode { get; set; }
        public string Type { get; set; }
        public int? AccountType { get; set; }
        public string Description { get; set; }
        public DateTime? Deletedon { get; set; }
        public DateTime CreatedOn { get; set; }
        // add Old
     
        public string ChildCode { get; set; }
        [NotMapped]
        public string VNO { get; set; }
   
        public string ChildName { get; set; }
        [NotMapped]
        public string GroupCode { get; set; }
        [NotMapped]
        public string SubGroupCode { get; set; }
        [NotMapped]

        public double Balance { get; set; }
        [NotMapped]
        public double OpCash { get; set; }
        [NotMapped]
        public double OpGold { get; set; }
        [NotMapped]
        public DateTime Date { get; set; }
        [NotMapped]

        public string Address { get; set; }

    }
}