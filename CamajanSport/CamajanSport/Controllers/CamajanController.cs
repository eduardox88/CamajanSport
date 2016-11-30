using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CamajanSport.Controllers
{
    [Authorize]
    public class CamajanController : Controller
    {
        //
        // GET: /Camajan/
        
        public ActionResult Index()
        {
            return View();
        }

    }
}
