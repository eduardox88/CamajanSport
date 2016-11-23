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
        public ActionResult Index() {

            ViewBag.Checked = false;
            HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            JavaScriptSerializer js = new JavaScriptSerializer();

            if (cookie != null)
            {
                ViewBag.Checked = true;
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

                if (ticket.IsPersistent)
                {
                    Usuario model = js.Deserialize<Usuario>(ticket.UserData);

                    return View(model);
                }
                else {
                    return View(new Usuario());
                }
            }
            else {
                return View(new Usuario());
            }
        }

        public async Task<JsonResult> SignIn(string correo, string password)
        {

            try
            {
                HttpResponseMessage responseMessage = await Helper.GetBearerToken("http://localhost:14678/", correo, password);

                if (responseMessage.IsSuccessStatusCode)
                {
                    Usuario user = new Usuario();
                    Token token = await responseMessage.Content.ReadAsAsync<Token>();
                    Session.Add("Token", token);
                    Session.Timeout = (token.ExpiresIn / 60);

                    HttpResponseMessage responseUser = await ApiHelper.POST<Usuario>("Usuario/GetUsuarioByEmail", new Usuario() { CorreoElec = correo }, Session["Token"] as Token);

                    if (responseUser.IsSuccessStatusCode) {
                       user =  await responseUser.Content.ReadAsAsync<Usuario>();
                    }

                    user.Contrasena = password;

                    Response.Cookies.Add(Manejadores.ManejadorUsuario.GetAuthenticationCookie(user, true));

                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(0);
                }
            }
            catch (Exception ex)
            {
                return Json(0);
            }
        }
    }
}
