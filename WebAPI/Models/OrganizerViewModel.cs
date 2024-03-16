using BusinesEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class OrganizerViewModel
    {
        public bool isFollow { get; set; }
        public Organizer Organizer { get; set; }
        public List<Event> Events { get; set; }
        public List<OrganizerCollections> Collections { get; set; }
    }
}