using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace AwesomeDesk.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.


    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
            : base("Website")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<Assistant>().ToTable("Assistants");

        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<AwesomeDesk.Models.Company> Companies { get; set; }

        public System.Data.Entity.DbSet<AwesomeDesk.Models.TicketHeader> TicketHeaders { get; set; }
        public DbSet<TicketPosition> TicketPositions { get; set; }

        public DbSet<TicketState> TicketStates { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<Operator> Operators { get; set; }
        public DbSet<Assistant> Assistants { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<TicketHeaderAssistant> TicketHeaderAssistants { get; set; }
        public DbSet<TicketHeaderCustomer> TicketHeaderCustomers { get; set; }

        public System.Data.Entity.DbSet<AwesomeDesk.Models.TicketWorkLog> TicketWorkLogs { get; set; }
    }

}
