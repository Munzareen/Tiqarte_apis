namespace WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb110 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OTPTableTemps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmailAddress = c.String(),
                        OTPCode = c.Int(nullable: false),
                        IsCodeVerified = c.Boolean(nullable: false),
                        ExpirationTime = c.DateTime(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateStoredProcedure(
                "dbo.OTPTableTemp_Insert",
                p => new
                    {
                        EmailAddress = p.String(),
                        OTPCode = p.Int(),
                        IsCodeVerified = p.Boolean(),
                        ExpirationTime = p.DateTime(),
                        CreateDateTime = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[OTPTableTemps]([EmailAddress], [OTPCode], [IsCodeVerified], [ExpirationTime], [CreateDateTime])
                      VALUES (@EmailAddress, @OTPCode, @IsCodeVerified, @ExpirationTime, @CreateDateTime)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[OTPTableTemps]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[OTPTableTemps] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.OTPTableTemp_Update",
                p => new
                    {
                        Id = p.Int(),
                        EmailAddress = p.String(),
                        OTPCode = p.Int(),
                        IsCodeVerified = p.Boolean(),
                        ExpirationTime = p.DateTime(),
                        CreateDateTime = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[OTPTableTemps]
                      SET [EmailAddress] = @EmailAddress, [OTPCode] = @OTPCode, [IsCodeVerified] = @IsCodeVerified, [ExpirationTime] = @ExpirationTime, [CreateDateTime] = @CreateDateTime
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.OTPTableTemp_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[OTPTableTemps]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.OTPTableTemp_Delete");
            DropStoredProcedure("dbo.OTPTableTemp_Update");
            DropStoredProcedure("dbo.OTPTableTemp_Insert");
            DropTable("dbo.OTPTableTemps");
        }
    }
}
