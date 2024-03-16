﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinesEntities
{
    public partial class TicketDetails
    {
        [Key]
        public int Id { get; set; }
        public int? PromotorId { get; set; }
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
        public bool? isActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int InitialTicketID { get; set; }
    }

    public partial class TicketBooking
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public int EventId { get; set; }
        public int TicketId { get; set; }
        public int UserId { get; set; }
        public int TicketCount { get; set; }
        public int TicketUniqueNumber { get; set; }
        public string BarCodeURL { get; set; }
        public string QRCodeURL { get; set; }
        public bool? isCancelled { get; set; }
        public string CancelReason { get; set; }
        public DateTime? CancelDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string PaymentStatus { get; set; } //0-Pending, 1-Success, 2-Failed
    }

    public class SeasonTicket
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int EventId { get; set; }
        public int PromotorId { get; set; }
        public bool isActive { get; set; }
        public DateTime CreatedTime { get; set; }
    }

    public class EventTickets
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
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
        public bool isActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class TicketPasswordProtection
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
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


    //=============================================================================================//


    public class BlockStands
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public int StadiumId { get; set; }
        public string BlockStandName { get; set; }
        public bool isActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class RowsInBlockStands
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public int BlockStandsId { get; set; }
        public string RowName { get; set; }
        public bool isActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class SeatsInRowBlockStands
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public int RowsInBlockStandsId { get; set; }
        public string SeatNumber { get; set; }
        public decimal Price { get; set; }
        public bool isActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class Exclusions
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public int StadiumId { get; set; }
        public int BlockStandsId { get; set; }
        public int RowsInBlockStandsId { get; set; }
        public int SeatsInRowBlockStandsId { get; set; }
        public bool isActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class StandardSeatTicket
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public int EventTicketId { get; set; }
        public int StadiumId { get; set; }
        public int BlockStandId { get; set; }
        public int RowsId { get; set; }
        public int SeatId { get; set; }
        public bool isActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class VariableSeatTicket
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public string VariationName { get; set; }
        public string VariationColor { get; set; }
        public decimal VariationPrice { get; set; }
        public int SeasonTicketId { get; set; }
        public string AttendeeAgeTitle { get; set; }
        public string SeatApplyFor { get; set; }
        public bool HideFromFrontEnd { get; set; }
        public int StadiumId { get; set; }
        public bool isActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}