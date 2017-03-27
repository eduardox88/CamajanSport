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
using CamajanSport.Properties;
using Utilidades;
using System.Web.Security;
using System.Net;

namespace CamajanSport.Controllers
{

    public class NoticiaController : Controller
    {
        //
        // GET: /Admin/
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
        [Authorize]
        public ActionResult MantNoticias()
        {
            return View();
        }

        /// <summary>
        /// Muestra el view ListarMisNoticias que presenta las membresias del usuario con Rol Regular
        /// </summary>
        /// <returns></returns>
        public List<Noticia> ListarMisNoticiasWOAuth()
        {
            if (Request.Params["paymentId"] != null && Request.Params["paymentId"] != "")
            {
                ViewBag.paymentSuccessful = true;
                ViewBag.PaymentID = Request.Params["paymentId"];
                ViewBag.PaymentMade = true;
            }
            else
            {
                ViewBag.PaymentMade = false;
            }
            return new List<Noticia>();
        }
        /// <summary>
        /// Obtiene las membresias del usuario con rol Regular
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<JsonResult> GetMisMembresias()
        {
            var membresias = await ApiHelper.GET_ListById<MembresiaUsuario>("MembresiaUsuarios/GetMembresiasUsuarioById", GetUserDecrypted.IdUsuario, GetAuthToken);
            return Json(membresias, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Obtiene las membresias del usuario con rol Regular
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<JsonResult> GetMembresiasUsuario(int IdUsuario)
        {
            try
            {
                var membresias = await ApiHelper.GET_ListById<MembresiaUsuario>("MembresiaUsuarios/GetMembresiasUsuarioById", IdUsuario, GetAuthToken);
                return Json(new { Result = "OK", Data = membresias });
            }
            catch (Exception)
            {
                return Json(new { Result = "ERROR", Message = "Ha ocurrido un error al obtener las membresías del usuario." });
            }

        }
        /// <summary>
        /// Obtiene las noticias disponibles pero necesita AUTENTICACION
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<JsonResult> GetNoticia(int id)
        {
            var noticia = await ApiHelper.GET_By_ID<Noticia>("Noticia/GetNoticia",id, GetAuthToken);
            return Json(noticia, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Obtiene la noticias por el ID
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<JsonResult> GetNoticias()
        {
            var noticias = await ApiHelper.GET_List<Noticia>("Noticia/GetNoticia", GetAuthToken);
            return Json(noticias, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Obtiene las noticias disponibles SIN NECESIDAD DE AUTENTICARSE
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetNoticiasWOAuth()
        {
           var noticias = await ApiHelper.GET_ListWOAuth<Noticia>("Noticia/GetNoticia");
            return Json(noticias, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public async Task<JsonResult> GuardarNoticia(Noticia noticia)
        {
            try
            {
                //SE OBTIENE EL ID DE USUARIO DE LA SESION
                noticia.IdUsuario = GetUserDecrypted.IdUsuario;
                if (noticia.Id > 0)
                {
                    await ApiHelper.PUT<Noticia>("Noticia/PutNoticia", noticia, GetAuthToken);
                    return Json("La noticia se ha editado exitosamente.");
                }
                else
                {
                    noticia.FechaIngreso = DateTime.Now;
                    await ApiHelper.POST<Noticia>("Noticia/PostNoticia",noticia, GetAuthToken);
                    return Json("La noticia se ha guardado exitosamente.");
                }

            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Ha ocurrido un error al guardar la noticia. Si el problema persiste contacte su administrador.");
            }

        }

        public async Task<Noticia> ObtieneNoticia(int id)
        {
            var noticia = await ApiHelper.GET_By_ID<Noticia>("Noticia/GetNoticia", id, GetAuthToken);
            return noticia;
        }
    }
}