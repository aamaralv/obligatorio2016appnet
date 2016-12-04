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
    public class UsuariosController : ApiController
    {
        private DBLicenciasEntities db = new DBLicenciasEntities();

        // GET: api/Usuarios
        public IQueryable<Usuario> GetUsuario()
        {
            return db.Usuario;
            
        }

        // GET: api/Usuarios/5
        [ResponseType(typeof(Usuario))]
        public IHttpActionResult GetUsuario(String nombre,String contr)
        {
            var user = db.Usuario.Where(Usuario => Usuario.Login == nombre);
            Usuario usuario = new Usuario();
            if (user != null)
            {
                foreach (Usuario cliente in user)
                {
                    if (cliente.Contraseña == contr)
                    {
                        //pregunto por rol
                        usuario.idusuario = cliente.idusuario;
                        usuario.Nombre = cliente.Nombre;
                        usuario.Login = cliente.Login;
                        usuario.Contraseña = cliente.Contraseña;
                        usuario.Correo = cliente.Correo;
                        Roles roles = new Roles();
                        roles.Descripcion = cliente.Roles.ElementAt(0).Descripcion;
                        roles.IdRol = cliente.Roles.ElementAt(0).IdRol;
                        usuario.Roles.Add(roles);
                        //db.Usuario.Find(cliente.idusuario);

                    }
                    else
                    {
                        //error = "La contraseña no es correcta";
                    }

                }
            }
            else
            {
                return NotFound();
            }



            return Ok(usuario);
        }

        // PUT: api/Usuarios/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUsuario(int id, Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuario.idusuario)
            {
                return BadRequest();
            }

            db.Entry(usuario).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
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

        // POST: api/Usuarios
        [ResponseType(typeof(Usuario))]
        public IHttpActionResult PostUsuario(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Usuario.Add(usuario);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = usuario.idusuario }, usuario);
        }

        // DELETE: api/Usuarios/5
        [ResponseType(typeof(Usuario))]
        public IHttpActionResult DeleteUsuario(int id)
        {
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }

            db.Usuario.Remove(usuario);
            db.SaveChanges();

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
            return db.Usuario.Count(e => e.idusuario == id) > 0;
        }
    }
}