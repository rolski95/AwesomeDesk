using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AwesomeDesk.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AwesomeDesk.Controllers
{
    public class AssistantsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize(Roles = "Assistant,Administrator")]
        public ActionResult List()
        {

            var model = (from ass in db.Assistants                                

                         select new AssistantListViewModel
                         {                              
                             AsS_Email = ass.AsS_Email,
                             AsS_Name= ass.AsS_Name,
                             AsS_Surname = ass.AsS_Surname,
                             AsS_PhoneNumber = ass.AsS_PhoneNumber,
                             AsS_IsAdmin=ass.Roles.Where(x=>x.RoleId ==db.Roles.Where(z=>z.Name=="Administrator").FirstOrDefault().Id).Count()==0 ? false:true
                         }
                       ).ToList();
            return View(model);
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            var model = new AssistantCreateViewModel();          
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AssistantCreateViewModel model)
        {
            
            UserManager<Assistant> UserManager = new UserManager<Assistant>(new UserStore<Assistant>(db));
            if (ModelState.IsValid)
            {
                var user = new Assistant
                {
                    AsS_Email = model.AsS_Email,
                    AsS_Login = model.AsS_Email,
                    AsS_Name = model.AsS_Name,
                    AsS_Surname = model.AsS_Surname,
                    AsS_PhoneNumber = model.AsS_PhoneNumber                   

                };
                var chkUser = UserManager.Create(user, model.AsS_Password);
                if (chkUser.Succeeded)
                {
                    var result1 = (model.AsS_IsAdmin) ? UserManager.AddToRole(user.Id, "Administrator") : UserManager.AddToRole(user.Id, "Assistant");
                }  
                
               
                return RedirectToAction("List");
            }
            return View(model);
        }

    }
}