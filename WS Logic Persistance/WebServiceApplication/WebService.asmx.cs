using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using LogicPersistance;


namespace WebServiceApplication{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]

    public class WebService : System.Web.Services.WebService{

        /*-------------------------------------------------USUARIOS----------------------------------------------------*/
        //loguea Usuario
        [WebMethod]
        public String LoguearUsuarioAdmin(String logueo, String contraseña)
        {
            String error = "No";
            try
            {
                using (LicenciasEntities DBF = new LicenciasEntities())
                {
                    var user = DBF.Usuario.Where(Usuario => Usuario.Usuario1 == logueo);

                    if (user != null)
                    {
                        foreach (Usuario cliente in user) {
                            if (cliente.Contraseña == contraseña)
                            {
                                //pregunto por rol
                                error = "No";
                            }
                            else {
                                error = "La contraseña no es correcta";
                            }
                        }
                    }
                    else{
                        error = "El usuario no existe";
                    }

                }

                return error;
            }
            catch (Exception ex)
            {
                System.Console.Write(ex.StackTrace);
                return error = ex.Message;
            }
        }

        //Insertar Usuarios
        [WebMethod]
        public String AddUsuario(String nombre, String logueo, String contraseña, String correo, int idRol)
        {
            String error = "No";
            try
            {
                using (LicenciasEntities DBF = new LicenciasEntities())
                {
                    var usuarioABuscar = DBF.Usuario.Where(Usuario => Usuario.Usuario1 == logueo);
                    if (usuarioABuscar.Count() == 0)
                    {
                        Usuario usuario = new Usuario();
                        {
                            usuario.Nombre = nombre;
                            usuario.Usuario1 = logueo;
                            usuario.Contraseña = contraseña;
                            usuario.Correo = correo;
                            DBF.Usuario.Add(usuario);
                            DBF.SaveChanges();
                        }
                        var usuarioABuscarParaAsiganrRol = DBF.Usuario.Where(Usuario => Usuario.Usuario1 == logueo);

                        foreach (Usuario user in usuarioABuscarParaAsiganrRol) {
                            AddRol(idRol,user.IdUsuario);
                        }
                    }
                    else
                    {
                        error = "Ya existe un usuario registrado con ese nombre de usuario.";
                    }

                }

                return error;
            }
            catch (Exception ex)
            {
                System.Console.Write(ex.StackTrace);
                return error = ex.Message;
            }
        }
        //Actualizar usuario
        [WebMethod]
        public void UpdateUsuario(int idUsuario, String nombre, String usuario1, String contrasena, String correo)
        {
            try
            {
                using (LicenciasEntities DBF = new LicenciasEntities())
                {
                    var original = DBF.Usuario.Find(idUsuario);
                    if (original != null)
                    {
                        original.Nombre = nombre;
                        original.Usuario1 = usuario1;
                        original.Contraseña = contrasena;
                        original.Correo = correo;
                        DBF.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.Write(ex.StackTrace);
            }
        }

        //Listar Usuarios
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(Usuario))]
        public List<Usuario> ListUsuarios(){
            List<Usuario> list = null;
            try{
                using (LicenciasEntities DBF = new LicenciasEntities())
                {
                    DBF.Configuration.ProxyCreationEnabled = false;
                    list = DBF.Usuario.ToList();
                }
            }catch (Exception ex){
                System.Console.Write(ex.StackTrace);
            }
            return list;
        }


        //Listar usuario
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(Usuario))]
        public Usuario ListUsuario(int idUsu)
        {
            Usuario user = null;
            try
            {
                using (LicenciasEntities DBF = new LicenciasEntities())
                {
                    DBF.Configuration.ProxyCreationEnabled = false;
                    user = DBF.Usuario.Find(idUsu);
                }
            }
            catch (Exception ex)
            {
                System.Console.Write(ex.StackTrace);
            }
            return user;
        }


        /*-------------------------------------------------ROLES----------------------------------------------------*/

        //Insertar Roles
        [WebMethod]
        public void AddRol(int idRol, int idUsu){
            try{
                using (LicenciasEntities DBF = new LicenciasEntities()){
                    Usuario user    = DBF.Usuario.Find(idUsu);
                    Roles roles     = DBF.Roles.Find(idRol);
                    {
                        roles.Usuario.Add(user);
                        user.Roles.Add(roles);
                        DBF.SaveChanges();
                    }
                    
                }
            }catch (Exception ex){
                System.Console.Write(ex.StackTrace);
            }
        }

        //Quitar Rol
        [WebMethod]
        public void RemoveRol(int idRol, int idUsu)
        {
            try
            {
                using (LicenciasEntities DBF = new LicenciasEntities())
                {
                    Usuario user    = DBF.Usuario.Find(idUsu);
                    Roles roles     = DBF.Roles.Find(idRol);
                    {
                        roles.Usuario.Remove(user);
                        user.Roles.Remove(roles);
                        DBF.SaveChanges();
                    }

                }
            }catch (Exception ex){
                System.Console.Write(ex.StackTrace);
            }
        }


        ///Listar rol
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(Roles))]
        [System.Xml.Serialization.XmlInclude(typeof(Usuario))]
        public List<Roles> ListRol(int idUsu){
            List<Roles> roles = null;
            try{
                using (LicenciasEntities DBF = new LicenciasEntities())
                {
                    DBF.Configuration.ProxyCreationEnabled = false;
                    DBF.Configuration.LazyLoadingEnabled = true;
                    Usuario user = DBF.Usuario.Find(idUsu);
                    //Obtengo la lista de roles para ese id
                    roles = DBF.Roles.SqlQuery("SELECT	r.IdRol, r.Descripcion FROM dbo.Roles r, dbo.UsuarioRol u WHERE   r.IdRol = u.IdRol AND u.IdUsuario = @param1", new SqlParameter("param1", idUsu)).ToList();
                }
            }catch (Exception ex){
                System.Console.Write(ex.StackTrace);
            }
            return roles;
        }


        ///Listar rol
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(Roles))]
        [System.Xml.Serialization.XmlInclude(typeof(Usuario))]
        public List<Roles> ListRoles(){
            List<Roles> roles = null;
            try{
                using (LicenciasEntities DBF = new LicenciasEntities())
                {
                    DBF.Configuration.ProxyCreationEnabled = false;
                    roles = DBF.Roles.ToList();
                }
            }catch (Exception ex){
                System.Console.Write(ex.StackTrace);
            }
            return roles;
        }


        /*-------------------------------------------------CLIENTES----------------------------------------------------*/

        //Insertar Clientes
        [WebMethod]
        public void AddClientes(String nombre, String carpeta){
            try{
                using (LicenciasEntities DBF = new LicenciasEntities())
                {
                    Clientes clientes = new Clientes();
                    {
                        clientes.Nombre = nombre;
                        clientes.Carpeta = carpeta;
                        DBF.Clientes.Add(clientes);
                        DBF.SaveChanges();
                    }
                }
            }catch (Exception ex){
                System.Console.Write(ex.StackTrace);
            }
        }

        //Listar Clientes
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(Clientes))]
        public List<Clientes> ListClientes(){
            List<Clientes> list = null;
            try{
                using (LicenciasEntities DBF = new LicenciasEntities())
                {
                    DBF.Configuration.ProxyCreationEnabled = false;
                    list = DBF.Clientes.ToList();
                }
            }catch (Exception ex){
                System.Console.Write(ex.StackTrace);
            }
            return list;
        }

        /*-------------------------------------------------CONFIGURACIONES----------------------------------------------------*/

        //Insertar Configuraciones
        [WebMethod]
        public void AddConfig(String clave, String valor){
            try{
                using (LicenciasEntities DBF = new LicenciasEntities())
                {
                    Configuraciones conf = new Configuraciones();
                    {
                        conf.Clave = clave;
                        conf.Valor = valor;
                        DBF.Configuraciones.Add(conf);
                        DBF.SaveChanges();
                    }
                }
            }catch (Exception ex){
                System.Console.Write(ex.StackTrace);
            }
        }

        //Listar Configuraciones
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(Configuraciones))]
        public List<Configuraciones> ListConfig(){
            List<Configuraciones> list = null;
            try{
                using (LicenciasEntities DBF = new LicenciasEntities())
                {
                    DBF.Configuration.ProxyCreationEnabled = false;
                    list = DBF.Configuraciones.ToList();
                }
            }catch (Exception ex){
                System.Console.Write(ex.StackTrace);
            }
            return list;
        }

        //Actualizar configuracion
        [WebMethod]
        public void UpdateConfig(String clave, String valor){
            try{
                using (LicenciasEntities DBF = new LicenciasEntities())
                {
                    var original = DBF.Configuraciones.Find(clave.Replace("\\", ""));
                    if (original != null){
                        original.Valor = valor;
                        DBF.SaveChanges();
                    }

                }
            }catch (Exception ex){
                System.Console.Write(ex.StackTrace);
            }
        }

        /*===================================================WEB=========================================================*/

        /*-------------------------------------------------AUDITORIA----------------------------------------------------*/

        //Insertar Auditoría
        [WebMethod]
        public void AddAuditoria(int idRegistro, Nullable<int>numeroLicencia, Nullable<System.DateTime>fecha, Nullable<int>idUsuario, string observacion){
            try{
                using (LicenciasEntities DBF = new LicenciasEntities())
                {
                    Auditoria auditoria = new Auditoria();
                    {
                        auditoria.IdRegistro     = idRegistro;
                        auditoria.NumeroLicencia = numeroLicencia;
                        auditoria.Fecha          = fecha;
                        auditoria.IdUsuario      = idUsuario;
                        auditoria.Observacion    = observacion;
                        DBF.Auditoria.Add(auditoria);
                        DBF.SaveChanges();
                    }
                }
            }catch (Exception ex){
                System.Console.Write(ex.StackTrace);
            }
        }

        /*-------------------------------------------------LICENCIAMIENTO----------------------------------------------------*/
        //Insertar Licenciamiento
        [WebMethod]
        public void AddLicenciamiento(int numLicencia, string nomArch, string nomCliente, string idLicencia, string version, string serialMod, string concurrencia, Nullable<System.DateTime>fecSol, Nullable<System.DateTime>fecGen, Nullable<System.DateTime>fecCad, string respSol, string respGen){
            try{
                using (LicenciasEntities DBF = new LicenciasEntities())
                {
                    Licenciamiento licencia = new Licenciamiento();
                    {
                        licencia.NumeroLicencia         = numLicencia;
                        licencia.NombreArchivo          = nomArch;
                        licencia.NombreCliente          = nomCliente;
                        licencia.IdLicencia             = idLicencia;
                        licencia.Version                = version;
                        licencia.SerialModulos          = serialMod;
                        licencia.Concurrencia           = concurrencia;
                        licencia.FechaSolicitud         = fecSol;
                        licencia.FechaGeneracion        = fecGen;
                        licencia.FechaCaducidad         = fecCad;
                        licencia.ResponsableGeneracion  = respGen;
                        licencia.ResponsableSolicitud   = respSol;
                        DBF.Licenciamiento.Add(licencia);
                        DBF.SaveChanges();
                    }
                }
            }catch (Exception ex){
                System.Console.Write(ex.StackTrace);
            }
        }


        //Actualizar Licenciamiento
        [WebMethod]
        public void UpdateLicenciamiento(int numLicencia, string nomArch, string nomCliente, string idLicencia, string version, string serialMod, string concurrencia, Nullable<System.DateTime> fecSol, Nullable<System.DateTime> fecGen, Nullable<System.DateTime> fecCad, string respSol, string respGen){
            try{
                using (LicenciasEntities DBF = new LicenciasEntities())
                {
                    var original = DBF.Licenciamiento.Find(numLicencia);
                    if (original != null)
                    {
                        original.NumeroLicencia         = numLicencia;
                        original.NombreArchivo          = nomArch;
                        original.NombreCliente          = nomCliente;
                        original.IdLicencia             = idLicencia;
                        original.Version                = version;
                        original.SerialModulos          = serialMod;
                        original.Concurrencia           = concurrencia;
                        original.FechaSolicitud         = fecSol;
                        original.FechaGeneracion        = fecGen;
                        original.FechaCaducidad         = fecCad;
                        original.ResponsableGeneracion  = respGen;
                        original.ResponsableSolicitud   = respSol;
                        DBF.SaveChanges();
                    }
                }
            }catch (Exception ex){
                System.Console.Write(ex.StackTrace);
            }
        }

        //Listar Licenciamientos
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(Licenciamiento))]
        public List<Licenciamiento> ListLicenciamiento()
        {
            List<Licenciamiento> list = null;
            try
            {
                using (LicenciasEntities DBF = new LicenciasEntities())
                {
                    DBF.Configuration.ProxyCreationEnabled = false;
                    list = DBF.Licenciamiento.ToList();
                }
            }
            catch (Exception ex)
            {
                System.Console.Write(ex.StackTrace);
            }
            return list;
        }


    }
}
