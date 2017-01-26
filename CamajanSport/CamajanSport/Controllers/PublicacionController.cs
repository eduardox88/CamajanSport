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
using CamajanSport.Properties;
using System.Web.Security;

namespace CamajanSport.Controllers
{
    [Authorize]
    public class PublicacionController : Controller
    {
        #region Propiedades
        private Token GetAuthToken
        {

            get
            {
                Token token = CookieHandler.GetCookieDecrypted<Token>(Settings.Default.TokenCookie);

                return token;
            }
        }

        private Usuario GetUserDecrypted
        {

            get
            {
                Usuario token = CookieHandler.GetCookieDecrypted<Usuario>(FormsAuthentication.FormsCookieName);

                return token;
            }
        }
        #endregion
        //
        // GET: /Admin/

        public ActionResult MantPublicaciones()
        {
            return View();
        }
        public async Task<JsonResult> GuardarPublicacion(Publicacion publicacion)
        {

            try
            {
                //SE DEBE OBTENER EL ID DE USUARIO DE LA SESION
                publicacion.IdUsuario = GetUserDecrypted.IdUsuario;
                if (publicacion.IdPublicacion > 0)
                {
                    await ApiHelper.PUT<Publicacion>("Publicacion/PutPublicacion", publicacion, GetAuthToken);
                    return Json("La publicación se ha editado exitosamente.");
                }
                else
                {
                    publicacion.FechaIngreso = DateTime.Now;
                    await ApiHelper.POST<Publicacion>("Publicacion/PostPublicacion", publicacion, GetAuthToken);
                    return Json("La publicación se ha guardado exitosamente.");
                }
                
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Ha ocurrido un error al guardar la publicación. Si el problema persiste contacte su administrador.");
            }

        }

        public async Task<JsonResult> CambiarEstatus(int IdPublicacion, int IdEstadoResultado)
        {
            try
            {
                Publicacion pub = new Publicacion();
                pub.IdPublicacion = IdPublicacion;
                pub.IdEstadoResultado = IdEstadoResultado;
                await ApiHelper.POST<Publicacion>("Publicacion/CambiarEstatus", pub, GetAuthToken);
                return Json("El estatus se ha cambiado satisfactoriamente.");
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Ha ocurrido un error al cambiar el estatus de la publicación. Si el problema persiste contacte su administrador.");
            }
        }
        public async Task<JsonResult> GetEstadosResultado_Select()
        {
            try
            {

                var estadosResultado = await ApiHelper.GET_List<SelectAttributes>("Publicacion/GetEstadosResultado_Select", GetAuthToken);
                return Json(estadosResultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Ha ocurrido un error al obtener los estados de la publicación. Si el problema persiste contacte su administrador.");
            }
        }
        
       [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetPublicacionesByFiltro(DateTime? FechaJuego, int IdDeporte, int IdEstadoResultado, string TipoPublicacion) 
        {
            try
            {
                int IdUsuario = 0;
                if (GetUserDecrypted.rol.IdRol == 2/*Camajan*/)
                { 
                    IdUsuario = GetUserDecrypted.IdUsuario;
                }
                var publicaciones = await ApiHelper.GET_List_ByFilter<Publicacion>("Publicacion/GetPublicacionesByFiltro", "FechaJuego=" + ((FechaJuego.HasValue) ? FechaJuego.Value.ToShortDateString() : "") + "&IdDeporte=" + IdDeporte.ToString() + "&IdEstadoResultado=" + IdEstadoResultado.ToString() + "&TipoPublicacion=" + TipoPublicacion+"&IdUsuario="+IdUsuario.ToString(), GetAuthToken);
                
                var json = Json(publicaciones, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = int.MaxValue;
                return json;
            }
            catch (Exception)
            {
                //Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new{Result = "ERROR" ,Message = "Ha ocurrido un error al obtener las publicaciones. Si el problema persiste contacte su administrador."});
            }
        }

    }
}