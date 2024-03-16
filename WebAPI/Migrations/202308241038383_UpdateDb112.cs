namespace WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb112 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AddToCarts", "CreatedDate", c => c.DateTime());
            AlterStoredProcedure(
                "dbo.AddToCart_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        UserId = p.Int(),
                        ProductId = p.Int(),
                        Attributes = p.String(),
                        Quantity = p.Int(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[AddToCarts]([PromotorId], [UserId], [ProductId], [Attributes], [Quantity], [isActive], [CreatedDate])
                      VALUES (@PromotorId, @UserId, @ProductId, @Attributes, @Quantity, @isActive, @CreatedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[AddToCarts]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[AddToCarts] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.AddToCart_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        UserId = p.Int(),
                        ProductId = p.Int(),
                        Attributes = p.String(),
                        Quantity = p.Int(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[AddToCarts]
                      SET [PromotorId] = @PromotorId, [UserId] = @UserId, [ProductId] = @ProductId, [Attributes] = @Attributes, [Quantity] = @Quantity, [isActive] = @isActive, [CreatedDate] = @CreatedDate
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.AddToCarts", "CreatedDate");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
