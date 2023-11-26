using PoliGest.BackEnd.Modelo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoliGest.BackEnd.Servicios
{
    class TipoInstalacionServicio : ServicioGenerico<tipo_instalacion>
    {
        private DbContext contexto;

        /*
         * Constructor
         */
        public TipoInstalacionServicio(DbContext context) : base(context)
        {
            contexto = context;
        }
    }
}
