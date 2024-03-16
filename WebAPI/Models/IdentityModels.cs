using BusinesEntities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Gender { get; set; }
        public string NickName { get; set; }
        public bool isProfileCompleted { get; set; }
        public bool isVerified { get; set; }
        public string DOB { get; set; }
        public string FullName { get; set; }
        public string ImageUrl { get; set; }
        public int UserTypeId { get; set; } // 0 For Promotor, 1 For Customer
        public string Location { get; set; } // Lat and Long
        public string State { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public int PromotorId { get; set; } = 1;
        public string Notes { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool isActive { get; set; } = true;
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<purli>();
            base.OnModelCreating(modelBuilder);
            //Used Store Procedure
            modelBuilder.Types().Configure(t => t.MapToStoredProcedures());
            //AspNetUsers -> User
            modelBuilder.Entity<ApplicationUser>()
                .ToTable("User");
            //AspNetRoles -> Role
            modelBuilder.Entity<IdentityRole>()
                .ToTable("Role");
            //AspNetUserRoles -> UserRole.. 
            modelBuilder.Entity<IdentityUserRole>()
                .ToTable("UserRole");
            //AspNetUserClaims -> UserClaim
            modelBuilder.Entity<IdentityUserClaim>()
                .ToTable("UserClaim");
            //AspNetUserLogins -> UserLogin
            modelBuilder.Entity<IdentityUserLogin>()
                .ToTable("UserLogin");
        }

        public DbSet<AccountMaster> AccountMasters { get; set; }
        public DbSet<ChildAccount> ChildAccounts { get; set; }
        public DbSet<ParentAccount> ParentAccounts { get; set; }
        public DbSet<HeadAccount> HeadAccounts { get; set; }
        public DbSet<MenuMaster> MenuMaster { get; set; }
        public DbSet<UserLogStatus> UserLogStatus { get; set; }
        public DbSet<EventStatus> EventStatus { get; set; }
        public DbSet<EventType> EventType { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<EventImages> EventImages { get; set; }
        public DbSet<EventReview> EventReviews { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<UILayout> UILayout { get; set; }
        public virtual DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<Favourites> Favorites { get; set; }
        public DbSet<Organizer> Organizers { get; set; }
        public DbSet<EventCatagory> EventCatagories { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<EventExternalLink> EventExternalLinks { get; set; }
        public DbSet<PasswordProtection> PasswordProtection { get; set; }
        public DbSet<Sponsor> Sponsors { get; set; }
        public DbSet<AppLogs> AppLogs { get; set; }
        public DbSet<OrganizerCollections> OrganizerCollections { get; set; }
        public DbSet<CustomerOrganizerFollow> CustomerOrganizerFollow { get; set; }
        public DbSet<TicketDetails> TicketDetails { get; set; }
        public DbSet<TicketBooking> TicketBookings { get; set; }
        public DbSet<CustomerContact> CustomerContacts { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<ShopProduct> ShopProduct { get; set; }
        public DbSet<ShopProductImages> ShopProductImages { get; set; }
        public DbSet<ShopCheckOut> ShopCheckOut { get; set; }
        public DbSet<CheckOutProducts> CheckOutProducts { get; set; }
        public DbSet<Attributes> Attributes { get; set; }
        public DbSet<Variations> Variations { get; set; }
        public DbSet<Policies> Policies { get; set; }
        public DbSet<FAQs> FAQs { get; set; }
        public DbSet<PropotorContacts> PropotorContact { get; set; }
        public DbSet<DiscountCode> DiscountCodes { get; set; }
        public DbSet<Links> Links { get; set; }
        public DbSet<AddToCart> AddToCart { get; set; }
        public DbSet<ContactUsLogs> ContactUsLogs { get; set; }
        public DbSet<OTPTable> OTPTable { get; set; }
        public DbSet<OTPTableTemp> OTPTableTemp { get; set; }
        public DbSet<ScheduledReports> ScheduledReports { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<NotificationTemplate> NotificationTemplate { get; set; }
        public DbSet<NotificationUserGroup> NotificationUserGroup { get; set; }
        public DbSet<EventPromotor> EventPromotors { get; set; }
        public DbSet<EventVenue> EventVenues { get; set; }
        public DbSet<EventUsers> EventUsers { get; set; }
        public DbSet<Reports> Reports { get; set; }
        public DbSet<SeasonTicket> SeasonTickets { get; set; }
        public DbSet<Pages> Pages { get; set; }
        public DbSet<HomePageHeader> HomePageHeaders { get; set; }
        public DbSet<HomePageContent> HomePageContent { get; set; }

        public DbSet<EventTickets> EventTickets { get; set; }
        public DbSet<TicketPasswordProtection> TicketPasswordProtection { get; set; }
        public DbSet<BlockStands> BlockStands { get; set; }
        public DbSet<RowsInBlockStands> RowsInBlockStands { get; set; }
        public DbSet<SeatsInRowBlockStands> SeatsInRowBlockStands { get; set; }
        public DbSet<Exclusions> Exclusions { get; set; }
        public DbSet<StandardSeatTicket> StandardSeatTicket { get; set; }
        public DbSet<VariableSeatTicket> VariableSeatTicket { get; set; }
        public DbSet<OnBoarding> OnBoarding { get; set; }
        public DbSet<CustomerBillingDetails> CustomerBillingDetails { get; set; }

        //========================== SUPER ADMIN ==========================\\

        public DbSet<SuperAdminUser> SuperAdminUsers { get; set; }
        public DbSet<Promotor> Promotors { get; set; }
        public DbSet<PromotorCompanyDetail> PromotorCompanyDetails { get; set; }
        public DbSet<PromotorCheckOut> PromotorCheckOut { get; set; }
        public DbSet<PromotorEvents> PromotorEvents { get; set; }
        public DbSet<PromotorConfirmationEmail> PromotorConfirmationEmail { get; set; }
        public DbSet<PromotorFormatting> PromotorFormatting { get; set; }
        public DbSet<PromotorCommissionStructure> PromotorCommissionStructure { get; set; }
        public DbSet<PromotorSubscription> PromotorSubscription { get; set; }
        public DbSet<PromotorSupportSubscription> PromotorSupportSubscription { get; set; }
        public DbSet<Roles> SARoles { get; set; }
        public DbSet<PromotorPayment> PromotorPayments { get; set; }
        public DbSet<UserNotifications> UserNotifications { get; set; }
        public DbSet<Locations> Locations { get; set; }
    }
}