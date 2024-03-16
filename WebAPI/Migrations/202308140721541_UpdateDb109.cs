namespace WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb109 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotorId = c.Int(nullable: false),
                        ReportName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateStoredProcedure(
                "dbo.Reports_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        ReportName = p.String(),
                        isActive = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[Reports]([PromotorId], [ReportName], [isActive])
                      VALUES (@PromotorId, @ReportName, @isActive)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Reports]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Reports] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Reports_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        ReportName = p.String(),
                        isActive = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[Reports]
                      SET [PromotorId] = @PromotorId, [ReportName] = @ReportName, [isActive] = @isActive
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Reports_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Reports]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Reports_Delete");
            DropStoredProcedure("dbo.Reports_Update");
            DropStoredProcedure("dbo.Reports_Insert");
            DropTable("dbo.Reports");
        }
    }
}
