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
    /// Lógica de interacción para UCListaUsuarios.xaml
    /// </summary>
    public partial class UCListaUsuarios : UserControl
    {
        private GestionPolideportivaEntities gestionEnt;
        private MVUsuario mvUsuario;
        private usuario user;
        public UCListaUsuarios(GestionPolideportivaEntities gestion, usuario user)
        {
            InitializeComponent();
            this.user = user;
            gestionEnt = gestion;
            mvUsuario = new MVUsuario(gestionEnt);
            DataContext = mvUsuario;
            quitarFiltro();
        }


        private void dgListaUsuarios_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            usuario usu = (usuario)dgListaUsuarios.SelectedItem;
            usuario original = new usuario();
            mvUsuario.crearCopia(usu, original);
            DiagUsuario diag = new DiagUsuario(gestionEnt, usu, user);
            diag.ShowDialog();
            if (diag.DialogResult == false)
            {
                mvUsuario.crearCopia(original, usu);
            }
            mvUsuario = new MVUsuario(gestionEnt);
            DataContext = mvUsuario;
            InitializeComponent();
        }

        private void filtro()
        {
            mvUsuario.criterios.Clear();
            mvUsuario.addCriterios();
            dgListaUsuarios.Items.Filter = new Predicate<object>(mvUsuario.filtroCombinadoLista);
        }

        private void quitarFiltro()
        {
            mvUsuario.textoDni = "";
            mvUsuario.textoNombre = "";
            mvUsuario.rolSeleccionado = null;
            dgListaUsuarios.Items.Filter = null;
        }

        private void btnQuitarFiltro_Click(object sender, RoutedEventArgs e)
        {
            quitarFiltro();

        }

        private void comboFiltroRol_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filtro();
        }

        private void txtNombre_SelectionChanged(object sender, RoutedEventArgs e)
        {
            filtro();
        }

        private void txtDni_SelectionChanged(object sender, RoutedEventArgs e)
        {
            filtro();
        }
    }
}
