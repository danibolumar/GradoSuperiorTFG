using NLog;
using PoliGest.BackEnd.Modelo;
using PoliGest.BackEnd.Servicios;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PoliGest.MVVM
{
    class MVTipoInstalacion : MVBaseCRUD<tipo_instalacion>
    {

        /* Declaramos los objetos privados que vamos a usar */

        private GestionPolideportivaEntities gestionEnt;
        private TipoInstalacionServicio tipoServ;
        private HorarioServicio horarioServ;
        private DiaServicio diaServ;

        private tipo_instalacion tipoInstalacion;
        private dia diaSel;
        private horario horarioSel;
        private List<instalaciones_tiene_horario> guardarRelacion;

        private String horaMax = String.Empty;

        private ListCollectionView listaAux;

        private List<horario> listaIn = new List<horario>();

        private List<horario> listaOut = new List<horario>();

        /* Creamos el log para guardar los posibles errores */
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /* Creamos la lista de objetos sacada de la base de datos */

        public List<tipo_instalacion> listTipos { get { return tipoServ.getAll().ToList(); } }

        public List<horario> listHorario { get { return horarioServ.getAll().ToList(); } }

        public List<dia> listDias { get { return diaServ.getAll().ToList(); } }

        /* Generamos objetos publicos a usar */

        public List<Predicate<tipo_instalacion>> criterios { get; set; }
        public ListCollectionView listaTablaInstalacion { get { return listaAux; } }
        public ListCollectionView listaHorarioOut { get { return new ListCollectionView(listaOut); } }
        public ListCollectionView listaHorarioIn { get { return new ListCollectionView(listaIn); } }
        public Dictionary<dia, List<horario>> listaSemanal = new Dictionary<dia, List<horario>>();

        public List<int> listHoraMax = new List<int>();

        public dia diaGuardado = new dia();


        /* Constructor de la clase */
        public MVTipoInstalacion(GestionPolideportivaEntities gestion)
        {
            inicializa(gestion);
        }

        /* Método usado para inicializar los objetos que necesitamos */
        private void inicializa(GestionPolideportivaEntities gestion)
        {
            gestionEnt = gestion;
            diaSel = new dia();
            horarioSel = new horario();
            tipoInstalacion = new tipo_instalacion();
            tipoServ = new TipoInstalacionServicio(gestion);
            servicio = new TipoInstalacionServicio(gestion);
            horarioServ = new HorarioServicio(gestion);
            diaServ = new DiaServicio(gestion);
            foreach (horario h in listHorario)
            {
                listHoraMax.Add(h.idhorario);
            }
            listaAux = new ListCollectionView(tipoServ.getAll().ToList());
            criterios = new List<Predicate<tipo_instalacion>>();
        }

        /* Creamos objetos usados en los binding de los diálogos */
        public tipo_instalacion bindingTipoInstalacion
        {
            get { return tipoInstalacion; }
            set { tipoInstalacion = value; NotifyPropertyChanged(nameof(bindingTipoInstalacion)); }
        }

        public dia diaSeleccionado
        {
            get { return diaSel; }
            set { diaSel = value; NotifyPropertyChanged(nameof(diaSeleccionado)); }
        }

        public horario horarioSeleccionado
        {
            get { return horarioSel; }
            set { horarioSel = value; NotifyPropertyChanged(nameof(horarioSeleccionado)); }
        }

        public String textoDescripcion
        {
            get { return horaMax; }
            set { horaMax = value; NotifyPropertyChanged(nameof(textoDescripcion)); }
        }

        /* Con este método cargamos en dos listas los horarios que tiene cada dia de la semana para este tipo de instalacion */

        public void cargarHorario()
        {

            this.listaIn = new List<horario>();
            this.listaOut = new List<horario>();
            if (listaSemanal.ContainsKey(diaSel))
            {
                for (int i = 0; i < listHorario.Count(); i++)
                {
                    if (listaSemanal[diaSel].Contains(listHorario[i])) listaIn.Add(listHorario[i]);
                    else listaOut.Add(listHorario[i]);
                }
                listaIn = listaSemanal[diaSel];
            }
            else
            {
                listaSemanal.Add(diaSel, listaIn);
                this.listaIn = new List<horario>();
                this.listaOut = listHorario;
            }
        }

        /* Guardamos el horario seleccionado para el dia de la semana seleccionada */

        public void guardarHorario()
        {

            if ((diaGuardado.descripcion != null && listaSemanal.ContainsKey(diaGuardado)) || listaSemanal.ContainsKey(diaSeleccionado))
            {
                listaSemanal.Remove(diaGuardado);
                listaSemanal.Add(diaGuardado, listaIn);
            }
            else
            {
                listaSemanal.Add(diaSel, listaIn);
            }
        }

        /* Con este método dependiendo del código pasamos todo el horario a una lista u otra */

        public void cambiarHorarioList(int cod)
        {
            switch (cod)
            {
                case 1:
                    this.listaOut = new List<horario>();
                    this.listaIn = this.listHorario;
                    break;

                case 2:
                    this.listaIn = new List<horario>();
                    this.listaOut = this.listHorario;
                    break;
            }

            guardarHorario();
        }

        /* Con este método dependiendo del código pasamos la hora seleccionada a una lista u otra */

        public void cambiarHorario(horario hora, int cod)
        {
            switch (cod)
            {
                case 1:
                    if (this.listaOut.Count != 0)
                    {
                        this.listaOut.Remove(hora);
                        this.listaIn.Add(hora);
                    }
                    break;

                case 2:
                    if (this.listaIn.Count != 0)
                    {
                        this.listaIn.Remove(hora);
                        this.listaOut.Add(hora);
                    }
                    break;
            }

            guardarHorario();
        }

        /* Guardamos el nuevo tipo de instalacion */

        public int guardarTipoInstalacion(int cod)
        {
            int listasVacias = 0;
            try
            {
                    guardarRelacion = new List<instalaciones_tiene_horario>();

                    /* listamos el dictionary creando un objeto instalaciones_tiene_horario para guardar la lista de ese objeto con nuestro tipo de instalacion */
                    foreach (var dato in this.listaSemanal)
                    {
                        foreach (horario h in dato.Value)
                        {
                            instalaciones_tiene_horario inst = new instalaciones_tiene_horario();
                            inst.tipo_instalacion = tipoInstalacion;
                            inst.dia = dato.Key;
                            inst.horario = h;
                            this.guardarRelacion.Add(inst);
                        }

                        if (dato.Value.Count == 0) listasVacias++;

                    }

                    if (listaSemanal.Count == listasVacias) return -1;

                if(cod == 1) add(tipoInstalacion);
                else update(tipoInstalacion);

                tipo_instalacion cogerId = gestionEnt.Set<tipo_instalacion>().Where(t => t.descripcion == tipoInstalacion.descripcion).FirstOrDefault();

                    cogerId.instalaciones_tiene_horario = guardarRelacion;
                    update(cogerId);
                    return 1;
                
            }
            catch (DbUpdateException dbex)
            {
                logger.Error("\n" + dbex.InnerException + "\n" + dbex.StackTrace);
                return -2;
            }
        }
    
        /* Métodos que usamos para el filtrado de tipo instalación */

        public bool filtroCombinadoLista(Object item)
        {
            tipo_instalacion tipo = item as tipo_instalacion;
            bool esta = true;
            if (criterios.Count() != 0)
            {
                esta = criterios.TrueForAll(x => x(tipo));
            }
            return esta;
        }

        public void addCriterios()
        {
            if (horarioSeleccionado != null)
            {
                criterios.Add(new Predicate<tipo_instalacion>(t => t.tiempo_max != 0 && t.tiempo_max == horarioSeleccionado.idhorario ));
            }
            if (!string.IsNullOrEmpty(textoDescripcion) && !textoDescripcion.Equals(""))
            {
                criterios.Add(new Predicate<tipo_instalacion>
                    (t => t.descripcion != null && t.descripcion.ToUpper().Contains(textoDescripcion.ToUpper())));
            }
        }

        /* Cargamos la lista de los días de la semana con los horarios que tienen cada uno para el tipo seleccionado */

       public void cargarListaSemanal()
       {
            this.listaSemanal = new Dictionary<dia, List<horario>>();
            foreach(instalaciones_tiene_horario h in bindingTipoInstalacion.instalaciones_tiene_horario)
            {

                if (listaSemanal.ContainsKey(h.dia))
                {
                    listaSemanal[h.dia].Add(h.horario);
                }
                else
                {
                    listaSemanal.Add(h.dia, new List<horario>());
                    listaSemanal[h.dia].Add(h.horario);
                }
            }
       }

        public void crearCopia(tipo_instalacion original, tipo_instalacion copia)
        {
            copia.idtipo = original.idtipo;
            copia.descripcion = original.descripcion;
            copia.tiempo_max = original.tiempo_max;
            copia.precio_hora = original.precio_hora;
            copia.instalacion = original.instalacion;
            copia.instalaciones_tiene_horario = original.instalaciones_tiene_horario;
        }
    }
}
