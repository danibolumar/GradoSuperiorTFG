using PoliGest.BackEnd.Modelo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoliGest.BackEnd.Servicios
{
    public class RolServicio : ServicioGenerico<rol>
    {
        private DbContext contexto;

        /*
         * Constructor
         */
        public RolServicio(DbContext context) : base(context)
        {
            contexto = context;
        }

        /* Esta clase contiene un método llamado “tienePerm” que devuelve un booleano, este método contiene dos parámetros uno de tipo rol y otro de tipo String.
        * El método busca entre todos los permisos que contiene el parámetro de tipo rol y si encuentra un permiso con una descripción igual al valor del parámetro de tipo String devuelve “true”, 
        * si se han buscado todos los permisos del rol y no se ha encontrado ninguno que coincida con el valor del tipo String el método devuelve “false”.
        */
        public bool tienePerm(rol rol, String permiso)
        {
            foreach(permisos perm in rol.permisos)
            {
                if (perm.descripcion.Equals(permiso))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
