using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Services.Emailing.Models
{
    public class SendGridSettings
    {
        public const string Key = "Email:SendGridSettings";

        public string APIKey { get; set; }
    }
}
