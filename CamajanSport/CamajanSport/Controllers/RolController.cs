using CamajanSport.App_Start;
using CamajanSport.BOL;
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
    public class RolController : Controller
    {
        [SessionHandle]
        public async Task<JsonResult> GetRol_Select() {

            try
            {
                var lista = await ApiHelper.GET_List<SelectAttributes>("Rol/GetRoles_Select", (Session["Token"] as Token));

                return Json(lista, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Ha ocurrido un error al momento de obtener el listado de roles para el select, si el problema persiste contacte al administrador");
            }
        }
    }
}
