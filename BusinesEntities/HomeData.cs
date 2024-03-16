using System;
using System.Collections.Generic;


namespace BusinesEntities
{
    public partial class HomeData
    {
        public string WelcomeMessage { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string UserName { get; set; }
        public BusinesEntities.Event[] FeaturedEvents { get; set; }
        public BusinesEntities.Event[] UpComingEvents { get; set; }
        public BusinesEntities.ShopProduct[] Shop { get; set; }
        public BusinesEntities.EventType[] EventType { get; set; }
        public int UserId { get; set; }
    }
}
