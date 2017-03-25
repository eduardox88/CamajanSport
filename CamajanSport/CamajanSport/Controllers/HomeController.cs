using CamajanSport.BOL;
using CamajanSport.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Utilidades;
using System.Net;
using System.Net.Http;

namespace CamajanSport.Controllers
{
    public class HomeController : Controller
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
        // GET: /Home/

        public ActionResult Index()
        {

            return View();
        }

        public async Task<ActionResult> Picks()
        {
            var cantPremiums = await ApiHelper.GET("Publicacion/GetCantPremiumPendiente", GetAuthToken);

            ViewBag.MenuDinamico = await ApiHelper.GET_List<Deporte>("Deporte/Getdeportes", GetAuthToken);

            ViewBag.Expertos = await ApiHelper.GET_List<Usuario>("Usuario/GetusuariosCamajanes", GetAuthToken);

            if (cantPremiums.IsSuccessStatusCode)
            {
                ViewBag.CantPremium = await cantPremiums.Content.ReadAsAsync<int>();
            }

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> PartialPublicaciones(int CodDeporte, int idUsuario = 0)
        {

            var publicaciones = await ApiHelper.GET_List_ByFilter<Publicacion>("Publicacion/GetPublicacionesByFiltro", "FechaJuego= " + "&IdDeporte=" + CodDeporte + "&IdEstadoResultado=0" + "&TipoPublicacion= &IdUsuario=" + idUsuario.ToString(), GetAuthToken);

            return PartialView("~/Views/Shared/_PartialPublicaciones.cshtml", publicaciones);
        }

        public async Task<ActionResult> Expertos()
        {

            List<Usuario> expertos = await ApiHelper.GET_List<Usuario>("Usuario/GetusuariosCamajanes", GetAuthToken);

            return View(expertos);
        }

        public ActionResult Membresias()
        {

            if (Request.Params["error"] != null && Convert.ToBoolean(Request.Params["error"]))
            {
                ViewBag.Error = true;
            }
            if (Request.Params["token"] != null)
            {
                ViewBag.PayPalCancel = true;
            }
            else
            {
                ViewBag.PayPalCancel = false;
            }

            return View();
        }

        public ActionResult Contactos()
        {
            return View("Contactos");
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult EnviarComentario(string Nombre, string Telefono, string CorreoElectronico, string Comentario)
        {

            try
            {
                MailHandler.SendEmailToCamajanSport("Formulario de Contacto", CorreoElectronico, "Usted ha recibido el siguiente comentario:<br/><br/>Nombre:" + Nombre + "<br/>Correo Electrónico:" + CorreoElectronico + "<br/><br/>Comentario:<br/><br/>" + Comentario);
                return Json(new { Result = "OK", Type = "success", Message = "Su comentario se ha enviado satisfactoriamente." });
            }
            catch (Exception)
            {
                return Json(new { Result = "ERROR", Type = "error", Message = "Ha ocurrido un error al enviar su comentario, si el problema persiste contacte el administrador." });

            }

        }
    }
}
