using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BusinesEntities
{
    public class Promotor
    {
        [Key]
        public int Id { get; set; }
        //Initials
        public string FullName { get; set; }
        public string NickName { get; set; }
        public string EmailAddress { get; set; }
        public int RoleId { get; set; }
        public int StatusId { get; set; }
        public string Password { get; set; }
        public int SerialNumber { get; set; }
        public DateTime? SuspendedFrom { get; set; }
        public DateTime? SuspendedTo { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class PromotorCompanyDetail
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
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
        public bool isActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class PromotorCheckOut
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public int ReservationDurationMinutes { get; set; }
        public bool DisableGuestCheckout { get; set; } = false;
        public bool AskMarketingOption { get; set; } = true;
        public string MarketingOptionLabel { get; set; }
        public string CopyForTopCheckout { get; set; }
        public bool isActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class PromotorEvents
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public string EventSlug { get; set; }
        public int EventTypeId { get; set; }
        public bool ShowRemainingTicketCount { get; set; }
        public bool isActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class PromotorConfirmationEmail
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public string To { get; set; }
        public string BCC { get; set; }
        public string CC { get; set; }
        public string Write { get; set; }
        public bool isActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class PromotorFormatting
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public int DateFormatId { get; set; }
        public int TimeFormat { get; set; }
        public int APIDomain { get; set; }
        public bool isActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class PromotorCommissionStructure
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
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
        public bool isActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class PromotorSubscription
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public int SubscriptionChargesId { get; set; }
        public int NumberofMonths { get; set; }
        public double Amount { get; set; }
        public int CurrencyId { get; set; }
        public bool isActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class PromotorSupportSubscription
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public int SubscriptionChargesId { get; set; }
        public int NumberofMonths { get; set; }
        public double Amount { get; set; }
        public int CurrencyId { get; set; }
        public bool isActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public partial class PropotorContacts
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public string CustomerService { get; set; }
        public string WhatsAppNumber { get; set; }
        public string WebsiteAddress { get; set; }
        public string EmailAddress { get; set; }
        public string FacebookId { get; set; }
        public string TwitterId { get; set; }
        public string InstagramId { get; set; }
        public string LinkedInId { get; set; }
        public string Address { get; set; }
    }
}

public enum PromotorRole
{
    User = 1,
    Admin = 2,
}

public enum PromotorStatus
{
    Pending = 0,
    Active = 1,
    Suspended = 2,
    Deleted = 3,
}