using CamajanSport.BOL;
using CamajanSport.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Utilidades;

namespace CamajanSport.Controllers
{
 
    public class UsuarioController : Controller
    {
        #region Propiedades
        private Token GetAuthToken { 
            
            get{
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

        public ActionResult MantUsuarios()
        {
            return View();
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult PerfilUsuario() {

            return View(GetUserDecrypted);
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> PerfilUsuario(Usuario modelo)
        {
            if (ModelState.IsValid) {
                try
                {
                    HttpResponseMessage respuesta = await ApiHelper.PUT<Usuario>("Usuario/PutUsuarioPerfil", modelo, GetAuthToken);

                    if (respuesta.IsSuccessStatusCode)
                    {
                        Usuario nuevo = await ApiHelper.GET_By_ID<Usuario>("Usuario/GetUsuario", modelo.IdUsuario, GetAuthToken);
                        HttpContext.Response.Cookies.Set(CookieHandler.UpdateTicket(nuevo));
                        
                    }
                    else {
                        ModelState.AddModelError("", "Ha ocurrido un problema");
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ha ocurrido un problema");
                    throw;
                }
            }

            return RedirectToAction("PerfilUsuario");
        }

        [System.Web.Mvc.Authorize]
        public async Task<JsonResult> UpdatePassword(string oldPass, string newPass, string RePass) {

            Usuario aux = GetUserDecrypted;

            if (oldPass == aux.Contrasena)
            {
                if (newPass == RePass)
                {

                    HttpResponseMessage respuesta = await ApiHelper.PUT<Usuario>("Usuario/PutChangePassword", new Usuario() { IdUsuario = aux.IdUsuario, Contrasena = Encrypt.ComputeHash(newPass,"SHA512",null) }, GetAuthToken);

                    if (respuesta.IsSuccessStatusCode)
                    {
                        Usuario nuevo = await ApiHelper.GET_By_ID<Usuario>("Usuario/GetUsuario", aux.IdUsuario, GetAuthToken);
                        HttpContext.Response.Cookies.Set(CookieHandler.UpdateTicket(nuevo, newPass));

                        return Json(new { message="Contraseña actualizada exitosamente", property = "alert alert-success" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { message = "Ha ocurrido un problema al momento de actualizar la contraseña", property = "alert alert-danger" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else {
                    return Json(new { message = "Las contraseñas no coinciden", property = "alert alert-danger" }, JsonRequestBehavior.AllowGet);
                }
            }
            else {
                return Json(new { message = "Contraseña Inválida", property = "alert alert-danger" }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> SaveUploadedFile()
        {
            try
            {
                string fName = "";
                int idUsuario = int.Parse(Request.Form["idUsuario"]);
                string Nombre = Request.Form["Nombre"];
                string Persona = Request.Form["NombrePersona"];
                string Correo = Request.Form["Correo"];
                string Telefono = Request.Form["Telefono"];
                int idPais = Convert.ToInt32(Request.Form["Pais"]);
                int idEstado = Convert.ToInt32(Request.Form["Estado"]);
                int idRol = Convert.ToInt32(Request.Form["Rol"]);
                string Pass = Request.Form["pass"];
                string Biografia = Request.Form["Biografia"];

                bool newFile = Convert.ToBoolean(Request.Form["newFile"].ToString());
                Byte[] imgByte = null;
                string path = string.Empty;

                if (newFile)
                {
                    string fileName;
                    HttpPostedFileBase file;
                    fileName = Request.Files.AllKeys[0];
                    file = Request.Files[fileName];
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {
                        string pic = System.IO.Path.GetFileName(fName);

                        path = System.IO.Path.Combine(Server.MapPath("~/ProfileImages"), pic);
                        file.SaveAs(path);
                        path = "/ProfileImages/" + pic;
                    }
                }

                Usuario user = new Usuario();

                user.IdUsuario = idUsuario;
                user.NombreUsuario = Nombre;
                user.NombreCompleto = Persona;
                user.CorreoElec = Correo;
                user.Telefono = Telefono;
                user.IdPais = idPais;
                user.IdRol = idRol;
                user.IdEstado = idEstado;
                user.Contrasena = Encrypt.ComputeHash(Pass, "SHA512", null); ;
                user.Biografia = Biografia;
                user.Imagen = path;

                HttpResponseMessage respuesta;

                if (idUsuario > 0)
                {
                    respuesta = await ApiHelper.PUT<Usuario>("Usuario/PutUsuario", user, GetAuthToken).ConfigureAwait(false);
                }
                else
                {
                    respuesta = await ApiHelper.POST<Usuario>("Usuario/PostUsuario", user, GetAuthToken).ConfigureAwait(false);
                }

                if (respuesta.IsSuccessStatusCode)
                {
                    return Json((idUsuario > 0) ? "El usuario se ha editado satisfactoriamente" : "El usuario ha sido guardado satisfactoriamente", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json("Ha ocurrido un error al guardar. Si el problema persiste contacte su administrador.");
                }
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Ha ocurrido un error al guardar. Si el problema persiste contacte su administrador.");
            }
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
                    respuesta = await ApiHelper.POST<Usuario>("Usuario/PostUsuario", data, GetAuthToken).ConfigureAwait(false);
                }
                else {
                    respuesta = await ApiHelper.PUT<Usuario>("Usuario/PutUsuario", data, GetAuthToken).ConfigureAwait(false);
                }

                if (respuesta.IsSuccessStatusCode)
                {
                    //Tener acceso a la cuenta camajan
                    if (data.IdUsuario == 0)
                    {
                        int idUsuarioInsertado = await respuesta.Content.ReadAsAsync<int>().ConfigureAwait(false);

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

                var response = await ApiHelper.POST<Usuario>("Usuario/ExisteUsuarioPorCorreo", new Usuario() { CorreoElec = correo }, GetAuthToken).ConfigureAwait(false);
                bool success = false;

                if (response.IsSuccessStatusCode)
                {
                    success = await response.Content.ReadAsAsync<bool>().ConfigureAwait(false);
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

                var response = await ApiHelper.POST<Usuario>("Usuario/ExisteUsuarioPorNombre", new Usuario() { NombreUsuario = usuario }, GetAuthToken).ConfigureAwait(false);
                bool success = false;

                if (response.IsSuccessStatusCode)
                {
                    success = await response.Content.ReadAsAsync<bool>().ConfigureAwait(false);

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
                var lista = await ApiHelper.GET_List<Usuario>("Usuario/Getusuarios", GetAuthToken).ConfigureAwait(false);
                return Json(lista, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Ha ocurrido un error al momento de obtener el listado de usuarios, si el problema persiste contacte al administrador");
                throw;
            }
        }

        public async Task<JsonResult> GetUsuariosByEstado(int idEstado)
        {
            try
            {
                int cantidad = 0;

                HttpClient client = new HttpClient();

                if (GetAuthToken != null)
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetAuthToken.AccessToken);
                }

                HttpResponseMessage result = await client.GetAsync("http://localhost:14678/api/Usuario/GetUsuariosByEstado/" + idEstado).ConfigureAwait(false);

                if (result.IsSuccessStatusCode)
                {
                    cantidad = await result.Content.ReadAsAsync<int>().ConfigureAwait(false);
                }

                return Json(cantidad, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Ha ocurrido un error, si el problema persiste contacte al administrador");
            }
        }

        public async Task<JsonResult> GetImagenUsuario()
        {
            try
            {
                int CodUsuario = Convert.ToInt32(Request.Form["idUsuario"]);
                HttpClient client = new HttpClient();

                if (GetAuthToken != null)
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetAuthToken.AccessToken);
                }

                HttpResponseMessage result = await client.GetAsync("http://localhost:14678/api/Usuario/GetImagenUsuario/" + CodUsuario).ConfigureAwait(false);

                if (result.IsSuccessStatusCode)
                {
                    var img = await result.Content.ReadAsAsync<byte[]>().ConfigureAwait(false);

                    if (img != null) {
                        return Json(img, JsonRequestBehavior.AllowGet);
                    }
                }

                return Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Ha ocurrido un error, si el problema persiste contacte al administrador");
            }
        }

        public ActionResult ConfirmacionCorreo(string id) {

            try
            {
                int idDecrypted = Convert.ToInt32(Encrypt.RijndaelDecrypt(id));

                var response = ApiHelper.PUT<Usuario>("Usuario/ConfirmacionUsuario", new Usuario() { IdUsuario = idDecrypted, IdEstado = 1 }, GetAuthToken).ConfigureAwait(false);

                return RedirectToAction("VistaConfirmacion", "LogIn");
            }
            catch (Exception)
            {
                return RedirectToAction("VistaConfirmacion", "LogIn");
            }

            
        }
    }
}
