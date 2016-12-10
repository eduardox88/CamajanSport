using CamajanSport.BOL;
using System;
using System.Collections.Generic;
using System.Linq;
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
            model.Imagen = null;
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
            return cookie;
        }

        public static HttpCookie GetAuthenticationCookie(Token token, bool persistLogin, int minutes_expire)
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
            HttpCookie cookie = new HttpCookie("Token", encTicket);
            cookie.Expires = authTicket.Expiration;
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

        public static Token GetDecryptToken()
        {

            HttpCookie cookie = HttpContext.Current.Request.Cookies["Token"];
            JavaScriptSerializer js = new JavaScriptSerializer();

            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

                return js.Deserialize<Token>(ticket.UserData);
            }
            else
            {
                return null;
            }
        }

    }
}
