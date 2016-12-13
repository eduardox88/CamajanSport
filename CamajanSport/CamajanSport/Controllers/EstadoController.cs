using CamajanSport.BOL;
using CamajanSport.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using Utilidades;

namespace CamajanSport.Controllers
{
    [System.Web.Mvc.Authorize]
    public class EstadoController : Controller
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
        public async Task<JsonResult> GetEstado_Select() {
            try
            {
                var lista = await ApiHelper.GET_List<SelectAttributes>("Estado/GetEstados_Select", GetAuthToken);

                return Json(lista, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Ha ocurrido un error al momento de obtener el listado de estados para el select, si el problema persiste contacte al administrador");
            }
        }
    }
}
