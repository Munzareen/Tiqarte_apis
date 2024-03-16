namespace WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb103 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventPromotors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotoId = c.Int(nullable: false),
                        Name = c.String(),
                        Email = c.String(),
                        Telephone = c.String(),
                        ImageUrl = c.String(),
                        PaymentGateway = c.Int(nullable: false),
                        SecretKey = c.String(),
                        APIKey = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EventVenues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotorId = c.Int(nullable: false),
                        Name = c.String(),
                        Location = c.String(),
                        Address = c.String(),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        GoogleMapEmbedCode = c.String(),
                        BlockAlias = c.String(),
                        BlocksAlias = c.String(),
                        RowAlias = c.String(),
                        RowsAlias = c.String(),
                        SeatAlias = c.String(),
                        SeatsAlias = c.String(),
                        TableAlias = c.String(),
                        TablesAlias = c.String(),
                        BasicStandardPlan = c.String(),
                        Notes = c.String(),
                        isActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateStoredProcedure(
                "dbo.EventPromotor_Insert",
                p => new
                    {
                        PromotoId = p.Int(),
                        Name = p.String(),
                        Email = p.String(),
                        Telephone = p.String(),
                        ImageUrl = p.String(),
                        PaymentGateway = p.Int(),
                        SecretKey = p.String(),
                        APIKey = p.String(),
                        CreatedDate = p.DateTime(),
                        isActive = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[EventPromotors]([PromotoId], [Name], [Email], [Telephone], [ImageUrl], [PaymentGateway], [SecretKey], [APIKey], [CreatedDate], [isActive])
                      VALUES (@PromotoId, @Name, @Email, @Telephone, @ImageUrl, @PaymentGateway, @SecretKey, @APIKey, @CreatedDate, @isActive)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[EventPromotors]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[EventPromotors] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.EventPromotor_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotoId = p.Int(),
                        Name = p.String(),
                        Email = p.String(),
                        Telephone = p.String(),
                        ImageUrl = p.String(),
                        PaymentGateway = p.Int(),
                        SecretKey = p.String(),
                        APIKey = p.String(),
                        CreatedDate = p.DateTime(),
                        isActive = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[EventPromotors]
                      SET [PromotoId] = @PromotoId, [Name] = @Name, [Email] = @Email, [Telephone] = @Telephone, [ImageUrl] = @ImageUrl, [PaymentGateway] = @PaymentGateway, [SecretKey] = @SecretKey, [APIKey] = @APIKey, [CreatedDate] = @CreatedDate, [isActive] = @isActive
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.EventPromotor_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[EventPromotors]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
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
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[EventVenues]([PromotorId], [Name], [Location], [Address], [Latitude], [Longitude], [GoogleMapEmbedCode], [BlockAlias], [BlocksAlias], [RowAlias], [RowsAlias], [SeatAlias], [SeatsAlias], [TableAlias], [TablesAlias], [BasicStandardPlan], [Notes], [isActive], [CreatedDate])
                      VALUES (@PromotorId, @Name, @Location, @Address, @Latitude, @Longitude, @GoogleMapEmbedCode, @BlockAlias, @BlocksAlias, @RowAlias, @RowsAlias, @SeatAlias, @SeatsAlias, @TableAlias, @TablesAlias, @BasicStandardPlan, @Notes, @isActive, @CreatedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[EventVenues]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[EventVenues] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
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
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[EventVenues]
                      SET [PromotorId] = @PromotorId, [Name] = @Name, [Location] = @Location, [Address] = @Address, [Latitude] = @Latitude, [Longitude] = @Longitude, [GoogleMapEmbedCode] = @GoogleMapEmbedCode, [BlockAlias] = @BlockAlias, [BlocksAlias] = @BlocksAlias, [RowAlias] = @RowAlias, [RowsAlias] = @RowsAlias, [SeatAlias] = @SeatAlias, [SeatsAlias] = @SeatsAlias, [TableAlias] = @TableAlias, [TablesAlias] = @TablesAlias, [BasicStandardPlan] = @BasicStandardPlan, [Notes] = @Notes, [isActive] = @isActive, [CreatedDate] = @CreatedDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.EventVenue_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[EventVenues]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.EventVenue_Delete");
            DropStoredProcedure("dbo.EventVenue_Update");
            DropStoredProcedure("dbo.EventVenue_Insert");
            DropStoredProcedure("dbo.EventPromotor_Delete");
            DropStoredProcedure("dbo.EventPromotor_Update");
            DropStoredProcedure("dbo.EventPromotor_Insert");
            DropTable("dbo.EventVenues");
            DropTable("dbo.EventPromotors");
        }
    }
}
