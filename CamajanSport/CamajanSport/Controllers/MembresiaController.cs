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
    [Authorize]
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
        public async Task<JsonResult> GetMembresias() 
        {
            var membresias = await ApiHelper.GET_List<Membresia>("Membresia/GetMembresias", GetAuthToken);
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