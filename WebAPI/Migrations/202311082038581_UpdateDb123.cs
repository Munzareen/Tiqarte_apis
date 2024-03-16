namespace WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb123 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PromotorPayments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotorId = c.Int(nullable: false),
                        PromotorName = c.String(),
                        Currency = c.String(),
                        Amount = c.Decimal(precision: 18, scale: 2),
                        Status = c.String(),
                        Date = c.DateTime(nullable: false),
                        InvoiceNumber = c.String(),
                        Description = c.String(),
                        isActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotorId = c.Int(nullable: false),
                        RoleName = c.String(),
                        PlanName = c.String(),
                        GiveAccess = c.String(),
                        Create = c.Boolean(nullable: false),
                        Update = c.Boolean(nullable: false),
                        Read = c.Boolean(nullable: false),
                        Delete = c.Boolean(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.EventVenues", "TotalBlocks", c => c.Int(nullable: false));
            AddColumn("dbo.EventVenues", "TotalAvailableSeats", c => c.Int(nullable: false));
            AddColumn("dbo.EventVenues", "TotalCapacity", c => c.Int(nullable: false));
            AddColumn("dbo.EventVenues", "ImageURL", c => c.String());
            AddColumn("dbo.EventVenues", "Status", c => c.String());
            AddColumn("dbo.Pages", "Description", c => c.String());
            AddColumn("dbo.UILayouts", "Title", c => c.String());
            AddColumn("dbo.UILayouts", "StickOnNumber", c => c.String());
            AddColumn("dbo.UILayouts", "Description", c => c.String());
            AddColumn("dbo.UILayouts", "isActive", c => c.Boolean(nullable: false));
            CreateStoredProcedure(
                "dbo.PromotorPayment_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        PromotorName = p.String(),
                        Currency = p.String(),
                        Amount = p.Decimal(precision: 18, scale: 2),
                        Status = p.String(),
                        Date = p.DateTime(),
                        InvoiceNumber = p.String(),
                        Description = p.String(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[PromotorPayments]([PromotorId], [PromotorName], [Currency], [Amount], [Status], [Date], [InvoiceNumber], [Description], [isActive], [CreatedDate])
                      VALUES (@PromotorId, @PromotorName, @Currency, @Amount, @Status, @Date, @InvoiceNumber, @Description, @isActive, @CreatedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[PromotorPayments]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[PromotorPayments] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.PromotorPayment_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        PromotorName = p.String(),
                        Currency = p.String(),
                        Amount = p.Decimal(precision: 18, scale: 2),
                        Status = p.String(),
                        Date = p.DateTime(),
                        InvoiceNumber = p.String(),
                        Description = p.String(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[PromotorPayments]
                      SET [PromotorId] = @PromotorId, [PromotorName] = @PromotorName, [Currency] = @Currency, [Amount] = @Amount, [Status] = @Status, [Date] = @Date, [InvoiceNumber] = @InvoiceNumber, [Description] = @Description, [isActive] = @isActive, [CreatedDate] = @CreatedDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.PromotorPayment_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[PromotorPayments]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Roles_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        RoleName = p.String(),
                        PlanName = p.String(),
                        GiveAccess = p.String(),
                        Create = p.Boolean(),
                        Update = p.Boolean(),
                        Read = p.Boolean(),
                        Delete = p.Boolean(),
                        isActive = p.Boolean(),
                        CreateDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[Roles]([PromotorId], [RoleName], [PlanName], [GiveAccess], [Create], [Update], [Read], [Delete], [isActive], [CreateDate])
                      VALUES (@PromotorId, @RoleName, @PlanName, @GiveAccess, @Create, @Update, @Read, @Delete, @isActive, @CreateDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Roles]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Roles] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Roles_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        RoleName = p.String(),
                        PlanName = p.String(),
                        GiveAccess = p.String(),
                        Create = p.Boolean(),
                        Update = p.Boolean(),
                        Read = p.Boolean(),
                        Delete = p.Boolean(),
                        isActive = p.Boolean(),
                        CreateDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Roles]
                      SET [PromotorId] = @PromotorId, [RoleName] = @RoleName, [PlanName] = @PlanName, [GiveAccess] = @GiveAccess, [Create] = @Create, [Update] = @Update, [Read] = @Read, [Delete] = @Delete, [isActive] = @isActive, [CreateDate] = @CreateDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Roles_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Roles]
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.EventVenue_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        Name = p.String(),
                        Location = p.String(),
                        Address = p.String(),
                        Latitude = p.Double(),
                        Longitude = p.Double(),
                        GoogleMapEmbedCode = p.String(),
                        BlockAlias = p.String(),
                        BlocksAlias = p.String(),
                        RowAlias = p.String(),
                        RowsAlias = p.String(),
                        SeatAlias = p.String(),
                        SeatsAlias = p.String(),
                        TableAlias = p.String(),
                        TablesAlias = p.String(),
                        BasicStandardPlan = p.String(),
                        Notes = p.String(),
                        TotalBlocks = p.Int(),
                        TotalAvailableSeats = p.Int(),
                        TotalCapacity = p.Int(),
                        ImageURL = p.String(),
                        Status = p.String(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[EventVenues]([PromotorId], [Name], [Location], [Address], [Latitude], [Longitude], [GoogleMapEmbedCode], [BlockAlias], [BlocksAlias], [RowAlias], [RowsAlias], [SeatAlias], [SeatsAlias], [TableAlias], [TablesAlias], [BasicStandardPlan], [Notes], [TotalBlocks], [TotalAvailableSeats], [TotalCapacity], [ImageURL], [Status], [isActive], [CreatedDate])
                      VALUES (@PromotorId, @Name, @Location, @Address, @Latitude, @Longitude, @GoogleMapEmbedCode, @BlockAlias, @BlocksAlias, @RowAlias, @RowsAlias, @SeatAlias, @SeatsAlias, @TableAlias, @TablesAlias, @BasicStandardPlan, @Notes, @TotalBlocks, @TotalAvailableSeats, @TotalCapacity, @ImageURL, @Status, @isActive, @CreatedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[EventVenues]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[EventVenues] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.EventVenue_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        Name = p.String(),
                        Location = p.String(),
                        Address = p.String(),
                        Latitude = p.Double(),
                        Longitude = p.Double(),
                        GoogleMapEmbedCode = p.String(),
                        BlockAlias = p.String(),
                        BlocksAlias = p.String(),
                        RowAlias = p.String(),
                        RowsAlias = p.String(),
                        SeatAlias = p.String(),
                        SeatsAlias = p.String(),
                        TableAlias = p.String(),
                        TablesAlias = p.String(),
                        BasicStandardPlan = p.String(),
                        Notes = p.String(),
                        TotalBlocks = p.Int(),
                        TotalAvailableSeats = p.Int(),
                        TotalCapacity = p.Int(),
                        ImageURL = p.String(),
                        Status = p.String(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[EventVenues]
                      SET [PromotorId] = @PromotorId, [Name] = @Name, [Location] = @Location, [Address] = @Address, [Latitude] = @Latitude, [Longitude] = @Longitude, [GoogleMapEmbedCode] = @GoogleMapEmbedCode, [BlockAlias] = @BlockAlias, [BlocksAlias] = @BlocksAlias, [RowAlias] = @RowAlias, [RowsAlias] = @RowsAlias, [SeatAlias] = @SeatAlias, [SeatsAlias] = @SeatsAlias, [TableAlias] = @TableAlias, [TablesAlias] = @TablesAlias, [BasicStandardPlan] = @BasicStandardPlan, [Notes] = @Notes, [TotalBlocks] = @TotalBlocks, [TotalAvailableSeats] = @TotalAvailableSeats, [TotalCapacity] = @TotalCapacity, [ImageURL] = @ImageURL, [Status] = @Status, [isActive] = @isActive, [CreatedDate] = @CreatedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.Pages_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        PageName = p.String(),
                        Title = p.String(),
                        Description = p.String(),
                        ImageURL = p.String(),
                        isActive = p.Boolean(),
                        CreationTime = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[Pages]([PromotorId], [PageName], [Title], [Description], [ImageURL], [isActive], [CreationTime])
                      VALUES (@PromotorId, @PageName, @Title, @Description, @ImageURL, @isActive, @CreationTime)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Pages]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Pages] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Pages_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        PageName = p.String(),
                        Title = p.String(),
                        Description = p.String(),
                        ImageURL = p.String(),
                        isActive = p.Boolean(),
                        CreationTime = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Pages]
                      SET [PromotorId] = @PromotorId, [PageName] = @PageName, [Title] = @Title, [Description] = @Description, [ImageURL] = @ImageURL, [isActive] = @isActive, [CreationTime] = @CreationTime
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.UILayout_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        PrimaryColor = p.String(),
                        SecondaryColor = p.String(),
                        AdditionalColors = p.String(),
                        LogoURL = p.String(),
                        DarkLogoURL = p.String(),
                        FaviconURL = p.String(),
                        LogoLink = p.String(),
                        TicketBgForOrderURL = p.String(),
                        FrontImageForSessionTicketURL = p.String(),
                        CheckBgURL = p.String(),
                        OrderCompletionImageURL = p.String(),
                        Title = p.String(),
                        StickOnNumber = p.String(),
                        Description = p.String(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[UILayouts]([PromotorId], [PrimaryColor], [SecondaryColor], [AdditionalColors], [LogoURL], [DarkLogoURL], [FaviconURL], [LogoLink], [TicketBgForOrderURL], [FrontImageForSessionTicketURL], [CheckBgURL], [OrderCompletionImageURL], [Title], [StickOnNumber], [Description], [isActive], [CreatedDate])
                      VALUES (@PromotorId, @PrimaryColor, @SecondaryColor, @AdditionalColors, @LogoURL, @DarkLogoURL, @FaviconURL, @LogoLink, @TicketBgForOrderURL, @FrontImageForSessionTicketURL, @CheckBgURL, @OrderCompletionImageURL, @Title, @StickOnNumber, @Description, @isActive, @CreatedDate)
                      
                      DECLARE @LayoutId int
                      SELECT @LayoutId = [LayoutId]
                      FROM [dbo].[UILayouts]
                      WHERE @@ROWCOUNT > 0 AND [LayoutId] = scope_identity()
                      
                      SELECT t0.[LayoutId]
                      FROM [dbo].[UILayouts] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[LayoutId] = @LayoutId"
            );
            

            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Roles_Delete");
            DropStoredProcedure("dbo.Roles_Update");
            DropStoredProcedure("dbo.Roles_Insert");
            DropStoredProcedure("dbo.PromotorPayment_Delete");
            DropStoredProcedure("dbo.PromotorPayment_Update");
            DropStoredProcedure("dbo.PromotorPayment_Insert");
            DropColumn("dbo.UILayouts", "isActive");
            DropColumn("dbo.UILayouts", "Description");
            DropColumn("dbo.UILayouts", "StickOnNumber");
            DropColumn("dbo.UILayouts", "Title");
            DropColumn("dbo.Pages", "Description");
            DropColumn("dbo.EventVenues", "Status");
            DropColumn("dbo.EventVenues", "ImageURL");
            DropColumn("dbo.EventVenues", "TotalCapacity");
            DropColumn("dbo.EventVenues", "TotalAvailableSeats");
            DropColumn("dbo.EventVenues", "TotalBlocks");
            DropTable("dbo.Roles");
            DropTable("dbo.PromotorPayments");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
