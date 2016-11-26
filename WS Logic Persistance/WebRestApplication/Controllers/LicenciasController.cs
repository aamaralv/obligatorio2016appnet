using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LogicPersistance; 

namespace WebRestApplication.Controllers
{
    public class LicenciasController : ApiController
    {

       
        public IEnumerable<Licenciamiento> GetListLicenciamiento()
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
