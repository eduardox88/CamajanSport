using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Formatting;
using CamajanSport.BOL;
using Utilidades;
using System.Net;
using CamajanSport.Properties;
using System.Web.Security;

namespace CamajanSport.Controllers
{
    [Authorize]
    public class DeporteController : Controller
    {
        #region Propiedades
        private Token GetAuthToken
        {

            get
            {
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
        public ActionResult MantDeportes()
        {
            return View("MantDeportes");
        }
       

        public async Task<JsonResult> SaveUploadedFile()
        {
            bool isSavedSuccessfully = true;
            
            try
            {
                string fName = "";
                int idDeporte = int.Parse(Request.Form["idDeporte"]);
                string nombre = Request.Form["nombre"];
                bool activo = Convert.ToBoolean(Request.Form["activo"].ToString());
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
                Deporte deporte = new Deporte();
                deporte.IdDeporte = idDeporte;
                deporte.Nombre = nombre;
                deporte.Activo = activo;
                deporte.Imagen = imgByte;
                HttpResponseMessage  respuesta;


                if (idDeporte > 0)
                {
                    respuesta = await ApiHelper.PUT<Deporte>("Deporte/PutDeporte", deporte, GetAuthToken);
                }
                else
                {
                    respuesta = await ApiHelper.POST<Deporte>("Deporte/PostDeporte", deporte, GetAuthToken);
                }
                
                if (respuesta.IsSuccessStatusCode)
                {
                    return Json((deporte.IdDeporte > 0)?"El deporte se ha editado satisfactoriamente":"El deporte ha sido guardado satisfactoriamente", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(DynamicObjectHandler.SwalResponse("Error",TypeResult.error,"Ha ocurrido un error al guardar. Si el problema persiste contacte su administrador."));
                }
                
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(DynamicObjectHandler.SwalResponse("Error", TypeResult.error, "Ha ocurrido un error al guardar. Si el problema persiste contacte su administrador."));
            }
            
        }

        [HttpPost]
        public async Task<JsonResult> Getdeportes() 
        {
            try
            {
                var lista = await ApiHelper.GET_List<Deporte>("Deporte/Getdeportes", GetAuthToken);
                return Json(lista, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(DynamicObjectHandler.SwalResponse("Error", TypeResult.error, "Ha ocurrido un error al momento de obtener el listado de deportes, si el problema persiste contacte al administrador"));
            }
        }


        public async Task<JsonResult> GetDeportes_Select()
        {
            try
            {
                var lista = await ApiHelper.GET_List<SelectAttributes>("Deporte/GetDeportes_Select", GetAuthToken);

                return Json(lista, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(DynamicObjectHandler.SwalResponse("Error", TypeResult.error, "Ha ocurrido un error al momento de obtener el listado de deportes para el select, si el problema persiste contacte al administrador"));
            }
        }



        public async Task<JsonResult> GetCountDeporte()
        {
            try
            {
                int cantidad = 0;
                var response = await ApiHelper.GET("Deporte/GetCountDeporte", GetAuthToken);

                if (response.IsSuccessStatusCode)
                {
                    cantidad = await response.Content.ReadAsAsync<int>();
                }

                return Json(cantidad, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(DynamicObjectHandler.SwalResponse("Error", TypeResult.error, "Ha ocurrido un error al momento de obtener la cantidad de deportes registrado, si el problema persiste contacte al administrador"));
            }
        }
    }
}