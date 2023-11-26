using PoliGest.BackEnd.Modelo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoliGest.BackEnd.Servicios
{
    public class ReservaServicio : ServicioGenerico<reserva>
    {
        private DbContext contexto;

        /*
         * Constructor
         */
        public ReservaServicio(DbContext context) : base(context)
        {
            contexto = context;
        }

        /*
         * Este método contiene usa tres parámetros, uno de tipo int que contiene un código, otra variable de tipo usuario y otro parámetro de tipo DateTime. 

         * Dependiendo del valor del parámetro del tipo int obtenemos la primera reserva guardada de la base de datos o la primera reserva la cual la fecha de la misma sea igual o superior al valor del parámetro DateTime.
		
         * En caso de que el nombre del rol del parámetro de tipo usuario sea “Socio” tendremos, además, que usar sólo las reservas creadas por el usuario pasado en el parámetro cuando busquemos la primera reserva.

         * Una vez obtenida la reserva devolvemos la fecha de la reserva.

         */
        public DateTime primeraFecha(int cod, usuario user, DateTime dia)
        { 
            try{
                reserva rev;
                if (cod == 1)
                {
                    if(user.rol.nombre.Equals("Socio")) rev = contexto.Set<reserva>().OrderBy(x => x.fecha_reserva).Where(r => r.usuario.dni == user.dni).First();
                    else rev = contexto.Set<reserva>().OrderBy(x => x.fecha_reserva).First();
                }
                else
                {
                    if (user.rol.nombre.Equals("Socio")) rev = contexto.Set<reserva>().OrderBy(x => x.fecha_reserva).Where(r => r.usuario.dni == user.dni && r.fecha_reserva >= dia).First();
                    else rev = contexto.Set<reserva>().OrderBy(x => x.fecha_reserva).Where(r => r.fecha_reserva >= dia).First();
                }
            
                return (DateTime)rev.fecha_reserva;
            } catch(InvalidOperationException i)
            {
                return contexto.Set<reserva>().OrderBy(x => x.fecha_reserva).First().fecha_reserva;
            }
        }

        /*
         * Este método desde la variable “contexto” ordena de forma descendente todas las reservas y coge la primera que encuentre siendo esta la última reserva realizada.
         */
        public DateTime ultimaFecha()
        {
            reserva rev = contexto.Set<reserva>().OrderByDescending(x => x.fecha_reserva).First();
            return (DateTime)rev.fecha_reserva;
        }
    }
}
