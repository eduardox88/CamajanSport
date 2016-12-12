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
    public class EquipoController : ApiController
    {
        private CamajanSportContext db = new CamajanSportContext();

        [Authorize]
        public List<Equipo> GetListEquipos()
        {
            return db.Equipoes.ToList();
        }

        [Authorize]
        public int GetCountEquipos()
        {
            return db.Equipoes.ToList().Count;
        }

        [Authorize]
        public List<SelectAttributes> GetEquipos_Select()
        {
            return db.Equipoes.Select(m => new SelectAttributes { Value = m.idEquipo, DisplayText = m.Nombre }).ToList();
        }
        [Authorize]
        public List<SelectAttributes> GetEquiposByDeporte_Select(int id)
        {
            return db.Equipoes.Where(e => e.idDeporte == id && e.Activo == true).Select(e => new SelectAttributes { Value = e.idEquipo, DisplayText = e.Nombre }).ToList();
        }

        
        [Authorize]
        [ResponseType(typeof(Equipo))]
        public async Task<IHttpActionResult> GetEquipo(int id)
        {
            Equipo equipo = await db.Equipoes.FindAsync(id);
            if (equipo == null)
            {
                return NotFound();
            }

            return Ok(equipo);
        }

        [Authorize]
        public async Task<IHttpActionResult> PutEquipo(Equipo equipo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(equipo).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize]
        [ResponseType(typeof(Equipo))]
        public async Task<IHttpActionResult> PostEquipo(Equipo equipo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Equipoes.Add(equipo);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = equipo.idEquipo }, equipo);
        }

        [Authorize]
        [ResponseType(typeof(Equipo))]
        public async Task<IHttpActionResult> DeleteEquipo(int id)
        {
            Equipo equipo = await db.Equipoes.FindAsync(id);
            if (equipo == null)
            {
                return NotFound();
            }

            db.Equipoes.Remove(equipo);
            await db.SaveChangesAsync();

            return Ok(equipo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EquipoExists(int id)
        {
            return db.Equipoes.Count(e => e.idEquipo == id) > 0;
        }
    }
}