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
    public class LicenciamientosController : ApiController
    {
        private DBLicenciasEntities db = new DBLicenciasEntities();

        // GET: api/Licenciamientos

        public IEnumerable<Licenciamiento> GetLicenciamiento()
        {
            List<Licenciamiento> list = null;
            try { 
          
                    db.Configuration.ProxyCreationEnabled = false;
                    list = db.Licenciamiento.ToList();
                
            }
            catch (Exception ex)
            {
                System.Console.Write(ex.StackTrace);
            }
            return list;
        }


        // GET: api/Licenciamientos/5
        [ResponseType(typeof(Licenciamiento))]
        public IHttpActionResult GetLicenciamiento(int id)
        {
            Licenciamiento licenciamiento = db.Licenciamiento.Find(id);
            if (licenciamiento == null)
            {
                return NotFound();
            }

            return Ok(licenciamiento);
        }

        // PUT: api/Licenciamientos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLicenciamiento(int id, Licenciamiento licenciamiento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != licenciamiento.NumeroLicencia)
            {
                return BadRequest();
            }

            db.Entry(licenciamiento).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LicenciamientoExists(id))
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

        // POST: api/Licenciamientos
        [ResponseType(typeof(Licenciamiento))]
        public IHttpActionResult PostLicenciamiento(Licenciamiento licenciamiento)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Licenciamiento.Add(licenciamiento);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                Clientes clientes = db.Clientes.Find(licenciamiento.NombreCliente);
                if (clientes == null)
                {
                    return NotFound();
                }

                if (LicenciamientoExists(licenciamiento.NumeroLicencia))
                {
                    return Conflict();
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = licenciamiento.NumeroLicencia }, licenciamiento);
        }

        // DELETE: api/Licenciamientos/5
        [ResponseType(typeof(Licenciamiento))]
        public IHttpActionResult DeleteLicenciamiento(int id)
        {
            Licenciamiento licenciamiento = db.Licenciamiento.Find(id);
            if (licenciamiento == null)
            {
                return NotFound();
            }

            db.Licenciamiento.Remove(licenciamiento);
            db.SaveChanges();

            return Ok(licenciamiento);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LicenciamientoExists(int id)
        {
            return db.Licenciamiento.Count(e => e.NumeroLicencia == id) > 0;
        }
        
    }
}