namespace WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb115 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OnBoardings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotorId = c.Int(nullable: false),
                        ImageUrl = c.String(),
                        Heading = c.String(),
                        Description = c.String(),
                        isActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.UILayouts", "PromotorId", c => c.Int(nullable: false));
            CreateStoredProcedure(
                "dbo.OnBoarding_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        ImageUrl = p.String(),
                        Heading = p.String(),
                        Description = p.String(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[OnBoardings]([PromotorId], [ImageUrl], [Heading], [Description], [isActive], [CreatedDate])
                      VALUES (@PromotorId, @ImageUrl, @Heading, @Description, @isActive, @CreatedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[OnBoardings]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[OnBoardings] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.OnBoarding_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        ImageUrl = p.String(),
                        Heading = p.String(),
                        Description = p.String(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[OnBoardings]
                      SET [PromotorId] = @PromotorId, [ImageUrl] = @ImageUrl, [Heading] = @Heading, [Description] = @Description, [isActive] = @isActive, [CreatedDate] = @CreatedDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.OnBoarding_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[OnBoardings]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.OnBoarding_Delete");
            DropStoredProcedure("dbo.OnBoarding_Update");
            DropStoredProcedure("dbo.OnBoarding_Insert");
            DropTable("dbo.OnBoardings");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
