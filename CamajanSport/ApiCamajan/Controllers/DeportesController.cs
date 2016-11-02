﻿using System;
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
    public class DeportesController : ApiController
    {
        private CamajanSportContext db = new CamajanSportContext();

        // GET: api/Deportes
        [Authorize]
        public IQueryable<Deporte> Getdeportes()
        {
            return db.deportes;
        }

        // GET: api/Deportes/5
        [ResponseType(typeof(Deporte))]
        [Authorize]
        public async Task<IHttpActionResult> GetDeporte(int id)
        {
            Deporte deporte = await db.deportes.FindAsync(id);
            if (deporte == null)
            {
                return NotFound();
            }

            return Ok(deporte);
        }

        // PUT: api/Deportes/5
        [ResponseType(typeof(void))]
        [Authorize]
        public async Task<IHttpActionResult> PutDeporte(int id, Deporte deporte)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != deporte.IdDeporte)
            {
                return BadRequest();
            }

            db.Entry(deporte).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeporteExists(id))
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

        // POST: api/Deportes
        [ResponseType(typeof(Deporte))]
        [Authorize]
        public async Task<IHttpActionResult> PostDeporte(Deporte deporte)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.deportes.Add(deporte);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = deporte.IdDeporte }, deporte);
        }

        // DELETE: api/Deportes/5
        [ResponseType(typeof(Deporte))]
        [Authorize]
        public async Task<IHttpActionResult> DeleteDeporte(int id)
        {
            Deporte deporte = await db.deportes.FindAsync(id);
            if (deporte == null)
            {
                return NotFound();
            }

            db.deportes.Remove(deporte);
            await db.SaveChangesAsync();

            return Ok(deporte);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [Authorize]
        private bool DeporteExists(int id)
        {
            return db.deportes.Count(e => e.IdDeporte == id) > 0;
        }
    }
}