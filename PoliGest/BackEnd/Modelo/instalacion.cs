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

    public partial class instalacion : PropertyChangedDataError
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public instalacion()
        {
            this.reserva = new HashSet<reserva>();
        }
    
        public int idinstalacion { get; set; }

        [StringLength(45, ErrorMessage = "La descrición es demasiado larga")]
        [Required(ErrorMessage = "Este campo obligatorio")]
        public string nombre { get; set; }
        public Nullable<sbyte> insalacion_cerrada { get; set; }
        public int idtipo { get; set; }

        [Required(ErrorMessage = "Este campo obligatorio")]
        public virtual tipo_instalacion tipo_instalacion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<reserva> reserva { get; set; }
    }
}
