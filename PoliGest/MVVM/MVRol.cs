using NLog;
using PoliGest.BackEnd.Modelo;
using PoliGest.BackEnd.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace PoliGest.MVVM
{
    class MVRol : MVBaseCRUD<rol>
    {
        /* Creamos los objetos privados que vamos a necesitar */

        private GestionPolideportivaEntities gestionEnt;
        private PermisosServicio permisosServ;

        /* Creamos el log para guardar los posibles errores */
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private rol rol;
        private permisos permisos;

        private List<permisos> listaOut = new List<permisos>();

        private List<permisos> listaIn = new List<permisos>();

        private List<permisos> listaPermisosRol = new List<permisos>();

        /* Creamos las listas públicas a usar */

        public List<rol> listaRol { get { return servicio.getAll().ToList(); } }

        public List<permisos> listaPermisos { get { return permisosServ.getAll().ToList(); } }

        public ListCollectionView listaPermisosOut { get { return new ListCollectionView(listaOut); } }

        public ListCollectionView listaPermisosIn { get { return new ListCollectionView(listaIn); } }

        /* Creamos el contructor de la clase */

        public MVRol(GestionPolideportivaEntities gestion)
        {
            gestionEnt = gestion;
            inicializa();
        }

        /* Método para inicializar todos los objetos*/

        private void inicializa()
        {
            rol = new rol();
            permisos = new permisos();
            servicio = new RolServicio(gestionEnt);
            permisosServ = new PermisosServicio(gestionEnt);
            listaOut = new List<permisos>();
            listaIn = new List<permisos>();
        }

        /* Creamos objetos usados en los binding de los diálogos */
        public rol rolSeleccionado
        {
            get { return rol; }
            set { rol = value; NotifyPropertyChanged(nameof(rolSeleccionado)); }
        }

        public List<permisos> permisosRolSeleccionado
        {
            get { return listaPermisosRol; }
            set { listaPermisosRol = value; NotifyPropertyChanged(nameof(permisosRolSeleccionado)); }
        }

        public permisos permisosSeleccionado
        {
            get { return permisos; }
            set { permisos = value; NotifyPropertyChanged(nameof(permisosSeleccionado)); }
        }

        /* Con este método cargamos en dos listas con los permisos que tiene el rol */
        public void editarPermisos()
        {
            this.listaIn = new List<permisos>();
            this.listaOut = new List<permisos>();

            for (int i = 0; i < listaPermisos.Count(); i++)
            {
                if (rolSeleccionado.permisos.Contains(listaPermisos[i])) listaIn.Add(listaPermisos[i]);
                else listaOut.Add(listaPermisos[i]);
            }
        }

        /* Método para guardar el nuevo rol con la lista de permisos */
        public int guardarNuevoRol()
        {
            rol rolExiste = gestionEnt.Set<rol>().Where(r => r.nombre == rol.nombre).FirstOrDefault();
            if (rolExiste != null) return -1;
            if (listaPermisosRol.Count == 0) return -2;
            try
            {
                rol.permisos = new List<permisos>();
                rol.permisos = listaPermisosRol;

                add(rol);
            }
            catch (Exception dbex)
            {
                logger.Error("\n" + dbex.InnerException + "\n" + dbex.StackTrace);
                return -3;
            }
            return 1;
        }

        /* Método para borrar los permisos */

        public void borrarPermiso()
        {
            foreach (rol rol in listaRol)
            {
                rol.permisos.Remove(permisos);
            }
            permisosServ.delete(permisos);
            permisosServ.save();
        }

        /* Con este método dependiendo del código pasamos todo el horario a una lista u otra */
        public void cambiarPermisosList(int cod)
        {
            switch (cod)
            {
                case 1:
                    this.listaOut = new List<permisos>();
                    this.listaIn = this.listaPermisos;
                    break;

                case 2:
                    this.listaIn = new List<permisos>();
                    this.listaOut = this.listaPermisos;
                    break;
            }
        }

        /* Con este método dependiendo del codigo pasamos el permiso seleccionada a una lista u otra */
        public void cambiarPermisos(permisos perm, int cod)
        {
            switch (cod)
            {
                case 1:
                    if (this.listaOut.Count != 0)
                    {
                        this.listaOut.Remove(perm);
                        this.listaIn.Add(perm);
                    }
                    break;

                case 2:
                    if (this.listaIn.Count != 0)
                    {
                        this.listaIn.Remove(perm);
                        this.listaOut.Add(perm);
                    }
                    break;
            }
        }

        /* Método que usamos para guardar un rol modificado */
        public int guardarRol()
        {
            if (listaIn.Count == 0) return -1;
            try
            {
                this.rol.permisos = this.listaIn;
                update(rol);
            }
            catch (Exception dbex)
            {

                logger.Error("\n" + dbex.InnerException + "\n" + dbex.StackTrace);
                return -2;
            }
            return 1;
        }

    }
}
