using BusinesEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class ScheduledReportsRequest
    {
        public string ScheduledReportName { get; set; }
        public int ReportId { get; set; }
        public DayOfWeek DayofWeek { get; set; }
        public ReportScheduled ReportScheduled { get; set; }
        public DateTime ReportScheduledTime { get; set; }
        public string EmailAddress { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class ScheduledReportsEditRequest
    {
        public int Id { get; set; }
        public string ScheduledReportName { get; set; }
        public int ReportId { get; set; }
        public DayOfWeek DayofWeek { get; set; }
        public ReportScheduled ReportScheduled { get; set; }
        public DateTime ReportScheduledTime { get; set; }
        public string EmailAddress { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class CheckInReport
    {
        public DateTime OrderDate { get; set; }
        public long OrderNumber { get; set; }
        public bool isCheckedIn { get; set; }
        public int CheckedInCount { get; set; }
        public DateTime CheckedInTime { get; set; }
        public string TicketType { get; set; }
        public int Block { get; set; }
        public int Row { get; set; }
        public int Seat { get; set; }
        public string Scanee { get; set; }
    }

    public class RefundReport
    {
        public int Id { get; set; }
        public long OrderNo { get; set; }
        public string Name { get; set; }
        public string  Address { get; set; }
        public decimal RefundAmount { get; set; }
    }

    public class TicketTypeCountReport
    {
        public string TicketType { get; set; }
        public int TicketCount { get; set; }
    }

}