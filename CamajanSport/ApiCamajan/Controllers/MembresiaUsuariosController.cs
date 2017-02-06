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
    public class MembresiaUsuariosController : ApiController
    {
        private CamajanSportContext db = new CamajanSportContext();

        // GET: api/MembresiaUsuarios
        public IQueryable<MembresiaUsuario> GetMembresiaUsuarios()
        {
            return db.MembresiaUsuarios;
        }
        
        public List<MembresiaUsuario> GetMembresiasUsuarioById(int id)
        {
            return db.MembresiaUsuarios.Where(m => m.IdUsuario == id).ToList();
        }
        [ResponseType(typeof(MembresiaUsuario))]
        public MembresiaUsuario ObtieneMembresiaActiva(int id)
        {
            return db.MembresiaUsuarios.Where(m => m.IdUsuario == id && m.FechaExpiracion >= DateTime.Now).FirstOrDefault();
        }
        [ResponseType(typeof(MembresiaUsuario))]
        [HttpPut]
        public async Task<IHttpActionResult> RenovarMembresiaUsuario(MembresiaUsuario membresiaUsuario) 
        {
            var membresiaActual = db.MembresiaUsuarios.Where(m => m.IdMembresiaUsuario == membresiaUsuario.IdMembresiaUsuario).FirstOrDefault();
            TimeSpan tSpanDiferencia = DateTime.Now.Subtract(membresiaActual.FechaExpiracion);
            membresiaActual.FechaExpiracion = membresiaActual.FechaExpiracion.AddDays(membresiaUsuario.Duracion + tSpanDiferencia.Days);
            membresiaActual.Renovada = true;

            db.Entry(membresiaActual).State = EntityState.Modified;

            await db.SaveChangesAsync();
            return Ok(membresiaActual); ;

        }
        // GET: api/MembresiaUsuarios/5
        [ResponseType(typeof(MembresiaUsuario))]
        public async Task<IHttpActionResult> GetMembresiaUsuario(int id)
        {
            MembresiaUsuario membresiaUsuario = await db.MembresiaUsuarios.FindAsync(id);
            if (membresiaUsuario == null)
            {
                return NotFound();
            }

            return Ok(membresiaUsuario);
        }

        // GET: api/MembresiaUsuarios/5
        [ResponseType(typeof(MembresiaUsuario))]
        public MembresiaUsuario GetMembresiaUsuarioByTransactionId(string IdTransaction)
        {
            MembresiaUsuario membresiaUsuario = db.MembresiaUsuarios.First(m => m.IdTransaccionPago == IdTransaction);

            return membresiaUsuario;
        }

        // PUT: api/MembresiaUsuarios/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMembresiaUsuario(int id, MembresiaUsuario membresiaUsuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != membresiaUsuario.IdMembresiaUsuario)
            {
                return BadRequest();
            }

            db.Entry(membresiaUsuario).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MembresiaUsuarioExists(id))
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

        // POST: api/MembresiaUsuarios
        [ResponseType(typeof(MembresiaUsuario))]
        public async Task<IHttpActionResult> PostMembresiaUsuario(MembresiaUsuario membresiaUsuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            membresiaUsuario.FechaIngreso = DateTime.Now;
            db.MembresiaUsuarios.Add(membresiaUsuario);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = membresiaUsuario.IdMembresiaUsuario }, membresiaUsuario);
        }

        // DELETE: api/MembresiaUsuarios/5
        [ResponseType(typeof(MembresiaUsuario))]
        public async Task<IHttpActionResult> DeleteMembresiaUsuario(int id)
        {
            MembresiaUsuario membresiaUsuario = await db.MembresiaUsuarios.FindAsync(id);
            if (membresiaUsuario == null)
            {
                return NotFound();
            }

            db.MembresiaUsuarios.Remove(membresiaUsuario);
            await db.SaveChangesAsync();

            return Ok(membresiaUsuario);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MembresiaUsuarioExists(int id)
        {
            return db.MembresiaUsuarios.Count(e => e.IdMembresiaUsuario == id) > 0;
        }
    }
}