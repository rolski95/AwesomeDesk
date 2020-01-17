namespace AwesomeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class miggg112 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketHeaders", "TiH_CMPID", c => c.Int(nullable: false));
            CreateIndex("dbo.TicketHeaders", "TiH_CMPID");
            AddForeignKey("dbo.TicketHeaders", "TiH_CMPID", "dbo.Companies", "CmP_ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TicketHeaders", "TiH_CMPID", "dbo.Companies");
            DropIndex("dbo.TicketHeaders", new[] { "TiH_CMPID" });
            DropColumn("dbo.TicketHeaders", "TiH_CMPID");
        }
    }
}
