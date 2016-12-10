using CamajanSport.BOL;
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

        [System.Web.Mvc.HttpGet]
        public ActionResult PerfilUsuario() {

            var ticket = Utilidades.CookieHandler.GetDecryptTicket();

            Usuario modelo = new Usuario();
            JavaScriptSerializer js = new JavaScriptSerializer();

            modelo = js.Deserialize<Usuario>(ticket.UserData);

            return View(modelo);
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> PerfilUsuario(Usuario modelo)
        {

            if (ModelState.IsValid) {
                try
                {
                    HttpResponseMessage respuesta = await ApiHelper.PUT<Usuario>("Usuario/PutUsuario", modelo, token);

                    if (!respuesta.IsSuccessStatusCode) {
                        ModelState.AddModelError("", "Ha ocurrido un problema");
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ha ocurrido un problema");
                    throw;
                }
            }

            return View(modelo);
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

                if (newFile)
                {
                    string fileName;
                    HttpPostedFileBase file;
                    fileName = Request.Files.AllKeys[0];
                    file = Request.Files[fileName];
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {
                        imgByte = new Byte[file.ContentLength];
                        //force the control to load data in array
                        file.InputStream.Read(imgByte, 0, file.ContentLength);

                    }
                }
                else
                {
                    int len = Request.Form["afile"].ToString().Split(',').Length;
                    imgByte = new Byte[len];
                    imgByte = Request.Form["afile"].ToString().Split(',').Select(s => Convert.ToByte(s)).ToArray();
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
                user.Imagen = imgByte;

                HttpResponseMessage respuesta;

                if (idUsuario > 0)
                {
                    respuesta = await ApiHelper.PUT<Usuario>("Usuario/PutUsuario", user, token);
                }
                else
                {
                    respuesta = await ApiHelper.POST<Usuario>("Usuario/PostUsuario", user, token);
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

        public async Task<JsonResult> GetUsuariosByEstado(int idEstado)
        {
            try
            {
                int cantidad = 0;

                HttpClient client = new HttpClient();

                if (token != null)
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.AccessToken);
                }

                HttpResponseMessage result = await client.GetAsync("http://localhost:14678/api/Usuario/GetUsuariosByEstado/" + idEstado );

                if (result.IsSuccessStatusCode)
                {
                    cantidad = await result.Content.ReadAsAsync<int>();
                }

                return Json(cantidad, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Ha ocurrido un error, si el problema persiste contacte al administrador");
            }
        }

        public async Task<JsonResult> GetImagenUsuario(int CodUsuario)
        {
            try
            {
                HttpClient client = new HttpClient();

                if (token != null)
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.AccessToken);
                }

                HttpResponseMessage result = await client.GetAsync("http://localhost:14678/api/Usuario/GetImagenUsuario/" + CodUsuario);

                if (result.IsSuccessStatusCode)
                {
                    var img = await result.Content.ReadAsAsync<byte[]>();

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
