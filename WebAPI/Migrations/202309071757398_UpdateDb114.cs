namespace WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb114 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StandardSeatTickets", "EventTicketId", c => c.Int(nullable: false));
            AddColumn("dbo.StandardSeatTickets", "BlockStandId", c => c.Int(nullable: false));
            AddColumn("dbo.StandardSeatTickets", "RowsId", c => c.Int(nullable: false));
            AddColumn("dbo.StandardSeatTickets", "SeatId", c => c.Int(nullable: false));
            AlterStoredProcedure(
                "dbo.StandardSeatTicket_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        EventTicketId = p.Int(),
                        StadiumId = p.Int(),
                        BlockStandId = p.Int(),
                        RowsId = p.Int(),
                        SeatId = p.Int(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[StandardSeatTickets]([PromotorId], [EventTicketId], [StadiumId], [BlockStandId], [RowsId], [SeatId], [isActive], [CreatedDate])
                      VALUES (@PromotorId, @EventTicketId, @StadiumId, @BlockStandId, @RowsId, @SeatId, @isActive, @CreatedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[StandardSeatTickets]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[StandardSeatTickets] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.StandardSeatTicket_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        EventTicketId = p.Int(),
                        StadiumId = p.Int(),
                        BlockStandId = p.Int(),
                        RowsId = p.Int(),
                        SeatId = p.Int(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[StandardSeatTickets]
                      SET [PromotorId] = @PromotorId, [EventTicketId] = @EventTicketId, [StadiumId] = @StadiumId, [BlockStandId] = @BlockStandId, [RowsId] = @RowsId, [SeatId] = @SeatId, [isActive] = @isActive, [CreatedDate] = @CreatedDate
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.StandardSeatTickets", "SeatId");
            DropColumn("dbo.StandardSeatTickets", "RowsId");
            DropColumn("dbo.StandardSeatTickets", "BlockStandId");
            DropColumn("dbo.StandardSeatTickets", "EventTicketId");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
