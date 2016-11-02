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

namespace CamajanSport.Controllers
{
    public class LogInController : Controller
    {
        public ActionResult Index() {

            return View();
        }

        public async Task<JsonResult> SignIn(string username, string password) {


            try
            {
                HttpResponseMessage responseMessage = await Helper.GetBearerToken("http://localhost:14678/", username, password);

                if (responseMessage.IsSuccessStatusCode)
                {
                    Token token = await responseMessage.Content.ReadAsAsync<Token>();
                    Session.Add("Token", token);
                    Session.Timeout = (token.ExpiresIn / 60);


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
