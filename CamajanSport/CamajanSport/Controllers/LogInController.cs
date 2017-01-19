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
using CamajanSport.Properties;

namespace CamajanSport.Controllers
{
    public class LogInController : Controller
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

        [System.Web.Mvc.AllowAnonymous]
        public ActionResult Index() {

            Usuario user = new Usuario();
            HttpCookie cookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];

            ViewBag.RecordarUsuario = false;
            
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

                if (ticket != null)
                {
                    if (ticket.IsPersistent)
                    {
                        ViewBag.RecordarUsuario = true;
                        user = GetUserDecrypted;
                    }
                }
            }

            return View(user);

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
                            ClearAllCookieSession();

                            Response.Cookies.Add(CookieHandler.GetAuthenticationCookie(Settings.Default.TokenCookie,token, recordar, expire));
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
                    return Json("Usuario y/o Contraseña son inválidas");
                }
            }
            catch (Exception ex)
            {
                return Json("Ha ocurrido un error");
            }
        }

        public async Task<JsonResult> SignInCompra(string correo, string password, bool recordar)
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
                        if (user.IdEstado == 1)
                        {
                            ClearAllCookieSession();

                            Response.Cookies.Add(CookieHandler.GetAuthenticationCookie(Settings.Default.TokenCookie, token, recordar, expire));
                            Response.Cookies.Add(CookieHandler.GetAuthenticationCookie(user, recordar, expire));
                            if (user.rol.IdRol != 1)
                            {
                                return Json(new { Result = "ERROR", Message = "Usted no está autorizado para comprar membresías. Contacte su administrador." });
                            }
                            else
                            {
                                return Json(new { Result = "OK" });
                            }
                           
                        }
                        else
                        {
                            if (user.IdEstado == 2)
                            {
                                return Json(new { Result = "ERROR", Message = "Debe confirmar su cuenta para poder realizar compras en Camajan Deportivo. Verifique su bandeja de entrada y siga los pasos del correo de confirmación." });
                            }
                            else
                            {
                                return Json(new { Result = "ERROR", Message = "Su usuario ha sido inhabilitado. Para más información contacte el administrador a camajandeportivo@gmail.com" });
                            }
                            
                        }
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
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


        public ActionResult LogOff() {

            ClearAllCookieSession();

            return RedirectToAction("Index", "Home");
        }
        private void ClearAllCookieSession() {
            Response.Cookies.Clear();

            HttpCookie c = new HttpCookie(Settings.Default.TokenCookie);
            HttpCookie c2 = new HttpCookie(FormsAuthentication.FormsCookieName);

            c.Expires = DateTime.Now.AddDays(-1);
            c2.Expires = DateTime.Now.AddDays(-1);

            Response.Cookies.Add(c);
            Response.Cookies.Add(c2);
        }


    }
}
