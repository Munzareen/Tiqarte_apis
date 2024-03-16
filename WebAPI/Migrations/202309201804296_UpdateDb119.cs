namespace WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb119 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "isActive", c => c.Boolean(nullable: false));
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
                        PromotorId = p.Int(),
                        Notes = p.String(),
                        CreationDate = p.DateTime(),
                        LastUpdate = p.DateTime(),
                        UpdatedBy = p.Int(),
                        isActive = p.Boolean(),
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
                    @"INSERT [dbo].[User]([Id], [UserId], [FirstName], [LastName], [Password], [Role], [Gender], [NickName], [isProfileCompleted], [isVerified], [DOB], [FullName], [ImageUrl], [UserTypeId], [Location], [State], [City], [ZipCode], [CountryCode], [PhoneNumber], [PromotorId], [Notes], [CreationDate], [LastUpdate], [UpdatedBy], [isActive], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName])
                      VALUES (@Id, @UserId, @FirstName, @LastName, @Password, @Role, @Gender, @NickName, @isProfileCompleted, @isVerified, @DOB, @FullName, @ImageUrl, @UserTypeId, @Location, @State, @City, @ZipCode, @CountryCode, @PhoneNumber, @PromotorId, @Notes, @CreationDate, @LastUpdate, @UpdatedBy, @isActive, @Email, @EmailConfirmed, @PasswordHash, @SecurityStamp, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEndDateUtc, @LockoutEnabled, @AccessFailedCount, @UserName)"
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
                        PromotorId = p.Int(),
                        Notes = p.String(),
                        CreationDate = p.DateTime(),
                        LastUpdate = p.DateTime(),
                        UpdatedBy = p.Int(),
                        isActive = p.Boolean(),
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
                      SET [UserId] = @UserId, [FirstName] = @FirstName, [LastName] = @LastName, [Password] = @Password, [Role] = @Role, [Gender] = @Gender, [NickName] = @NickName, [isProfileCompleted] = @isProfileCompleted, [isVerified] = @isVerified, [DOB] = @DOB, [FullName] = @FullName, [ImageUrl] = @ImageUrl, [UserTypeId] = @UserTypeId, [Location] = @Location, [State] = @State, [City] = @City, [ZipCode] = @ZipCode, [CountryCode] = @CountryCode, [PhoneNumber] = @PhoneNumber, [PromotorId] = @PromotorId, [Notes] = @Notes, [CreationDate] = @CreationDate, [LastUpdate] = @LastUpdate, [UpdatedBy] = @UpdatedBy, [isActive] = @isActive, [Email] = @Email, [EmailConfirmed] = @EmailConfirmed, [PasswordHash] = @PasswordHash, [SecurityStamp] = @SecurityStamp, [PhoneNumberConfirmed] = @PhoneNumberConfirmed, [TwoFactorEnabled] = @TwoFactorEnabled, [LockoutEndDateUtc] = @LockoutEndDateUtc, [LockoutEnabled] = @LockoutEnabled, [AccessFailedCount] = @AccessFailedCount, [UserName] = @UserName
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "isActive");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
