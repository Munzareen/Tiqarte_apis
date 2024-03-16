namespace WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb120 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketDetails", "InitialTicketID", c => c.Int(nullable: false));
            AlterStoredProcedure(
                "dbo.TicketDetails_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        EventId = p.Int(),
                        TicketType = p.String(),
                        TicketPrice = p.Decimal(precision: 18, scale: 2),
                        BookingFee = p.Decimal(precision: 18, scale: 2),
                        AvailableTickets = p.Int(),
                        SeasonTicketId = p.Int(),
                        AttendeeAge = p.Int(),
                        HideFromFrontend = p.Boolean(),
                        ExcludeFromOverallCapacity = p.Boolean(),
                        MaximumTickets = p.Int(),
                        MinimumTickets = p.Int(),
                        UnitCost = p.Decimal(precision: 18, scale: 2),
                        RequiredTicketHolderDetails = p.Boolean(),
                        TicketDescription = p.String(),
                        DocumentURL = p.String(),
                        AcknowledgementURL = p.String(),
                        MetaDataURL = p.String(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                        InitialTicketID = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[TicketDetails]([PromotorId], [EventId], [TicketType], [TicketPrice], [BookingFee], [AvailableTickets], [SeasonTicketId], [AttendeeAge], [HideFromFrontend], [ExcludeFromOverallCapacity], [MaximumTickets], [MinimumTickets], [UnitCost], [RequiredTicketHolderDetails], [TicketDescription], [DocumentURL], [AcknowledgementURL], [MetaDataURL], [isActive], [CreatedDate], [InitialTicketID])
                      VALUES (@PromotorId, @EventId, @TicketType, @TicketPrice, @BookingFee, @AvailableTickets, @SeasonTicketId, @AttendeeAge, @HideFromFrontend, @ExcludeFromOverallCapacity, @MaximumTickets, @MinimumTickets, @UnitCost, @RequiredTicketHolderDetails, @TicketDescription, @DocumentURL, @AcknowledgementURL, @MetaDataURL, @isActive, @CreatedDate, @InitialTicketID)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[TicketDetails]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[TicketDetails] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.TicketDetails_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        EventId = p.Int(),
                        TicketType = p.String(),
                        TicketPrice = p.Decimal(precision: 18, scale: 2),
                        BookingFee = p.Decimal(precision: 18, scale: 2),
                        AvailableTickets = p.Int(),
                        SeasonTicketId = p.Int(),
                        AttendeeAge = p.Int(),
                        HideFromFrontend = p.Boolean(),
                        ExcludeFromOverallCapacity = p.Boolean(),
                        MaximumTickets = p.Int(),
                        MinimumTickets = p.Int(),
                        UnitCost = p.Decimal(precision: 18, scale: 2),
                        RequiredTicketHolderDetails = p.Boolean(),
                        TicketDescription = p.String(),
                        DocumentURL = p.String(),
                        AcknowledgementURL = p.String(),
                        MetaDataURL = p.String(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                        InitialTicketID = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[TicketDetails]
                      SET [PromotorId] = @PromotorId, [EventId] = @EventId, [TicketType] = @TicketType, [TicketPrice] = @TicketPrice, [BookingFee] = @BookingFee, [AvailableTickets] = @AvailableTickets, [SeasonTicketId] = @SeasonTicketId, [AttendeeAge] = @AttendeeAge, [HideFromFrontend] = @HideFromFrontend, [ExcludeFromOverallCapacity] = @ExcludeFromOverallCapacity, [MaximumTickets] = @MaximumTickets, [MinimumTickets] = @MinimumTickets, [UnitCost] = @UnitCost, [RequiredTicketHolderDetails] = @RequiredTicketHolderDetails, [TicketDescription] = @TicketDescription, [DocumentURL] = @DocumentURL, [AcknowledgementURL] = @AcknowledgementURL, [MetaDataURL] = @MetaDataURL, [isActive] = @isActive, [CreatedDate] = @CreatedDate, [InitialTicketID] = @InitialTicketID
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.TicketDetails", "InitialTicketID");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
