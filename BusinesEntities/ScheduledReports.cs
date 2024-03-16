using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Permissions;

namespace BusinesEntities
{
    public partial class ScheduledReports
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public string ScheduledReportName { get; set; }
        public int ReportId { get; set; }
        public DayOfWeek DayofWeek { get; set; }
        public ReportScheduled ReportScheduled { get; set; }
        public DateTime ReportScheduledTime { get; set; }
        public string EmailAddress { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool isActive { get; set; }
        public DateTime CreationTime { get; set; }

    }

    public enum ReportScheduled
    {
        [EnumMember(Value = "Weekly")]
        Weekly = 1,
        [EnumMember(Value = "BiMonthly")]
        BiMonthly = 2,
        [EnumMember(Value = "Monthly")]
        Monthly = 3,
    }
}
