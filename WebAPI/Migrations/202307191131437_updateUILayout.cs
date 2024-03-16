namespace WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateUILayout : DbMigration
    {
        public override void Up()
        {
            
            CreateTable(
                "dbo.UILayouts",
                c => new
                    {
                        LayoutId = c.Int(nullable: false, identity: true),
                        PromotorId = c.Decimal(precision: 18, scale: 2),
                        PrimaryColor = c.String(),
                        SecondaryColor = c.String(),
                        AdditionalColors = c.String(),
                        LogoURL = c.String(),
                        DarkLogoURL = c.String(),
                        FaviconURL = c.String(),
                        LogoLink = c.String(),
                        TicketBgForOrderURL = c.String(),
                        FrontImageForSessionTicketURL = c.String(),
                        CheckBgURL = c.String(),
                        OrderCompletionImageURL = c.String(),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.LayoutId);

            CreateStoredProcedure(
               "dbo.UILayout_Insert",
               p => new
               {
                   PromotorId = p.Decimal(precision: 18, scale: 2),
                   PrimaryColor = p.String(),
                   SecondaryColor = p.String(),
                   AdditionalColors = p.String(),
                   LogoURL = p.String(),
                   DarkLogoURL = p.String(),
                   FaviconURL = p.String(),
                   LogoLink = p.String(),
                   TicketBgForOrderURL = p.String(),
                   FrontImageForSessionTicketURL = p.String(),
                   CheckBgURL = p.String(),
                   OrderCompletionImageURL = p.String(),
                   CreatedDate = p.DateTime(),
               },
               body:
                   @"INSERT [dbo].[UILayouts]([PromotorId], [PrimaryColor], [SecondaryColor], [AdditionalColors], [LogoURL], [DarkLogoURL], [FaviconURL], [LogoLink], [TicketBgForOrderURL], [FrontImageForSessionTicketURL], [CheckBgURL], [OrderCompletionImageURL], [CreatedDate])
                      VALUES (@PromotorId, @PrimaryColor, @SecondaryColor, @AdditionalColors, @LogoURL, @DarkLogoURL, @FaviconURL, @LogoLink, @TicketBgForOrderURL, @FrontImageForSessionTicketURL, @CheckBgURL, @OrderCompletionImageURL, @CreatedDate)
                      
                      DECLARE @LayoutId int
                      SELECT @LayoutId = [LayoutId]
                      FROM [dbo].[UILayouts]
                      WHERE @@ROWCOUNT > 0 AND [LayoutId] = scope_identity()
                      
                      SELECT t0.[LayoutId]
                      FROM [dbo].[UILayouts] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[LayoutId] = @LayoutId"
           );
        }
        
        public override void Down()
        {
            DropTable("dbo.UILayouts");
        }
    }
}
