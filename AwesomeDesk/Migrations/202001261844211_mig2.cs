namespace AwesomeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TicketPositions", "TiP_Content", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TicketPositions", "TiP_Content", c => c.String(maxLength: 3000));
        }
    }
}
