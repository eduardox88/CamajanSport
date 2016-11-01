using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Utilidades;
using CamajanSport.Models;
using System.Net;

namespace CamajanSport.Controllers
{
    public class LogInController : Controller
    {
        public ActionResult Index() {

            return View();
        }

        public async Task<JsonResult> SignIn(string username, string password) {

            HttpResponseMessage responseMessage = await Helper.GetBearerToken("http://localhost:14678/", username, password);

            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            if (responseMessage.IsSuccessStatusCode)
            {
                Token response = await responseMessage.Content.ReadAsAsync<Token>();

                if (response != null) {
                    HttpClient client = new HttpClient();

                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + response.AccessToken);

                    HttpResponseMessage test = await client.GetAsync("http://localhost:14678/api/Deporte/Getdeportes");

                    if (test.IsSuccessStatusCode) {

                        var message = test.Content.ReadAsStringAsync().Result;

                        var result1 = JsonConvert.DeserializeObject<Deporte>(message);
                        var result2 = test.Content.ReadAsAsync<Deporte>().Result;

                       
                    }

                }

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(0);
            }
            
        }
    }

    public class Deporte
    {
        [JsonProperty("IdDeporte")]
        public int IdDeporte { get; set; }

        public string Nombre { get; set; }

        public byte?[] Imagen { get; set; }

        public bool? Activo { get; set; }

        public DateTime? FechaIngreso { get; set; }

    }  
}
