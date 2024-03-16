namespace WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb105 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShopCheckOuts", "isHide", c => c.Boolean(nullable: false));
            AddColumn("dbo.ShopCheckOuts", "isActive", c => c.Boolean(nullable: false));
            AlterStoredProcedure(
                "dbo.ShopCheckOut_Insert",
                p => new
                    {
                        UserId = p.Int(),
                        CustomerName = p.String(),
                        CustomerEmail = p.String(),
                        State = p.String(),
                        City = p.String(),
                        PostalCode = p.String(),
                        MobileNumber = p.String(),
                        PurchaseDate = p.DateTime(),
                        OrderNo = p.Long(),
                        PromotorId = p.Int(),
                        isHide = p.Boolean(),
                        isActive = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[ShopCheckOuts]([UserId], [CustomerName], [CustomerEmail], [State], [City], [PostalCode], [MobileNumber], [PurchaseDate], [OrderNo], [PromotorId], [isHide], [isActive])
                      VALUES (@UserId, @CustomerName, @CustomerEmail, @State, @City, @PostalCode, @MobileNumber, @PurchaseDate, @OrderNo, @PromotorId, @isHide, @isActive)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ShopCheckOuts]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ShopCheckOuts] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.ShopCheckOut_Update",
                p => new
                    {
                        Id = p.Int(),
                        UserId = p.Int(),
                        CustomerName = p.String(),
                        CustomerEmail = p.String(),
                        State = p.String(),
                        City = p.String(),
                        PostalCode = p.String(),
                        MobileNumber = p.String(),
                        PurchaseDate = p.DateTime(),
                        OrderNo = p.Long(),
                        PromotorId = p.Int(),
                        isHide = p.Boolean(),
                        isActive = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[ShopCheckOuts]
                      SET [UserId] = @UserId, [CustomerName] = @CustomerName, [CustomerEmail] = @CustomerEmail, [State] = @State, [City] = @City, [PostalCode] = @PostalCode, [MobileNumber] = @MobileNumber, [PurchaseDate] = @PurchaseDate, [OrderNo] = @OrderNo, [PromotorId] = @PromotorId, [isHide] = @isHide, [isActive] = @isActive
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShopCheckOuts", "isActive");
            DropColumn("dbo.ShopCheckOuts", "isHide");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
