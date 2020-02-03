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
    public class Tickets : Controller
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
                                 TiP_Content = TiP.TiP_Content,
                                 MailState = (db.TicketPositions.Where(x => x.TiP_TiHID == TiH.TiH_ID).OrderByDescending(y => y.TiP_LP).FirstOrDefault().TiP_ASSID != null ? 1 :
                                            db.TicketPositions.Where(x => x.TiP_TiHID == TiH.TiH_ID).OrderByDescending(y => y.TiP_LP).FirstOrDefault().TiP_CUSID != null ? 2 : -1)

                             }).ToList();
                string sql = model.ToString();
                return View("ListCustomers", model);
            }
            else
            {
                var model = (from TiH in db.TicketHeaders

                             join TiP in db.TicketPositions on TiH.TiH_ID equals TiP.TiP_TiHID

                             join TiS in db.TicketStates on TiH.TiH_TiSID equals TiS.TiS_ID
                             join CmP in db.Companies on TiH.TiH_CMPID equals CmP.CmP_ID
                             where TiP.TiP_LP == 1
                             orderby TiH.TiH_ID descending
                             select new AssistantListTicketViewModel
                             {
                                 TiH_Date = TiP.TiP_Date,
                                 TiH_Subject = TiH.TiH_Subject,
                                 TiS_Name = TiS.TiS_Name,
                                 Assistants = (from AsS in db.Assistants
                                               join tia in db.TicketHeaderAssistants on AsS.Id equals tia.TiA_AsSID
                                               where tia.TiA_TiHID == TiH.TiH_ID
                                               select AsS.AsS_Name + " " + AsS.AsS_Surname
                                               ).ToList(),
                                 TiH_ID = TiH.TiH_ID,
                                 TiP_Content = TiP.TiP_Content,
                                 CmP_Name = CmP.CmP_Name,
                                 Customers = (from cus in db.Customers
                                              join tic in db.TicketHeaderCustomers on cus.Id equals tic.TiC_CuSID
                                              where tic.TiC_TiHID == TiH.TiH_ID
                                              select cus.CuS_Name + " " + cus.CuS_Surname

                                               ).ToList(),
                                 TicketStates = db.TicketStates.ToList(),
                                 MailState = (db.TicketPositions.Where(x => x.TiP_TiHID == TiH.TiH_ID).OrderByDescending(y => y.TiP_LP).FirstOrDefault().TiP_ASSID != null ? 1 :
                                            db.TicketPositions.Where(x => x.TiP_TiHID == TiH.TiH_ID).OrderByDescending(y => y.TiP_LP).FirstOrDefault().TiP_CUSID != null ? 2 : -1)

                             }).ToList();


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
                var model = new AssistantCreateTicketViewModel();
                model.Assistants = db.Assistants.ToList();
                model.Companies = db.Companies.ToList();
                return View("CreateAssistants", model);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        [ActionName("CreateCustomer")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerCreateTicketViewModel model)
        {

            var userid = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                TicketHeader tih = db.TicketHeaders.Add(new TicketHeader
                {
                    TiH_Subject = model.TiH_Subject,
                    TiH_CMPID = db.Customers.Where(x => x.Id == userid).FirstOrDefault().CuS_CMPID,
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
                    TiH_TiSID = 1,
                    TiH_CMPID = model.TiH_CMPID

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
                    TiA_AsSID = model.TiP_ASSID,
                });
                db.SaveChanges();
                return RedirectToAction("List");
            }
            model.Assistants = db.Assistants.ToList();
            model.Companies = db.Companies.ToList();

            return View("CreateAssistants", model);
        }

        [Authorize(Roles = "Customer,Assistant")]
        public ActionResult Details(int? id)
        {
            var userid = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Id = id;
            ViewBag.Subject = db.TicketHeaders.Where(x => x.TiH_ID == id).FirstOrDefault().TiH_Subject;
            if (User.IsInRole("Customer"))
            {
                if (db.TicketHeaders.Where(x => x.TiH_ID == id).FirstOrDefault().TiH_CMPID != db.Customers.Where(y => y.Id == userid).FirstOrDefault().CuS_CMPID) //blokada przed próbą podejrzenia  ze strony "innych" klientów
                {
                    TempData["Error"] = "Nie masz uprawnień do przeglądania wybranego zgłoszenia.";
                    return RedirectToAction("List");

                }
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
                    AssistantDetailsTickets = model,
                    TicketWorkLog = new TicketWorkLog
                    {
                        TwL_ASSID = userid,
                        TwL_TIHID = id,
                        TwL_StartDate = DateTime.Now,
                        TwL_EndDate = DateTime.Now
                    }

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
                    return RedirectToAction("Details", new { id = tmp.CustomerDetailsTickets.FirstOrDefault().TiH_ID });

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
            var userid = User.Identity.GetUserId();
            var tihid = tmp.AssistantDetailsTickets.FirstOrDefault().TiH_ID;
            int counter = db.TicketHeaderAssistants.Where(x => x.TiA_TiHID == tihid && x.TiA_AsSID == userid).Count();

            if (counter == 0) //blokada przed próbą odpowiedzi ze strony niepodppiętych asystentów
            {
                TempData["Error"] = "Nie masz udzielić odpowiedzi na zgłoszenie, ponieważ nie jesteś do niego przypisany!";
                return RedirectToAction("List");
            }

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
            ViewBag.Id = tihid;
            ViewBag.Subject = db.TicketHeaders.Where(x => x.TiH_ID == tihid).FirstOrDefault().TiH_Subject;
            return View("DetailsAssistants", tmp);
        }


        [Authorize(Roles = "Assistant")]
        public ActionResult AssignToYourself(int? id)
        {
            var assID = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (db.TicketHeaderAssistants.Where(x => x.TiA_AsSID == assID && x.TiA_TiHID == (int)id).Count() == 0)
            {
                db.TicketHeaderAssistants.Add(new TicketHeaderAssistant
                {
                    TiA_TiHID = (int)id,
                    TiA_AsSID = assID

                });
                db.SaveChanges();
                TempData["Success"] = "Pomyślnie dodano cię do zgłoszenia";
                return RedirectToAction("List");
            }
            else
            {
                TempData["Error"] = "Już jesteś przydzielony do tego zgłoszenia!";
                return RedirectToAction("List");
            }



        }


        [Authorize(Roles = "Assistant")]
        public ActionResult ChangeState(int? id, int? idstate)
        {
            var assID = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketHeader tih = db.TicketHeaders.Where(x => x.TiH_ID == id).FirstOrDefault();
            tih.TiH_TiSID = (int)idstate;
            db.SaveChanges();
            TempData["Success"] = "Pomyślnie zmieniono status zgłoszenia.";
            return RedirectToAction("List");
        }


        public ActionResult AddWorkTimeLog(int? id)
        {
            var _twlModel = new TicketWorkLogViewModel {

                TwL_TIHID = id,
                TwL_StartDate = DateTime.Now,
                TwL_EndDate = DateTime.Now
                
            };
          
            return PartialView("AddWorkTimeLog", _twlModel);
        }
        [HttpPost]
        public ActionResult AddWorkTimeLog(TicketWorkLogViewModel model)
        {
            var userid = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                db.TicketWorkLogs.Add(new TicketWorkLog
                {
                    TwL_ASSID = userid,
                    TwL_TIHID = model.TwL_TIHID,
                    TwL_StartDate = model.TwL_StartDate,
                    TwL_EndDate = model.TwL_EndDate.Date,
                    TwL_SpendMinutes = (model.TwL_SpendHours * 60) + model.TwL_SpendMinutes,
                    TwL_Description = model.TwL_Description == null ? "": model.TwL_Description,
                    TwL_PublicDescription = model.TwL_PublicDescription


                }) ;
                db.SaveChanges();
                TempData["Success"] = "Pomyśłnie dodano wpis w dzienniku pracy";
                return RedirectToAction("Details",new { id = model.TwL_TIHID });
            }
            TempData["Error"] ="Przy próbie dodania dziennika pracy wystąpły błędy:<p>"+  string.Join("<p>", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage +"</p>"));
            return RedirectToAction("Details", new { id = model.TwL_TIHID });


        }



        public ActionResult ListWorkTime(int? id)
        {
            var model = (from twl in db.TicketWorkLogs

                         join ass in db.Assistants on twl.TwL_ASSID equals ass.Id


                         where twl.TwL_TIHID == id
                         select new ListWorkLogViewModel
                         {
                             TwL_TIHID = id,
                             TwL_StartDate = twl.TwL_StartDate,
                             TwL_EndDate = twl.TwL_EndDate,
                             TwL_Description = twl.TwL_Description,
                             TwL_PublicDescription = twl.TwL_PublicDescription,
                             TwL_SpendHours = (twl.TwL_SpendMinutes - (twl.TwL_SpendMinutes % 60)) / 60,
                             TwL_SpendMinutes= twl.TwL_SpendMinutes % 60,
                             TwL_ID=twl.TwL_ID,
                             Asystent= ass.AsS_Name + " " + ass.AsS_Surname

                         }).ToList();
            return PartialView("ListWorkTime", model);
        }





        public ActionResult Changelog()
        {

            return View("Changelog");
        }
        public ActionResult Functional()
        {
        
                return View("Functional");
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