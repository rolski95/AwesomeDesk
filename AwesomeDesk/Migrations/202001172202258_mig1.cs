namespace AwesomeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.TicketHeaders", name: "TiH_TiTID", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.TicketHeaders", name: "TiH_TiSID", newName: "TiH_TiTID");
            RenameColumn(table: "dbo.TicketHeaders", name: "__mig_tmp__0", newName: "TiH_TiSID");
            RenameIndex(table: "dbo.TicketHeaders", name: "IX_TiH_TiTID", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.TicketHeaders", name: "IX_TiH_TiSID", newName: "IX_TiH_TiTID");
            RenameIndex(table: "dbo.TicketHeaders", name: "__mig_tmp__0", newName: "IX_TiH_TiSID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.TicketHeaders", name: "IX_TiH_TiSID", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.TicketHeaders", name: "IX_TiH_TiTID", newName: "IX_TiH_TiSID");
            RenameIndex(table: "dbo.TicketHeaders", name: "__mig_tmp__0", newName: "IX_TiH_TiTID");
            RenameColumn(table: "dbo.TicketHeaders", name: "TiH_TiSID", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.TicketHeaders", name: "TiH_TiTID", newName: "TiH_TiSID");
            RenameColumn(table: "dbo.TicketHeaders", name: "__mig_tmp__0", newName: "TiH_TiTID");
        }
    }
}
