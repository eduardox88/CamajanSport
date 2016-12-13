using CamajanSport.BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace Utilidades
{
    public class CookieHandler
    {
        public static HttpCookie GetAuthenticationCookie(Usuario model, bool persistLogin, int minutes_expire)
        {
            // userData storing data in ticktet and then cookie 
            JavaScriptSerializer js = new JavaScriptSerializer();

            var userData = js.Serialize(model);
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                     1,
                     model.NombreUsuario,
                     DateTime.Now,
                     DateTime.Now.AddMinutes(minutes_expire),
                     persistLogin,
                     userData);
            
            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            cookie.Expires = authTicket.Expiration;
            cookie.HttpOnly = true;
            return cookie;
        }

        public static HttpCookie GetAuthenticationCookie(string nameCookie,Token token, bool persistLogin, int minutes_expire)
        {
            // userData storing data in ticktet and then cookie 
            JavaScriptSerializer js = new JavaScriptSerializer();

            var userData = js.Serialize(token);
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                     1,
                     "Token",
                     DateTime.Now,
                     DateTime.Now.AddMinutes(minutes_expire),
                     persistLogin,
                     userData);

            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie cookie = new HttpCookie(nameCookie, encTicket);
            cookie.Expires = authTicket.Expiration;
            cookie.HttpOnly = true;
            return cookie;
        }

        public static FormsAuthenticationTicket GetDecryptTicket() {

            HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

                return ticket;
            }
            else {
                return null;
            }  
        }

        public static T GetCookieDecrypted<T>(string cookieName) where T : class {

            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            JavaScriptSerializer js = new JavaScriptSerializer();

            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

                return js.Deserialize<T>(ticket.UserData);
            }
            else
            {
                return null;
            }
        }

        public static HttpCookie UpdateTicket(Usuario usuario, string newPass = null)
        {
            FormsAuthenticationTicket ticket = GetDecryptTicket();

            Usuario modelo = new Usuario();
            JavaScriptSerializer js = new JavaScriptSerializer();

            modelo = js.Deserialize<Usuario>(ticket.UserData);

            if (newPass != null)
            {
                usuario.Contrasena = newPass;
            }
            else {
                usuario.Contrasena = modelo.Contrasena;
            }

           

            var userData = js.Serialize(usuario);
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                     1,
                     modelo.NombreUsuario,
                     ticket.IssueDate,
                     ticket.Expiration,
                     ticket.IsPersistent,
                     userData);

            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            cookie.Expires = authTicket.Expiration;
            cookie.HttpOnly = true;

            return cookie;
        }
    }
}
