namespace AwesomeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migone : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.OperatorCustomizationDefinitions",
                c => new
                    {
                        OcD_OPEID = c.String(nullable: false, maxLength: 128),
                        OcD_CSDID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OcD_OPEID, t.OcD_CSDID })
                .ForeignKey("dbo.CustomizationDefinitions", t => t.OcD_CSDID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.OcD_OPEID, cascadeDelete: true)
                .Index(t => t.OcD_OPEID)
                .Index(t => t.OcD_CSDID);
            
            CreateTable(
                "dbo.CustomizationDefinitions",
                c => new
                    {
                        CsD_ID = c.Int(nullable: false, identity: true),
                        CsD_DefinitionName = c.String(),
                        CsD_Value = c.String(),
                        CsD_Description = c.String(),
                    })
                .PrimaryKey(t => t.CsD_ID);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CmP_ID = c.Int(nullable: false, identity: true),
                        CmP_Name = c.String(nullable: false, maxLength: 60),
                        CmP_PhoneNumber = c.String(),
                        CmP_PageAdress = c.String(),
                    })
                .PrimaryKey(t => t.CmP_ID)
                .Index(t => t.CmP_Name, unique: true);
            
            CreateTable(
                "dbo.TicketHeaderCustomers",
                c => new
                    {
                        TiC_ID = c.Int(nullable: false, identity: true),
                        TiC_CuSID = c.String(maxLength: 128),
                        TiC_TiHID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TiC_ID)
                .ForeignKey("dbo.Customers", t => t.TiC_CuSID)
                .ForeignKey("dbo.TicketHeaders", t => t.TiC_TiHID, cascadeDelete: true)
                .Index(t => t.TiC_CuSID)
                .Index(t => t.TiC_TiHID);
            
            CreateTable(
                "dbo.TicketHeaders",
                c => new
                    {
                        TiH_ID = c.Int(nullable: false, identity: true),
                        TiH_Subject = c.String(maxLength: 150),
                        TiH_TiSID = c.Int(nullable: false),
                        TiH_TiTID = c.Int(nullable: false),
                        TiH_CMPID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TiH_ID)
                .ForeignKey("dbo.Companies", t => t.TiH_CMPID, cascadeDelete: true)
                .ForeignKey("dbo.TicketStates", t => t.TiH_TiSID, cascadeDelete: true)
                .ForeignKey("dbo.TicketTypes", t => t.TiH_TiTID, cascadeDelete: true)
                .Index(t => t.TiH_TiSID)
                .Index(t => t.TiH_TiTID)
                .Index(t => t.TiH_CMPID);
            
            CreateTable(
                "dbo.TicketHeaderAssistants",
                c => new
                    {
                        TiA_AsSID = c.String(nullable: false, maxLength: 128),
                        TiA_TiHID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TiA_AsSID, t.TiA_TiHID })
                .ForeignKey("dbo.Assistants", t => t.TiA_AsSID)
                .ForeignKey("dbo.TicketHeaders", t => t.TiA_TiHID, cascadeDelete: true)
                .Index(t => t.TiA_AsSID)
                .Index(t => t.TiA_TiHID);
            
            CreateTable(
                "dbo.TicketHistories",
                c => new
                    {
                        ThS_ID = c.Int(nullable: false, identity: true),
                        ThS_TiHID = c.Int(nullable: false),
                        ThS_LP = c.Int(nullable: false),
                        ThS_Description = c.String(),
                    })
                .PrimaryKey(t => t.ThS_ID)
                .ForeignKey("dbo.TicketHeaders", t => t.ThS_TiHID, cascadeDelete: true)
                .Index(t => t.ThS_TiHID);
            
            CreateTable(
                "dbo.TicketPositions",
                c => new
                    {
                        TiP_ID = c.Int(nullable: false, identity: true),
                        TiP_TiHID = c.Int(nullable: false),
                        TiP_LP = c.Int(nullable: false),
                        TiP_Content = c.String(),
                        TiP_Date = c.DateTime(nullable: false),
                        TiP_CUSID = c.String(maxLength: 128),
                        TiP_ASSID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.TiP_ID)
                .ForeignKey("dbo.Assistants", t => t.TiP_ASSID)
                .ForeignKey("dbo.Customers", t => t.TiP_CUSID)
                .ForeignKey("dbo.TicketHeaders", t => t.TiP_TiHID, cascadeDelete: true)
                .Index(t => t.TiP_TiHID)
                .Index(t => t.TiP_CUSID)
                .Index(t => t.TiP_ASSID);
            
            CreateTable(
                "dbo.TicketStates",
                c => new
                    {
                        TiS_ID = c.Int(nullable: false, identity: true),
                        TiS_Name = c.String(maxLength: 50),
                        TiS_Description = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.TiS_ID);
            
            CreateTable(
                "dbo.TicketTypes",
                c => new
                    {
                        TiT_ID = c.Int(nullable: false, identity: true),
                        TiT_Name = c.String(maxLength: 50),
                        TiT_Description = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.TiT_ID);
            
            CreateTable(
                "dbo.TicketWorkLogs",
                c => new
                    {
                        TwL_ID = c.Int(nullable: false, identity: true),
                        TwL_TIHID = c.Int(),
                        TwL_ASSID = c.String(maxLength: 128),
                        TwL_StartDate = c.DateTime(nullable: false),
                        TwL_EndDate = c.DateTime(nullable: false),
                        TwL_SpendMinutes = c.Int(nullable: false),
                        TwL_Description = c.String(),
                        TwL_PublicDescription = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TwL_ID)
                .ForeignKey("dbo.Assistants", t => t.TwL_ASSID)
                .ForeignKey("dbo.TicketHeaders", t => t.TwL_TIHID)
                .Index(t => t.TwL_TIHID)
                .Index(t => t.TwL_ASSID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.Assistants",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AsS_Name = c.String(),
                        AsS_Surname = c.String(),
                        AsS_Image = c.String(),
                        AsS_Email = c.String(),
                        AsS_Login = c.String(),
                        AsS_PhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CuS_CMPID = c.Int(nullable: false),
                        CuS_Name = c.String(),
                        CuS_Surname = c.String(),
                        CuS_Image = c.String(),
                        CuS_Email = c.String(),
                        CuS_Login = c.String(),
                        CuS_PhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CuS_CMPID, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.CuS_CMPID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "CuS_CMPID", "dbo.Companies");
            DropForeignKey("dbo.Customers", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Assistants", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.OperatorCustomizationDefinitions", "OcD_OPEID", "dbo.AspNetUsers");
            DropForeignKey("dbo.TicketHeaderCustomers", "TiC_TiHID", "dbo.TicketHeaders");
            DropForeignKey("dbo.TicketWorkLogs", "TwL_TIHID", "dbo.TicketHeaders");
            DropForeignKey("dbo.TicketWorkLogs", "TwL_ASSID", "dbo.Assistants");
            DropForeignKey("dbo.TicketHeaders", "TiH_TiTID", "dbo.TicketTypes");
            DropForeignKey("dbo.TicketHeaders", "TiH_TiSID", "dbo.TicketStates");
            DropForeignKey("dbo.TicketPositions", "TiP_TiHID", "dbo.TicketHeaders");
            DropForeignKey("dbo.TicketPositions", "TiP_CUSID", "dbo.Customers");
            DropForeignKey("dbo.TicketPositions", "TiP_ASSID", "dbo.Assistants");
            DropForeignKey("dbo.TicketHistories", "ThS_TiHID", "dbo.TicketHeaders");
            DropForeignKey("dbo.TicketHeaderAssistants", "TiA_TiHID", "dbo.TicketHeaders");
            DropForeignKey("dbo.TicketHeaderAssistants", "TiA_AsSID", "dbo.Assistants");
            DropForeignKey("dbo.TicketHeaders", "TiH_CMPID", "dbo.Companies");
            DropForeignKey("dbo.TicketHeaderCustomers", "TiC_CuSID", "dbo.Customers");
            DropForeignKey("dbo.OperatorCustomizationDefinitions", "OcD_CSDID", "dbo.CustomizationDefinitions");
            DropIndex("dbo.Customers", new[] { "CuS_CMPID" });
            DropIndex("dbo.Customers", new[] { "Id" });
            DropIndex("dbo.Assistants", new[] { "Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.TicketWorkLogs", new[] { "TwL_ASSID" });
            DropIndex("dbo.TicketWorkLogs", new[] { "TwL_TIHID" });
            DropIndex("dbo.TicketPositions", new[] { "TiP_ASSID" });
            DropIndex("dbo.TicketPositions", new[] { "TiP_CUSID" });
            DropIndex("dbo.TicketPositions", new[] { "TiP_TiHID" });
            DropIndex("dbo.TicketHistories", new[] { "ThS_TiHID" });
            DropIndex("dbo.TicketHeaderAssistants", new[] { "TiA_TiHID" });
            DropIndex("dbo.TicketHeaderAssistants", new[] { "TiA_AsSID" });
            DropIndex("dbo.TicketHeaders", new[] { "TiH_CMPID" });
            DropIndex("dbo.TicketHeaders", new[] { "TiH_TiTID" });
            DropIndex("dbo.TicketHeaders", new[] { "TiH_TiSID" });
            DropIndex("dbo.TicketHeaderCustomers", new[] { "TiC_TiHID" });
            DropIndex("dbo.TicketHeaderCustomers", new[] { "TiC_CuSID" });
            DropIndex("dbo.Companies", new[] { "CmP_Name" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.OperatorCustomizationDefinitions", new[] { "OcD_CSDID" });
            DropIndex("dbo.OperatorCustomizationDefinitions", new[] { "OcD_OPEID" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropTable("dbo.Customers");
            DropTable("dbo.Assistants");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.TicketWorkLogs");
            DropTable("dbo.TicketTypes");
            DropTable("dbo.TicketStates");
            DropTable("dbo.TicketPositions");
            DropTable("dbo.TicketHistories");
            DropTable("dbo.TicketHeaderAssistants");
            DropTable("dbo.TicketHeaders");
            DropTable("dbo.TicketHeaderCustomers");
            DropTable("dbo.Companies");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.CustomizationDefinitions");
            DropTable("dbo.OperatorCustomizationDefinitions");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
        }
    }
}
