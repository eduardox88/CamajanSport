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
        public List<SelectAttributes> GetEstadosResultado_Select()
        {
            return db.estadosResultado.Select(m => new SelectAttributes { Value = m.IdEstadoResultado, DisplayText = m.Descripcion}).ToList();
        }

        [Authorize]
        // GET: api/Publicacion
        public List<Publicacion> GetPublicacionesByFiltro(string FechaJuego,int IdDeporte,int IdEstadoResultado,string TipoPublicacion)
        {

            var query = from s in db.Publicacions
                            select s;            
            if (FechaJuego != null)
            {
                DateTime fecha = Convert.ToDateTime(FechaJuego);
                query = query.Where(f => f.FechaJuego == fecha);
            }
            if (IdDeporte > 0)
            {
                query = query.Where(f => f.Equipo1.idDeporte == IdDeporte);
            }
            if (TipoPublicacion != null)
            {
                bool EsPremium = Convert.ToBoolean(int.Parse(TipoPublicacion));
                query = query.Where(f => f.EsPremium == EsPremium);
            }
            if (IdEstadoResultado > 0)
            {
                query = query.Where(f => f.IdEstadoResultado == IdEstadoResultado);
            }
            return query.ToList();
            
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
        // GET: api/Publicacion/5
        [ResponseType(typeof(Publicacion))]
        public async Task<IHttpActionResult> CambiarEstatus(Publicacion pub)
        {
            Publicacion publicacion = await db.Publicacions.FindAsync(pub.IdPublicacion);
            if (publicacion == null)
            {
                return NotFound();
            }
            publicacion.IdEstadoResultado = pub.IdEstadoResultado;            
            try
            {
                await db.SaveChangesAsync();
                return Ok(publicacion);
            }
            catch (Exception)
            {
                throw;
            }
            
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