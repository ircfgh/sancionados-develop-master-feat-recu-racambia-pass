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
using Cdmx.Scg.Sancionados.Service.Models;

namespace Cdmx.Scg.Sancionados.Service.Controllers
{
    public class TipoOrigenController : ApiController
    {
        private DbContextSan db = new DbContextSan();

        // GET: api/TipoOrigen
        public IQueryable<TipoOrigen> GetTipoOrigen()
        {
            return db.TipoOrigen;
        }

        // GET: api/TipoOrigen/5
        [ResponseType(typeof(TipoOrigen))]
        public async Task<IHttpActionResult> GetTipoOrigen(int id)
        {
            TipoOrigen tipoOrigen = await db.TipoOrigen.FindAsync(id);
            if (tipoOrigen == null)
            {
                return NotFound();
            }

            return Ok(tipoOrigen);
        }

        // PUT: api/TipoOrigen/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTipoOrigen(int id, TipoOrigen tipoOrigen)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipoOrigen.IdTipoOrigen)
            {
                return BadRequest();
            }

            db.Entry(tipoOrigen).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
                
                return StatusCode(HttpStatusCode.NoContent);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoOrigenExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            
        }

        // POST: api/TipoOrigen
        [ResponseType(typeof(TipoOrigen))]
        public async Task<IHttpActionResult> PostTipoOrigen(TipoOrigen tipoOrigen)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TipoOrigen.Add(tipoOrigen);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tipoOrigen.IdTipoOrigen }, tipoOrigen);
        }

        // DELETE: api/TipoOrigen/5
        [ResponseType(typeof(TipoOrigen))]
        public async Task<IHttpActionResult> DeleteTipoOrigen(int id)
        {
            TipoOrigen tipoOrigen = await db.TipoOrigen.FindAsync(id);
            if (tipoOrigen == null)
            {
                return NotFound();
            }

            db.TipoOrigen.Remove(tipoOrigen);
            await db.SaveChangesAsync();

            return Ok(tipoOrigen);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TipoOrigenExists(int id)
        {
            return db.TipoOrigen.Count(e => e.IdTipoOrigen == id) > 0;
        }
    }
}