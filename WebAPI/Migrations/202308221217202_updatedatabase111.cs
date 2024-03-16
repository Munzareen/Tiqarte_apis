namespace WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedatabase111 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HomePageContents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotorId = c.Int(nullable: false),
                        ImageURL = c.String(),
                        Title = c.String(),
                        Content = c.String(),
                        isActive = c.Boolean(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HomePageHeaders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotorId = c.Int(nullable: false),
                        HeaderURL = c.String(),
                        BannerURL = c.String(),
                        BackgroundURL = c.String(),
                        Title = c.String(),
                        Content = c.String(),
                        FromPrice = c.String(),
                        Link = c.String(),
                        HREF = c.String(),
                        isActive = c.Boolean(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotorId = c.Int(nullable: false),
                        PageName = c.String(),
                        Title = c.String(),
                        ImageURL = c.String(),
                        isActive = c.Boolean(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SeasonTickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        EventId = c.Int(nullable: false),
                        PromotorId = c.Int(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Notifications", "NotificationTitle", c => c.String());
            AddColumn("dbo.Notifications", "FrequencyId", c => c.Int(nullable: false));
            AddColumn("dbo.Notifications", "TriggerId", c => c.Int(nullable: false));
            DropColumn("dbo.Notifications", "NotificationName");
            CreateStoredProcedure(
                "dbo.HomePageContent_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        ImageURL = p.String(),
                        Title = p.String(),
                        Content = p.String(),
                        isActive = p.Boolean(),
                        CreationTime = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[HomePageContents]([PromotorId], [ImageURL], [Title], [Content], [isActive], [CreationTime])
                      VALUES (@PromotorId, @ImageURL, @Title, @Content, @isActive, @CreationTime)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[HomePageContents]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[HomePageContents] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.HomePageContent_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        ImageURL = p.String(),
                        Title = p.String(),
                        Content = p.String(),
                        isActive = p.Boolean(),
                        CreationTime = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[HomePageContents]
                      SET [PromotorId] = @PromotorId, [ImageURL] = @ImageURL, [Title] = @Title, [Content] = @Content, [isActive] = @isActive, [CreationTime] = @CreationTime
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.HomePageContent_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[HomePageContents]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.HomePageHeader_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        HeaderURL = p.String(),
                        BannerURL = p.String(),
                        BackgroundURL = p.String(),
                        Title = p.String(),
                        Content = p.String(),
                        FromPrice = p.String(),
                        Link = p.String(),
                        HREF = p.String(),
                        isActive = p.Boolean(),
                        CreationTime = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[HomePageHeaders]([PromotorId], [HeaderURL], [BannerURL], [BackgroundURL], [Title], [Content], [FromPrice], [Link], [HREF], [isActive], [CreationTime])
                      VALUES (@PromotorId, @HeaderURL, @BannerURL, @BackgroundURL, @Title, @Content, @FromPrice, @Link, @HREF, @isActive, @CreationTime)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[HomePageHeaders]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[HomePageHeaders] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.HomePageHeader_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        HeaderURL = p.String(),
                        BannerURL = p.String(),
                        BackgroundURL = p.String(),
                        Title = p.String(),
                        Content = p.String(),
                        FromPrice = p.String(),
                        Link = p.String(),
                        HREF = p.String(),
                        isActive = p.Boolean(),
                        CreationTime = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[HomePageHeaders]
                      SET [PromotorId] = @PromotorId, [HeaderURL] = @HeaderURL, [BannerURL] = @BannerURL, [BackgroundURL] = @BackgroundURL, [Title] = @Title, [Content] = @Content, [FromPrice] = @FromPrice, [Link] = @Link, [HREF] = @HREF, [isActive] = @isActive, [CreationTime] = @CreationTime
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.HomePageHeader_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[HomePageHeaders]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Pages_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        PageName = p.String(),
                        Title = p.String(),
                        ImageURL = p.String(),
                        isActive = p.Boolean(),
                        CreationTime = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[Pages]([PromotorId], [PageName], [Title], [ImageURL], [isActive], [CreationTime])
                      VALUES (@PromotorId, @PageName, @Title, @ImageURL, @isActive, @CreationTime)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Pages]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Pages] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Pages_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        PageName = p.String(),
                        Title = p.String(),
                        ImageURL = p.String(),
                        isActive = p.Boolean(),
                        CreationTime = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Pages]
                      SET [PromotorId] = @PromotorId, [PageName] = @PageName, [Title] = @Title, [ImageURL] = @ImageURL, [isActive] = @isActive, [CreationTime] = @CreationTime
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Pages_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Pages]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.SeasonTicket_Insert",
                p => new
                    {
                        Name = p.String(),
                        EventId = p.Int(),
                        PromotorId = p.Int(),
                        isActive = p.Boolean(),
                        CreatedTime = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[SeasonTickets]([Name], [EventId], [PromotorId], [isActive], [CreatedTime])
                      VALUES (@Name, @EventId, @PromotorId, @isActive, @CreatedTime)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[SeasonTickets]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[SeasonTickets] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.SeasonTicket_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(),
                        EventId = p.Int(),
                        PromotorId = p.Int(),
                        isActive = p.Boolean(),
                        CreatedTime = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[SeasonTickets]
                      SET [Name] = @Name, [EventId] = @EventId, [PromotorId] = @PromotorId, [isActive] = @isActive, [CreatedTime] = @CreatedTime
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.SeasonTicket_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[SeasonTickets]
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.Notifications_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        NotificationTitle = p.String(),
                        NotificationTemplateId = p.Int(),
                        NotificationUserGroupId = p.Int(),
                        NotificationText = p.String(),
                        ScheduledDate = p.DateTime(),
                        ScheduledTime = p.DateTime(),
                        FrequencyId = p.Int(),
                        TriggerId = p.Int(),
                        isActive = p.Boolean(),
                        CreationTime = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[Notifications]([PromotorId], [NotificationTitle], [NotificationTemplateId], [NotificationUserGroupId], [NotificationText], [ScheduledDate], [ScheduledTime], [FrequencyId], [TriggerId], [isActive], [CreationTime])
                      VALUES (@PromotorId, @NotificationTitle, @NotificationTemplateId, @NotificationUserGroupId, @NotificationText, @ScheduledDate, @ScheduledTime, @FrequencyId, @TriggerId, @isActive, @CreationTime)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Notifications]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Notifications] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Notifications_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        NotificationTitle = p.String(),
                        NotificationTemplateId = p.Int(),
                        NotificationUserGroupId = p.Int(),
                        NotificationText = p.String(),
                        ScheduledDate = p.DateTime(),
                        ScheduledTime = p.DateTime(),
                        FrequencyId = p.Int(),
                        TriggerId = p.Int(),
                        isActive = p.Boolean(),
                        CreationTime = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Notifications]
                      SET [PromotorId] = @PromotorId, [NotificationTitle] = @NotificationTitle, [NotificationTemplateId] = @NotificationTemplateId, [NotificationUserGroupId] = @NotificationUserGroupId, [NotificationText] = @NotificationText, [ScheduledDate] = @ScheduledDate, [ScheduledTime] = @ScheduledTime, [FrequencyId] = @FrequencyId, [TriggerId] = @TriggerId, [isActive] = @isActive, [CreationTime] = @CreationTime
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.SeasonTicket_Delete");
            DropStoredProcedure("dbo.SeasonTicket_Update");
            DropStoredProcedure("dbo.SeasonTicket_Insert");
            DropStoredProcedure("dbo.Pages_Delete");
            DropStoredProcedure("dbo.Pages_Update");
            DropStoredProcedure("dbo.Pages_Insert");
            DropStoredProcedure("dbo.HomePageHeader_Delete");
            DropStoredProcedure("dbo.HomePageHeader_Update");
            DropStoredProcedure("dbo.HomePageHeader_Insert");
            DropStoredProcedure("dbo.HomePageContent_Delete");
            DropStoredProcedure("dbo.HomePageContent_Update");
            DropStoredProcedure("dbo.HomePageContent_Insert");
            AddColumn("dbo.Notifications", "NotificationName", c => c.String());
            DropColumn("dbo.Notifications", "TriggerId");
            DropColumn("dbo.Notifications", "FrequencyId");
            DropColumn("dbo.Notifications", "NotificationTitle");
            DropTable("dbo.SeasonTickets");
            DropTable("dbo.Pages");
            DropTable("dbo.HomePageHeaders");
            DropTable("dbo.HomePageContents");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
