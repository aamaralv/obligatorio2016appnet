using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LogicPersistance;
using WebRestApplication.Models;

namespace WebRestApplication.Controllers
{
    public class AuditoriasController : ApiController
    {
        //api/Auditorias/idRegistro/2/numeroLicencia/1111/fecha/2014-01-01/idUsuario/1/observacion/asdasd
        /*http://localhost:55988/api/Auditorias/3/1111/2014-01-01/1/asdasd*/
        /*{"idRegistro":3,"numeroLicencia":1111,"fecha":"2014-01-01","idUsuario":1,"observacion":"ssssd"}*/


        public HttpResponseMessage PostAddAuditoria(Prueba p)//,System.DateTime fecha, int idUsuario, string observacion)
        {
            try
            {
                
                using (LicenciasEntities DBF = new LicenciasEntities())
                {
                    /*Auditoria auditoria = new Auditoria();
                    {

                        auditoria.IdRegistro = idRegistro;
                        auditoria.NumeroLicencia = numeroLicencia;
                        auditoria.Fecha = fecha;
                        auditoria.IdUsuario = idUsuario;
                        auditoria.Observacion = observacion;
                        DBF.Auditoria.Add(auditoria);
                        DBF.SaveChanges();
                    }*/
                }



                var res = Request.CreateResponse(System.Net.HttpStatusCode.Created);
                res.Content = new StringContent(System.Net.HttpStatusCode.Created.ToString(), System.Text.Encoding.UTF8, "application/json");
                return res;

            }
            catch (Exception ex)
            {
                System.Console.Write(ex.StackTrace);
                var res = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                res.Content = new StringContent(System.Net.HttpStatusCode.BadRequest.ToString(), System.Text.Encoding.UTF8, "application/json");
                return res;

            }
        }

    }
}
