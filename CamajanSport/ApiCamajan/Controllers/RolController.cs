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
    public class RolController : ApiController
    {
        private CamajanSportContext db = new CamajanSportContext();

        // GET api/Rol
        [Authorize]
        public List<Rol> Getroles()
        {
            return db.roles.ToList();
        }

        public List<SelectAttributes> GetRoles_Select()
        {
            return db.roles.Select(m => new SelectAttributes{ Value = m.IdRol, DisplayText = m.Nombre }).ToList();
        }

        // GET api/Rol/5
        [ResponseType(typeof(Rol))]
        public async Task<IHttpActionResult> GetRol(int id)
        {
            Rol rol = await db.roles.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }

            return Ok(rol);
        }

        // PUT api/Rol/5
        public async Task<IHttpActionResult> PutRol(int id, Rol rol)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rol.IdRol)
            {
                return BadRequest();
            }

            db.Entry(rol).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RolExists(id))
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

        // POST api/Rol
        [ResponseType(typeof(Rol))]
        public async Task<IHttpActionResult> PostRol(Rol rol)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.roles.Add(rol);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = rol.IdRol }, rol);
        }

        // DELETE api/Rol/5
        [ResponseType(typeof(Rol))]
        public async Task<IHttpActionResult> DeleteRol(int id)
        {
            Rol rol = await db.roles.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }

            db.roles.Remove(rol);
            await db.SaveChangesAsync();

            return Ok(rol);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RolExists(int id)
        {
            return db.roles.Count(e => e.IdRol == id) > 0;
        }
    }
}