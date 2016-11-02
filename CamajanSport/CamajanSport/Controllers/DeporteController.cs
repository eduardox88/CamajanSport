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

namespace CamajanSport.Controllers
{
    public class DeporteController : Controller
    {

        public ActionResult MantDeportes()
        {
            return View("MantDeportes");
        }
       
        [SessionHandle]
        public async Task<ActionResult> SaveUploadedFile()
        {
            bool isSavedSuccessfully = true;
            
            try
            {
                string fName = "";
                string nombre = Request.Form["deporte"];
                bool activo = Convert.ToBoolean(Request.Form["activo"].ToString());
                string fileName = Request.Files.AllKeys[0];
                HttpPostedFileBase file = Request.Files[fileName];
                fName = file.FileName;
                Byte[] imgByte = null;
                if (file != null && file.ContentLength > 0)
                {
                    imgByte = new Byte[file.ContentLength];
                    //force the control to load data in array
                    file.InputStream.Read(imgByte, 0, file.ContentLength);
                    
                }
                Deporte deporte = new Deporte();
                deporte.Nombre = nombre;
                deporte.Activo = activo;
                deporte.Imagen = imgByte;
                var respuesta = await ApiHelper.POST<Deporte>("Deporte/PostDeporte", deporte, (Token)Session["Token"]);

                if (respuesta.IsSuccessStatusCode)
                {
                    return Json("El deporte ha sido guardado satisfactoriamente", JsonRequestBehavior.AllowGet);
                    //List<Deporte> deportes = await test.Content.ReadAsAsync<List<Deporte>>();

                    //var result1 = JsonConvert.DeserializeObject<Deporte>(message);
                    //var result2 = test.Content.ReadAsAsync<Deporte>().Result;


                    //}
                    //var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\WallImages", Server.MapPath(@"\")));

                    //string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "imagepath");

                    //var fileName1 = Path.GetFileName(file.FileName);

                    //bool isExists = System.IO.Directory.Exists(pathString);

                    //if (!isExists)
                    //    System.IO.Directory.CreateDirectory(pathString);

                    //var path = string.Format("{0}\\{1}", pathString, "Prueba");
                    //file.SaveAs(path);

                }
                else
                {
                    return Json("Error al guardar el deporte.", JsonRequestBehavior.AllowGet);
                }
                
            }
            catch (Exception ex)
            {
                return Json("Error al guardar el deporte.", JsonRequestBehavior.AllowGet);
            }
            
        }
        [SessionHandle]
        [HttpPost]
        public async Task<JsonResult> Getdeportes() 
        {
            try
            {
                var lista = await ApiHelper.GET_List<Deporte>("Deporte/Getdeportes", (Session["Token"] as Token));
                return Json(new { Result = "OK",Deportes = lista, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR",Message="Ha ocurrido un error al cargar la lista de deportes."}, JsonRequestBehavior.AllowGet);
            }
        }

    }
}