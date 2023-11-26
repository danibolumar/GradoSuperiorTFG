using PoliGest.BackEnd.Modelo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoliGest.BackEnd.Servicios
{
    class UsuarioServicio : ServicioGenerico<usuario>
    {
        private DbContext contexto;
        public usuario usuLog { get; set; }
        /*
         * Constructor
         */
        public UsuarioServicio(DbContext context) : base(context)
        {
            contexto = context;
        }

        /*
         * Este método tiene dos parámetros de tipo String, uno para el nombre de usuario que será el DNI del usuario “username”, el otro parámetro se usa para la contraseña “password”. 

        * La primera parte del método se basa en usar la variable contexto para encontrar un usuario donde el parámetro “username” sea igual al dni y guardar el usuario que coincide en la variable “usuLog”.

        * La segunda parte del método consta en comprobar si la variable “usuLog” no es nula y en caso de que “usuLog” contenga un usuario se comprueba que la contraseña del usuario es igual al valor del parámetro “password”, en caso de que coincida el método devuelve “true”, si no, el método devolverá “false”.
        */
        public bool login(String username, String password)
        {
            try
            {
                usuLog = contexto.Set<usuario>().Where(u => u.dni == username).FirstOrDefault();
            }catch(Exception e)
            {
                System.Console.WriteLine(e.StackTrace);
            }

            if(usuLog != null && usuLog.contraseña.Equals(password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
