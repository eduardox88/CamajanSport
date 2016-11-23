using CamajanSport.BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace CamajanSport.Manejadores
{
    public class ManejadorUsuario
    {
        public static HttpCookie GetAuthenticationCookie(Usuario model, bool persistLogin)
        {
            // userData storing data in ticktet and then cookie 
            JavaScriptSerializer js = new JavaScriptSerializer();

            var userData = js.Serialize(model);
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                     1,
                     "akash",
                     DateTime.Now,
                     DateTime.Now.AddHours(1),
                     persistLogin,
                     userData);

            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            cookie.Expires = authTicket.Expiration;
            return cookie;
        }

        internal static bool Login(string UserName, string Password)
        {
            //UserName="akash" Password="akash"
            //check can be done by DB
            if (UserName == "akash" && Password == "akash")
                return true;
            else
                return false;
        }
    }
}