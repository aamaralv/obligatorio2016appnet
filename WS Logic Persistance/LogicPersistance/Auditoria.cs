
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace LogicPersistance
{

using System;
    using System.Collections.Generic;
    
public partial class Auditoria
{

    public int IdRegistro { get; set; }

    public Nullable<int> NumeroLicencia { get; set; }

    public Nullable<System.DateTime> Fecha { get; set; }

    public Nullable<int> IdUsuario { get; set; }

    public string Observacion { get; set; }



    public virtual Usuario Usuario { get; set; }

    public virtual Licenciamiento Licenciamiento { get; set; }

}

}
