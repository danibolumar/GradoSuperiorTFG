using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using PoliGest.BackEnd.Modelo;
using PoliGest.MVVM;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PoliGest.FrontEnd.Dialogos
{
    /// <summary>
    /// Lógica de interacción para DiagInstalacion.xaml
    /// </summary>
    public partial class DiagInstalacion : MetroWindow
    {
        private GestionPolideportivaEntities gestionEnt;
        private MVInstalacion mvInstalacion;
        private instalacion instalacionSele = new instalacion();
        int cerrar = 0;

        public DiagInstalacion(GestionPolideportivaEntities gestion)
        {
            InitializeComponent();
            gestionEnt = gestion;
            mvInstalacion = new MVInstalacion(gestionEnt);
            inicializa();
            this.cerrarInstalacion.Visibility = Visibility.Collapsed;
        }

        public DiagInstalacion(GestionPolideportivaEntities gestion, instalacion instalacionSeleccionada)
        {
            InitializeComponent();
            instalacionSele = instalacionSeleccionada;
            gestionEnt = gestion;
            mvInstalacion = new MVInstalacion(gestionEnt, instalacionSele);
            inicializa();
            cambiaBoton(Convert.ToInt32(instalacionSele.insalacion_cerrada));
            mvInstalacion.nuevaInstalacion = instalacionSele;
        }

        private void inicializa()
        {

            this.AddHandler(Validation.ErrorEvent, new RoutedEventHandler(mvInstalacion.OnErrorEvent));
            DataContext = mvInstalacion;
            mvInstalacion.btnGuardar = this.aceptar;
        }


        /* Este método se encarga de comprobar que la nueva instalación pertenezca a un tipo de instalación y si ya existe una instalación con el mismo nombre, en caso que ya exista una instalación igual o no pertenezca a ningún tipo de instalación la nueva instalación no se guardará. */
        private async void aceptar_Click(object sender, RoutedEventArgs e)
        {
            instalacion cogerId = gestionEnt.Set<instalacion>().Where(i => i.nombre == mvInstalacion.nuevaInstalacion.nombre && (mvInstalacion.nuevaInstalacion.idinstalacion == 0 || mvInstalacion.nuevaInstalacion.idinstalacion != i.idinstalacion)).FirstOrDefault();
            if (mvInstalacion.tipoInstalacionSeleccionado == null)
            {
                this.comboTipoInstalacion.Foreground = Brushes.Red;
            }
            else
            {
                if (cogerId != null)
                {
                    await this.ShowMessageAsync("CREAR INSTALACION", "Esta instalacion ya existe");
                    mvInstalacion.nuevaInstalacion.nombre = null;
                    this.txtNombre.Text = null;
                }
                else
                {
                    if (!mvInstalacion.guardarInstalacion()) await this.ShowMessageAsync("CREAR INSTALACION", "No se ha podido guardar la instalacion");
                    else DialogResult = true;
                }
            }
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        /* Evento accionado cuando se pulsa sobre el botón llamado “cerrarInstalacion”. Dependiendo del valor de la variable “cerrar” cambia la instalación y la marca como instalación cerrada o abierta. */
        private void cerrarInstalacion_Click(object sender, RoutedEventArgs e)
        {
            switch (cerrar)
            {
                case 0:
                    mvInstalacion.nuevaInstalacion.insalacion_cerrada = 1;
                    cerrar = 1;
                    cambiaBoton(1);
                    break;
                case 1:
                    mvInstalacion.nuevaInstalacion.insalacion_cerrada = 0;
                    cerrar = 0;
                    cambiaBoton(0);
                    break;
            }
        }

        /* Este método privado tiene como parámetro un código de tipo int. Dependiendo del código cambia el estilo del botón que llamado “cerrarInstalacion” en el xaml. */
        private void cambiaBoton(int cod)
        {
            switch (cod)
            {
                case 0:
                    this.cerrarInstalacion.Background = Brushes.LightBlue;
                    this.cerrarInstalacion.Content = "CERRRAR INSTALACION";
                    break;
                case 1:
                    this.cerrarInstalacion.Background = Brushes.LightGray;
                    this.cerrarInstalacion.Content = "ABRIR INSTALACION";
                    break;
            }
        }
    }
}
