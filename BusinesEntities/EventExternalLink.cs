using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinesEntities
{
    public partial class EventExternalLink
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string URL { get; set; }
        public string ImageURL { get; set; }
        public string Discription { get; set; }
        public string AdvertImageURL1 { get; set; }
        public string AdvertImageURL2 { get; set; }
        public int EventId { get; set; }
    }
}
