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
    public class PublicacionController : ApiController
    {
        private CamajanSportContext db = new CamajanSportContext();
        [Authorize]
        // GET: api/Publicacion
        public List<Publicacion> GetPublicaciones()
        {
            return db.Publicacions.ToList();
        }
        [Authorize]
        // GET: api/Publicacion/5
        [ResponseType(typeof(Publicacion))]
        public async Task<IHttpActionResult> GetPublicacion(int id)
        {
            Publicacion publicacion = await db.Publicacions.FindAsync(id);
            if (publicacion == null)
            {
                return NotFound();
            }

            return Ok(publicacion);
        }
        [Authorize]
        // PUT: api/Publicacion/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPublicacion(Publicacion publicacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if (id != publicacion.IdPublicacion)
            //{
            //    return BadRequest();
            //}

            db.Entry(publicacion).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //if (!PublicacionExists(id))
                //{
                //    return NotFound();
                //}
                //else
                //{
                    throw;
                //}
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
        [Authorize]
        // POST: api/Publicacion
        [ResponseType(typeof(Publicacion))]
        public async Task<IHttpActionResult> PostPublicacion(Publicacion publicacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Publicacions.Add(publicacion);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = publicacion.IdPublicacion }, publicacion);
        }
        [Authorize]
        // DELETE: api/Publicacion/5
        [ResponseType(typeof(Publicacion))]
        public async Task<IHttpActionResult> DeletePublicacion(int id)
        {
            Publicacion publicacion = await db.Publicacions.FindAsync(id);
            if (publicacion == null)
            {
                return NotFound();
            }

            db.Publicacions.Remove(publicacion);
            await db.SaveChangesAsync();

            return Ok(publicacion);
        }
        [Authorize]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [Authorize]
        private bool PublicacionExists(int id)
        {
            return db.Publicacions.Count(e => e.IdPublicacion == id) > 0;
        }
    }
}