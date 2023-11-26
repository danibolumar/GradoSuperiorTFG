using PoliGest.BackEnd.Modelo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoliGest.BackEnd.Servicios
{
    public class PermisosServicio : ServicioGenerico<permisos>
    {
        private DbContext contexto;

        /*
         * Constructor
         */
        public PermisosServicio(DbContext context) : base(context)
        {
            contexto = context;
        }
    }
}
