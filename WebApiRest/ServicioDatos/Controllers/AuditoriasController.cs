using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ServicioDatos;

namespace ServicioDatos.Controllers
{
    public class AuditoriasController : ApiController
    {
        private DBLicenciasEntities db = new DBLicenciasEntities();

        // GET: api/Auditorias
        public IQueryable<Auditoria> GetAuditoria()
        {
            return db.Auditoria;
        }

        // GET: api/Auditorias/5
        [ResponseType(typeof(Auditoria))]
        public IHttpActionResult GetAuditoria(int id)
        {
            Auditoria auditoria = db.Auditoria.Find(id);
            if (auditoria == null)
            {
                return NotFound();
            }

            return Ok(auditoria);
        }

        // PUT: api/Auditorias/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAuditoria(int id, Auditoria auditoria)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != auditoria.IdRegistro)
            {
                return BadRequest();
            }

            db.Entry(auditoria).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuditoriaExists(id))
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

        // POST: api/Auditorias
        [ResponseType(typeof(Auditoria))]
        public IHttpActionResult PostAuditoria(Auditoria auditoria)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Auditoria.Add(auditoria);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AuditoriaExists(auditoria.IdRegistro))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = auditoria.IdRegistro }, auditoria);
        }

        // DELETE: api/Auditorias/5
        [ResponseType(typeof(Auditoria))]
        public IHttpActionResult DeleteAuditoria(int id)
        {
            Auditoria auditoria = db.Auditoria.Find(id);
            if (auditoria == null)
            {
                return NotFound();
            }

            db.Auditoria.Remove(auditoria);
            db.SaveChanges();

            return Ok(auditoria);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AuditoriaExists(int id)
        {
            return db.Auditoria.Count(e => e.IdRegistro == id) > 0;
        }
    }
}