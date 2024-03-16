﻿namespace WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb111 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BlockStands",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotorId = c.Int(nullable: false),
                        StadiumId = c.Int(nullable: false),
                        BlockStandName = c.String(),
                        isActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EventTickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotorId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Time = c.DateTime(nullable: false),
                        EventRunTime = c.Double(nullable: false),
                        DisplayEventTime = c.Boolean(nullable: false),
                        Location = c.String(),
                        ManagementFeeType = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Add1EuroBookingFeeUnder10 = c.Boolean(nullable: false),
                        Copy = c.String(),
                        OverrideCapacityScheduleSoldOut = c.String(),
                        MinimumAge = c.Int(nullable: false),
                        ProductURL = c.String(),
                        isItBuyable = c.String(),
                        MarkAsSold = c.String(),
                        Venue = c.String(),
                        isActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Exclusions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotorId = c.Int(nullable: false),
                        StadiumId = c.Int(nullable: false),
                        BlockStandsId = c.Int(nullable: false),
                        RowsInBlockStandsId = c.Int(nullable: false),
                        SeatsInRowBlockStandsId = c.Int(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RowsInBlockStands",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotorId = c.Int(nullable: false),
                        BlockStandsId = c.Int(nullable: false),
                        RowName = c.String(),
                        isActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SeatsInRowBlockStands",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotorId = c.Int(nullable: false),
                        RowsInBlockStandsId = c.Int(nullable: false),
                        SeatNumber = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        isActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StandardSeatTickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotorId = c.Int(nullable: false),
                        StadiumId = c.Int(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TicketPasswordProtections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotorId = c.Int(nullable: false),
                        EventTicketsId = c.Int(nullable: false),
                        Password = c.String(),
                        isEnablePasswordProtection = c.Boolean(),
                        AutoGeneratedLink = c.String(),
                        Visibility = c.String(),
                        Slug = c.String(),
                        URL = c.String(),
                        isActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VariableSeatTickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotorId = c.Int(nullable: false),
                        VariationName = c.String(),
                        VariationColor = c.String(),
                        VariationPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SeasonTicketId = c.Int(nullable: false),
                        AttendeeAgeTitle = c.String(),
                        SeatApplyFor = c.String(),
                        HideFromFrontEnd = c.Boolean(nullable: false),
                        StadiumId = c.Int(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TicketDetails", "PromotorId", c => c.Int());
            AddColumn("dbo.TicketDetails", "SeasonTicketId", c => c.Int());
            AddColumn("dbo.TicketDetails", "AttendeeAge", c => c.Int());
            AddColumn("dbo.TicketDetails", "HideFromFrontend", c => c.Boolean());
            AddColumn("dbo.TicketDetails", "ExcludeFromOverallCapacity", c => c.Boolean());
            AddColumn("dbo.TicketDetails", "MaximumTickets", c => c.Int());
            AddColumn("dbo.TicketDetails", "MinimumTickets", c => c.Int());
            AddColumn("dbo.TicketDetails", "UnitCost", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.TicketDetails", "RequiredTicketHolderDetails", c => c.Boolean());
            AddColumn("dbo.TicketDetails", "TicketDescription", c => c.String());
            AddColumn("dbo.TicketDetails", "DocumentURL", c => c.String());
            AddColumn("dbo.TicketDetails", "AcknowledgementURL", c => c.String());
            AddColumn("dbo.TicketDetails", "MetaDataURL", c => c.String());
            AddColumn("dbo.TicketDetails", "isActive", c => c.Boolean());
            AddColumn("dbo.TicketDetails", "CreatedDate", c => c.DateTime());
            CreateStoredProcedure(
                "dbo.BlockStands_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        StadiumId = p.Int(),
                        BlockStandName = p.String(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[BlockStands]([PromotorId], [StadiumId], [BlockStandName], [isActive], [CreatedDate])
                      VALUES (@PromotorId, @StadiumId, @BlockStandName, @isActive, @CreatedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[BlockStands]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[BlockStands] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.BlockStands_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        StadiumId = p.Int(),
                        BlockStandName = p.String(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[BlockStands]
                      SET [PromotorId] = @PromotorId, [StadiumId] = @StadiumId, [BlockStandName] = @BlockStandName, [isActive] = @isActive, [CreatedDate] = @CreatedDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.BlockStands_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[BlockStands]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.EventTickets_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        Date = p.DateTime(),
                        Time = p.DateTime(),
                        EventRunTime = p.Double(),
                        DisplayEventTime = p.Boolean(),
                        Location = p.String(),
                        ManagementFeeType = p.String(),
                        Amount = p.Decimal(precision: 18, scale: 2),
                        Add1EuroBookingFeeUnder10 = p.Boolean(),
                        Copy = p.String(),
                        OverrideCapacityScheduleSoldOut = p.String(),
                        MinimumAge = p.Int(),
                        ProductURL = p.String(),
                        isItBuyable = p.String(),
                        MarkAsSold = p.String(),
                        Venue = p.String(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[EventTickets]([PromotorId], [Date], [Time], [EventRunTime], [DisplayEventTime], [Location], [ManagementFeeType], [Amount], [Add1EuroBookingFeeUnder10], [Copy], [OverrideCapacityScheduleSoldOut], [MinimumAge], [ProductURL], [isItBuyable], [MarkAsSold], [Venue], [isActive], [CreatedDate])
                      VALUES (@PromotorId, @Date, @Time, @EventRunTime, @DisplayEventTime, @Location, @ManagementFeeType, @Amount, @Add1EuroBookingFeeUnder10, @Copy, @OverrideCapacityScheduleSoldOut, @MinimumAge, @ProductURL, @isItBuyable, @MarkAsSold, @Venue, @isActive, @CreatedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[EventTickets]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[EventTickets] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.EventTickets_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        Date = p.DateTime(),
                        Time = p.DateTime(),
                        EventRunTime = p.Double(),
                        DisplayEventTime = p.Boolean(),
                        Location = p.String(),
                        ManagementFeeType = p.String(),
                        Amount = p.Decimal(precision: 18, scale: 2),
                        Add1EuroBookingFeeUnder10 = p.Boolean(),
                        Copy = p.String(),
                        OverrideCapacityScheduleSoldOut = p.String(),
                        MinimumAge = p.Int(),
                        ProductURL = p.String(),
                        isItBuyable = p.String(),
                        MarkAsSold = p.String(),
                        Venue = p.String(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[EventTickets]
                      SET [PromotorId] = @PromotorId, [Date] = @Date, [Time] = @Time, [EventRunTime] = @EventRunTime, [DisplayEventTime] = @DisplayEventTime, [Location] = @Location, [ManagementFeeType] = @ManagementFeeType, [Amount] = @Amount, [Add1EuroBookingFeeUnder10] = @Add1EuroBookingFeeUnder10, [Copy] = @Copy, [OverrideCapacityScheduleSoldOut] = @OverrideCapacityScheduleSoldOut, [MinimumAge] = @MinimumAge, [ProductURL] = @ProductURL, [isItBuyable] = @isItBuyable, [MarkAsSold] = @MarkAsSold, [Venue] = @Venue, [isActive] = @isActive, [CreatedDate] = @CreatedDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.EventTickets_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[EventTickets]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Exclusions_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        StadiumId = p.Int(),
                        BlockStandsId = p.Int(),
                        RowsInBlockStandsId = p.Int(),
                        SeatsInRowBlockStandsId = p.Int(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[Exclusions]([PromotorId], [StadiumId], [BlockStandsId], [RowsInBlockStandsId], [SeatsInRowBlockStandsId], [isActive], [CreatedDate])
                      VALUES (@PromotorId, @StadiumId, @BlockStandsId, @RowsInBlockStandsId, @SeatsInRowBlockStandsId, @isActive, @CreatedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Exclusions]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Exclusions] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Exclusions_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        StadiumId = p.Int(),
                        BlockStandsId = p.Int(),
                        RowsInBlockStandsId = p.Int(),
                        SeatsInRowBlockStandsId = p.Int(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Exclusions]
                      SET [PromotorId] = @PromotorId, [StadiumId] = @StadiumId, [BlockStandsId] = @BlockStandsId, [RowsInBlockStandsId] = @RowsInBlockStandsId, [SeatsInRowBlockStandsId] = @SeatsInRowBlockStandsId, [isActive] = @isActive, [CreatedDate] = @CreatedDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Exclusions_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Exclusions]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.RowsInBlockStands_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        BlockStandsId = p.Int(),
                        RowName = p.String(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[RowsInBlockStands]([PromotorId], [BlockStandsId], [RowName], [isActive], [CreatedDate])
                      VALUES (@PromotorId, @BlockStandsId, @RowName, @isActive, @CreatedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[RowsInBlockStands]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[RowsInBlockStands] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.RowsInBlockStands_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        BlockStandsId = p.Int(),
                        RowName = p.String(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[RowsInBlockStands]
                      SET [PromotorId] = @PromotorId, [BlockStandsId] = @BlockStandsId, [RowName] = @RowName, [isActive] = @isActive, [CreatedDate] = @CreatedDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.RowsInBlockStands_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[RowsInBlockStands]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.SeatsInRowBlockStands_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        RowsInBlockStandsId = p.Int(),
                        SeatNumber = p.String(),
                        Price = p.Decimal(precision: 18, scale: 2),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[SeatsInRowBlockStands]([PromotorId], [RowsInBlockStandsId], [SeatNumber], [Price], [isActive], [CreatedDate])
                      VALUES (@PromotorId, @RowsInBlockStandsId, @SeatNumber, @Price, @isActive, @CreatedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[SeatsInRowBlockStands]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[SeatsInRowBlockStands] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.SeatsInRowBlockStands_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        RowsInBlockStandsId = p.Int(),
                        SeatNumber = p.String(),
                        Price = p.Decimal(precision: 18, scale: 2),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[SeatsInRowBlockStands]
                      SET [PromotorId] = @PromotorId, [RowsInBlockStandsId] = @RowsInBlockStandsId, [SeatNumber] = @SeatNumber, [Price] = @Price, [isActive] = @isActive, [CreatedDate] = @CreatedDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.SeatsInRowBlockStands_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[SeatsInRowBlockStands]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.StandardSeatTicket_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        StadiumId = p.Int(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[StandardSeatTickets]([PromotorId], [StadiumId], [isActive], [CreatedDate])
                      VALUES (@PromotorId, @StadiumId, @isActive, @CreatedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[StandardSeatTickets]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[StandardSeatTickets] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.StandardSeatTicket_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        StadiumId = p.Int(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[StandardSeatTickets]
                      SET [PromotorId] = @PromotorId, [StadiumId] = @StadiumId, [isActive] = @isActive, [CreatedDate] = @CreatedDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.StandardSeatTicket_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[StandardSeatTickets]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.TicketPasswordProtection_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        EventTicketsId = p.Int(),
                        Password = p.String(),
                        isEnablePasswordProtection = p.Boolean(),
                        AutoGeneratedLink = p.String(),
                        Visibility = p.String(),
                        Slug = p.String(),
                        URL = p.String(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[TicketPasswordProtections]([PromotorId], [EventTicketsId], [Password], [isEnablePasswordProtection], [AutoGeneratedLink], [Visibility], [Slug], [URL], [isActive], [CreatedDate])
                      VALUES (@PromotorId, @EventTicketsId, @Password, @isEnablePasswordProtection, @AutoGeneratedLink, @Visibility, @Slug, @URL, @isActive, @CreatedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[TicketPasswordProtections]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[TicketPasswordProtections] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.TicketPasswordProtection_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        EventTicketsId = p.Int(),
                        Password = p.String(),
                        isEnablePasswordProtection = p.Boolean(),
                        AutoGeneratedLink = p.String(),
                        Visibility = p.String(),
                        Slug = p.String(),
                        URL = p.String(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[TicketPasswordProtections]
                      SET [PromotorId] = @PromotorId, [EventTicketsId] = @EventTicketsId, [Password] = @Password, [isEnablePasswordProtection] = @isEnablePasswordProtection, [AutoGeneratedLink] = @AutoGeneratedLink, [Visibility] = @Visibility, [Slug] = @Slug, [URL] = @URL, [isActive] = @isActive, [CreatedDate] = @CreatedDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.TicketPasswordProtection_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[TicketPasswordProtections]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.VariableSeatTicket_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        VariationName = p.String(),
                        VariationColor = p.String(),
                        VariationPrice = p.Decimal(precision: 18, scale: 2),
                        SeasonTicketId = p.Int(),
                        AttendeeAgeTitle = p.String(),
                        SeatApplyFor = p.String(),
                        HideFromFrontEnd = p.Boolean(),
                        StadiumId = p.Int(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[VariableSeatTickets]([PromotorId], [VariationName], [VariationColor], [VariationPrice], [SeasonTicketId], [AttendeeAgeTitle], [SeatApplyFor], [HideFromFrontEnd], [StadiumId], [isActive], [CreatedDate])
                      VALUES (@PromotorId, @VariationName, @VariationColor, @VariationPrice, @SeasonTicketId, @AttendeeAgeTitle, @SeatApplyFor, @HideFromFrontEnd, @StadiumId, @isActive, @CreatedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[VariableSeatTickets]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[VariableSeatTickets] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.VariableSeatTicket_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        VariationName = p.String(),
                        VariationColor = p.String(),
                        VariationPrice = p.Decimal(precision: 18, scale: 2),
                        SeasonTicketId = p.Int(),
                        AttendeeAgeTitle = p.String(),
                        SeatApplyFor = p.String(),
                        HideFromFrontEnd = p.Boolean(),
                        StadiumId = p.Int(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[VariableSeatTickets]
                      SET [PromotorId] = @PromotorId, [VariationName] = @VariationName, [VariationColor] = @VariationColor, [VariationPrice] = @VariationPrice, [SeasonTicketId] = @SeasonTicketId, [AttendeeAgeTitle] = @AttendeeAgeTitle, [SeatApplyFor] = @SeatApplyFor, [HideFromFrontEnd] = @HideFromFrontEnd, [StadiumId] = @StadiumId, [isActive] = @isActive, [CreatedDate] = @CreatedDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.VariableSeatTicket_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[VariableSeatTickets]
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.TicketDetails_Insert",
                p => new
                    {
                        PromotorId = p.Int(),
                        EventId = p.Int(),
                        TicketType = p.String(),
                        TicketPrice = p.Decimal(precision: 18, scale: 2),
                        BookingFee = p.Decimal(precision: 18, scale: 2),
                        AvailableTickets = p.Int(),
                        SeasonTicketId = p.Int(),
                        AttendeeAge = p.Int(),
                        HideFromFrontend = p.Boolean(),
                        ExcludeFromOverallCapacity = p.Boolean(),
                        MaximumTickets = p.Int(),
                        MinimumTickets = p.Int(),
                        UnitCost = p.Decimal(precision: 18, scale: 2),
                        RequiredTicketHolderDetails = p.Boolean(),
                        TicketDescription = p.String(),
                        DocumentURL = p.String(),
                        AcknowledgementURL = p.String(),
                        MetaDataURL = p.String(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[TicketDetails]([PromotorId], [EventId], [TicketType], [TicketPrice], [BookingFee], [AvailableTickets], [SeasonTicketId], [AttendeeAge], [HideFromFrontend], [ExcludeFromOverallCapacity], [MaximumTickets], [MinimumTickets], [UnitCost], [RequiredTicketHolderDetails], [TicketDescription], [DocumentURL], [AcknowledgementURL], [MetaDataURL], [isActive], [CreatedDate])
                      VALUES (@PromotorId, @EventId, @TicketType, @TicketPrice, @BookingFee, @AvailableTickets, @SeasonTicketId, @AttendeeAge, @HideFromFrontend, @ExcludeFromOverallCapacity, @MaximumTickets, @MinimumTickets, @UnitCost, @RequiredTicketHolderDetails, @TicketDescription, @DocumentURL, @AcknowledgementURL, @MetaDataURL, @isActive, @CreatedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[TicketDetails]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[TicketDetails] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.TicketDetails_Update",
                p => new
                    {
                        Id = p.Int(),
                        PromotorId = p.Int(),
                        EventId = p.Int(),
                        TicketType = p.String(),
                        TicketPrice = p.Decimal(precision: 18, scale: 2),
                        BookingFee = p.Decimal(precision: 18, scale: 2),
                        AvailableTickets = p.Int(),
                        SeasonTicketId = p.Int(),
                        AttendeeAge = p.Int(),
                        HideFromFrontend = p.Boolean(),
                        ExcludeFromOverallCapacity = p.Boolean(),
                        MaximumTickets = p.Int(),
                        MinimumTickets = p.Int(),
                        UnitCost = p.Decimal(precision: 18, scale: 2),
                        RequiredTicketHolderDetails = p.Boolean(),
                        TicketDescription = p.String(),
                        DocumentURL = p.String(),
                        AcknowledgementURL = p.String(),
                        MetaDataURL = p.String(),
                        isActive = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[TicketDetails]
                      SET [PromotorId] = @PromotorId, [EventId] = @EventId, [TicketType] = @TicketType, [TicketPrice] = @TicketPrice, [BookingFee] = @BookingFee, [AvailableTickets] = @AvailableTickets, [SeasonTicketId] = @SeasonTicketId, [AttendeeAge] = @AttendeeAge, [HideFromFrontend] = @HideFromFrontend, [ExcludeFromOverallCapacity] = @ExcludeFromOverallCapacity, [MaximumTickets] = @MaximumTickets, [MinimumTickets] = @MinimumTickets, [UnitCost] = @UnitCost, [RequiredTicketHolderDetails] = @RequiredTicketHolderDetails, [TicketDescription] = @TicketDescription, [DocumentURL] = @DocumentURL, [AcknowledgementURL] = @AcknowledgementURL, [MetaDataURL] = @MetaDataURL, [isActive] = @isActive, [CreatedDate] = @CreatedDate
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.VariableSeatTicket_Delete");
            DropStoredProcedure("dbo.VariableSeatTicket_Update");
            DropStoredProcedure("dbo.VariableSeatTicket_Insert");
            DropStoredProcedure("dbo.TicketPasswordProtection_Delete");
            DropStoredProcedure("dbo.TicketPasswordProtection_Update");
            DropStoredProcedure("dbo.TicketPasswordProtection_Insert");
            DropStoredProcedure("dbo.StandardSeatTicket_Delete");
            DropStoredProcedure("dbo.StandardSeatTicket_Update");
            DropStoredProcedure("dbo.StandardSeatTicket_Insert");
            DropStoredProcedure("dbo.SeatsInRowBlockStands_Delete");
            DropStoredProcedure("dbo.SeatsInRowBlockStands_Update");
            DropStoredProcedure("dbo.SeatsInRowBlockStands_Insert");
            DropStoredProcedure("dbo.RowsInBlockStands_Delete");
            DropStoredProcedure("dbo.RowsInBlockStands_Update");
            DropStoredProcedure("dbo.RowsInBlockStands_Insert");
            DropStoredProcedure("dbo.Exclusions_Delete");
            DropStoredProcedure("dbo.Exclusions_Update");
            DropStoredProcedure("dbo.Exclusions_Insert");
            DropStoredProcedure("dbo.EventTickets_Delete");
            DropStoredProcedure("dbo.EventTickets_Update");
            DropStoredProcedure("dbo.EventTickets_Insert");
            DropStoredProcedure("dbo.BlockStands_Delete");
            DropStoredProcedure("dbo.BlockStands_Update");
            DropStoredProcedure("dbo.BlockStands_Insert");
            DropColumn("dbo.TicketDetails", "CreatedDate");
            DropColumn("dbo.TicketDetails", "isActive");
            DropColumn("dbo.TicketDetails", "MetaDataURL");
            DropColumn("dbo.TicketDetails", "AcknowledgementURL");
            DropColumn("dbo.TicketDetails", "DocumentURL");
            DropColumn("dbo.TicketDetails", "TicketDescription");
            DropColumn("dbo.TicketDetails", "RequiredTicketHolderDetails");
            DropColumn("dbo.TicketDetails", "UnitCost");
            DropColumn("dbo.TicketDetails", "MinimumTickets");
            DropColumn("dbo.TicketDetails", "MaximumTickets");
            DropColumn("dbo.TicketDetails", "ExcludeFromOverallCapacity");
            DropColumn("dbo.TicketDetails", "HideFromFrontend");
            DropColumn("dbo.TicketDetails", "AttendeeAge");
            DropColumn("dbo.TicketDetails", "SeasonTicketId");
            DropColumn("dbo.TicketDetails", "PromotorId");
            DropTable("dbo.VariableSeatTickets");
            DropTable("dbo.TicketPasswordProtections");
            DropTable("dbo.StandardSeatTickets");
            DropTable("dbo.SeatsInRowBlockStands");
            DropTable("dbo.RowsInBlockStands");
            DropTable("dbo.Exclusions");
            DropTable("dbo.EventTickets");
            DropTable("dbo.BlockStands");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
