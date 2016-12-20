using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CamajanSport.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Expertos() {

            return View();
        }

        public ActionResult Membresias() {

            return View();
        }
    }
}
