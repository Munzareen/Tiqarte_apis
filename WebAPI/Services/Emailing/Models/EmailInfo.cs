using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Services.Emailing.Models
{
    public class EmailInfo
    {
        public string ToEmails { get; set; }

        public string CCEmails { get; set; }

        public string BCCEmails { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public bool IsBodyHtml { get; set; }

        public List<EmailAttachment> Attachments { get; set; }
    }
}
