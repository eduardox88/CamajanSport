using CamajanSport.BOL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Utilidades;

namespace CamajanSport.Controllers
{
    public class PaisController : Controller
    {
        private Token token = CookieHandler.GetDecryptToken();
        public async Task<JsonResult> GetPaises_Select() {

            try
            {
                var lista = await ApiHelper.GET_List<SelectAttributes>("Pais/GetPaises_Select", token);

                return Json(lista, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Ha ocurrido un error al momento de obtener el listado de paises para el select, si el problema persiste contacte al administrador");
            }
        }
    }
}
