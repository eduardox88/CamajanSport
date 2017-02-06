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

        /// <summary>
        /// Muestra el view ListarMisMembresias que presenta las membresias del usuario con Rol Regular
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ListarMisMembresias()
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
            return View();
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
                return Json(new { Result ="OK",Data=membresias});
            }
            catch (Exception)
            {
                return Json(new {Result="ERROR",Message="Ha ocurrido un error al obtener las membresías del usuario." });
            }
            
        }
        /// <summary>
        /// Obtiene las membresias disponibles pero necesita AUTENTICACION
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<JsonResult> GetMembresias() 
        {
            var membresias = await ApiHelper.GET_List<Membresia>("Membresia/GetMembresias", GetAuthToken);
            return Json(membresias, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Obtiene las membresias disponibles SIN NECESIDAD DE AUTENTICARSE
        /// </summary>
        /// <returns></returns>
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
                //SE OBTIENE EL ID DE USUARIO DE LA SESION
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
        [Authorize]
        public async Task<JsonResult> RenovarMembresiaUsuario(int IdMembresiaUsuario, int CantDiasRenovacion) 
        {

            try
            {
                MembresiaUsuario memb = new MembresiaUsuario();
                memb.IdMembresiaUsuario = IdMembresiaUsuario;
                memb.Duracion = CantDiasRenovacion;
                var membresia = await ApiHelper.PUT<MembresiaUsuario>("MembresiaUsuarios/RenovarMembresiaUsuario", memb, GetAuthToken);
                return Json(new { Result = "OK", Message = "La membresía del usuario ha sido renovada exitosamente. Su nueva fecha de expiración es "+(DateTime.Now.AddDays(CantDiasRenovacion).ToShortDateString())+"." });
            }
            catch (Exception)
            {
                return Json(new { Result = "ERROR",Type="error", Message = "Ha ocurrido un error al renovar la membresia del usuario. Si el problema persiste contacte su administrador." });
            }
            
        
        }
        public async Task<MembresiaUsuario> ObtieneMembresiaActiva()
        {
            var membresia = await ApiHelper.GET_By_ID<MembresiaUsuario>("MembresiaUsuarios/ObtieneMembresiaActiva", GetUserDecrypted.IdUsuario, GetAuthToken);
            return membresia;
        }
    }
}