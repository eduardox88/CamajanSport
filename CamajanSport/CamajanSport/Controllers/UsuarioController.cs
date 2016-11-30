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
        private Token token = CookieHandler.GetDecryptToken();
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
                    data.IdEstado = 2; // El 2 significa Pendiente por verificacion
                    data.IdRol = 1; // Significa que es un usuario comun y corriente
                    respuesta = await ApiHelper.POST<Usuario>("Usuario/PostUsuario", data, token);
                }
                else {
                    respuesta = await ApiHelper.PUT<Usuario>("Usuario/PutUsuario", data, token);
                }

                if (respuesta.IsSuccessStatusCode)
                {
                    //Tener acceso a la cuenta camajan
                    if (data.IdUsuario == 0)
                    {
                        int idUsuarioInsertado = await respuesta.Content.ReadAsAsync<int>();

                        MailHandler.SendEmail(idUsuarioInsertado, data.CorreoElec);
                    }
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

        [System.Web.Mvc.HttpPost]
        public async Task<JsonResult> ExisteUsuarioPorCorreo(string correo)
        {
            try
            {

                var response = await ApiHelper.POST<Usuario>("Usuario/ExisteUsuarioPorCorreo", new Usuario() { CorreoElec = correo }, token);
                bool success = false;

                if (response.IsSuccessStatusCode)
                {
                    success = await response.Content.ReadAsAsync<bool>();
                    return Json(success);
                }
                else {
                    return Json(false);
                }

            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Ha ocurrido un error al momento de obtener el resultado, si el problema persiste contacte al administrador");
            }

        }

        public async Task<JsonResult> ExisteUsuarioPorNombre(string usuario)
        {
            try
            {

                var response = await ApiHelper.POST<Usuario>("Usuario/ExisteUsuarioPorNombre", new Usuario() { NombreUsuario = usuario }, token);
                bool success = false;

                if (response.IsSuccessStatusCode)
                {
                    success = await response.Content.ReadAsAsync<bool>();

                    return Json(success);
                }
                else
                {
                    throw new Exception();
                }

            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Ha ocurrido un error al momento de obtener el resultado, si el problema persiste contacte al administrador");
            }

        }

        public async Task<JsonResult> GetUsuarios() {

            try
            {
                var lista = await ApiHelper.GET_List<Usuario>("Usuario/Getusuarios", token);
                return Json(lista, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Ha ocurrido un error al momento de obtener el listado de usuarios, si el problema persiste contacte al administrador");
                throw;
            }
        }

        public async Task<JsonResult> GetCountUsuarios()
        {
            try
            {
                int cantidad = 0;
                var response = await ApiHelper.GET("Usuario/GetCountUsuarios", token);

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

        public ActionResult ConfirmacionCorreo(string id) {

            try
            {
                int idDecrypted = Convert.ToInt32(Encrypt.RijndaelDecrypt(id));

                var response = ApiHelper.PUT<Usuario>("Usuario/ConfirmacionUsuario", new Usuario() { IdUsuario = idDecrypted, IdEstado = 1 }, token);

                return RedirectToAction("VistaConfirmacion", "LogIn");
            }
            catch (Exception)
            {
                return RedirectToAction("VistaConfirmacion", "LogIn");
            }
            
        }
    }
}
