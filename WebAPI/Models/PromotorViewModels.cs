using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class AddPromotorContactsViewModels
    {
        public int PromotorId { get; set; }
        public string CustomerService { get; set; }
        public string WhatsAppNumber { get; set; }
        public string WebsiteAddress { get; set; }
        public string FacebookId { get; set; }
        public string TwitterId { get; set; }
        public string InstagramId { get; set; }
    }
}