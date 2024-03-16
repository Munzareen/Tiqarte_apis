using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinesEntities
{
    public partial class OTPTable
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OTPCode { get; set; }
        public bool IsCodeVerified { get; set; }
        public DateTime ExpirationTime { get; set; }
        public DateTime CreateDateTime { get; set; } = DateTime.Now;
    }

    public partial class OTPTableTemp
    {
        [Key]
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public int OTPCode { get; set; }
        public bool IsCodeVerified { get; set; }
        public DateTime ExpirationTime { get; set; }
        public DateTime CreateDateTime { get; set; } = DateTime.Now;
    }
}
