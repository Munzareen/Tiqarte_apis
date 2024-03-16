namespace WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUser100 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotorId = c.Int(nullable: false),
                        NotificationName = c.String(),
                        NotificationTemplateId = c.Int(nullable: false),
                        NotificationUserGroupId = c.Int(nullable: false),
                        NotificationText = c.String(),
                        ScheduledDate = c.DateTime(nullable: false),
                        ScheduledTime = c.DateTime(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NotificationTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotorId = c.Int(nullable: false),
                        TemplateTitle = c.String(),
                        TemplateDescription = c.String(),
                        isActive = c.Boolean(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NotificationUserGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotorId = c.Int(nullable: false),
                        UserGroupTitle = c.String(),
                        Location = c.String(),
                        UserTypeId = c.Int(nullable: false),
                        Criteria = c.String(),
                        isActive = c.Boolean(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ScheduledReports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotorId = c.Int(nullable: false),
                        ScheduledReportName = c.String(),
                        ReportId = c.Int(nullable: false),
                        DayofWeek = c.Int(nullable: false),
                        ReportScheduled = c.Int(nullable: false),
                        ReportScheduledTime = c.DateTime(nullable: false),
                        EmailAddress = c.String(),
                        isActive = c.Boolean(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.User", "isVerified", c => c.Boolean(nullable: false));
            CreateStoredProcedure(
                "dbo.Notifications_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        NotificationName = p.String(),
                        NotificationTemplateId = p.Int(),
                        NotificationUserGroupId = p.Int(),
                        NotificationText = p.String(),
                        ScheduledDate = p.DateTime(),
                        ScheduledTime = p.DateTime(),
                        isActive = p.Boolean(),
                        CreationTime = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[Notifications]([PromotorId], [NotificationName], [NotificationTemplateId], [NotificationUserGroupId], [NotificationText], [ScheduledDate], [ScheduledTime], [isActive], [CreationTime])
                      VALUES (@PromotorId, @NotificationName, @NotificationTemplateId, @NotificationUserGroupId, @NotificationText, @ScheduledDate, @ScheduledTime, @isActive, @CreationTime)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Notifications]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Notifications] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Notifications_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        NotificationName = p.String(),
                        NotificationTemplateId = p.Int(),
                        NotificationUserGroupId = p.Int(),
                        NotificationText = p.String(),
                        ScheduledDate = p.DateTime(),
                        ScheduledTime = p.DateTime(),
                        isActive = p.Boolean(),
                        CreationTime = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Notifications]
                      SET [PromotorId] = @PromotorId, [NotificationName] = @NotificationName, [NotificationTemplateId] = @NotificationTemplateId, [NotificationUserGroupId] = @NotificationUserGroupId, [NotificationText] = @NotificationText, [ScheduledDate] = @ScheduledDate, [ScheduledTime] = @ScheduledTime, [isActive] = @isActive, [CreationTime] = @CreationTime
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Notifications_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Notifications]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.NotificationTemplate_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        TemplateTitle = p.String(),
                        TemplateDescription = p.String(),
                        isActive = p.Boolean(),
                        CreationTime = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[NotificationTemplates]([PromotorId], [TemplateTitle], [TemplateDescription], [isActive], [CreationTime])
                      VALUES (@PromotorId, @TemplateTitle, @TemplateDescription, @isActive, @CreationTime)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[NotificationTemplates]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[NotificationTemplates] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.NotificationTemplate_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        TemplateTitle = p.String(),
                        TemplateDescription = p.String(),
                        isActive = p.Boolean(),
                        CreationTime = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[NotificationTemplates]
                      SET [PromotorId] = @PromotorId, [TemplateTitle] = @TemplateTitle, [TemplateDescription] = @TemplateDescription, [isActive] = @isActive, [CreationTime] = @CreationTime
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.NotificationTemplate_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[NotificationTemplates]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.NotificationUserGroup_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        UserGroupTitle = p.String(),
                        Location = p.String(),
                        UserTypeId = p.Int(),
                        Criteria = p.String(),
                        isActive = p.Boolean(),
                        CreationTime = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[NotificationUserGroups]([PromotorId], [UserGroupTitle], [Location], [UserTypeId], [Criteria], [isActive], [CreationTime])
                      VALUES (@PromotorId, @UserGroupTitle, @Location, @UserTypeId, @Criteria, @isActive, @CreationTime)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[NotificationUserGroups]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[NotificationUserGroups] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.NotificationUserGroup_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        UserGroupTitle = p.String(),
                        Location = p.String(),
                        UserTypeId = p.Int(),
                        Criteria = p.String(),
                        isActive = p.Boolean(),
                        CreationTime = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[NotificationUserGroups]
                      SET [PromotorId] = @PromotorId, [UserGroupTitle] = @UserGroupTitle, [Location] = @Location, [UserTypeId] = @UserTypeId, [Criteria] = @Criteria, [isActive] = @isActive, [CreationTime] = @CreationTime
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.NotificationUserGroup_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[NotificationUserGroups]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ScheduledReports_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        ScheduledReportName = p.String(),
                        ReportId = p.Int(),
                        DayofWeek = p.Int(),
                        ReportScheduled = p.Int(),
                        ReportScheduledTime = p.DateTime(),
                        EmailAddress = p.String(),
                        isActive = p.Boolean(),
                        CreationTime = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[ScheduledReports]([PromotorId], [ScheduledReportName], [ReportId], [DayofWeek], [ReportScheduled], [ReportScheduledTime], [EmailAddress], [isActive], [CreationTime])
                      VALUES (@PromotorId, @ScheduledReportName, @ReportId, @DayofWeek, @ReportScheduled, @ReportScheduledTime, @EmailAddress, @isActive, @CreationTime)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ScheduledReports]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ScheduledReports] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.ScheduledReports_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        ScheduledReportName = p.String(),
                        ReportId = p.Int(),
                        DayofWeek = p.Int(),
                        ReportScheduled = p.Int(),
                        ReportScheduledTime = p.DateTime(),
                        EmailAddress = p.String(),
                        isActive = p.Boolean(),
                        CreationTime = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[ScheduledReports]
                      SET [PromotorId] = @PromotorId, [ScheduledReportName] = @ScheduledReportName, [ReportId] = @ReportId, [DayofWeek] = @DayofWeek, [ReportScheduled] = @ReportScheduled, [ReportScheduledTime] = @ReportScheduledTime, [EmailAddress] = @EmailAddress, [isActive] = @isActive, [CreationTime] = @CreationTime
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ScheduledReports_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[ScheduledReports]
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.ApplicationUser_Insert",
                p => new
                    {
                        Id = p.String(maxLength: 128),
                        UserId = p.Int(),
                        FirstName = p.String(),
                        LastName = p.String(),
                        Password = p.String(),
                        Role = p.String(),
                        Gender = p.String(),
                        NickName = p.String(),
                        isProfileCompleted = p.Boolean(),
                        isVerified = p.Boolean(),
                        DOB = p.String(),
                        FullName = p.String(),
                        ImageUrl = p.String(),
                        UserTypeId = p.Int(),
                        Location = p.String(),
                        State = p.String(),
                        City = p.String(),
                        ZipCode = p.String(),
                        CountryCode = p.String(),
                        PhoneNumber = p.String(),
                        Email = p.String(maxLength: 256),
                        EmailConfirmed = p.Boolean(),
                        PasswordHash = p.String(),
                        SecurityStamp = p.String(),
                        PhoneNumberConfirmed = p.Boolean(),
                        TwoFactorEnabled = p.Boolean(),
                        LockoutEndDateUtc = p.DateTime(),
                        LockoutEnabled = p.Boolean(),
                        AccessFailedCount = p.Int(),
                        UserName = p.String(maxLength: 256),
                    },
                body:
                    @"INSERT [dbo].[User]([Id], [UserId], [FirstName], [LastName], [Password], [Role], [Gender], [NickName], [isProfileCompleted], [isVerified], [DOB], [FullName], [ImageUrl], [UserTypeId], [Location], [State], [City], [ZipCode], [CountryCode], [PhoneNumber], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName])
                      VALUES (@Id, @UserId, @FirstName, @LastName, @Password, @Role, @Gender, @NickName, @isProfileCompleted, @isVerified, @DOB, @FullName, @ImageUrl, @UserTypeId, @Location, @State, @City, @ZipCode, @CountryCode, @PhoneNumber, @Email, @EmailConfirmed, @PasswordHash, @SecurityStamp, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEndDateUtc, @LockoutEnabled, @AccessFailedCount, @UserName)"
            );
            
            AlterStoredProcedure(
                "dbo.ApplicationUser_Update",
                p => new
                    {
                        Id = p.String(maxLength: 128),
                        UserId = p.Int(),
                        FirstName = p.String(),
                        LastName = p.String(),
                        Password = p.String(),
                        Role = p.String(),
                        Gender = p.String(),
                        NickName = p.String(),
                        isProfileCompleted = p.Boolean(),
                        isVerified = p.Boolean(),
                        DOB = p.String(),
                        FullName = p.String(),
                        ImageUrl = p.String(),
                        UserTypeId = p.Int(),
                        Location = p.String(),
                        State = p.String(),
                        City = p.String(),
                        ZipCode = p.String(),
                        CountryCode = p.String(),
                        PhoneNumber = p.String(),
                        Email = p.String(maxLength: 256),
                        EmailConfirmed = p.Boolean(),
                        PasswordHash = p.String(),
                        SecurityStamp = p.String(),
                        PhoneNumberConfirmed = p.Boolean(),
                        TwoFactorEnabled = p.Boolean(),
                        LockoutEndDateUtc = p.DateTime(),
                        LockoutEnabled = p.Boolean(),
                        AccessFailedCount = p.Int(),
                        UserName = p.String(maxLength: 256),
                    },
                body:
                    @"UPDATE [dbo].[User]
                      SET [UserId] = @UserId, [FirstName] = @FirstName, [LastName] = @LastName, [Password] = @Password, [Role] = @Role, [Gender] = @Gender, [NickName] = @NickName, [isProfileCompleted] = @isProfileCompleted, [isVerified] = @isVerified, [DOB] = @DOB, [FullName] = @FullName, [ImageUrl] = @ImageUrl, [UserTypeId] = @UserTypeId, [Location] = @Location, [State] = @State, [City] = @City, [ZipCode] = @ZipCode, [CountryCode] = @CountryCode, [PhoneNumber] = @PhoneNumber, [Email] = @Email, [EmailConfirmed] = @EmailConfirmed, [PasswordHash] = @PasswordHash, [SecurityStamp] = @SecurityStamp, [PhoneNumberConfirmed] = @PhoneNumberConfirmed, [TwoFactorEnabled] = @TwoFactorEnabled, [LockoutEndDateUtc] = @LockoutEndDateUtc, [LockoutEnabled] = @LockoutEnabled, [AccessFailedCount] = @AccessFailedCount, [UserName] = @UserName
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.ScheduledReports_Delete");
            DropStoredProcedure("dbo.ScheduledReports_Update");
            DropStoredProcedure("dbo.ScheduledReports_Insert");
            DropStoredProcedure("dbo.NotificationUserGroup_Delete");
            DropStoredProcedure("dbo.NotificationUserGroup_Update");
            DropStoredProcedure("dbo.NotificationUserGroup_Insert");
            DropStoredProcedure("dbo.NotificationTemplate_Delete");
            DropStoredProcedure("dbo.NotificationTemplate_Update");
            DropStoredProcedure("dbo.NotificationTemplate_Insert");
            DropStoredProcedure("dbo.Notifications_Delete");
            DropStoredProcedure("dbo.Notifications_Update");
            DropStoredProcedure("dbo.Notifications_Insert");
            DropColumn("dbo.User", "isVerified");
            DropTable("dbo.ScheduledReports");
            DropTable("dbo.NotificationUserGroups");
            DropTable("dbo.NotificationTemplates");
            DropTable("dbo.Notifications");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
