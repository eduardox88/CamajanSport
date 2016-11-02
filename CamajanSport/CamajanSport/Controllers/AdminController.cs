using CamajanSport.App_Start;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Formatting;
using CamajanSport.BOL;

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


        #region Deportes

        [SessionHandle]
        public ActionResult MantDeportes() 
        {
            return View("MantDeportes");
        }
        #endregion
        [SessionHandle]
        public ActionResult MantUsuarios()
        {
            return View("MantUsuarios");
        }
    }
}