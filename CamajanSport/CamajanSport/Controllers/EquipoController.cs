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
using System.Web.Security;
using Utilidades;

namespace CamajanSport.Controllers
{
    [System.Web.Mvc.Authorize]
    public class EquipoController : Controller
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
        public ActionResult MantEquipos() {
            return View();
        }

        public async Task<ActionResult> SaveUploadedFile()
        {
            bool isSavedSuccessfully = true;

            try
            {
                string fName = "";
                int idEquipo = int.Parse(Request.Form["idEquipo"]);
                string Nombre = Request.Form["Equipo"];
                string Abreviacion = Request.Form["Abreviacion"];
                int idDeporte = Convert.ToInt32(Request.Form["idDeporte"]);
                bool Activo = Convert.ToBoolean(Request.Form["Activo"]);
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
                Equipo equipo = new Equipo();
                equipo.idEquipo = idEquipo;
                equipo.Nombre = Nombre;
                equipo.Abrev = Abreviacion;
                equipo.Imagen = imgByte;
                equipo.idDeporte = idDeporte;
                equipo.Activo = Activo;

                HttpResponseMessage respuesta;

                if (idEquipo > 0)
                {
                    respuesta = await ApiHelper.PUT<Equipo>("Equipo/PutEquipo", equipo, GetAuthToken);
                }
                else
                {
                    respuesta = await ApiHelper.POST<Equipo>("Equipo/PostEquipo", equipo, GetAuthToken);
                }

                if (respuesta.IsSuccessStatusCode)
                {
                    return Json((equipo.idEquipo > 0) ? "El equipo se ha editado satisfactoriamente" : "El equipo ha sido guardado satisfactoriamente", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(DynamicObjectHandler.SwalResponse("Error", TypeResult.error, "Ha ocurrido un error al guardar. Si el problema persiste contacte su administrador."));
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(DynamicObjectHandler.SwalResponse("Error", TypeResult.error, "Ha ocurrido un error al guardar. Si el problema persiste contacte su administrador."));
            }
        }

        public async Task<JsonResult> GetEquipos() {

            try
            {
                var lista = await ApiHelper.GET_List<Equipo>("Equipo/GetListEquipos", GetAuthToken);
                return Json(lista, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return Json(DynamicObjectHandler.SwalResponse("Error", TypeResult.error, "Ha ocurrido un error al momento de obtener el listado de equipos, si el problema persiste contacte al administrador"));
            }
        }

        public async Task<JsonResult> GetCountEquipos()
        {
            try
            {
                int cantidad = 0;
                var response = await ApiHelper.GET("Equipo/GetCountEquipos", GetAuthToken);

                if (response.IsSuccessStatusCode)
                {
                    cantidad = await response.Content.ReadAsAsync<int>();
                }

                return Json(cantidad, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return Json(DynamicObjectHandler.SwalResponse("Error", TypeResult.error, "Ha ocurrido un error al momento de obtener la cantidad de equipos registrados, si el problema persiste contacte al administrador"));
            }
        }

        public async Task<JsonResult> GetEquipos_Select()
        {

            try
            {
                var lista = await ApiHelper.GET_List<SelectAttributes>("Equipo/GetEquipos_Select", GetAuthToken);
                return Json(lista, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return Json(DynamicObjectHandler.SwalResponse("Error", TypeResult.error, "Ha ocurrido un error al momento de obtener el listado de equipos, si el problema persiste contacte al administrador"));
            }
        }
        public async Task<JsonResult> GetEquiposByDeporte_Select(int idDeporte)
        {

            try
            {
                var lista = await ApiHelper.GET_ListById<SelectAttributes>("Equipo/GetEquiposByDeporte_Select", idDeporte, GetAuthToken);
                return Json(lista, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return Json(DynamicObjectHandler.SwalResponse("Error", TypeResult.error, "Ha ocurrido un error al momento de obtener el listado de equipos, si el problema persiste contacte al administrador"));
            }
        }
    }
}
