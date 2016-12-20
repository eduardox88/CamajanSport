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
    public class MembresiaController : ApiController
    {
        private CamajanSportContext db = new CamajanSportContext();

        // GET: api/Membresia
        public List<Membresia> GetMembresias()
        {
            return db.membresias.ToList();
        }

        // GET: api/Membresia/5
        [ResponseType(typeof(Membresia))]
        public async Task<IHttpActionResult> GetMembresia(int id)
        {
            Membresia membresia = await db.membresias.FindAsync(id);
            if (membresia == null)
            {
                return NotFound();
            }

            return Ok(membresia);
        }

        // PUT: api/Membresia/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMembresia(Membresia membresia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if (id != membresia.IdMembresia)
            //{
            //    return BadRequest();
            //}

            db.Entry(membresia).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MembresiaExists(membresia.IdMembresia))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Membresia
        [ResponseType(typeof(Membresia))]
        public async Task<IHttpActionResult> PostMembresia(Membresia membresia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.membresias.Add(membresia);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = membresia.IdMembresia }, membresia);
        }

        // DELETE: api/Membresia/5
        [ResponseType(typeof(Membresia))]
        public async Task<IHttpActionResult> DeleteMembresia(int id)
        {
            Membresia membresia = await db.membresias.FindAsync(id);
            if (membresia == null)
            {
                return NotFound();
            }

            db.membresias.Remove(membresia);
            await db.SaveChangesAsync();

            return Ok(membresia);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MembresiaExists(int id)
        {
            return db.membresias.Count(e => e.IdMembresia == id) > 0;
        }
    }
}