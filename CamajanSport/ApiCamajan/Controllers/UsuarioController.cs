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

        [HttpPost]
        public bool ExisteUsuarioPorCorreo(Usuario user)
        {
            bool existe = db.usuarios.Count(m => m.CorreoElec == user.CorreoElec) > 0;

            return existe;
        }

        [HttpPost]
        public bool ExisteUsuarioPorNombre(Usuario user)
        {
            bool existe = db.usuarios.Count(m => m.NombreUsuario == user.NombreUsuario) > 0;

            return existe;
        }

        [Authorize]
        public int GetCountUsuarios()
        {
            return db.usuarios.ToList().Count;
        }


        [Authorize]
        [Route("api/Usuario/GetUsuariosByEstado/{idEstado}")]
        public int GetUsuariosByEstado(int idEstado)
        {
            return db.usuarios.Count(m => m.IdEstado == idEstado);
        }

        [Authorize]
        [ResponseType(typeof(byte []))]
        [Route("api/Usuario/GetImagenUsuario/{CodUsuario}")]
        public IHttpActionResult GetImagenUsuario(int CodUsuario)
        {
             Usuario usuario = db.usuarios.Where(m => m.IdUsuario == CodUsuario).FirstOrDefault();

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario.Imagen);
        }

        // PUT api/Usuario/5
        [Authorize]
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

        [HttpPut]
        public void ConfirmacionUsuario(Usuario usuario)
        {
       
            db.usuarios.Attach(usuario);
            db.Entry(usuario).Property(x => x.IdEstado).IsModified = true;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
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

            return Ok(usuario.IdUsuario);
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