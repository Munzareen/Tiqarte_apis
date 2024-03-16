using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinesEntities
{
    public partial class ContactUsLogs
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Municipality { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public bool isRead { get; set; }
        public bool isAnswered { get; set; }
    }
}
