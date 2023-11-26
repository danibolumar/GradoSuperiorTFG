using NLog;
using PoliGest.BackEnd.Modelo;
using PoliGest.BackEnd.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PoliGest.MVVM
{
    class MVReserva : MVBaseCRUD<reserva>
    {
        /* Creamos los objetos privados a usar */
        private GestionPolideportivaEntities gestionEnt;
        private ReservaServicio reservaServ;
        private InstalacionServicio instalacionServ;
        private TipoInstalacionServicio tipoInstalacionServ;
        private InstalacionesHorarioService instalacionHorarioServ;
        private SociosServicio socioServ;

        private reserva reserva;
        private tipo_instalacion tipo;
        private String usuario;
        private String instalacion;
        private horario ini = new horario();
        private horario fin = new horario();
        private DateTime fechaInicial;
        private DateTime fechaFin;
        private usuario usu;
        private int cod;

        private ListCollectionView listaAux;

        /* Creamos las listas privadas */

        private Dictionary<instalacion, List<horario>> tiempoLibrePorInstalacion = new Dictionary<instalacion, List<horario>>();

        private List<instalacion> todasInstalaciones = new List<instalacion>();

        private HashSet<horario> horarioDisponible = new HashSet<horario>();

        public List<reserva> listaRes { get { return reservaServ.getAll().ToList(); } }

        private List<instalaciones_tiene_horario> listaInstalacionHorario { get { return instalacionHorarioServ.getAll().ToList(); } }

        /* Creamos los objetos y listas públicas que vamos a usar */

        public int time;
     
        public List<reserva> instalacionesEnReserva = new List<reserva>();

        public List<horario> listaHorario = new List<horario>();

        
        public List<TimeSpan> listaHoraInicio = new List<TimeSpan>();
        public List<TimeSpan> listaHoraFin = new List<TimeSpan>();


        public HashSet<instalacion> instalacionesUsadas = new HashSet<instalacion>();
        public List<horario> listaHorarioInstalacion = new List<horario>();

        public List<reserva> mostrarListaRes;

        public List<tipo_instalacion> listaTipoInstalaciones { get { return tipoInstalacionServ.getAll().ToList(); } }

        public List<Predicate<reserva>> criterios { get; set; }

        public ListCollectionView listaTablaUsuario { get { return listaAux; } }

        /* Creamos el log para guardar los posibles errores */
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /* Creamos los constructores para esta clase */

        public MVReserva(GestionPolideportivaEntities gestion)
        {
            inicializa(gestion);
            
          
        }
        public MVReserva(GestionPolideportivaEntities gestion, usuario usu, int codigo)
        {

            this.usu = usu;
            cod = codigo;
            inicializa(gestion);

            /* Cogemos del servicio la primera y la ultima fecha de las reservas */
            fechaInicial = reservaServ.primeraFecha(cod, this.usu, DateTime.Today);
            fechaFin = reservaServ.ultimaFecha();
            /* dependiendo del código mostramos todas las reservas o solo las reservas activas */

            if (codigo == 1)
            {
                if (usu.rol.nombre.Equals("Socio")) mostrarListaReserva = reservaServ.getAll().Where(r => r.usuario.dni == usu.dni).ToList();
                else mostrarListaReserva = reservaServ.getAll().ToList();
            }
            else
            {
                if (usu.rol.nombre.Equals("Socio")) mostrarListaReserva = reservaServ.getAll().Where(r => r.usuario.dni == usu.dni && ((r.fecha_reserva > DateTime.Now && r.anulado == 0 && r.no_presentado == 0) || (r.fecha_reserva == fechaInicial && time <= r.hora_inicio.Hours && r.anulado == 0 && r.no_presentado == 0))).ToList();
                else mostrarListaReserva = reservaServ.getAll().Where(r => (r.fecha_reserva > DateTime.Now && r.anulado == 0 && r.no_presentado == 0) || (r.fecha_reserva == fechaInicial && time <= r.hora_inicio.Hours && r.anulado == 0 && r.no_presentado == 0)).ToList();
            }

            
        }

        /* Método usado en los contructores para inicializar los objetos que vamos a usar */
        private void inicializa(GestionPolideportivaEntities gest)
        {
            gestionEnt = gest;
            
            instalacionServ = new InstalacionServicio(gestionEnt);
            reservaServ = new ReservaServicio(gestionEnt);
            servicio = new ReservaServicio(gestionEnt);
            tipoInstalacionServ = new TipoInstalacionServicio(gestionEnt);
            instalacionHorarioServ = new InstalacionesHorarioService(gestionEnt);
            socioServ = new SociosServicio(gestionEnt);
            reserva = new reserva();
            tipo = new tipo_instalacion();
            time = DateTime.Now.Hour;
            listaAux = new ListCollectionView(reservaServ.getAll().ToList());
            criterios = new List<Predicate<reserva>>();
        }

        /* Creamos objetos usados en los binding de los diálogos */
        public List<reserva> mostrarListaReserva
        {
            get { return mostrarListaRes; }
            set { mostrarListaRes = value; NotifyPropertyChanged(nameof(mostrarListaReserva)); }
        }
        public reserva nuevaReserva
        {
            get { return reserva; }
            set { reserva = value; NotifyPropertyChanged(nameof(nuevaReserva)); }
        }

        public tipo_instalacion tipoInstalacionSeleccionado
        {
            get { return tipo; }
            set { tipo = value; NotifyPropertyChanged(nameof(tipoInstalacionSeleccionado)); }
        }

        public String textoNombreSocio
        {
            get { return usuario; }
            set { usuario = value; NotifyPropertyChanged(nameof(textoNombreSocio)); }
        }

        public String textoNombreInstalacion
        {
            get { return instalacion; }
            set { instalacion = value; NotifyPropertyChanged(nameof(textoNombreInstalacion)); }
        }

        public DateTime fechaInicioSeleccinada
        {
            get { return fechaInicial; }
            set { fechaInicial = value; NotifyPropertyChanged(nameof(fechaInicioSeleccinada)); }
        }

        public DateTime fechaFinSeleccionada
        {
            get { return fechaFin; }
            set { fechaFin = value; NotifyPropertyChanged(nameof(fechaFinSeleccionada)); }
        }

        /* Este método devuelve si hay alguna isntalación disponible del tipo que el usuario que elige el usuario y las franjas horarias disponibles*/
        public bool reservasDelTipoInst()
        {
            /* inicializamos los objetos a usar */

            tiempoLibrePorInstalacion = new Dictionary<instalacion, List<horario>>();
            instalacionesEnReserva = new List<reserva>();
            horarioDisponible = new HashSet<horario>();
            ini = new horario();
            fin = new horario();
            instalacionesUsadas = new HashSet<instalacion>();
            listaHorarioInstalacion = new List<horario>();
            listaHorario = new List<horario>();

            /* Cogemos el día de la semana que el usuario ha elegido */

            int diaSemana = (int)nuevaReserva.fecha_reserva.DayOfWeek;

            if (diaSemana == 0) diaSemana = 7;

            /* Cogemos las reservas que hay para el día que el usuario ha elegido y del tipo elegido que no estén anuladas ni no presentadas */
            foreach(reserva res in listaRes)
            {
                if(res.instalacion.tipo_instalacion == tipo && res.fecha_reserva.Date == reserva.fecha_reserva.Date && res.anulado == 0 && res.no_presentado == 0)
                {
                    instalacionesEnReserva.Add(res);
                }
            }
            
            /* cogemos todas las instalaciones que hay del tipo seleccionado */
            todasInstalaciones = instalacionServ.getAll().Where(i => i.tipo_instalacion == tipo).ToList();

            /* listamos cada una de las instalaciones para añadirlo al dictionari con la clave instalación y el valor es una lista de horarios */
            foreach (instalacion instala in todasInstalaciones)
            {
                tiempoLibrePorInstalacion.Add(instala, new List<horario>());

                /* Listamos los horarios que hay del tipo elegido y del dia seleccionado, y si el dia seleccionado es hoy comprobamos que la hora sea posterior a la hora actual y añadimos el horario al dictionary */
                foreach (instalaciones_tiene_horario instH in listaInstalacionHorario)
                {
                    if (instH.tipo_instalacion == tipo && instH.dia_iddia == diaSemana && !(nuevaReserva.fecha_reserva.Date == DateTime.Now.Date && instH.horario.hora_inicio.Hours <= time))
                    {
                       listaHorario.Add(instH.horario);
                        tiempoLibrePorInstalacion[instala].Add(instH.horario);
                    }
                }
            }
           
           /* Cogemos las instalaciones que tienen reserva y eliminamos del dictionay el franjo de horas de la reserva*/
            foreach (reserva res in instalacionesEnReserva)
            {

                ini = gestionEnt.Set<horario>().Where(h => h.hora_inicio == res.hora_inicio).FirstOrDefault();
                fin = gestionEnt.Set<horario>().Where(h => h.hora_fin == res.hora_fin).FirstOrDefault();
                for (int i = ini.idhorario; i <= fin.idhorario; i++)
                {
                    tiempoLibrePorInstalacion[res.instalacion].Remove(gestionEnt.Set<horario>().Where(h => h.idhorario == i).FirstOrDefault());
                }
                /* Y guardamos la cantidad de instalaciones que hay usadas */
                instalacionesUsadas.Add(res.instalacion);
            }


            /* En caso de que todas las instalaciones estén en uso ese dia  */
            if (todasInstalaciones.Count == instalacionesUsadas.Count)
            {
                /* listamos el dictionary para sacar el horario disponible que hay */
                foreach (var lista in tiempoLibrePorInstalacion)
                {
                    for (int i = 0; i < listaHorario.Count(); i++)
                    {
                        if (lista.Value.Contains(listaHorario[i]))
                        {
                            horarioDisponible.Add(listaHorario[i]);
                        }
                    }
                }

                /* Si hay algún horario disponible cargamos el horario, si no hay horario disponible devolvemos false */
                if (horarioDisponible.Count != 0)
                {
                    cargarHorario();

                    return true;
                }
                else return false;

            }
            else
            {
                /* Si no están todas las instalaciones del tipo elegido en uso ponemos como horario disponible la lista horaria de ese tipo */
                for (int i = 0; i < listaHorario.Count(); i++)
                {
                       horarioDisponible.Add(listaHorario[i]);
                }

                /* Si hay algún horario disponible cargamos el horario, si no hay horario disponible devolvemos false */
                if (horarioDisponible.Count != 0)
                {
                    cargarHorario();
                    return true;
                }
                else return false;
               
            }
        }

        /* Este método lista el horario disponible para guardarlo en las listas usadas en los comboBox de los dialogos */
        private void cargarHorario()
        {
            listaHoraInicio = new List<TimeSpan>();
            listaHoraFin = new List<TimeSpan>();
            foreach (horario hora in horarioDisponible)
            {
                    listaHoraInicio.Add(hora.hora_inicio);
                    listaHoraFin.Add(hora.hora_fin);
            }

            listaHoraInicio.Sort();
            listaHoraFin.Sort();
        }

        /* Este método cogemos el rango de horas elegido por el usuario y le asignamos una isntalación */

        public bool asignarInstalacion()
        {
            this.ini = gestionEnt.Set<horario>().Where(h => h.hora_inicio == nuevaReserva.hora_inicio).FirstOrDefault();
            this.fin = gestionEnt.Set<horario>().Where(h => h.hora_fin == nuevaReserva.hora_fin).FirstOrDefault();
            int posicionIni = 0;
            int posicionFin = 0;
            double siguientePosi = 0;
            bool acepta = true;
            List<horario> horarios;

            foreach (var lista in tiempoLibrePorInstalacion)
            {
                horarios = lista.Value.OrderBy(h => h.hora_inicio).ToList();
                listaHorarioInstalacion = new List<horario>();
                acepta = true;
                siguientePosi = ini.hora_inicio.TotalMinutes;
                posicionIni = horarios.IndexOf(ini);
                posicionFin = horarios.IndexOf(fin);

                if (lista.Key.insalacion_cerrada == 1)
                {
                    acepta = false;
                }
                if (posicionIni != -1 && posicionFin != -1)
                {
                    for (int i = posicionIni; i <= posicionFin && acepta; i++)
                    {

                        if (horarios[i].hora_inicio.TotalMinutes != siguientePosi) acepta = false;
                        else listaHorarioInstalacion.Add(lista.Value[i]);

                        siguientePosi = siguientePosi + 60;
                    }
                    if (acepta)
                    {
                        nuevaReserva.instalacion = lista.Key;
                        return true;
                    }

                }
            }
            return false;
        }

        /* Método usado para guardar la reserva */

        public bool guardar(int cod)
        {
            try
            {
               if(cod == 1) add(reserva);
                else update(reserva);
            }
            catch (Exception e)
            {
                logger.Error("\n" + e.InnerException + "\n" + e.StackTrace);
                return false;
            }
            return true;
        }

        /* Método que pone la reserva como no presentado */

        public void noPresentado()
        {
          
            nuevaReserva.no_presentado = 1;
            ponerPenalizacion();
        }

        /* Método usado en caso que una reserva no sea anulada y el usuario de la reserva sea de tipo Socio, se pone una penalización de 15 días */

        public void ponerPenalizacion()
        {
            socios socio;
            if (nuevaReserva.usuario.rol.nombre.Equals("Socio"))
            {
                socio = gestionEnt.Set<socios>().Where(s => s.usuario_idusuario == nuevaReserva.usuario.idusuario).FirstOrDefault();
                socio.penalizado = 1;
                socio.fecha_fin_penalizacion = nuevaReserva.fecha_reserva.AddDays(15).Date;
                socioServ.edit(socio);
                socioServ.save();
            }
            guardar(2);
        }


        /* El método 'filtroCombinadoLista' y 'addCriterios' lo usamos para los fintrados */

        public bool filtroCombinadoLista(Object item)
        {
            reserva res = item as reserva;
            bool esta = true;
            if (criterios.Count() != 0)
            {
                esta = criterios.TrueForAll(x => x(res));
            }
            return esta;
        }

        public void addCriterios()
        {
            if (tipoInstalacionSeleccionado != null)
            {
                criterios.Add(new Predicate<reserva>(r => r.instalacion.tipo_instalacion != null && r.instalacion.tipo_instalacion.Equals(tipoInstalacionSeleccionado)));
            }

            if (!string.IsNullOrEmpty(textoNombreSocio) && !textoNombreSocio.Equals(""))
            {
                criterios.Add(new Predicate<reserva>
                    (r => r.usuario.nombre != null && r.usuario.nombre.ToUpper().Contains(textoNombreSocio.ToUpper()) ||
                    r.usuario.apellido1 != null && r.usuario.apellido1.ToUpper().Contains(textoNombreSocio.ToUpper())
                    || (r.usuario.nombre + ", " + r.usuario.apellido1).ToUpper().Contains(textoNombreSocio.ToUpper())));
            }
            if (!string.IsNullOrEmpty(textoNombreInstalacion) && !textoNombreInstalacion.Equals(""))
            {
                criterios.Add(new Predicate<reserva>(r => r.instalacion.nombre.Contains(textoNombreInstalacion)));
            }

            if (fechaInicioSeleccinada != null)
            {
                criterios.Add(new Predicate<reserva>(r => r.fecha_reserva >= fechaInicioSeleccinada));
            }

            if (fechaInicioSeleccinada != null)
            {
                criterios.Add(new Predicate<reserva>(r => r.fecha_reserva <= fechaFinSeleccionada));
            }
        }

        /* Este método quita el filtro devolviendo los valores que usamos para filtrar a null */
        public void quitarFiltros()
        {
            textoNombreSocio = "";
            textoNombreInstalacion = "";
            tipoInstalacionSeleccionado = null;
            fechaInicioSeleccinada = reservaServ.primeraFecha(cod, usu, DateTime.Today);
            fechaFinSeleccionada = reservaServ.ultimaFecha();
        }
    }
}
