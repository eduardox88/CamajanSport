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
    public class UsuarioController : ApiController
    {
        private CamajanSportContext db = new CamajanSportContext();

        // GET api/Usuario
        [Authorize]
        public List<Usuario> Getusuarios()
        {
            return db.usuarios.ToList();
        }

        // GET api/Usuario/5
        [ResponseType(typeof(Usuario))]
        public async Task<IHttpActionResult> GetUsuario(int id)
        {
            Usuario usuario = await db.usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        [ResponseType(typeof(Usuario))]
        [HttpPost]
        public IHttpActionResult GetUsuarioByEmail(Usuario user)
        {
            Usuario usuario =  db.usuarios.Where(m => m.CorreoElec == user.CorreoElec).FirstOrDefault();
            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        [Authorize]
        public int GetCountUsuarios()
        {
            return db.usuarios.ToList().Count;
        }


        // PUT api/Usuario/5
        public async Task<IHttpActionResult> PutUsuario(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            db.Entry(usuario).State = EntityState.Modified;
            db.Entry(usuario).Property(x => x.Contrasena).IsModified = false;

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

        // POST api/Usuario
        [ResponseType(typeof(Usuario))]
        public async Task<IHttpActionResult> PostUsuario(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.usuarios.Add(usuario);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = usuario.IdUsuario }, usuario);
        }

        // DELETE api/Usuario/5
        [ResponseType(typeof(Usuario))]
        public async Task<IHttpActionResult> DeleteUsuario(int id)
        {
            Usuario usuario = await db.usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            db.usuarios.Remove(usuario);
            await db.SaveChangesAsync();

            return Ok(usuario);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UsuarioExists(int id)
        {
            return db.usuarios.Count(e => e.IdUsuario == id) > 0;
        }
    }
}