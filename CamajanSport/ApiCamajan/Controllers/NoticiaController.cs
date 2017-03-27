using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ApiCamajan.Context;
using CamajanSport.BOL;

namespace ApiCamajan.Controllers
{
    public class NoticiaController : ApiController
    {
        private CamajanSportContext db = new CamajanSportContext();

        // GET: api/Noticia
        public List<Noticia> GetNoticia()
        {
            var noticias = db.Noticias.SqlQuery("select Id,Titulo,'' as Contenido,FechaIngreso, IdUsuario,Activo from Noticias").ToList<Noticia>();
            return noticias;
                /*from n in db.Noticias
                               select n.Id,n.Titulo,n.IdUsuario,n.FechaIngreso;*/
        }

        // GET: api/Noticia/5
        [ResponseType(typeof(Noticia))]
        public async Task<IHttpActionResult> GetNoticia(int id)
        {
            Noticia Noticia = await db.Noticias.FindAsync(id);
            if (Noticia == null)
            {
                return NotFound();
            }

            return Ok(Noticia);
        }

        // PUT: api/Noticia/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutNoticia(Noticia Noticia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if (id != Noticia.Id)
            //{
            //    return BadRequest();
            //}

            db.Entry(Noticia).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //if (!NoticiaExists(id))
                //{
                //    return NotFound();
                //}
                //else
                //{
                    throw;
               // }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Noticia
        [ResponseType(typeof(Noticia))]
        public async Task<IHttpActionResult> PostNoticia(Noticia Noticia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Noticias.Add(Noticia);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = Noticia.Id }, Noticia);
        }

        // DELETE: api/Noticia/5
        [ResponseType(typeof(Noticia))]
        public async Task<IHttpActionResult> DeleteNoticia(int id)
        {
            Noticia Noticia = await db.Noticias.FindAsync(id);
            if (Noticia == null)
            {
                return NotFound();
            }

            db.Noticias.Remove(Noticia);
            await db.SaveChangesAsync();

            return Ok(Noticia);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NoticiaExists(int id)
        {
            return db.Noticias.Count(e => e.Id == id) > 0;
        }
    }
}