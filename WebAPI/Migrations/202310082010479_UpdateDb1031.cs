namespace WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb1031 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PromotorCommissionStructures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotorId = c.Int(nullable: false),
                        ContractDuration = c.Int(nullable: false),
                        CurrencyId = c.Int(nullable: false),
                        CommissionPercentage = c.Double(nullable: false),
                        OnEachEntry = c.Boolean(nullable: false),
                        OnEachProduct = c.Boolean(nullable: false),
                        FixAmount = c.Double(nullable: false),
                        TransactionAmount = c.Double(nullable: false),
                        CommissionCap = c.Double(nullable: false),
                        TaxTypeId = c.Int(nullable: false),
                        TaxPercentage = c.Double(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PromotorSubscriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotorId = c.Int(nullable: false),
                        SubscriptionChargesId = c.Int(nullable: false),
                        NumberofMonths = c.Int(nullable: false),
                        Amount = c.Double(nullable: false),
                        CurrencyId = c.Int(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PromotorSupportSubscriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotorId = c.Int(nullable: false),
                        SubscriptionChargesId = c.Int(nullable: false),
                        NumberofMonths = c.Int(nullable: false),
                        Amount = c.Double(nullable: false),
                        CurrencyId = c.Int(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.PromotorCheckOuts", "isActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.PromotorCompanyDetails", "isActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.PromotorConfirmationEmails", "isActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.PromotorEvents", "isActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.PromotorFormattings", "isActive", c => c.Boolean(nullable: false));
            CreateStoredProcedure(
                "dbo.PromotorCommissionStructure_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        ContractDuration = p.Int(),
                        CurrencyId = p.Int(),
                        CommissionPercentage = p.Double(),
                        OnEachEntry = p.Boolean(),
                        OnEachProduct = p.Boolean(),
                        FixAmount = p.Double(),
                        TransactionAmount = p.Double(),
                        CommissionCap = p.Double(),
                        TaxTypeId = p.Int(),
                        TaxPercentage = p.Double(),
                        isActive = p.Boolean(),
                        CreatedAt = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[PromotorCommissionStructures]([PromotorId], [ContractDuration], [CurrencyId], [CommissionPercentage], [OnEachEntry], [OnEachProduct], [FixAmount], [TransactionAmount], [CommissionCap], [TaxTypeId], [TaxPercentage], [isActive], [CreatedAt])
                      VALUES (@PromotorId, @ContractDuration, @CurrencyId, @CommissionPercentage, @OnEachEntry, @OnEachProduct, @FixAmount, @TransactionAmount, @CommissionCap, @TaxTypeId, @TaxPercentage, @isActive, @CreatedAt)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[PromotorCommissionStructures]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[PromotorCommissionStructures] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.PromotorCommissionStructure_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        ContractDuration = p.Int(),
                        CurrencyId = p.Int(),
                        CommissionPercentage = p.Double(),
                        OnEachEntry = p.Boolean(),
                        OnEachProduct = p.Boolean(),
                        FixAmount = p.Double(),
                        TransactionAmount = p.Double(),
                        CommissionCap = p.Double(),
                        TaxTypeId = p.Int(),
                        TaxPercentage = p.Double(),
                        isActive = p.Boolean(),
                        CreatedAt = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[PromotorCommissionStructures]
                      SET [PromotorId] = @PromotorId, [ContractDuration] = @ContractDuration, [CurrencyId] = @CurrencyId, [CommissionPercentage] = @CommissionPercentage, [OnEachEntry] = @OnEachEntry, [OnEachProduct] = @OnEachProduct, [FixAmount] = @FixAmount, [TransactionAmount] = @TransactionAmount, [CommissionCap] = @CommissionCap, [TaxTypeId] = @TaxTypeId, [TaxPercentage] = @TaxPercentage, [isActive] = @isActive, [CreatedAt] = @CreatedAt
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.PromotorCommissionStructure_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[PromotorCommissionStructures]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.PromotorSubscription_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        SubscriptionChargesId = p.Int(),
                        NumberofMonths = p.Int(),
                        Amount = p.Double(),
                        CurrencyId = p.Int(),
                        isActive = p.Boolean(),
                        CreatedAt = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[PromotorSubscriptions]([PromotorId], [SubscriptionChargesId], [NumberofMonths], [Amount], [CurrencyId], [isActive], [CreatedAt])
                      VALUES (@PromotorId, @SubscriptionChargesId, @NumberofMonths, @Amount, @CurrencyId, @isActive, @CreatedAt)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[PromotorSubscriptions]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[PromotorSubscriptions] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.PromotorSubscription_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        SubscriptionChargesId = p.Int(),
                        NumberofMonths = p.Int(),
                        Amount = p.Double(),
                        CurrencyId = p.Int(),
                        isActive = p.Boolean(),
                        CreatedAt = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[PromotorSubscriptions]
                      SET [PromotorId] = @PromotorId, [SubscriptionChargesId] = @SubscriptionChargesId, [NumberofMonths] = @NumberofMonths, [Amount] = @Amount, [CurrencyId] = @CurrencyId, [isActive] = @isActive, [CreatedAt] = @CreatedAt
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.PromotorSubscription_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[PromotorSubscriptions]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.PromotorSupportSubscription_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        SubscriptionChargesId = p.Int(),
                        NumberofMonths = p.Int(),
                        Amount = p.Double(),
                        CurrencyId = p.Int(),
                        isActive = p.Boolean(),
                        CreatedAt = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[PromotorSupportSubscriptions]([PromotorId], [SubscriptionChargesId], [NumberofMonths], [Amount], [CurrencyId], [isActive], [CreatedAt])
                      VALUES (@PromotorId, @SubscriptionChargesId, @NumberofMonths, @Amount, @CurrencyId, @isActive, @CreatedAt)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[PromotorSupportSubscriptions]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[PromotorSupportSubscriptions] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.PromotorSupportSubscription_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        SubscriptionChargesId = p.Int(),
                        NumberofMonths = p.Int(),
                        Amount = p.Double(),
                        CurrencyId = p.Int(),
                        isActive = p.Boolean(),
                        CreatedAt = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[PromotorSupportSubscriptions]
                      SET [PromotorId] = @PromotorId, [SubscriptionChargesId] = @SubscriptionChargesId, [NumberofMonths] = @NumberofMonths, [Amount] = @Amount, [CurrencyId] = @CurrencyId, [isActive] = @isActive, [CreatedAt] = @CreatedAt
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.PromotorSupportSubscription_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[PromotorSupportSubscriptions]
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.PromotorCheckOut_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        ReservationDurationMinutes = p.Int(),
                        DisableGuestCheckout = p.Boolean(),
                        AskMarketingOption = p.Boolean(),
                        MarketingOptionLabel = p.String(),
                        CopyForTopCheckout = p.String(),
                        isActive = p.Boolean(),
                        CreatedAt = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[PromotorCheckOuts]([PromotorId], [ReservationDurationMinutes], [DisableGuestCheckout], [AskMarketingOption], [MarketingOptionLabel], [CopyForTopCheckout], [isActive], [CreatedAt])
                      VALUES (@PromotorId, @ReservationDurationMinutes, @DisableGuestCheckout, @AskMarketingOption, @MarketingOptionLabel, @CopyForTopCheckout, @isActive, @CreatedAt)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[PromotorCheckOuts]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[PromotorCheckOuts] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
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
                        isActive = p.Boolean(),
                        CreatedAt = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[PromotorCheckOuts]
                      SET [PromotorId] = @PromotorId, [ReservationDurationMinutes] = @ReservationDurationMinutes, [DisableGuestCheckout] = @DisableGuestCheckout, [AskMarketingOption] = @AskMarketingOption, [MarketingOptionLabel] = @MarketingOptionLabel, [CopyForTopCheckout] = @CopyForTopCheckout, [isActive] = @isActive, [CreatedAt] = @CreatedAt
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
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
                        isActive = p.Boolean(),
                        CreatedAt = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[PromotorCompanyDetails]([PromotorId], [MunicipalityId], [CityId], [CompanyEmailAddress], [BillingAddress], [AddressLine1], [AddressLine2], [AddressLine3], [Telephone], [TaxIdentificationNumber], [PostalCode], [CopyRights], [LegalDisclaimer], [FacebookLine], [InstaLink], [LinkedinLink], [TwitterLink], [isActive], [CreatedAt])
                      VALUES (@PromotorId, @MunicipalityId, @CityId, @CompanyEmailAddress, @BillingAddress, @AddressLine1, @AddressLine2, @AddressLine3, @Telephone, @TaxIdentificationNumber, @PostalCode, @CopyRights, @LegalDisclaimer, @FacebookLine, @InstaLink, @LinkedinLink, @TwitterLink, @isActive, @CreatedAt)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[PromotorCompanyDetails]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[PromotorCompanyDetails] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
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
                        isActive = p.Boolean(),
                        CreatedAt = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[PromotorCompanyDetails]
                      SET [PromotorId] = @PromotorId, [MunicipalityId] = @MunicipalityId, [CityId] = @CityId, [CompanyEmailAddress] = @CompanyEmailAddress, [BillingAddress] = @BillingAddress, [AddressLine1] = @AddressLine1, [AddressLine2] = @AddressLine2, [AddressLine3] = @AddressLine3, [Telephone] = @Telephone, [TaxIdentificationNumber] = @TaxIdentificationNumber, [PostalCode] = @PostalCode, [CopyRights] = @CopyRights, [LegalDisclaimer] = @LegalDisclaimer, [FacebookLine] = @FacebookLine, [InstaLink] = @InstaLink, [LinkedinLink] = @LinkedinLink, [TwitterLink] = @TwitterLink, [isActive] = @isActive, [CreatedAt] = @CreatedAt
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.PromotorConfirmationEmail_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        To = p.String(),
                        BCC = p.String(),
                        CC = p.String(),
                        Write = p.String(),
                        isActive = p.Boolean(),
                        CreatedAt = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[PromotorConfirmationEmails]([PromotorId], [To], [BCC], [CC], [Write], [isActive], [CreatedAt])
                      VALUES (@PromotorId, @To, @BCC, @CC, @Write, @isActive, @CreatedAt)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[PromotorConfirmationEmails]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[PromotorConfirmationEmails] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.PromotorConfirmationEmail_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        To = p.String(),
                        BCC = p.String(),
                        CC = p.String(),
                        Write = p.String(),
                        isActive = p.Boolean(),
                        CreatedAt = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[PromotorConfirmationEmails]
                      SET [PromotorId] = @PromotorId, [To] = @To, [BCC] = @BCC, [CC] = @CC, [Write] = @Write, [isActive] = @isActive, [CreatedAt] = @CreatedAt
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.PromotorEvents_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        EventSlug = p.String(),
                        EventTypeId = p.Int(),
                        ShowRemainingTicketCount = p.Boolean(),
                        isActive = p.Boolean(),
                        CreatedAt = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[PromotorEvents]([PromotorId], [EventSlug], [EventTypeId], [ShowRemainingTicketCount], [isActive], [CreatedAt])
                      VALUES (@PromotorId, @EventSlug, @EventTypeId, @ShowRemainingTicketCount, @isActive, @CreatedAt)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[PromotorEvents]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[PromotorEvents] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.PromotorEvents_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        EventSlug = p.String(),
                        EventTypeId = p.Int(),
                        ShowRemainingTicketCount = p.Boolean(),
                        isActive = p.Boolean(),
                        CreatedAt = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[PromotorEvents]
                      SET [PromotorId] = @PromotorId, [EventSlug] = @EventSlug, [EventTypeId] = @EventTypeId, [ShowRemainingTicketCount] = @ShowRemainingTicketCount, [isActive] = @isActive, [CreatedAt] = @CreatedAt
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.PromotorFormatting_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        DateFormatId = p.Int(),
                        TimeFormat = p.Int(),
                        APIDomain = p.Int(),
                        isActive = p.Boolean(),
                        CreatedAt = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[PromotorFormattings]([PromotorId], [DateFormatId], [TimeFormat], [APIDomain], [isActive], [CreatedAt])
                      VALUES (@PromotorId, @DateFormatId, @TimeFormat, @APIDomain, @isActive, @CreatedAt)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[PromotorFormattings]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[PromotorFormattings] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.PromotorFormatting_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        DateFormatId = p.Int(),
                        TimeFormat = p.Int(),
                        APIDomain = p.Int(),
                        isActive = p.Boolean(),
                        CreatedAt = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[PromotorFormattings]
                      SET [PromotorId] = @PromotorId, [DateFormatId] = @DateFormatId, [TimeFormat] = @TimeFormat, [APIDomain] = @APIDomain, [isActive] = @isActive, [CreatedAt] = @CreatedAt
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.PromotorSupportSubscription_Delete");
            DropStoredProcedure("dbo.PromotorSupportSubscription_Update");
            DropStoredProcedure("dbo.PromotorSupportSubscription_Insert");
            DropStoredProcedure("dbo.PromotorSubscription_Delete");
            DropStoredProcedure("dbo.PromotorSubscription_Update");
            DropStoredProcedure("dbo.PromotorSubscription_Insert");
            DropStoredProcedure("dbo.PromotorCommissionStructure_Delete");
            DropStoredProcedure("dbo.PromotorCommissionStructure_Update");
            DropStoredProcedure("dbo.PromotorCommissionStructure_Insert");
            DropColumn("dbo.PromotorFormattings", "isActive");
            DropColumn("dbo.PromotorEvents", "isActive");
            DropColumn("dbo.PromotorConfirmationEmails", "isActive");
            DropColumn("dbo.PromotorCompanyDetails", "isActive");
            DropColumn("dbo.PromotorCheckOuts", "isActive");
            DropTable("dbo.PromotorSupportSubscriptions");
            DropTable("dbo.PromotorSubscriptions");
            DropTable("dbo.PromotorCommissionStructures");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
