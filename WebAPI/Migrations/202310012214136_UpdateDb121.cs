namespace WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb121 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HomePageContents", "Position", c => c.Int(nullable: false));
            AddColumn("dbo.ScheduledReports", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.ScheduledReports", "EndDate", c => c.DateTime(nullable: false));
            AlterStoredProcedure(
                "dbo.HomePageContent_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        ImageURL = p.String(),
                        Title = p.String(),
                        Content = p.String(),
                        Position = p.Int(),
                        isActive = p.Boolean(),
                        CreationTime = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[HomePageContents]([PromotorId], [ImageURL], [Title], [Content], [Position], [isActive], [CreationTime])
                      VALUES (@PromotorId, @ImageURL, @Title, @Content, @Position, @isActive, @CreationTime)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[HomePageContents]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[HomePageContents] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.HomePageContent_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        ImageURL = p.String(),
                        Title = p.String(),
                        Content = p.String(),
                        Position = p.Int(),
                        isActive = p.Boolean(),
                        CreationTime = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[HomePageContents]
                      SET [PromotorId] = @PromotorId, [ImageURL] = @ImageURL, [Title] = @Title, [Content] = @Content, [Position] = @Position, [isActive] = @isActive, [CreationTime] = @CreationTime
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
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
                        StartDate = p.DateTime(),
                        EndDate = p.DateTime(),
                        isActive = p.Boolean(),
                        CreationTime = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[ScheduledReports]([PromotorId], [ScheduledReportName], [ReportId], [DayofWeek], [ReportScheduled], [ReportScheduledTime], [EmailAddress], [StartDate], [EndDate], [isActive], [CreationTime])
                      VALUES (@PromotorId, @ScheduledReportName, @ReportId, @DayofWeek, @ReportScheduled, @ReportScheduledTime, @EmailAddress, @StartDate, @EndDate, @isActive, @CreationTime)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ScheduledReports]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ScheduledReports] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
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
                        StartDate = p.DateTime(),
                        EndDate = p.DateTime(),
                        isActive = p.Boolean(),
                        CreationTime = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[ScheduledReports]
                      SET [PromotorId] = @PromotorId, [ScheduledReportName] = @ScheduledReportName, [ReportId] = @ReportId, [DayofWeek] = @DayofWeek, [ReportScheduled] = @ReportScheduled, [ReportScheduledTime] = @ReportScheduledTime, [EmailAddress] = @EmailAddress, [StartDate] = @StartDate, [EndDate] = @EndDate, [isActive] = @isActive, [CreationTime] = @CreationTime
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.ScheduledReports", "EndDate");
            DropColumn("dbo.ScheduledReports", "StartDate");
            DropColumn("dbo.HomePageContents", "Position");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
