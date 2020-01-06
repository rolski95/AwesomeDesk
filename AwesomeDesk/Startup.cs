using AwesomeDesk.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System.Linq;

[assembly: OwinStartupAttribute(typeof(AwesomeDesk.Startup))]
namespace AwesomeDesk
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            RO_Seed();
            CreateRolesandUsers();
          
        }
        private void RO_Seed()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                if (context.TicketStates.ToList().Count() == 0)
                {
                    context.TicketStates.Add(new TicketState { TiS_Name = "Otwarte", TiS_Description = "Zgłoszenie jest otwarte" });
                    context.TicketTypes.Add(new TicketType { TiT_Name = "Zgłoszenie", TiT_Description = "Standardowe zgłoszenie" });
                
                    context.Companies.Add(new Company { CmP_Name = "Company#1" });
                    context.Companies.Add(new Company { CmP_Name = "Company#2" });

                    context.SaveChanges();
                }
            }





        }


        private void CreateRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            UserManager<Operator> UserManager = new UserManager<Operator>(new UserStore<Operator>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Adminstrator"))
            {

                // first we create Admin rool   
                var role = new IdentityRole
                {
                    Name = "Adminstrator"
                };
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                  

                var user = new Operator
                {
                    UserName = "admin@o2.pl",
                    Email = "admin@o2.pl",
                   
                };
                var chkUser = UserManager.Create(user, "Qwerty!12345");
                
                
                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Adminstrator");

                }
            }

          
            if (!roleManager.RoleExists("Customer"))
            {
                var role = new IdentityRole
                {
                    Name = "Customer"
                };
                roleManager.Create(role);
                AddCustomer("Com1Cus1@example.com",1, UserManager);
                AddCustomer("Com1Cus2@example.com", 1, UserManager);
                AddCustomer("Com2Cus1@example.com", 2, UserManager);
                AddCustomer("Com2Cus2@example.com", 2,UserManager);


            }

            if (!roleManager.RoleExists("Assistant"))
            {
                var role = new IdentityRole
                {
                    Name = "Assistant"
                };
                roleManager.Create(role);
                AddAssistant("Ass1@example.com", UserManager);
                AddAssistant("Ass2@example.com", UserManager);
                AddAssistant("Ass3@example.com", UserManager);
                AddAssistant("Ass4@example.com", UserManager);

            }

        }
        private void AddCustomer(string Email ,int CompanyID, UserManager<Operator> UserManager)
        {

            var user = new Customer
            {
                UserName = Email,
                Email = Email,
                CuS_CMPID = CompanyID

            };
            var chkUser = UserManager.Create(user, "Qwerty!12345");

            if (chkUser.Succeeded)
            {
                var result1 = UserManager.AddToRole(user.Id, "Customer");
            }


        }
        private void AddAssistant(string Email, UserManager<Operator> UserManager)
        {

            var user = new Assistant
            {
                UserName = Email,
                Email = Email,

            };
            var chkUser = UserManager.Create(user, "Qwerty!12345");


            //Add default User to Role Admin   
            if (chkUser.Succeeded)
            {
                var result1 = UserManager.AddToRole(user.Id, "Assistant");

            }

        }

    }
}
