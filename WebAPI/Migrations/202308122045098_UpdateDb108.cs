namespace WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb108 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "isActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.ShopProducts", "EventId", c => c.Int(nullable: false));
            AddColumn("dbo.User", "PromotorId", c => c.Int(nullable: false));
            AddColumn("dbo.User", "Notes", c => c.String());
            AddColumn("dbo.User", "CreationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.User", "LastUpdate", c => c.DateTime());
            AddColumn("dbo.User", "UpdatedBy", c => c.Int());
            AlterStoredProcedure(
                "dbo.Event_Insert",
                p => new
                    {
                        Name = p.String(),
                        CustomSlang = p.String(),
                        Discription = p.String(),
                        EventTypeId = p.Decimal(precision: 18, scale: 2),
                        CatagoryId = p.Int(),
                        Location = p.String(),
                        City = p.String(),
                        EventStatusId = p.Decimal(precision: 18, scale: 2),
                        Price = p.Double(),
                        OrganizerID = p.Int(),
                        EventDate = p.DateTime(),
                        LastUpdated = p.DateTime(),
                        CreationTime = p.DateTime(),
                        CreationUserId = p.Decimal(precision: 18, scale: 2),
                        isFav = p.Boolean(),
                        isReviewed = p.Boolean(),
                        StandingTitle = p.String(),
                        SeatingTitle = p.String(),
                        TicketSoldOutText = p.String(),
                        CompnayName = p.String(),
                        IsPublished = p.Boolean(),
                        isActive = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[Events]([Name], [CustomSlang], [Discription], [EventTypeId], [CatagoryId], [Location], [City], [EventStatusId], [Price], [OrganizerID], [EventDate], [LastUpdated], [CreationTime], [CreationUserId], [isFav], [isReviewed], [StandingTitle], [SeatingTitle], [TicketSoldOutText], [CompnayName], [IsPublished], [isActive])
                      VALUES (@Name, @CustomSlang, @Discription, @EventTypeId, @CatagoryId, @Location, @City, @EventStatusId, @Price, @OrganizerID, @EventDate, @LastUpdated, @CreationTime, @CreationUserId, @isFav, @isReviewed, @StandingTitle, @SeatingTitle, @TicketSoldOutText, @CompnayName, @IsPublished, @isActive)
                      
                      DECLARE @EventId int
                      SELECT @EventId = [EventId]
                      FROM [dbo].[Events]
                      WHERE @@ROWCOUNT > 0 AND [EventId] = scope_identity()
                      
                      SELECT t0.[EventId]
                      FROM [dbo].[Events] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[EventId] = @EventId"
            );
            
            AlterStoredProcedure(
                "dbo.Event_Update",
                p => new
                    {
                        EventId = p.Int(),
                        Name = p.String(),
                        CustomSlang = p.String(),
                        Discription = p.String(),
                        EventTypeId = p.Decimal(precision: 18, scale: 2),
                        CatagoryId = p.Int(),
                        Location = p.String(),
                        City = p.String(),
                        EventStatusId = p.Decimal(precision: 18, scale: 2),
                        Price = p.Double(),
                        OrganizerID = p.Int(),
                        EventDate = p.DateTime(),
                        LastUpdated = p.DateTime(),
                        CreationTime = p.DateTime(),
                        CreationUserId = p.Decimal(precision: 18, scale: 2),
                        isFav = p.Boolean(),
                        isReviewed = p.Boolean(),
                        StandingTitle = p.String(),
                        SeatingTitle = p.String(),
                        TicketSoldOutText = p.String(),
                        CompnayName = p.String(),
                        IsPublished = p.Boolean(),
                        isActive = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[Events]
                      SET [Name] = @Name, [CustomSlang] = @CustomSlang, [Discription] = @Discription, [EventTypeId] = @EventTypeId, [CatagoryId] = @CatagoryId, [Location] = @Location, [City] = @City, [EventStatusId] = @EventStatusId, [Price] = @Price, [OrganizerID] = @OrganizerID, [EventDate] = @EventDate, [LastUpdated] = @LastUpdated, [CreationTime] = @CreationTime, [CreationUserId] = @CreationUserId, [isFav] = @isFav, [isReviewed] = @isReviewed, [StandingTitle] = @StandingTitle, [SeatingTitle] = @SeatingTitle, [TicketSoldOutText] = @TicketSoldOutText, [CompnayName] = @CompnayName, [IsPublished] = @IsPublished, [isActive] = @isActive
                      WHERE ([EventId] = @EventId)"
            );
            
            AlterStoredProcedure(
                "dbo.ShopProduct_Insert",
                p => new
                    {
                        Sku = p.String(),
                        ProductName = p.String(),
                        Description = p.String(),
                        DeliveryDetails = p.String(),
                        Price = p.Decimal(precision: 18, scale: 2),
                        CatagoryId = p.Int(),
                        ProductFor = p.String(),
                        isActive = p.Boolean(),
                        PromotorId = p.Int(),
                        CreatedDate = p.DateTime(),
                        EventId = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[ShopProducts]([Sku], [ProductName], [Description], [DeliveryDetails], [Price], [CatagoryId], [ProductFor], [isActive], [PromotorId], [CreatedDate], [EventId])
                      VALUES (@Sku, @ProductName, @Description, @DeliveryDetails, @Price, @CatagoryId, @ProductFor, @isActive, @PromotorId, @CreatedDate, @EventId)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ShopProducts]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ShopProducts] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.ShopProduct_Update",
                p => new
                    {
                        Id = p.Int(),
                        Sku = p.String(),
                        ProductName = p.String(),
                        Description = p.String(),
                        DeliveryDetails = p.String(),
                        Price = p.Decimal(precision: 18, scale: 2),
                        CatagoryId = p.Int(),
                        ProductFor = p.String(),
                        isActive = p.Boolean(),
                        PromotorId = p.Int(),
                        CreatedDate = p.DateTime(),
                        EventId = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[ShopProducts]
                      SET [Sku] = @Sku, [ProductName] = @ProductName, [Description] = @Description, [DeliveryDetails] = @DeliveryDetails, [Price] = @Price, [CatagoryId] = @CatagoryId, [ProductFor] = @ProductFor, [isActive] = @isActive, [PromotorId] = @PromotorId, [CreatedDate] = @CreatedDate, [EventId] = @EventId
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.ApplicationUser_Insert",
                p => new
                    {
                        Id = p.String(maxLength: 128),
                        UserId = p.Int(),
                        FirstName = p.String(),
                        LastName = p.String(),
                        Password = p.String(),
                        Role = p.String(),
                        Gender = p.String(),
                        NickName = p.String(),
                        isProfileCompleted = p.Boolean(),
                        isVerified = p.Boolean(),
                        DOB = p.String(),
                        FullName = p.String(),
                        ImageUrl = p.String(),
                        UserTypeId = p.Int(),
                        Location = p.String(),
                        State = p.String(),
                        City = p.String(),
                        ZipCode = p.String(),
                        CountryCode = p.String(),
                        PhoneNumber = p.String(),
                        PromotorId = p.Int(),
                        Notes = p.String(),
                        CreationDate = p.DateTime(),
                        LastUpdate = p.DateTime(),
                        UpdatedBy = p.Int(),
                        Email = p.String(maxLength: 256),
                        EmailConfirmed = p.Boolean(),
                        PasswordHash = p.String(),
                        SecurityStamp = p.String(),
                        PhoneNumberConfirmed = p.Boolean(),
                        TwoFactorEnabled = p.Boolean(),
                        LockoutEndDateUtc = p.DateTime(),
                        LockoutEnabled = p.Boolean(),
                        AccessFailedCount = p.Int(),
                        UserName = p.String(maxLength: 256),
                    },
                body:
                    @"INSERT [dbo].[User]([Id], [UserId], [FirstName], [LastName], [Password], [Role], [Gender], [NickName], [isProfileCompleted], [isVerified], [DOB], [FullName], [ImageUrl], [UserTypeId], [Location], [State], [City], [ZipCode], [CountryCode], [PhoneNumber], [PromotorId], [Notes], [CreationDate], [LastUpdate], [UpdatedBy], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName])
                      VALUES (@Id, @UserId, @FirstName, @LastName, @Password, @Role, @Gender, @NickName, @isProfileCompleted, @isVerified, @DOB, @FullName, @ImageUrl, @UserTypeId, @Location, @State, @City, @ZipCode, @CountryCode, @PhoneNumber, @PromotorId, @Notes, @CreationDate, @LastUpdate, @UpdatedBy, @Email, @EmailConfirmed, @PasswordHash, @SecurityStamp, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEndDateUtc, @LockoutEnabled, @AccessFailedCount, @UserName)"
            );
            
            AlterStoredProcedure(
                "dbo.ApplicationUser_Update",
                p => new
                    {
                        Id = p.String(maxLength: 128),
                        UserId = p.Int(),
                        FirstName = p.String(),
                        LastName = p.String(),
                        Password = p.String(),
                        Role = p.String(),
                        Gender = p.String(),
                        NickName = p.String(),
                        isProfileCompleted = p.Boolean(),
                        isVerified = p.Boolean(),
                        DOB = p.String(),
                        FullName = p.String(),
                        ImageUrl = p.String(),
                        UserTypeId = p.Int(),
                        Location = p.String(),
                        State = p.String(),
                        City = p.String(),
                        ZipCode = p.String(),
                        CountryCode = p.String(),
                        PhoneNumber = p.String(),
                        PromotorId = p.Int(),
                        Notes = p.String(),
                        CreationDate = p.DateTime(),
                        LastUpdate = p.DateTime(),
                        UpdatedBy = p.Int(),
                        Email = p.String(maxLength: 256),
                        EmailConfirmed = p.Boolean(),
                        PasswordHash = p.String(),
                        SecurityStamp = p.String(),
                        PhoneNumberConfirmed = p.Boolean(),
                        TwoFactorEnabled = p.Boolean(),
                        LockoutEndDateUtc = p.DateTime(),
                        LockoutEnabled = p.Boolean(),
                        AccessFailedCount = p.Int(),
                        UserName = p.String(maxLength: 256),
                    },
                body:
                    @"UPDATE [dbo].[User]
                      SET [UserId] = @UserId, [FirstName] = @FirstName, [LastName] = @LastName, [Password] = @Password, [Role] = @Role, [Gender] = @Gender, [NickName] = @NickName, [isProfileCompleted] = @isProfileCompleted, [isVerified] = @isVerified, [DOB] = @DOB, [FullName] = @FullName, [ImageUrl] = @ImageUrl, [UserTypeId] = @UserTypeId, [Location] = @Location, [State] = @State, [City] = @City, [ZipCode] = @ZipCode, [CountryCode] = @CountryCode, [PhoneNumber] = @PhoneNumber, [PromotorId] = @PromotorId, [Notes] = @Notes, [CreationDate] = @CreationDate, [LastUpdate] = @LastUpdate, [UpdatedBy] = @UpdatedBy, [Email] = @Email, [EmailConfirmed] = @EmailConfirmed, [PasswordHash] = @PasswordHash, [SecurityStamp] = @SecurityStamp, [PhoneNumberConfirmed] = @PhoneNumberConfirmed, [TwoFactorEnabled] = @TwoFactorEnabled, [LockoutEndDateUtc] = @LockoutEndDateUtc, [LockoutEnabled] = @LockoutEnabled, [AccessFailedCount] = @AccessFailedCount, [UserName] = @UserName
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "UpdatedBy");
            DropColumn("dbo.User", "LastUpdate");
            DropColumn("dbo.User", "CreationDate");
            DropColumn("dbo.User", "Notes");
            DropColumn("dbo.User", "PromotorId");
            DropColumn("dbo.ShopProducts", "EventId");
            DropColumn("dbo.Events", "isActive");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
