using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class EventVenueRequest
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string GoogleMapEmbedCode { get; set; }
        public string BlockAlias { get; set; }
        public string BlocksAlias { get; set; }
        public string RowAlias { get; set; }
        public string RowsAlias { get; set; }
        public string SeatAlias { get; set; }
        public string SeatsAlias { get; set; }
        public string TableAlias { get; set; }
        public string TablesAlias { get; set; }
        public string BasicStandardPlan { get; set; }
        public string Notes { get; set; }
    }

    public class EventVenueRequestSA
    {
        public int PromotorId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public int TotalBlocks { get; set; }
        public int TotalAvailableSeats { get; set; }
        public int TotalCapacity { get; set; }
        public string ImageURL { get; set; }
    }

    public class UpdateEventVenueRequestSA
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public int TotalBlocks { get; set; }
        public int TotalAvailableSeats { get; set; }
        public int TotalCapacity { get; set; }
        public string ImageURL { get; set; }
    }

    public class UpdateEventVenueRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string GoogleMapEmbedCode { get; set; }
        public string BlockAlias { get; set; }
        public string BlocksAlias { get; set; }
        public string RowAlias { get; set; }
        public string RowsAlias { get; set; }
        public string SeatAlias { get; set; }
        public string SeatsAlias { get; set; }
        public string TableAlias { get; set; }
        public string TablesAlias { get; set; }
        public string BasicStandardPlan { get; set; }
        public string Notes { get; set; }
    }
}