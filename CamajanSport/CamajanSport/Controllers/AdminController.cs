using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CamajanSport.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Dashboard()
        {
            return View();
        }

<<<<<<< .mine
        #region Deportes
||||||| .r7
=======
        public ActionResult MantUsuarios() {
>>>>>>> .r15

<<<<<<< .mine
        public ActionResult MantDeportes() 
        {
            return View("MantDeportes");
        }
#endregion
||||||| .r7
=======
            return View("MantUsuarios");
        }
>>>>>>> .r15

    }
}