namespace WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb122 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PromotorCheckOuts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotorId = c.Int(nullable: false),
                        ReservationDurationMinutes = c.Int(nullable: false),
                        DisableGuestCheckout = c.Boolean(nullable: false),
                        AskMarketingOption = c.Boolean(nullable: false),
                        MarketingOptionLabel = c.String(),
                        CopyForTopCheckout = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PromotorCompanyDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotorId = c.Int(nullable: false),
                        MunicipalityId = c.Int(nullable: false),
                        CityId = c.Int(nullable: false),
                        CompanyEmailAddress = c.String(),
                        BillingAddress = c.String(),
                        AddressLine1 = c.String(),
                        AddressLine2 = c.String(),
                        AddressLine3 = c.String(),
                        Telephone = c.String(),
                        TaxIdentificationNumber = c.String(),
                        PostalCode = c.String(),
                        CopyRights = c.String(),
                        LegalDisclaimer = c.String(),
                        FacebookLine = c.String(),
                        InstaLink = c.String(),
                        LinkedinLink = c.String(),
                        TwitterLink = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PromotorConfirmationEmails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotorId = c.Int(nullable: false),
                        To = c.String(),
                        BCC = c.String(),
                        CC = c.String(),
                        Write = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PromotorEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotorId = c.Int(nullable: false),
                        EventSlug = c.String(),
                        EventTypeId = c.Int(nullable: false),
                        ShowRemainingTicketCount = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PromotorFormattings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotorId = c.Int(nullable: false),
                        DateFormatId = c.Int(nullable: false),
                        TimeFormat = c.Int(nullable: false),
                        APIDomain = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Promotors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        NickName = c.String(),
                        EmailAddress = c.String(),
                        RoleId = c.Int(nullable: false),
                        StatusId = c.Int(nullable: false),
                        Password = c.String(),
                        SerialNumber = c.Int(nullable: false),
                        SuspendedFrom = c.DateTime(),
                        SuspendedTo = c.DateTime(),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SuperAdminUsers",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        Gender = c.String(),
                        ImageUrl = c.String(),
                        State = c.String(),
                        City = c.String(),
                        ZipCode = c.String(),
                        CountryCode = c.String(),
                        PhoneNumber = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateStoredProcedure(
                "dbo.PromotorCheckOut_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        ReservationDurationMinutes = p.Int(),
                        DisableGuestCheckout = p.Boolean(),
                        AskMarketingOption = p.Boolean(),
                        MarketingOptionLabel = p.String(),
                        CopyForTopCheckout = p.String(),
                        CreatedAt = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[PromotorCheckOuts]([PromotorId], [ReservationDurationMinutes], [DisableGuestCheckout], [AskMarketingOption], [MarketingOptionLabel], [CopyForTopCheckout], [CreatedAt])
                      VALUES (@PromotorId, @ReservationDurationMinutes, @DisableGuestCheckout, @AskMarketingOption, @MarketingOptionLabel, @CopyForTopCheckout, @CreatedAt)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[PromotorCheckOuts]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[PromotorCheckOuts] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.PromotorCheckOut_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        ReservationDurationMinutes = p.Int(),
                        DisableGuestCheckout = p.Boolean(),
                        AskMarketingOption = p.Boolean(),
                        MarketingOptionLabel = p.String(),
                        CopyForTopCheckout = p.String(),
                        CreatedAt = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[PromotorCheckOuts]
                      SET [PromotorId] = @PromotorId, [ReservationDurationMinutes] = @ReservationDurationMinutes, [DisableGuestCheckout] = @DisableGuestCheckout, [AskMarketingOption] = @AskMarketingOption, [MarketingOptionLabel] = @MarketingOptionLabel, [CopyForTopCheckout] = @CopyForTopCheckout, [CreatedAt] = @CreatedAt
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.PromotorCheckOut_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[PromotorCheckOuts]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.PromotorCompanyDetail_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        MunicipalityId = p.Int(),
                        CityId = p.Int(),
                        CompanyEmailAddress = p.String(),
                        BillingAddress = p.String(),
                        AddressLine1 = p.String(),
                        AddressLine2 = p.String(),
                        AddressLine3 = p.String(),
                        Telephone = p.String(),
                        TaxIdentificationNumber = p.String(),
                        PostalCode = p.String(),
                        CopyRights = p.String(),
                        LegalDisclaimer = p.String(),
                        FacebookLine = p.String(),
                        InstaLink = p.String(),
                        LinkedinLink = p.String(),
                        TwitterLink = p.String(),
                        CreatedAt = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[PromotorCompanyDetails]([PromotorId], [MunicipalityId], [CityId], [CompanyEmailAddress], [BillingAddress], [AddressLine1], [AddressLine2], [AddressLine3], [Telephone], [TaxIdentificationNumber], [PostalCode], [CopyRights], [LegalDisclaimer], [FacebookLine], [InstaLink], [LinkedinLink], [TwitterLink], [CreatedAt])
                      VALUES (@PromotorId, @MunicipalityId, @CityId, @CompanyEmailAddress, @BillingAddress, @AddressLine1, @AddressLine2, @AddressLine3, @Telephone, @TaxIdentificationNumber, @PostalCode, @CopyRights, @LegalDisclaimer, @FacebookLine, @InstaLink, @LinkedinLink, @TwitterLink, @CreatedAt)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[PromotorCompanyDetails]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[PromotorCompanyDetails] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.PromotorCompanyDetail_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        MunicipalityId = p.Int(),
                        CityId = p.Int(),
                        CompanyEmailAddress = p.String(),
                        BillingAddress = p.String(),
                        AddressLine1 = p.String(),
                        AddressLine2 = p.String(),
                        AddressLine3 = p.String(),
                        Telephone = p.String(),
                        TaxIdentificationNumber = p.String(),
                        PostalCode = p.String(),
                        CopyRights = p.String(),
                        LegalDisclaimer = p.String(),
                        FacebookLine = p.String(),
                        InstaLink = p.String(),
                        LinkedinLink = p.String(),
                        TwitterLink = p.String(),
                        CreatedAt = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[PromotorCompanyDetails]
                      SET [PromotorId] = @PromotorId, [MunicipalityId] = @MunicipalityId, [CityId] = @CityId, [CompanyEmailAddress] = @CompanyEmailAddress, [BillingAddress] = @BillingAddress, [AddressLine1] = @AddressLine1, [AddressLine2] = @AddressLine2, [AddressLine3] = @AddressLine3, [Telephone] = @Telephone, [TaxIdentificationNumber] = @TaxIdentificationNumber, [PostalCode] = @PostalCode, [CopyRights] = @CopyRights, [LegalDisclaimer] = @LegalDisclaimer, [FacebookLine] = @FacebookLine, [InstaLink] = @InstaLink, [LinkedinLink] = @LinkedinLink, [TwitterLink] = @TwitterLink, [CreatedAt] = @CreatedAt
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.PromotorCompanyDetail_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[PromotorCompanyDetails]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.PromotorConfirmationEmail_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        To = p.String(),
                        BCC = p.String(),
                        CC = p.String(),
                        Write = p.String(),
                        CreatedAt = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[PromotorConfirmationEmails]([PromotorId], [To], [BCC], [CC], [Write], [CreatedAt])
                      VALUES (@PromotorId, @To, @BCC, @CC, @Write, @CreatedAt)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[PromotorConfirmationEmails]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[PromotorConfirmationEmails] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.PromotorConfirmationEmail_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        To = p.String(),
                        BCC = p.String(),
                        CC = p.String(),
                        Write = p.String(),
                        CreatedAt = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[PromotorConfirmationEmails]
                      SET [PromotorId] = @PromotorId, [To] = @To, [BCC] = @BCC, [CC] = @CC, [Write] = @Write, [CreatedAt] = @CreatedAt
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.PromotorConfirmationEmail_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[PromotorConfirmationEmails]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.PromotorEvents_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        EventSlug = p.String(),
                        EventTypeId = p.Int(),
                        ShowRemainingTicketCount = p.Boolean(),
                        CreatedAt = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[PromotorEvents]([PromotorId], [EventSlug], [EventTypeId], [ShowRemainingTicketCount], [CreatedAt])
                      VALUES (@PromotorId, @EventSlug, @EventTypeId, @ShowRemainingTicketCount, @CreatedAt)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[PromotorEvents]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[PromotorEvents] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.PromotorEvents_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        EventSlug = p.String(),
                        EventTypeId = p.Int(),
                        ShowRemainingTicketCount = p.Boolean(),
                        CreatedAt = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[PromotorEvents]
                      SET [PromotorId] = @PromotorId, [EventSlug] = @EventSlug, [EventTypeId] = @EventTypeId, [ShowRemainingTicketCount] = @ShowRemainingTicketCount, [CreatedAt] = @CreatedAt
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.PromotorEvents_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[PromotorEvents]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.PromotorFormatting_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        DateFormatId = p.Int(),
                        TimeFormat = p.Int(),
                        APIDomain = p.Int(),
                        CreatedAt = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[PromotorFormattings]([PromotorId], [DateFormatId], [TimeFormat], [APIDomain], [CreatedAt])
                      VALUES (@PromotorId, @DateFormatId, @TimeFormat, @APIDomain, @CreatedAt)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[PromotorFormattings]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[PromotorFormattings] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.PromotorFormatting_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        DateFormatId = p.Int(),
                        TimeFormat = p.Int(),
                        APIDomain = p.Int(),
                        CreatedAt = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[PromotorFormattings]
                      SET [PromotorId] = @PromotorId, [DateFormatId] = @DateFormatId, [TimeFormat] = @TimeFormat, [APIDomain] = @APIDomain, [CreatedAt] = @CreatedAt
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.PromotorFormatting_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[PromotorFormattings]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Promotor_Insert",
                p => new
                    {
                        FullName = p.String(),
                        NickName = p.String(),
                        EmailAddress = p.String(),
                        RoleId = p.Int(),
                        StatusId = p.Int(),
                        Password = p.String(),
                        SerialNumber = p.Int(),
                        SuspendedFrom = p.DateTime(),
                        SuspendedTo = p.DateTime(),
                        CreatedAt = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[Promotors]([FullName], [NickName], [EmailAddress], [RoleId], [StatusId], [Password], [SerialNumber], [SuspendedFrom], [SuspendedTo], [CreatedAt])
                      VALUES (@FullName, @NickName, @EmailAddress, @RoleId, @StatusId, @Password, @SerialNumber, @SuspendedFrom, @SuspendedTo, @CreatedAt)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Promotors]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Promotors] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Promotor_Update",
                p => new
                    {
                        Id = p.Int(),
                        FullName = p.String(),
                        NickName = p.String(),
                        EmailAddress = p.String(),
                        RoleId = p.Int(),
                        StatusId = p.Int(),
                        Password = p.String(),
                        SerialNumber = p.Int(),
                        SuspendedFrom = p.DateTime(),
                        SuspendedTo = p.DateTime(),
                        CreatedAt = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Promotors]
                      SET [FullName] = @FullName, [NickName] = @NickName, [EmailAddress] = @EmailAddress, [RoleId] = @RoleId, [StatusId] = @StatusId, [Password] = @Password, [SerialNumber] = @SerialNumber, [SuspendedFrom] = @SuspendedFrom, [SuspendedTo] = @SuspendedTo, [CreatedAt] = @CreatedAt
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Promotor_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Promotors]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.SuperAdminUser_Insert",
                p => new
                    {
                        FirstName = p.String(),
                        LastName = p.String(),
                        Email = p.String(),
                        Password = p.String(),
                        Gender = p.String(),
                        ImageUrl = p.String(),
                        State = p.String(),
                        City = p.String(),
                        ZipCode = p.String(),
                        CountryCode = p.String(),
                        PhoneNumber = p.String(),
                        CreationDate = p.DateTime(),
                        LastUpdate = p.DateTime(),
                        UpdatedBy = p.Int(),
                        isActive = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[SuperAdminUsers]([FirstName], [LastName], [Email], [Password], [Gender], [ImageUrl], [State], [City], [ZipCode], [CountryCode], [PhoneNumber], [CreationDate], [LastUpdate], [UpdatedBy], [isActive])
                      VALUES (@FirstName, @LastName, @Email, @Password, @Gender, @ImageUrl, @State, @City, @ZipCode, @CountryCode, @PhoneNumber, @CreationDate, @LastUpdate, @UpdatedBy, @isActive)
                      
                      DECLARE @UserId int
                      SELECT @UserId = [UserId]
                      FROM [dbo].[SuperAdminUsers]
                      WHERE @@ROWCOUNT > 0 AND [UserId] = scope_identity()
                      
                      SELECT t0.[UserId]
                      FROM [dbo].[SuperAdminUsers] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[UserId] = @UserId"
            );
            
            CreateStoredProcedure(
                "dbo.SuperAdminUser_Update",
                p => new
                    {
                        UserId = p.Int(),
                        FirstName = p.String(),
                        LastName = p.String(),
                        Email = p.String(),
                        Password = p.String(),
                        Gender = p.String(),
                        ImageUrl = p.String(),
                        State = p.String(),
                        City = p.String(),
                        ZipCode = p.String(),
                        CountryCode = p.String(),
                        PhoneNumber = p.String(),
                        CreationDate = p.DateTime(),
                        LastUpdate = p.DateTime(),
                        UpdatedBy = p.Int(),
                        isActive = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[SuperAdminUsers]
                      SET [FirstName] = @FirstName, [LastName] = @LastName, [Email] = @Email, [Password] = @Password, [Gender] = @Gender, [ImageUrl] = @ImageUrl, [State] = @State, [City] = @City, [ZipCode] = @ZipCode, [CountryCode] = @CountryCode, [PhoneNumber] = @PhoneNumber, [CreationDate] = @CreationDate, [LastUpdate] = @LastUpdate, [UpdatedBy] = @UpdatedBy, [isActive] = @isActive
                      WHERE ([UserId] = @UserId)"
            );
            
            CreateStoredProcedure(
                "dbo.SuperAdminUser_Delete",
                p => new
                    {
                        UserId = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[SuperAdminUsers]
                      WHERE ([UserId] = @UserId)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.SuperAdminUser_Delete");
            DropStoredProcedure("dbo.SuperAdminUser_Update");
            DropStoredProcedure("dbo.SuperAdminUser_Insert");
            DropStoredProcedure("dbo.Promotor_Delete");
            DropStoredProcedure("dbo.Promotor_Update");
            DropStoredProcedure("dbo.Promotor_Insert");
            DropStoredProcedure("dbo.PromotorFormatting_Delete");
            DropStoredProcedure("dbo.PromotorFormatting_Update");
            DropStoredProcedure("dbo.PromotorFormatting_Insert");
            DropStoredProcedure("dbo.PromotorEvents_Delete");
            DropStoredProcedure("dbo.PromotorEvents_Update");
            DropStoredProcedure("dbo.PromotorEvents_Insert");
            DropStoredProcedure("dbo.PromotorConfirmationEmail_Delete");
            DropStoredProcedure("dbo.PromotorConfirmationEmail_Update");
            DropStoredProcedure("dbo.PromotorConfirmationEmail_Insert");
            DropStoredProcedure("dbo.PromotorCompanyDetail_Delete");
            DropStoredProcedure("dbo.PromotorCompanyDetail_Update");
            DropStoredProcedure("dbo.PromotorCompanyDetail_Insert");
            DropStoredProcedure("dbo.PromotorCheckOut_Delete");
            DropStoredProcedure("dbo.PromotorCheckOut_Update");
            DropStoredProcedure("dbo.PromotorCheckOut_Insert");
            DropTable("dbo.SuperAdminUsers");
            DropTable("dbo.Promotors");
            DropTable("dbo.PromotorFormattings");
            DropTable("dbo.PromotorEvents");
            DropTable("dbo.PromotorConfirmationEmails");
            DropTable("dbo.PromotorCompanyDetails");
            DropTable("dbo.PromotorCheckOuts");
        }
    }
}
