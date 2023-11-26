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
    /// Lógica de interacción para UCInstalaciones.xaml
    /// </summary>
    public partial class UCInstalaciones : UserControl
    {
        private GestionPolideportivaEntities gestionEnt;
        private MVInstalacion mvInstalacion;
        public UCInstalaciones(GestionPolideportivaEntities gestion)
        {
            InitializeComponent();
            gestionEnt = gestion;
            mvInstalacion = new MVInstalacion(gestion);
            DataContext = mvInstalacion;
            quitarFiltro();
        }
        private void dgListaInstalacion_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            instalacion instalacion = (instalacion)dgListaInstalacion.SelectedItem;
            instalacion original = new instalacion();
            mvInstalacion.crearCopia(instalacion, original);
            DiagInstalacion diag = new DiagInstalacion(gestionEnt, instalacion);
            diag.ShowDialog();
            if (diag.DialogResult == false)
            {
                mvInstalacion.crearCopia(original, instalacion);
            }
            mvInstalacion = new MVInstalacion(gestionEnt);
            DataContext = mvInstalacion;
            InitializeComponent();
        }

        public void filtro()
        {
            mvInstalacion.criterios.Clear();
            mvInstalacion.addCriterios();
            this.dgListaInstalacion.Items.Filter = new Predicate<object>(mvInstalacion.filtroCombinadoLista);
        }

        private void quitarFiltro()
        {
            mvInstalacion.textoNombre = "";
            mvInstalacion.tipoInstalacionSeleccionado = null;
            dgListaInstalacion.Items.Filter = null;
        }

        private void comboFiltroTipoInst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filtro();
        }

        private void txtNombre_SelectionChanged(object sender, RoutedEventArgs e)
        {
            filtro();
        }

        private void btnQuitarFiltro_Click(object sender, RoutedEventArgs e)
        {
            quitarFiltro();
        }
    }
}
