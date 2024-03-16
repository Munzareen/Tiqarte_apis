using BusinesEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class TicketBookingRequest
    {
        public int EventId { get; set; }
        public List<EventTicketDetails> TicketDetails { get; set; }
        public CustomerContactViewModel CustomerContactInfo { get; set; }
    }

    public partial class CustomerContactViewModel
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string NickName { get; set; }
        public int Gender { get; set; } // 0 For Male and 1 For Female
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public int CountryId { get; set; } // Based On Country Table
    }

    public class TicketBookingResponse
    {
        public string Event { get; set; }
        public string EventDate { get; set; }
        public long EventDateTimeStamp { get; set; }
        public string Location { get; set; }
        public string Organizer { get; set; }
        public string FullName { get; set; }
        public string NickName { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string Country { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public List<TicketBookingDetailResponse> TicketBookingDetail { get; set; }
        public string PaymentMethod { get; set; }
        public int OrderId { get; set; }
        public string Status { get; set; }
        public string BarcodeURL { get; set; }
        public string QRcodeURL { get; set; }
    }

    public class TicketBookingDetailResponse
    {
        public string TicketType { get; set; }
        public int TicketCount { get; set; }
        public decimal TicketPrice { get; set; }
        public decimal TaxAmount { get; set; }
    }

    public class TicketTabList
    {
        public int TicketId { get; set; }
        public int TicketUniqueNumber { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventDate { get; set; }
        public long EventDateTimeStamp { get; set; }
        public string Location { get; set; }
        public string City { get; set; }
        public string ImageURL { get; set; }
        public string Status { get; set; }
        public int TicketCount { get; set; }
        public bool isReviewed { get; set; }
    }

    public class SeasonTicketRequest
    {
        public string Name { get; set; }
        public int EventId { get; set; }
    }

    public class UpdateSeasonTicketRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EventId { get; set; }
    }


    //=============================================================================================//

    public class AddEventTicketRequest
    {
        public EventTicketsRequest TicketInitials { get; set; }
        public TicketDetailsRequest TicketDetails { get; set; }
        public StandardSeatTicketRequest StandardSeatTicketRequest { get; set; }
        public TicketPasswordProtectionRequest TicketPasswordProtectionRequest { get; set; }
    }

    public class TicketDetailsRequest
    {
        public int EventId { get; set; }
        public string TicketType { get; set; }
        public decimal TicketPrice { get; set; }
        public decimal BookingFee { get; set; }
        public int AvailableTickets { get; set; }
        public int? SeasonTicketId { get; set; }
        public int? AttendeeAge { get; set; }
        public bool? HideFromFrontend { get; set; }
        public bool? ExcludeFromOverallCapacity { get; set; }
        public int? MaximumTickets { get; set; }
        public int? MinimumTickets { get; set; }
        public decimal? UnitCost { get; set; }
        public bool? RequiredTicketHolderDetails { get; set; }
        public string TicketDescription { get; set; }
        public string DocumentURL { get; set; }
        public string AcknowledgementURL { get; set; }
        public string MetaDataURL { get; set; }
    }

    public class EventTicketsRequest
    {
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public double EventRunTime { get; set; }
        public bool DisplayEventTime { get; set; }
        public string Location { get; set; }
        public string ManagementFeeType { get; set; }
        public decimal Amount { get; set; }
        public bool Add1EuroBookingFeeUnder10 { get; set; }
        public string Copy { get; set; }
        public string OverrideCapacityScheduleSoldOut { get; set; }
        public int MinimumAge { get; set; }
        public string ProductURL { get; set; }
        public string isItBuyable { get; set; }
        public string MarkAsSold { get; set; }
        public int PromotorId { get; set; }
        public string Venue { get; set; }
    }

    public class TicketPasswordProtectionRequest
    {
        public int EventTicketsId { get; set; }
        public string Password { get; set; }
        public bool? isEnablePasswordProtection { get; set; }
        public string AutoGeneratedLink { get; set; }
        public string Visibility { get; set; }
        public string Slug { get; set; }
        public string URL { get; set; }
        public bool isActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class BlockStandsRequest
    {
        public int StadiumId { get; set; }
        public string BlockStandName { get; set; }
    }

    public class RowsInBlockStandsRequest
    {
        public int BlockStandsId { get; set; }
        public string RowName { get; set; }
    }

    public class SeatsInRowBlockStandsRequest
    {
        public int RowsInBlockStandsId { get; set; }
        public string SeatNumber { get; set; }
        public decimal Price { get; set; }
    }

    public class ExclusionsRequest
    {
        public int StadiumId { get; set; }
        public int BlockStandsId { get; set; }
        public int RowsInBlockStandsId { get; set; }
        public int SeatsInRowBlockStandsId { get; set; }
    }

    public class StandardSeatTicketRequest
    {
        public int EventTicketId { get; set; }
        public int StadiumId { get; set; }
        public int BlockStandId { get; set; }
        public int RowsId { get; set; }
        public int SeatId { get; set; }
    }

    public class VariableSeatTicketRequest
    {
        public string VariationName { get; set; }
        public string VariationColor { get; set; }
        public decimal VariationPrice { get; set; }
        public int SeasonTicketId { get; set; }
        public string AttendeeAgeTitle { get; set; }
        public string SeatApplyFor { get; set; }
        public bool HideFromFrontEnd { get; set; }
        public int StadiumId { get; set; }
    }


    //=============================================================================================//


    public class UpdateTicketDetailsRequest
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string TicketType { get; set; }
        public decimal TicketPrice { get; set; }
        public decimal BookingFee { get; set; }
        public int AvailableTickets { get; set; }
        public int? SeasonTicketId { get; set; }
        public int? AttendeeAge { get; set; }
        public bool? HideFromFrontend { get; set; }
        public bool? ExcludeFromOverallCapacity { get; set; }
        public int? MaximumTickets { get; set; }
        public int? MinimumTickets { get; set; }
        public decimal? UnitCost { get; set; }
        public bool? RequiredTicketHolderDetails { get; set; }
        public string TicketDescription { get; set; }
        public string DocumentURL { get; set; }
        public string AcknowledgementURL { get; set; }
        public string MetaDataURL { get; set; }
    }

    public class UpdateEventTicketsRequest
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public double EventRunTime { get; set; }
        public bool DisplayEventTime { get; set; }
        public string Location { get; set; }
        public string ManagementFeeType { get; set; }
        public decimal Amount { get; set; }
        public bool Add1EuroBookingFeeUnder10 { get; set; }
        public string Copy { get; set; }
        public string OverrideCapacityScheduleSoldOut { get; set; }
        public int MinimumAge { get; set; }
        public string ProductURL { get; set; }
        public string isItBuyable { get; set; }
        public string MarkAsSold { get; set; }
        public string Venue { get; set; }
    }

    public class UpdateTicketPasswordProtectionRequest
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public bool? isEnablePasswordProtection { get; set; }
        public string AutoGeneratedLink { get; set; }
        public string Visibility { get; set; }
        public string Slug { get; set; }
        public string URL { get; set; }
    }

    public class UpdateBlockStandsRequest
    {
        public int Id { get; set; }
        public int StadiumId { get; set; }
        public string BlockStandName { get; set; }
    }

    public class UpdateRowsInBlockStandsRequest
    {
        public int Id { get; set; }
        public int BlockStandsId { get; set; }
        public string RowName { get; set; }
    }

    public class UpdateSeatsInRowBlockStandsRequest
    {
        public int Id { get; set; }
        public int RowsInBlockStandsId { get; set; }
        public string SeatNumber { get; set; }
        public decimal Price { get; set; }
    }

    public class UpdateExclusionsRequest
    {
        public int Id { get; set; }
        public int StadiumId { get; set; }
        public int BlockStandsId { get; set; }
        public int RowsInBlockStandsId { get; set; }
        public int SeatsInRowBlockStandsId { get; set; }
    }

    public class UpdateStandardSeatTicketRequest
    {
        public int Id { get; set; }
        public int StadiumId { get; set; }
        public int RowsId { get; set; }
        public int SeatId { get; set; }
        public int BlockStandId { get; set; }
    }

    public class UpdateVariableSeatTicketRequest
    {
        public int Id { get; set; }
        public string VariationName { get; set; }
        public string VariationColor { get; set; }
        public decimal VariationPrice { get; set; }
        public int SeasonTicketId { get; set; }
        public string AttendeeAgeTitle { get; set; }
        public string SeatApplyFor { get; set; }
        public bool HideFromFrontEnd { get; set; }
        public int StadiumId { get; set; }
    }

    public class UpdateEventTicketRequest
    {
        public UpdateEventTicketsRequest TicketInitials { get; set; }
        public UpdateTicketDetailsRequest TicketDetails { get; set; }
        public UpdateStandardSeatTicketRequest StandardSeatTicketRequest { get; set; }
        public UpdateTicketPasswordProtectionRequest TicketPasswordProtectionRequest { get; set; }
    }


}