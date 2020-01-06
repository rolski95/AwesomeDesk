using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AwesomeDesk.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles = "Administrator,Assistant,Customer")]

        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Administrator,Assistant,Customer")]

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Authorize(Roles = "Administrator,Assistant,Customer")]

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}