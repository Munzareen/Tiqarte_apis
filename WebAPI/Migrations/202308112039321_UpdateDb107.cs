namespace WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb107 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotorId = c.Int(nullable: false),
                        Name = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        RoleId = c.Int(nullable: false),
                        EventId = c.Int(nullable: false),
                        isPOSUser = c.Boolean(nullable: false),
                        isReportOrders = c.Boolean(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateStoredProcedure(
                "dbo.EventUsers_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        Name = p.String(),
                        Email = p.String(),
                        Password = p.String(),
                        RoleId = p.Int(),
                        EventId = p.Int(),
                        isPOSUser = p.Boolean(),
                        isReportOrders = p.Boolean(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[EventUsers]([PromotorId], [Name], [Email], [Password], [RoleId], [EventId], [isPOSUser], [isReportOrders], [isActive], [CreatedDate])
                      VALUES (@PromotorId, @Name, @Email, @Password, @RoleId, @EventId, @isPOSUser, @isReportOrders, @isActive, @CreatedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[EventUsers]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[EventUsers] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.EventUsers_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        Name = p.String(),
                        Email = p.String(),
                        Password = p.String(),
                        RoleId = p.Int(),
                        EventId = p.Int(),
                        isPOSUser = p.Boolean(),
                        isReportOrders = p.Boolean(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[EventUsers]
                      SET [PromotorId] = @PromotorId, [Name] = @Name, [Email] = @Email, [Password] = @Password, [RoleId] = @RoleId, [EventId] = @EventId, [isPOSUser] = @isPOSUser, [isReportOrders] = @isReportOrders, [isActive] = @isActive, [CreatedDate] = @CreatedDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.EventUsers_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[EventUsers]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.EventUsers_Delete");
            DropStoredProcedure("dbo.EventUsers_Update");
            DropStoredProcedure("dbo.EventUsers_Insert");
            DropTable("dbo.EventUsers");
        }
    }
}
