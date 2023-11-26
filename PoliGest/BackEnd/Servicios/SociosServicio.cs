using PoliGest.BackEnd.Modelo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoliGest.BackEnd.Servicios
{
    class SociosServicio : ServicioGenerico<socios>
    {
        private DbContext contexto;

        /*
         * Constructor
         */
        public SociosServicio(DbContext context) : base(context)
        {
            contexto = context;
        }
    }
}
