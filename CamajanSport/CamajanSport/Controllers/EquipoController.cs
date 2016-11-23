using CamajanSport.App_Start;
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
using Utilidades;

namespace CamajanSport.Controllers
{
    public class EquipoController : Controller
    {
        public ActionResult MantEquipos() {

            return View();
        }

        [SessionHandle]
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
                    respuesta = await ApiHelper.PUT<Equipo>("Equipo/PutEquipo", equipo, (Token)Session["Token"]);
                }
                else
                {
                    respuesta = await ApiHelper.POST<Equipo>("Equipo/PostEquipo", equipo, (Token)Session["Token"]);
                }

                if (respuesta.IsSuccessStatusCode)
                {
                    return Json((equipo.idEquipo > 0) ? "El equipo se ha editado satisfactoriamente" : "El equipo ha sido guardado satisfactoriamente", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json("Ha ocurrido un error al guardar. Si el problema persiste contacte su administrador.");
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Ha ocurrido un error al guardar. Si el problema persiste contacte su administrador.");
            }
        }

        [SessionHandle]
        public async Task<JsonResult> GetEquipos() {

            try
            {
                var lista = await ApiHelper.GET_List<Equipo>("Equipo/GetListEquipos", (Session["Token"] as Token));
                return Json(lista, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Ha ocurrido un error al momento de obtener el listado de equipos, si el problema persiste contacte al administrador");
            }
        }


        [SessionHandle]
        public async Task<JsonResult> GetCountEquipos()
        {
            try
            {
                int cantidad = 0;
                var response = await ApiHelper.GET("Equipo/GetCountEquipos", (Session["Token"] as Token));

                if (response.IsSuccessStatusCode)
                {
                    cantidad = await response.Content.ReadAsAsync<int>();
                }

                return Json(cantidad, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Ha ocurrido un error al momento de obtener la cantidad de equipos registrados, si el problema persiste contacte al administrador");
            }
        }

        [SessionHandle]
        public async Task<JsonResult> GetEquipos_Select()
        {

            try
            {
                var lista = await ApiHelper.GET_List<SelectAttributes>("Equipo/GetEquipos_Select", (Session["Token"] as Token));
                return Json(lista, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Ha ocurrido un error al momento de obtener el listado de equipos, si el problema persiste contacte al administrador");
            }
        }
        [SessionHandle]
        public async Task<JsonResult> GetEquiposByDeporte_Select(int idDeporte)
        {

            try
            {
                var lista = await ApiHelper.GET_ListById<SelectAttributes>("Equipo/GetEquiposByDeporte_Select", idDeporte, (Session["Token"] as Token));
                return Json(lista, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Ha ocurrido un error al momento de obtener el listado de equipos, si el problema persiste contacte al administrador");
            }
        }
    }
}
