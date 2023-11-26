using NLog;
using PoliGest.BackEnd.Modelo;
using PoliGest.BackEnd.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace PoliGest.MVVM
{
    class MVInstalacion : MVBaseCRUD<instalacion>
    {
        /* Creamos los objetos privados a usar */
        private GestionPolideportivaEntities gestionEnt;
        private InstalacionServicio instalacionServ;
        private TipoInstalacionServicio tipoServ;
        private ReservaServicio reservaServ;

        private instalacion instalacionSeleccionado = new instalacion();
        private instalacion instala;
        private tipo_instalacion tipoInstalacion;
        private String nombre = String.Empty;

        private ListCollectionView listaAux;

        /* Creamos el log para guardar los posibles errores */
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /* Creamos los objetos y listas de objetos publicas que vamos a usar */
        public List<tipo_instalacion> listaTipoInstalaciones { get { return tipoServ.getAll().ToList(); } }
        public List<instalacion> listaInstalaciones { get { return instalacionServ.getAll().ToList(); } }
        public List<Predicate<instalacion>> criterios { get; set; }
        public ListCollectionView listaTablaInstalacion { get { return listaAux; } }

        /* Creamos un constructor para la clase inicializanfo las variables */
        public MVInstalacion(GestionPolideportivaEntities gestion)
        {
            gestionEnt = gestion;
            inicializa(gestionEnt);
            listaAux = new ListCollectionView(instalacionServ.getAll().ToList());
            criterios = new List<Predicate<instalacion>>();
        }

        public MVInstalacion(GestionPolideportivaEntities gestion, instalacion instalaSel)
        {
            gestionEnt = gestion;
            instalacionSeleccionado = instalaSel;
            inicializa(gestion);
            reservaServ = new ReservaServicio(gestionEnt);
            listaAux = new ListCollectionView(instalacionServ.getAll().ToList());
            criterios = new List<Predicate<instalacion>>();
        }

        private void inicializa(GestionPolideportivaEntities gestion)
        {
            instalacionServ = new InstalacionServicio(gestion);
            servicio = new InstalacionServicio(gestion);
            tipoServ = new TipoInstalacionServicio(gestion);
            instala = new instalacion();
            tipoInstalacion = new tipo_instalacion();
        }

        /* Creamos objetos usados en los binding de los diálogos */

        public instalacion nuevaInstalacion
        {
            get { return instala; }
            set { instala = value; NotifyPropertyChanged(nameof(nuevaInstalacion)); }
        }

        public tipo_instalacion tipoInstalacionSeleccionado
        {
            get { return tipoInstalacion; }
            set { tipoInstalacion = value; NotifyPropertyChanged(nameof(tipoInstalacionSeleccionado)); }
        }

        public String textoNombre
        {
            get { return nombre; }
            set { nombre = value; NotifyPropertyChanged(nameof(textoNombre)); }
        }

        /* Método para guardar nuevas instalaciones */

        public bool guardarInstalacion()
        {
            try
            {
                /* comprobamos que la instalación existe, si existe modificamos la instalación si no, creamos una nueva */
                if (instalacionSeleccionado.nombre != null)
                {
                    /* Comprobamos que la instalación esté abierta en caso contrario anulamos las reservas que usan la instalación y se devuelve el dinero */
                    if (instalacionSeleccionado.insalacion_cerrada == 1)
                    {
                        List<reserva> reservas = reservaServ.getAll()
                            .Where(r => r.instalacion == instalacionSeleccionado && r.fecha_reserva > DateTime.Today || (r.fecha_reserva == DateTime.Today && DateTime.Now.Hour <= r.hora_inicio.Hours) && r.anulado == 0).ToList();
                        foreach (reserva res in reservas)
                        {
                            res.anulado = 1;
                            res.pagado = 0;
                            reservaServ.edit(res);
                            reservaServ.save();
                        }
                    }
                    update(this.instalacionSeleccionado);
                }
                else
                {
                    this.instala.insalacion_cerrada = 0;
                    add(this.instala);

                }
                return true;
            }
            catch (Exception e)
            {
                logger.Error("\n" + e.InnerException + "\n" + e.StackTrace);
                return false;
            }
        }

        /* El método 'filtroCombinadoLista' y 'addCriterios' lo usamos para los fintrados */

        public bool filtroCombinadoLista(Object item)
        {
            instalacion inst = item as instalacion;
            bool esta = true;
            if (criterios.Count() != 0)
            {
                esta = criterios.TrueForAll(x => x(inst));
            }
            return esta;
        }

        public void addCriterios()
        {
            if (tipoInstalacionSeleccionado != null)
            {
                criterios.Add(new Predicate<instalacion>(i => i.tipo_instalacion != null && i.tipo_instalacion.Equals(tipoInstalacionSeleccionado)));
            }
            if (!string.IsNullOrEmpty(textoNombre) && !textoNombre.Equals(""))
            {
                criterios.Add(new Predicate<instalacion>
                    (i => i.nombre != null && i.nombre.ToUpper().Contains(textoNombre.ToUpper())));
            }
        }
        public void crearCopia(instalacion original, instalacion copia)
        {
            copia.idinstalacion = original.idinstalacion;
            copia.nombre = original.nombre;
            copia.insalacion_cerrada = original.insalacion_cerrada;
            copia.idtipo = original.idtipo;
            copia.tipo_instalacion = original.tipo_instalacion;
            copia.reserva = original.reserva;
        }
    }
}
