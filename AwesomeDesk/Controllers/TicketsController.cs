﻿using System;
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
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Customers
        [Authorize(Roles = "Customer,Assistant")]
        public ActionResult List()
        {
            var userid = User.Identity.GetUserId();
            if (User.IsInRole("Customer"))
            {
                var company = db.Customers.Where(x => x.Id == userid).FirstOrDefault().CuS_CMPID;
                var model = (from TiH in db.TicketHeaders

                             join TiP in db.TicketPositions on TiH.TiH_ID equals TiP.TiP_TiHID
                             join tic in db.TicketHeaderCustomers on TiH.TiH_ID equals tic.TiC_TiHID
                             join cus in db.Customers on tic.TiC_CuSID equals cus.Id


                             //join TiA in db.TicketHeaderAssistants on TiH.TiH_ID equals TiA.TiA_TiHID into _TiA
                             //from TiA in _TiA.DefaultIfEmpty()
                             //join AsS in db.Assistants on TiA.TiA_AsSID equals AsS.Id into _AsS
                             //from AsS in _AsS.DefaultIfEmpty()
                             join TiS in db.TicketStates on TiH.TiH_TiSID equals TiS.TiS_ID
                             where cus.CuS_CMPID == company && TiP.TiP_LP == 1
                             select new CustomerListTicketViewModel
                             {
                                 TiH_Date = TiP.TiP_Date,
                                 TiH_Subject = TiH.TiH_Subject,
                                 TiS_Name = TiS.TiS_Name,
                                 Assistants = (from AsS in db.Assistants
                                               join tia in db.TicketHeaderAssistants on AsS.Id equals tia.TiA_AsSID
                                               where tia.TiA_TiHID == TiH.TiH_ID
                                               select AsS.UserName
                                               ).ToList(),
                                 TiH_ID = TiH.TiH_ID,
                                 TiP_Content = TiP.TiP_Content

                             }).ToList();
                string sql = model.ToString();
                return View("ListCustomers", model);
            }
            else
            {
                var model = (from TiH in db.TicketHeaders

                             join TiP in db.TicketPositions on TiH.TiH_ID equals TiP.TiP_TiHID

                             join TiS in db.TicketStates on TiH.TiH_TiSID equals TiS.TiS_ID
                             where TiP.TiP_LP == 1
                             select new AssistantListTicketViewModel
                             {
                                 TiH_Date = TiP.TiP_Date,
                                 TiH_Subject = TiH.TiH_Subject,
                                 TiS_Name = TiS.TiS_Name,
                                 Assistants = (from AsS in db.Assistants
                                               join tia in db.TicketHeaderAssistants on AsS.Id equals tia.TiA_AsSID
                                               where tia.TiA_TiHID == TiH.TiH_ID
                                               select AsS.UserName
                                               ).ToList(),
                                 TiH_ID = TiH.TiH_ID,
                                 TiP_Content = TiP.TiP_Content,
                                 CmP_Name = "",
                                 Customers = (from cus in db.Customers
                                              join tic in db.TicketHeaderCustomers on cus.Id equals tic.TiC_CuSID
                                              where tic.TiC_TiHID == TiH.TiH_ID
                                              select cus.UserName
                                               ).ToList()

                             }).ToList();
                string sql = model.ToString();
                return View("ListAssistants", model);
            }
        }

        [Authorize(Roles = "Customer,Assistant")]
        public ActionResult Create()
        {
            if (User.IsInRole("Customer"))
            {
                return View("CreateCustomers");
            }
            else
            {
                return View("CreateAssistants");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        [ActionName("CreateCustomer")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerCreateTicketViewModel model)
        {
            if (ModelState.IsValid)
            {
                TicketHeader tih = db.TicketHeaders.Add(new TicketHeader
                {
                    TiH_Subject = model.TiH_Subject,
                    TiH_TiTID = 1,
                    TiH_TiSID = 1
                });
                TicketPosition tip = db.TicketPositions.Add(new TicketPosition
                {
                    TiP_Date = DateTime.Now,
                    TiP_LP = 1,
                    TiP_TiHID = tih.TiH_ID,
                    TiP_CUSID = User.Identity.GetUserId(),
                    TiP_Content = model.TiP_Content,
                });
                db.TicketHeaderCustomers.Add(new TicketHeaderCustomer
                {
                    TiC_TiHID = tih.TiH_ID,
                    TiC_CuSID = User.Identity.GetUserId()
                });
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View("CreateCustomers");
        }

        [HttpPost]
        [Authorize(Roles = "Assistant")]
        [ActionName("CreateAssistant")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AssistantCreateTicketViewModel model)
        {
            if (ModelState.IsValid)
            {
                TicketHeader tih = db.TicketHeaders.Add(new TicketHeader
                {
                    TiH_Subject = model.TiH_Subject,
                    TiH_TiTID = 1,
                    TiH_TiSID = 1
                });
                TicketPosition tip = db.TicketPositions.Add(new TicketPosition
                {
                    TiP_Date = DateTime.Now,
                    TiP_LP = 1,
                    TiP_TiHID = tih.TiH_ID,
                    TiP_ASSID = User.Identity.GetUserId(),
                    TiP_Content = model.TiP_Content,
                });
                db.TicketHeaderAssistants.Add(new TicketHeaderAssistant
                {
                    TiA_TiHID = tih.TiH_ID,
                    TiA_AsSID = User.Identity.GetUserId()
                });
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View("CreateAssistants");
        }

        [Authorize(Roles = "Customer,Assistant")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Id = id;
            ViewBag.Subject = db.TicketHeaders.Where(x => x.TiH_ID == id).FirstOrDefault().TiH_Subject;
            if (User.IsInRole("Customer"))
            {
                var model = (from tih in db.TicketHeaders
                             join tip in db.TicketPositions on tih.TiH_ID equals tip.TiP_TiHID
                             join ass in db.Assistants on tip.TiP_ASSID equals ass.Id into _ass
                             from ass in _ass.DefaultIfEmpty()
                             join cus in db.Customers on tip.TiP_CUSID equals cus.Id into _cus
                             from cus in _cus.DefaultIfEmpty()
                             join tis in db.TicketStates on tih.TiH_TiSID equals tis.TiS_ID
                             where tih.TiH_ID == id
                             select new CustomerDetailsTicketViewModel
                             {
                                 TiH_ID = tih.TiH_ID,
                                 TiP_LP = tip.TiP_LP,
                                 TiP_Date = tip.TiP_Date,
                                 AssOrCus = (ass.Id == null && cus.Id != null) ? 1 :
                                          (ass.Id != null && cus.Id == null) ? 2 : -1,
                                 TiP_Content = tip.TiP_Content,
                                 OperatorName = (ass.Id == null && cus.Id != null) ? cus.CuS_Name + " " + cus.CuS_Surname :
                                          (ass.Id != null && cus.Id == null) ? ass.AsS_Name + " " + ass.AsS_Surname : "Error",

                             }).ToList();

                var model2 = new CustomerAddResponseViewModel()
                {
                    CustomerDetailsTickets = model
                };
         

                return View("DetailsCustomers", model2);
            }
            else
            {
                var model = (from tih in db.TicketHeaders
                             join tip in db.TicketPositions on tih.TiH_ID equals tip.TiP_TiHID
                             join ass in db.Assistants on tip.TiP_ASSID equals ass.Id into _ass
                             from ass in _ass.DefaultIfEmpty()
                             join cus in db.Customers on tip.TiP_CUSID equals cus.Id into _cus
                             from cus in _cus.DefaultIfEmpty()
                             join tis in db.TicketStates on tih.TiH_TiSID equals tis.TiS_ID
                             where tih.TiH_ID == id
                             select new AssistantDetailsTicketViewModel
                             {
                                 TiH_ID = tih.TiH_ID,
                                 TiP_LP = tip.TiP_LP,
                                 TiP_Date = tip.TiP_Date,
                                 AssOrCus = (ass.Id == null && cus.Id != null) ? 1 :
                                          (ass.Id != null && cus.Id == null) ? 2 : -1,
                                 TiP_Content = tip.TiP_Content,
                                 OperatorName = (ass.Id == null && cus.Id != null) ? cus.CuS_Name + " " + cus.CuS_Surname :
                                          (ass.Id != null && cus.Id == null) ? ass.AsS_Name + " " + ass.AsS_Surname : "Error",

                             }).ToList();
                var model2 = new AssistantAddResponseViewModel()
                {
                    AssistantDetailsTickets = model
                };                
                return View("DetailsAssistants", model2);
            }
        }

        [Authorize(Roles = "Customer,Assistant")]
        [ValidateAntiForgeryToken]
        [ActionName("DetailsCustomer")]
        [HttpPost]
        public ActionResult Details(CustomerAddResponseViewModel modelCustomer)
        {
            try
            {

                CustomerAddResponseViewModel tmp = modelCustomer;
                if (ModelState.IsValid)
                {

                    db.TicketPositions.Add(new TicketPosition
                    {
                        TiP_Date = DateTime.Now,
                        TiP_LP = (from m in tmp.CustomerDetailsTickets
                                  orderby m.TiP_LP descending
                                  select m.TiP_LP).FirstOrDefault() + 1,
                        TiP_TiHID = tmp.CustomerDetailsTickets.FirstOrDefault().TiH_ID,
                        TiP_CUSID = User.Identity.GetUserId(),
                        TiP_Content = tmp.NewPositionContent
                    });
                    db.SaveChanges();
                    return RedirectToAction("Details",new {id=tmp.CustomerDetailsTickets.FirstOrDefault().TiH_ID });

                    // return RedirectToAction("Details", "Tickets",modelCustomer.CustomerDetailsTickets.FirstOrDefault().TiH_ID.ToString());
                }
                return View("DetailsCustomers");

            }
            catch (Exception e)
            {
                string exx = e.ToString();
                return View();
            }
        }

        [Authorize(Roles = "Customer,Assistant")]
        [ActionName("DetailsAssistant")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Details(AssistantAddResponseViewModel modelAssistant)
        {
            AssistantAddResponseViewModel tmp = modelAssistant;
            if (ModelState.IsValid)
            {
                db.TicketPositions.Add(new TicketPosition
                {
                    TiP_Date = DateTime.Now,
                    TiP_LP = (from m in tmp.AssistantDetailsTickets
                              orderby m.TiP_LP descending
                              select m.TiP_LP).FirstOrDefault() + 1,
                    TiP_TiHID = tmp.AssistantDetailsTickets.FirstOrDefault().TiH_ID,
                    TiP_ASSID = User.Identity.GetUserId(),
                    TiP_Content = tmp.NewPositionContent
                });
                db.SaveChanges();
                return RedirectToAction("Details", new { id = tmp.AssistantDetailsTickets.FirstOrDefault().TiH_ID });
            }
            return View("DetailsAssistants");
        }

        public IQueryable<string> GetAssistantsNames(int _TiH_ID)
        {

            IQueryable<string> query = (from TiA in db.TicketHeaderAssistants
                                        join AsS in db.Assistants on TiA.TiA_AsSID equals AsS.Id
                                        where TiA.TiA_TiHID == _TiH_ID
                                        select AsS.AsS_Name + " " + AsS.AsS_Surname
                          );
            return query;
        }

    }
}