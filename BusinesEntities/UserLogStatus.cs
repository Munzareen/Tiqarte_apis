using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinesEntities
{
    public class UserLogStatus
    {
        [Key]
        public int LogId { get; set; }
        public string TableName { get; set; }
        public string PerformOpration { get; set; }
        public int TableEntryId { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string NewValue { get; set; }
        public string OldValue { get; set; }
    }
}
