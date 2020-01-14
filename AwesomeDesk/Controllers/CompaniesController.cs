using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AwesomeDesk.Models;

namespace AwesomeDesk.Controllers
{
    public class CompaniesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Companies
        [Authorize(Roles = "Administrator,Assistant")]
        public ActionResult List()
        {
            return View(db.Companies.ToList());
        }
        // GET: Companies/Details/5
        [Authorize(Roles = "Administrator,Assistant")]

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }
        // GET: Companies/Create
        [Authorize(Roles = "Administrator,Assistant")]

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Assistant")]

        public ActionResult Create([Bind(Include = "CmP_ID,CmP_Name,CmP_PhoneNumber,CmP_PageAdress")] Company company)
        {
            if (ModelState.IsValid)
            {
                db.Companies.Add(company);
                db.SaveChanges();
                return RedirectToAction("List");
            }

            return View(company);
        }
        // GET: Companies/Edit/5
        [Authorize(Roles = "Administrator,Assistant")]

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Assistant")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CmP_ID,CmP_Name,CmP_PhoneNumber,CmP_PageAdress")] Company company)
        {
            if (ModelState.IsValid)
            {
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(company);
        }

        // GET: Companies/Delete/5
        [Authorize(Roles = "Administrator,Assistant")]

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [Authorize(Roles = "Administrator,Assistant")]

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
