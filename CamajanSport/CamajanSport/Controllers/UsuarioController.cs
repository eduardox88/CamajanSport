using CamajanSport.App_Start;
using CamajanSport.BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Utilidades;

namespace CamajanSport.Controllers
{
    public class UsuarioController : Controller
    {
        public ActionResult MantUsuarios()
        {


            return View();
        }

        [System.Web.Mvc.HttpPost]
        public async Task<JsonResult> InsertarUsuario(Usuario data) {

            HttpResponseMessage respuesta;

            try
            {
                //Significa que esta insertando
                if (data.IdUsuario == 0)
                {
                    data.Contrasena = Encrypt.ComputeHash(data.Contrasena, "SHA512", null);
                    respuesta = await ApiHelper.POST<Usuario>("Usuario/PostUsuario", data, (Session["Token"] as Token));
                }
                else {
                    respuesta = await ApiHelper.PUT<Usuario>("Usuario/PutUsuario", data, (Session["Token"] as Token));
                }

                if (respuesta.IsSuccessStatusCode)
                {
                    return Json((data.IdUsuario > 0) ? "El usuario se ha editado satisfactoriamente" : "El usuario ha sido guardado satisfactoriamente", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json("Ha ocurrido un error con las validaciones del usuario. Si el problema persiste contacte su administrador.");
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Ha ocurrido un error al momento de guardar el usuario, si el problema persiste contacte al administrador");
            }

        }

        public async Task<JsonResult> GetUsuarios() {

            try
            {
                var lista = await ApiHelper.GET_List<Usuario>("Usuario/Getusuarios", (Session["Token"] as Token));
                return Json(lista, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Ha ocurrido un error al momento de obtener el listado de usuarios, si el problema persiste contacte al administrador");
                throw;
            }


            return Json(true);
        }

        [SessionHandle]
        public async Task<JsonResult> GetCountUsuarios()
        {
            try
            {
                int cantidad = 0;
                var response = await ApiHelper.GET("Usuario/GetCountUsuarios", (Session["Token"] as Token));

                if (response.IsSuccessStatusCode)
                {
                    cantidad = await response.Content.ReadAsAsync<int>();
                }

                return Json(cantidad, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Ha ocurrido un error al momento de obtener la cantidad de usuarios registrados, si el problema persiste contacte al administrador");
            }
        }
    }
}
