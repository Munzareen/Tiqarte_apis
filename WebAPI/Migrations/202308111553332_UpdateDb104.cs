namespace WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb104 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AddToCarts", "PromotorId", c => c.Int(nullable: false));
            AddColumn("dbo.CheckOutProducts", "PromotorId", c => c.Int(nullable: false));
            AddColumn("dbo.ShopCheckOuts", "PromotorId", c => c.Int(nullable: false));
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
                    },
                body:
                    @"INSERT [dbo].[AddToCarts]([PromotorId], [UserId], [ProductId], [Attributes], [Quantity], [isActive])
                      VALUES (@PromotorId, @UserId, @ProductId, @Attributes, @Quantity, @isActive)
                      
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
                    },
                body:
                    @"UPDATE [dbo].[AddToCarts]
                      SET [PromotorId] = @PromotorId, [UserId] = @UserId, [ProductId] = @ProductId, [Attributes] = @Attributes, [Quantity] = @Quantity, [isActive] = @isActive
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.CheckOutProducts_Insert",
                p => new
                    {
                        CheckOutId = p.Int(),
                        AddToCartId = p.Int(),
                        PromotorId = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[CheckOutProducts]([CheckOutId], [AddToCartId], [PromotorId])
                      VALUES (@CheckOutId, @AddToCartId, @PromotorId)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[CheckOutProducts]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[CheckOutProducts] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.CheckOutProducts_Update",
                p => new
                    {
                        Id = p.Int(),
                        CheckOutId = p.Int(),
                        AddToCartId = p.Int(),
                        PromotorId = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[CheckOutProducts]
                      SET [CheckOutId] = @CheckOutId, [AddToCartId] = @AddToCartId, [PromotorId] = @PromotorId
                      WHERE ([Id] = @Id)"
            );
            
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
                    },
                body:
                    @"INSERT [dbo].[ShopCheckOuts]([UserId], [CustomerName], [CustomerEmail], [State], [City], [PostalCode], [MobileNumber], [PurchaseDate], [OrderNo], [PromotorId])
                      VALUES (@UserId, @CustomerName, @CustomerEmail, @State, @City, @PostalCode, @MobileNumber, @PurchaseDate, @OrderNo, @PromotorId)
                      
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
                    },
                body:
                    @"UPDATE [dbo].[ShopCheckOuts]
                      SET [UserId] = @UserId, [CustomerName] = @CustomerName, [CustomerEmail] = @CustomerEmail, [State] = @State, [City] = @City, [PostalCode] = @PostalCode, [MobileNumber] = @MobileNumber, [PurchaseDate] = @PurchaseDate, [OrderNo] = @OrderNo, [PromotorId] = @PromotorId
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShopCheckOuts", "PromotorId");
            DropColumn("dbo.CheckOutProducts", "PromotorId");
            DropColumn("dbo.AddToCarts", "PromotorId");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
