namespace WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserTables101 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "CustomSlang", c => c.String());
            AddColumn("dbo.Events", "CreationTime", c => c.DateTime(nullable: false));
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
                    },
                body:
                    @"INSERT [dbo].[Events]([Name], [CustomSlang], [Discription], [EventTypeId], [CatagoryId], [Location], [City], [EventStatusId], [Price], [OrganizerID], [EventDate], [LastUpdated], [CreationTime], [CreationUserId], [isFav], [isReviewed], [StandingTitle], [SeatingTitle], [TicketSoldOutText], [CompnayName], [IsPublished])
                      VALUES (@Name, @CustomSlang, @Discription, @EventTypeId, @CatagoryId, @Location, @City, @EventStatusId, @Price, @OrganizerID, @EventDate, @LastUpdated, @CreationTime, @CreationUserId, @isFav, @isReviewed, @StandingTitle, @SeatingTitle, @TicketSoldOutText, @CompnayName, @IsPublished)
                      
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
                    },
                body:
                    @"UPDATE [dbo].[Events]
                      SET [Name] = @Name, [CustomSlang] = @CustomSlang, [Discription] = @Discription, [EventTypeId] = @EventTypeId, [CatagoryId] = @CatagoryId, [Location] = @Location, [City] = @City, [EventStatusId] = @EventStatusId, [Price] = @Price, [OrganizerID] = @OrganizerID, [EventDate] = @EventDate, [LastUpdated] = @LastUpdated, [CreationTime] = @CreationTime, [CreationUserId] = @CreationUserId, [isFav] = @isFav, [isReviewed] = @isReviewed, [StandingTitle] = @StandingTitle, [SeatingTitle] = @SeatingTitle, [TicketSoldOutText] = @TicketSoldOutText, [CompnayName] = @CompnayName, [IsPublished] = @IsPublished
                      WHERE ([EventId] = @EventId)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "CreationTime");
            DropColumn("dbo.Events", "CustomSlang");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
