using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using Utilidades;
using CamajanSport.BOL;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace CamajanSport.Controllers
{
    public class LogInController : Controller
    {


        [System.Web.Mvc.AllowAnonymous]
        public ActionResult Index() {

            var ticket = CookieHandler.GetDecryptTicket();
            ViewBag.Checked = false;

            Usuario model  = new Usuario();
            JavaScriptSerializer js = new JavaScriptSerializer();

            if (ticket != null)
            {
                if (ticket.IsPersistent)
                {
                    ViewBag.Checked = true;
                    model = js.Deserialize<Usuario>(ticket.UserData);
                }
            }

            return View(model);

        }

        public async Task<JsonResult> SignIn(string correo, string password, bool recordar )
        {

            try
            {
                HttpResponseMessage responseMessage = await ApiHelper.GetBearerToken("http://localhost:14678/", correo, password);

                if (responseMessage.IsSuccessStatusCode)
                {

                    Token token = await responseMessage.Content.ReadAsAsync<Token>();
                    int expire = (token.ExpiresIn / 60);

                    HttpResponseMessage responseUser = await ApiHelper.POST<Usuario>("Usuario/GetUsuarioByEmail", new Usuario() { CorreoElec = correo }, token);

                    if (responseUser.IsSuccessStatusCode)
                    {
                        
                        Usuario user = new Usuario();
                        user = await responseUser.Content.ReadAsAsync<Usuario>();
                        user.Contrasena = password;

                        if (user.IdEstado != 2)
                        {
                            Response.Cookies.Add(CookieHandler.GetAuthenticationCookie(token, recordar, expire));
                            Response.Cookies.Add(CookieHandler.GetAuthenticationCookie(user, recordar, expire));

                            return Json(true, JsonRequestBehavior.AllowGet);
                        }
                        else {
                            return Json(false);
                        }
                    }
                    else {
                        throw new Exception();
                    } 
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json("Usuario y/o Contraseña son inválidas");
                }
            }
            catch (Exception ex)
            {
                return Json("Ha ocurrido un error");
            }
        }

        public ActionResult CrearUsuario() {


            return View();
        }

        public ActionResult VistaConfirmacion()
        {
            return View();
        }
    }
}
