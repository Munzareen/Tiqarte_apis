namespace WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb118 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerBillingDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        BillingName = c.String(),
                        BillingCountry = c.String(),
                        BillingAddressLine1 = c.String(),
                        BillingAddressLine2 = c.String(),
                        BillingTown = c.String(),
                        BillingPostalCode = c.String(),
                        BillingEmail = c.String(),
                        BillingPhone = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateStoredProcedure(
                "dbo.CustomerBillingDetails_Insert",
                p => new
                    {
                        UserId = p.Int(),
                        BillingName = p.String(),
                        BillingCountry = p.String(),
                        BillingAddressLine1 = p.String(),
                        BillingAddressLine2 = p.String(),
                        BillingTown = p.String(),
                        BillingPostalCode = p.String(),
                        BillingEmail = p.String(),
                        BillingPhone = p.String(),
                    },
                body:
                    @"INSERT [dbo].[CustomerBillingDetails]([UserId], [BillingName], [BillingCountry], [BillingAddressLine1], [BillingAddressLine2], [BillingTown], [BillingPostalCode], [BillingEmail], [BillingPhone])
                      VALUES (@UserId, @BillingName, @BillingCountry, @BillingAddressLine1, @BillingAddressLine2, @BillingTown, @BillingPostalCode, @BillingEmail, @BillingPhone)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[CustomerBillingDetails]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[CustomerBillingDetails] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.CustomerBillingDetails_Update",
                p => new
                    {
                        Id = p.Int(),
                        UserId = p.Int(),
                        BillingName = p.String(),
                        BillingCountry = p.String(),
                        BillingAddressLine1 = p.String(),
                        BillingAddressLine2 = p.String(),
                        BillingTown = p.String(),
                        BillingPostalCode = p.String(),
                        BillingEmail = p.String(),
                        BillingPhone = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[CustomerBillingDetails]
                      SET [UserId] = @UserId, [BillingName] = @BillingName, [BillingCountry] = @BillingCountry, [BillingAddressLine1] = @BillingAddressLine1, [BillingAddressLine2] = @BillingAddressLine2, [BillingTown] = @BillingTown, [BillingPostalCode] = @BillingPostalCode, [BillingEmail] = @BillingEmail, [BillingPhone] = @BillingPhone
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.CustomerBillingDetails_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[CustomerBillingDetails]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.CustomerBillingDetails_Delete");
            DropStoredProcedure("dbo.CustomerBillingDetails_Update");
            DropStoredProcedure("dbo.CustomerBillingDetails_Insert");
            DropTable("dbo.CustomerBillingDetails");
        }
    }
}
