using NLog;
using PoliGest.BackEnd.Modelo;
using PoliGest.BackEnd.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PoliGest.MVVM
{
    class MVPermiso : MVBaseCRUD<permisos>
    {
        /* Creamos los objetos privados a usar */
        private GestionPolideportivaEntities gestionEnt;
        private RolServicio rolServ;
        private PermisosServicio permisosServ;
        public List<rol> listaRol { get { return rolServ.getAll().ToList(); } }

        private List<rol> listaRolPermisos = new List<rol>();

        /* Creamos el log para guardar los posibles errores */
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private permisos permiso;

        /* Creamos el constructor de la clase */

        public MVPermiso(GestionPolideportivaEntities gestion)
        {
            gestionEnt = gestion;
            rolServ = new RolServicio(gestion);
            permisosServ = new PermisosServicio(gestion);
            servicio = new PermisosServicio(gestion);
            permiso = new permisos();
        }

        /* Creamos objetos usados en los binding de los diálogos */

        public permisos nuevoPermiso
        {
            get { return permiso; }
            set { permiso = value; NotifyPropertyChanged(nameof(nuevoPermiso)); }
        }
        public List<rol> rolPermisoSeleccionado
        {
            get { return listaRolPermisos; }
            set { listaRolPermisos = value; NotifyPropertyChanged(nameof(rolPermisoSeleccionado)); }
        }

        /* Método para guardar el nuevo permiso*/

        public int guardarNuevoPermiso()
        {
            /* Si ya existe un permiso con esa descripción impedimos que se pueda guardar */
            permisos permExiste = gestionEnt.Set<permisos>().Where(p => p.descripcion == permiso.descripcion).FirstOrDefault();
            if (permExiste != null) return -1;
            if (listaRolPermisos.Count == 0) return -2;
            try
            {
                permiso.rol = new List<rol>();
                permiso.rol = listaRolPermisos;

                add(permiso);
            }
            catch (Exception e)
            {
                logger.Error("\n" + e.InnerException + "\n" + e.StackTrace);
                return -3;
            }
            return 1;
        }
    }
}
