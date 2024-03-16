using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinesEntities
{
    public partial class FAQs
    {
        [Key]
        public int Id { get; set; }
        public string FAQType { get; set; }
        public string FAQQuestion { get; set; }
        public string FAQAnswer { get; set; }
    }
}
