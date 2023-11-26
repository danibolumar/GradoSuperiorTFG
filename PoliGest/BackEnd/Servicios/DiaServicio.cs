using PoliGest.BackEnd.Modelo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoliGest.BackEnd.Servicios
{
    public class DiaServicio : ServicioGenerico<dia>
    {
        private DbContext contexto;

        /*
         * Constructor
         */
        public DiaServicio(DbContext context) : base(context)
        {
            contexto = context;
        }
    }
}
