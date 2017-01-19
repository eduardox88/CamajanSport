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

    public class MembresiaController : Controller
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
        public ActionResult MantMembresias()
        {
            return View();  
        }

        [Authorize]
        public ActionResult ListarMisMembresias()
        {
            if (Request.Params["paymentId"] != null && Request.Params["paymentId"] != "")
            {
                ViewBag.paymentSuccessful = true;
                ViewBag.PaymentID = Request.Params["paymentId"];
            }
            return View();
        }
        [Authorize]
        public async Task<JsonResult> GetMisMembresias()
        {
            var membresias = await ApiHelper.GET_ListById<MembresiaUsuario>("MembresiaUsuarios/GetMembresiasUsuarioById", GetUserDecrypted.IdUsuario, GetAuthToken);
            return Json(membresias, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public async Task<JsonResult> GetMembresias() 
        {
            var membresias = await ApiHelper.GET_List<Membresia>("Membresia/GetMembresias", GetAuthToken);
            return Json(membresias, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetMembresiasWOAuth()
        {
            var membresias = await ApiHelper.GET_ListWOAuth<Membresia>("Membresia/GetMembresias");
            return Json(membresias, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public async Task<JsonResult> GuardarMembresia(Membresia membresia)
        {
            try
            {
                //SE DEBE OBTENER EL ID DE USUARIO DE LA SESION
                membresia.IdUsuario = GetUserDecrypted.IdUsuario;
                if (membresia.IdMembresia > 0)
                {
                    await ApiHelper.PUT<Membresia>("Membresia/PutMembresia", membresia, GetAuthToken);
                    return Json("La publicación se ha editado exitosamente.");
                }
                else
                {
                    membresia.FechaIngreso = DateTime.Now;
                    await ApiHelper.POST<Membresia>("Membresia/PostMembresia", membresia, GetAuthToken);
                    return Json("La membresia se ha guardado exitosamente.");
                }

            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Ha ocurrido un error al guardar la membresia. Si el problema persiste contacte su administrador.");
            }

        }

    }
}