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
using Utilidades;
using System.Net;

namespace CamajanSport.Controllers
{
    public class PublicacionController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult MantPublicaciones()
        {
            return View();
        }
        [SessionHandle]
        public async Task<JsonResult> GuardarPublicacion(Publicacion publicacion)
        {

            try
            {
                //SE DEBE OBTENER EL ID DE USUARIO DE LA SESION
                publicacion.IdUsuario = 5;
                if (publicacion.IdPublicacion > 0)
                {
                    await ApiHelper.PUT<Publicacion>("Publicacion/PutPublicacion", publicacion, (Token)Session["Token"]);
                    return Json("La publicación se ha editado exitosamente.");
                }
                else
                {
                    publicacion.FechaIngreso = DateTime.Now;
                    await ApiHelper.POST<Publicacion>("Publicacion/PostPublicacion", publicacion, (Token)Session["Token"]);
                    return Json("La publicación se ha guardado exitosamente.");
                }
                
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Ha ocurrido un error al guardar la publicación. Si el problema persiste contacte su administrador.");
            }

        }

        [SessionHandle]
        [HttpPost]
        public async Task<JsonResult> GetPublicaciones() 
        {
            try
            {
                var publicaciones = await ApiHelper.GET_List<Publicacion>("Publicacion/GetPublicaciones", (Token)Session["Token"]);
                return Json(publicaciones, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Ha ocurrido un error al obtener las publicaciones. Si el problema persiste contacte su administrador.");
            }
        }

    }
}