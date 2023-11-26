using PoliGest.BackEnd.Modelo;
using PoliGest.FrontEnd.Dialogos;
using PoliGest.MVVM;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PoliGest.FrontEnd.ControlUsuario
{
    /// <summary>
    /// Lógica de interacción para UCTipoInstalaciones.xaml
    /// </summary>
    public partial class UCTipoInstalaciones : UserControl
    {
        private GestionPolideportivaEntities gestionEnt;
        private MVTipoInstalacion mvTipoInstalacion;
        public UCTipoInstalaciones(GestionPolideportivaEntities gestion)
        {
            InitializeComponent();
            gestionEnt = gestion;
            mvTipoInstalacion = new MVTipoInstalacion(gestion);
            DataContext = mvTipoInstalacion;
            quitarFiltro();
        }

        public void filtro()
        {
            mvTipoInstalacion.criterios.Clear();
            mvTipoInstalacion.addCriterios();
            this.dgListaTipoInstalacion.Items.Filter = new Predicate<object>(mvTipoInstalacion.filtroCombinadoLista);
        }

        private void quitarFiltro()
        {
            mvTipoInstalacion.textoDescripcion = "";
            mvTipoInstalacion.horarioSeleccionado = null;
            dgListaTipoInstalacion.Items.Filter = null;
        }

        private void comboFiltroInstalacion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filtro();
        }

        private void txtDescripcion_SelectionChanged(object sender, RoutedEventArgs e)
        {
            filtro();
        }

        private void btnQuitarFiltro_Click(object sender, RoutedEventArgs e)
        {
            quitarFiltro();
        }

        private void dgListaTipoInstalacion_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            tipo_instalacion tipo = (tipo_instalacion)dgListaTipoInstalacion.SelectedItem;
            tipo_instalacion original = new tipo_instalacion();
            mvTipoInstalacion.crearCopia(tipo, original);
            DiagTipoInstalacion diag = new DiagTipoInstalacion(gestionEnt, tipo);
            diag.ShowDialog();
            if (diag.DialogResult == false)
            {
                mvTipoInstalacion.crearCopia(original, tipo);
            }
            mvTipoInstalacion = new MVTipoInstalacion(gestionEnt);
            DataContext = mvTipoInstalacion;
            InitializeComponent();
        }
    }
}
