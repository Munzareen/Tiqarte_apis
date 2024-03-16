using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinesEntities
{
    public partial class Payment
    {
        public decimal Id { get; set; }
        public decimal? Amount { get; set; }
        public decimal? CustomerId { get; set; }
        public TimeSpan? Time { get; set; }
        public DateTime? Date { get; set; }
    }

    public class PromotorPayment
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public string PromotorName { get; set; }
        public string Currency { get; set; }
        public decimal? Amount { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public string InvoiceNumber { get; set; }
        public string Description { get; set; }
        public bool isActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
