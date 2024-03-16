namespace WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb125 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserNotifications", "iconURL", c => c.String());
            AlterStoredProcedure(
                "dbo.UserNotifications_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        UserId = p.Int(),
                        NotificationHeader = p.String(),
                        NotificationText = p.String(),
                        NotificationType = p.String(),
                        iconURL = p.String(),
                        isRead = p.Boolean(),
                        isActive = p.Boolean(),
                        CreationTime = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[UserNotifications]([PromotorId], [UserId], [NotificationHeader], [NotificationText], [NotificationType], [iconURL], [isRead], [isActive], [CreationTime])
                      VALUES (@PromotorId, @UserId, @NotificationHeader, @NotificationText, @NotificationType, @iconURL, @isRead, @isActive, @CreationTime)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[UserNotifications]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[UserNotifications] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.UserNotifications_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        UserId = p.Int(),
                        NotificationHeader = p.String(),
                        NotificationText = p.String(),
                        NotificationType = p.String(),
                        iconURL = p.String(),
                        isRead = p.Boolean(),
                        isActive = p.Boolean(),
                        CreationTime = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[UserNotifications]
                      SET [PromotorId] = @PromotorId, [UserId] = @UserId, [NotificationHeader] = @NotificationHeader, [NotificationText] = @NotificationText, [NotificationType] = @NotificationType, [iconURL] = @iconURL, [isRead] = @isRead, [isActive] = @isActive, [CreationTime] = @CreationTime
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserNotifications", "iconURL");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
