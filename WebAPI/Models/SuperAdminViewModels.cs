using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    //========================================== CREATE ==========================================\\
    public class CreatePromotorRequest
    {
        public PromotorRequest PromotorRequest { get; set; }
        public PromotorCompanyDetailRequest PromotorCompanyDetailRequest { get; set; }
        public PromotorCheckOutRequest PromotorCheckOutRequest { get; set; }
        public PromotorEventsRequest PromotorEventsRequest { get; set; }
        public PromotorConfirmationEmailRequest PromotorConfirmationEmailRequest { get; set; }
        public PromotorFormattingRequest PromotorFormattingRequest { get; set; }
        public PromotorCommissionStructureRequest PromotorCommissionStructureRequest { get; set; }
        public PromotorSubscriptionRequest PromotorSubscriptionRequest { get; set; }
        public PromotorSupportSubscriptionRequest PromotorSupportSubscriptionRequest { get; set; }
    }

    public class PromotorRequest
    {
        public string FullName { get; set; }
        public string NickName { get; set; }
        public string EmailAddress { get; set; }
        public int RoleId { get; set; }
        public int StatusId { get; set; }
        public string Password { get; set; }
        public int SerialNumber { get; set; }
    }

    public class PromotorCompanyDetailRequest
    {
        public int MunicipalityId { get; set; }
        public int CityId { get; set; }
        public string CompanyEmailAddress { get; set; }
        public string BillingAddress { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string Telephone { get; set; }
        public string TaxIdentificationNumber { get; set; }
        public string PostalCode { get; set; }
        public string CopyRights { get; set; }
        public string LegalDisclaimer { get; set; }
        public string FacebookLine { get; set; }
        public string InstaLink { get; set; }
        public string LinkedinLink { get; set; }
        public string TwitterLink { get; set; }
    }

    public class PromotorCheckOutRequest
    {
        public int ReservationDurationMinutes { get; set; }
        public bool DisableGuestCheckout { get; set; } = false;
        public bool AskMarketingOption { get; set; } = true;
        public string MarketingOptionLabel { get; set; }
        public string CopyForTopCheckout { get; set; }
    }

    public class PromotorEventsRequest
    {
        public string EventSlug { get; set; }
        public int EventTypeId { get; set; }
        public bool ShowRemainingTicketCount { get; set; }
    }

    public class PromotorConfirmationEmailRequest
    {
        public string To { get; set; }
        public string BCC { get; set; }
        public string CC { get; set; }
        public string Write { get; set; }
    }

    public class PromotorFormattingRequest
    {
        public int DateFormatId { get; set; }
        public int TimeFormat { get; set; }
        public int APIDomain { get; set; }
        public bool isActive { get; set; }
    }

    public class PromotorCommissionStructureRequest
    {
        public int ContractDuration { get; set; }
        public int CurrencyId { get; set; }
        public double CommissionPercentage { get; set; }
        public bool OnEachEntry { get; set; }
        public bool OnEachProduct { get; set; }
        public double FixAmount { get; set; }
        public double TransactionAmount { get; set; }
        public double CommissionCap { get; set; }
        public int TaxTypeId { get; set; }
        public double TaxPercentage { get; set; }
    }

    public class PromotorSubscriptionRequest
    {
        public int SubscriptionChargesId { get; set; }
        public int NumberofMonths { get; set; }
        public double Amount { get; set; }
        public int CurrencyId { get; set; }
    }

    public class PromotorSupportSubscriptionRequest
    {
        public int SubscriptionChargesId { get; set; }
        public int NumberofMonths { get; set; }
        public double Amount { get; set; }
        public int CurrencyId { get; set; }
    }

    //========================================== UPDATE ==========================================\\

    public class UpdatePromotorRequest
    {
        public UpdatePromotor PromotorRequest { get; set; }
        public UpdatePromotorCompanyDetail PromotorCompanyDetailRequest { get; set; }
        public UpdatePromotorCheckOut PromotorCheckOutRequest { get; set; }
        public UpdatePromotorEvents PromotorEventsRequest { get; set; }
        public UpdatePromotorConfirmationEmail PromotorConfirmationEmailRequest { get; set; }
        public UpdatePromotorFormatting PromotorFormattingRequest { get; set; }
        public UpdatePromotorCommissionStructure PromotorCommissionStructureRequest { get; set; }
        public UpdatePromotorSubscription PromotorSubscriptionRequest { get; set; }
        public UpdatePromotorSupportSubscription PromotorSupportSubscriptionRequest { get; set; }
    }

    public class UpdatePromotor
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string NickName { get; set; }
        public string EmailAddress { get; set; }
        public int RoleId { get; set; }
        public int StatusId { get; set; }
        public string Password { get; set; }
        public int SerialNumber { get; set; }
    }

    public class UpdatePromotorCompanyDetail
    {
        public int Id { get; set; }
        public int MunicipalityId { get; set; }
        public int CityId { get; set; }
        public string CompanyEmailAddress { get; set; }
        public string BillingAddress { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string Telephone { get; set; }
        public string TaxIdentificationNumber { get; set; }
        public string PostalCode { get; set; }
        public string CopyRights { get; set; }
        public string LegalDisclaimer { get; set; }
        public string FacebookLine { get; set; }
        public string InstaLink { get; set; }
        public string LinkedinLink { get; set; }
        public string TwitterLink { get; set; }
    }

    public class UpdatePromotorCheckOut
    {
        public int Id { get; set; }
        public int ReservationDurationMinutes { get; set; }
        public bool DisableGuestCheckout { get; set; } = false;
        public bool AskMarketingOption { get; set; } = true;
        public string MarketingOptionLabel { get; set; }
        public string CopyForTopCheckout { get; set; }
    }

    public class UpdatePromotorEvents
    {
        public int Id { get; set; }
        public string EventSlug { get; set; }
        public int EventTypeId { get; set; }
        public bool ShowRemainingTicketCount { get; set; }
    }

    public class UpdatePromotorConfirmationEmail
    {
        public int Id { get; set; }
        public string To { get; set; }
        public string BCC { get; set; }
        public string CC { get; set; }
        public string Write { get; set; }
    }

    public class UpdatePromotorFormatting
    {
        public int Id { get; set; }
        public int DateFormatId { get; set; }
        public int TimeFormat { get; set; }
        public int APIDomain { get; set; }
        public bool isActive { get; set; }
    }

    public class UpdatePromotorCommissionStructure
    {
        public int Id { get; set; }
        public int ContractDuration { get; set; }
        public int CurrencyId { get; set; }
        public double CommissionPercentage { get; set; }
        public bool OnEachEntry { get; set; }
        public bool OnEachProduct { get; set; }
        public double FixAmount { get; set; }
        public double TransactionAmount { get; set; }
        public double CommissionCap { get; set; }
        public int TaxTypeId { get; set; }
        public double TaxPercentage { get; set; }
    }

    public class UpdatePromotorSubscription
    {
        public int Id { get; set; }
        public int SubscriptionChargesId { get; set; }
        public int NumberofMonths { get; set; }
        public double Amount { get; set; }
        public int CurrencyId { get; set; }
    }

    public class UpdatePromotorSupportSubscription
    {
        public int Id { get; set; }
        public int SubscriptionChargesId { get; set; }
        public int NumberofMonths { get; set; }
        public double Amount { get; set; }
        public int CurrencyId { get; set; }
    }

    //========================================== User ==========================================\\

    public class PromotorUser
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int PromotorId { get; set; }
        public string Role { get; set; }
        public bool Status { get; set; } = true;
        public string Password { get; set; }
    }
}