//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PoliGest.BackEnd.Modelo
{
    using PoliGest.MVVM;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class socios : PropertyChangedDataError
    {
        public int idsocios { get; set; }

        [StringLength(45, ErrorMessage = "El texto es demasiado grande para este campo")]
        public string observaciones { get; set; }
        public Nullable<sbyte> penalizado { get; set; }
        public Nullable<System.DateTime> fecha_fin_penalizacion { get; set; }
        public int usuario_idusuario { get; set; }
    
        public virtual usuario usuario { get; set; }
    }
}