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
using CamajanSport.App_Start;
using Utilidades;
using System.Net;

namespace CamajanSport.Controllers
{
    public class DeporteController : Controller
    {

        public ActionResult MantDeportes()
        {
            return View("MantDeportes");
        }
       
        [SessionHandle]
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
                    respuesta = await ApiHelper.PUT<Deporte>("Deporte/PutDeporte", deporte, (Token)Session["Token"]);
                }
                else
                {
                    respuesta = await ApiHelper.POST<Deporte>("Deporte/PostDeporte", deporte, (Token)Session["Token"]);
                }
                
                if (respuesta.IsSuccessStatusCode)
                {
                    return Json((deporte.IdDeporte > 0)?"El deporte se ha editado satisfactoriamente":"El deporte ha sido guardado satisfactoriamente", JsonRequestBehavior.AllowGet);
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
        [SessionHandle]
        [HttpPost]
        public async Task<JsonResult> Getdeportes() 
        {
            try
            {
                var lista = await ApiHelper.GET_List<Deporte>("Deporte/Getdeportes", (Session["Token"] as Token));
                return Json(lista, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR",Message="Ha ocurrido un error al cargar la lista de deportes."}, JsonRequestBehavior.AllowGet);
            }
        }
    }
}