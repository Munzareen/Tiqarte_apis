namespace WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb106 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShopCheckOuts", "BillingCountry", c => c.String());
            AddColumn("dbo.ShopCheckOuts", "AddressLine1", c => c.String());
            AddColumn("dbo.ShopCheckOuts", "AddressLine2", c => c.String());
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
                        BillingCountry = p.String(),
                        AddressLine1 = p.String(),
                        AddressLine2 = p.String(),
                    },
                body:
                    @"INSERT [dbo].[ShopCheckOuts]([UserId], [CustomerName], [CustomerEmail], [State], [City], [PostalCode], [MobileNumber], [PurchaseDate], [OrderNo], [PromotorId], [isHide], [isActive], [BillingCountry], [AddressLine1], [AddressLine2])
                      VALUES (@UserId, @CustomerName, @CustomerEmail, @State, @City, @PostalCode, @MobileNumber, @PurchaseDate, @OrderNo, @PromotorId, @isHide, @isActive, @BillingCountry, @AddressLine1, @AddressLine2)
                      
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
                        BillingCountry = p.String(),
                        AddressLine1 = p.String(),
                        AddressLine2 = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[ShopCheckOuts]
                      SET [UserId] = @UserId, [CustomerName] = @CustomerName, [CustomerEmail] = @CustomerEmail, [State] = @State, [City] = @City, [PostalCode] = @PostalCode, [MobileNumber] = @MobileNumber, [PurchaseDate] = @PurchaseDate, [OrderNo] = @OrderNo, [PromotorId] = @PromotorId, [isHide] = @isHide, [isActive] = @isActive, [BillingCountry] = @BillingCountry, [AddressLine1] = @AddressLine1, [AddressLine2] = @AddressLine2
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShopCheckOuts", "AddressLine2");
            DropColumn("dbo.ShopCheckOuts", "AddressLine1");
            DropColumn("dbo.ShopCheckOuts", "BillingCountry");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
