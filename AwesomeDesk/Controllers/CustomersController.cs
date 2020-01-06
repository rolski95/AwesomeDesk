using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AwesomeDesk.Models;
using Microsoft.AspNet.Identity;

namespace AwesomeDesk.Controllers
{
    public class CustomersController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Assistant")]
        public ActionResult List()
        {
            var model = (from cus in db.Customers
                         join cmp in db.Companies on cus.CuS_CMPID equals cmp.CmP_ID
                         select new CustomerListViewModel
                         {
                             CmP_Name = cmp.CmP_Name,
                             CuS_Email = cus.CuS_Email,
                             CuS_Name = cus.CuS_Name,
                             CuS_Surname = cus.CuS_Surname,
                             CuS_PhoneNumber = cus.CuS_PhoneNumber
                         }
                       ).ToList();
            return View(model);
        }
        [Authorize(Roles = "Assistant")]

        public ActionResult Create()
        {
            //this.ViewData["CmP"] = new SelectList(db.Companies.ToList(), "CmP_ID", "CmP_Name");
        
            var model = new CustomerCreateViewModel();
            model.Companies = db.Companies.ToList();
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Assistant")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(new Customer
                {
                    CuS_Email=model.CuS_Email,
                    CuS_Login=model.CuS_Email,
                    CuS_Name=model.CuS_Name,
                    CuS_Surname=model.CuS_Surname,
                    CuS_PhoneNumber=model.CuS_PhoneNumber,
                    CuS_CMPID= model.CuS_CMPID
                }

                );   
            
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View();
        }

     
    }
}