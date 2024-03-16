namespace WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb124 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LocationName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserNotifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotorId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        NotificationHeader = c.String(),
                        NotificationText = c.String(),
                        NotificationType = c.String(),
                        isRead = c.Boolean(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateStoredProcedure(
                "dbo.Locations_Insert",
                p => new
                    {
                        LocationName = p.String(),
                        isActive = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[Locations]([LocationName], [isActive])
                      VALUES (@LocationName, @isActive)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Locations]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Locations] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Locations_Update",
                p => new
                    {
                        Id = p.Int(),
                        LocationName = p.String(),
                        isActive = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[Locations]
                      SET [LocationName] = @LocationName, [isActive] = @isActive
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Locations_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Locations]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.UserNotifications_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        UserId = p.Int(),
                        NotificationHeader = p.String(),
                        NotificationText = p.String(),
                        NotificationType = p.String(),
                        isRead = p.Boolean(),
                        isActive = p.Boolean(),
                        CreationTime = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[UserNotifications]([PromotorId], [UserId], [NotificationHeader], [NotificationText], [NotificationType], [isRead], [isActive], [CreationTime])
                      VALUES (@PromotorId, @UserId, @NotificationHeader, @NotificationText, @NotificationType, @isRead, @isActive, @CreationTime)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[UserNotifications]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[UserNotifications] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.UserNotifications_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        UserId = p.Int(),
                        NotificationHeader = p.String(),
                        NotificationText = p.String(),
                        NotificationType = p.String(),
                        isRead = p.Boolean(),
                        isActive = p.Boolean(),
                        CreationTime = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[UserNotifications]
                      SET [PromotorId] = @PromotorId, [UserId] = @UserId, [NotificationHeader] = @NotificationHeader, [NotificationText] = @NotificationText, [NotificationType] = @NotificationType, [isRead] = @isRead, [isActive] = @isActive, [CreationTime] = @CreationTime
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.UserNotifications_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[UserNotifications]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.UserNotifications_Delete");
            DropStoredProcedure("dbo.UserNotifications_Update");
            DropStoredProcedure("dbo.UserNotifications_Insert");
            DropStoredProcedure("dbo.Locations_Delete");
            DropStoredProcedure("dbo.Locations_Update");
            DropStoredProcedure("dbo.Locations_Insert");
            DropTable("dbo.UserNotifications");
            DropTable("dbo.Locations");
        }
    }
}
