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
using Utilidades;
using System.Web.Security;

namespace CamajanSport.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        public ActionResult Dashboard()
        {
            
            return View();
        }


        #region Deportes

        public ActionResult MantDeportes() 
        {
            return View("MantDeportes");
        }
        #endregion
    }
}