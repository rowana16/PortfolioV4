using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CFPortfolio.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
           
            return View();
        }

        public ActionResult Blog()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Portfolio()
        {
            
            return View();
        }

        public ActionResult Skills()
        {
            return View();
        }

        public ActionResult Week1()
        {
            return View();
        }

        public ActionResult Week1exercises()
        {
            return View();
        }
    }
}