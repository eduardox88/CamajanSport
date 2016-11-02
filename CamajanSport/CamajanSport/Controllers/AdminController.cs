using CamajanSport.App_Start;
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

namespace CamajanSport.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Dashboard()
        {
            return View();
        }


        #region Deportes

        [SessionHandle]
        public ActionResult MantDeportes() 
        {
            return View("MantDeportes");
        }
        #endregion
        [SessionHandle]
        public ActionResult MantUsuarios()
        {
            return View("MantUsuarios");
        }

        public async Task<ActionResult> SaveUploadedFile()
        {
            bool isSavedSuccessfully = true;
            string fName = "";
            string deporte = Request.Form["deporte"];
            try
            {
                string fileName = Request.Files.AllKeys[0];                
                HttpPostedFileBase file = Request.Files[fileName];                
                fName = file.FileName;
                Byte[] imgByte = null;
                if (file != null && file.ContentLength > 0)
                {
                    imgByte = new Byte[file.ContentLength];
                    //force the control to load data in array
                    file.InputStream.Read(imgByte, 0, file.ContentLength);
                    Deporte depo = new Deporte();
                    depo.Nombre = deporte;
                    depo.Imagen = imgByte;
                    HttpClient client = new HttpClient();
                    //api/Deportes
                    HttpResponseMessage test = await client.PostAsync<Deporte>("http://localhost:14678/api/Deportes/PostDeporte",depo,new JsonMediaTypeFormatter());

                    if (test.IsSuccessStatusCode)
                    {

                        List<Deporte> deportes = await test.Content.ReadAsAsync<List<Deporte>>();

                        //var result1 = JsonConvert.DeserializeObject<Deporte>(message);
                        //var result2 = test.Content.ReadAsAsync<Deporte>().Result;


                    }
                    //var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\WallImages", Server.MapPath(@"\")));

                    //string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "imagepath");

                    //var fileName1 = Path.GetFileName(file.FileName);

                    //bool isExists = System.IO.Directory.Exists(pathString);

                    //if (!isExists)
                    //    System.IO.Directory.CreateDirectory(pathString);

                    //var path = string.Format("{0}\\{1}", pathString, "Prueba");
                    //file.SaveAs(path);

                }

                

            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }


            if (isSavedSuccessfully)
            {
                return Json(new { Message = fName });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }
        }

    }
}