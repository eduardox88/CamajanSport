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
using CamajanSport.BOL;
using ApiCamajan.Context;

namespace ApiCamajan.Controllers
{
    public class PaisController : ApiController
    {
        private CamajanSportContext db = new CamajanSportContext();

        // GET api/Pais
        [Authorize]
        public List<Pais> Getpaises()
        {
            return db.paises.ToList();
        }

        public List<SelectAttributes> GetPaises_Select()
        {
            return db.paises.Select(m => new SelectAttributes { Value = m.idPais, DisplayText = m.Nombre }).ToList();
        }

        // GET api/Pais/5
        [ResponseType(typeof(Pais))]
        public async Task<IHttpActionResult> GetPais(int id)
        {
            Pais pais = await db.paises.FindAsync(id);
            if (pais == null)
            {
                return NotFound();
            }

            return Ok(pais);
        }

        // PUT api/Pais/5
        public async Task<IHttpActionResult> PutPais(int id, Pais pais)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(pais).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Pais
        [ResponseType(typeof(Pais))]
        public async Task<IHttpActionResult> PostPais(Pais pais)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.paises.Add(pais);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = pais.idPais }, pais);
        }

        // DELETE api/Pais/5
        [ResponseType(typeof(Pais))]
        public async Task<IHttpActionResult> DeletePais(int id)
        {
            Pais pais = await db.paises.FindAsync(id);
            if (pais == null)
            {
                return NotFound();
            }

            db.paises.Remove(pais);
            await db.SaveChangesAsync();

            return Ok(pais);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PaisExists(int id)
        {
            return db.paises.Count(e => e.idPais == id) > 0;
        }
    }
}