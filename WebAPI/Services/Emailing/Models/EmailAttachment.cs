using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Services.Emailing.Models
{
    public class EmailAttachment
    {
        public EmailAttachment(string attachmentName, string attachmentPath)
        {
            AttachmentName = attachmentName;
            AttachmentPath = attachmentPath;
        }

        public EmailAttachment(string attachmentName, byte[] attachmentContent)
        {
            AttachmentName = attachmentName;
            AttachmentContent = attachmentContent;
        }
        
        public string AttachmentName { get; set; }

        public string AttachmentPath { get; set; }

        public byte[] AttachmentContent { get; set; }
    }
}
