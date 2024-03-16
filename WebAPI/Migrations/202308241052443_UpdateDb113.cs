namespace WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb113 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketBookings", "PromotorId", c => c.Int(nullable: false));
            AddColumn("dbo.TicketBookings", "CreatedDate", c => c.DateTime(nullable: false));
            AlterStoredProcedure(
                "dbo.TicketBooking_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        EventId = p.Int(),
                        TicketId = p.Int(),
                        UserId = p.Int(),
                        TicketCount = p.Int(),
                        TicketUniqueNumber = p.Int(),
                        BarCodeURL = p.String(),
                        QRCodeURL = p.String(),
                        isCancelled = p.Boolean(),
                        CancelReason = p.String(),
                        CancelDate = p.DateTime(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[TicketBookings]([PromotorId], [EventId], [TicketId], [UserId], [TicketCount], [TicketUniqueNumber], [BarCodeURL], [QRCodeURL], [isCancelled], [CancelReason], [CancelDate], [CreatedDate])
                      VALUES (@PromotorId, @EventId, @TicketId, @UserId, @TicketCount, @TicketUniqueNumber, @BarCodeURL, @QRCodeURL, @isCancelled, @CancelReason, @CancelDate, @CreatedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[TicketBookings]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[TicketBookings] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.TicketBooking_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        EventId = p.Int(),
                        TicketId = p.Int(),
                        UserId = p.Int(),
                        TicketCount = p.Int(),
                        TicketUniqueNumber = p.Int(),
                        BarCodeURL = p.String(),
                        QRCodeURL = p.String(),
                        isCancelled = p.Boolean(),
                        CancelReason = p.String(),
                        CancelDate = p.DateTime(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[TicketBookings]
                      SET [PromotorId] = @PromotorId, [EventId] = @EventId, [TicketId] = @TicketId, [UserId] = @UserId, [TicketCount] = @TicketCount, [TicketUniqueNumber] = @TicketUniqueNumber, [BarCodeURL] = @BarCodeURL, [QRCodeURL] = @QRCodeURL, [isCancelled] = @isCancelled, [CancelReason] = @CancelReason, [CancelDate] = @CancelDate, [CreatedDate] = @CreatedDate
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.TicketBookings", "CreatedDate");
            DropColumn("dbo.TicketBookings", "PromotorId");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
