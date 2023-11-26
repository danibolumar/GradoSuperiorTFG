using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using PoliGest.BackEnd.Modelo;
using PoliGest.MVVM;
using System.Windows;
using System.Windows.Controls;

namespace PoliGest.FrontEnd.Dialogos
{
    /// <summary>
    /// Lógica de interacción para DiagModificarPermisos.xaml
    /// </summary>
    public partial class DiagModificarPermisos : MetroWindow
    {
        private GestionPolideportivaEntities gestionEnt;

        private MVRol mvRol;

        public DiagModificarPermisos(GestionPolideportivaEntities gestion)
        {
            InitializeComponent();
            gestionEnt = gestion;
            mvRol = new MVRol(gestion);
            DataContext = mvRol;
        }

        private void comboFiltroRol_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mvRol.editarPermisos();
            this.dgTablaPermisoIn.ItemsSource = mvRol.listaPermisosIn;
            this.dgTablaPermisoOut.ItemsSource = mvRol.listaPermisosOut;
            this.gridDataGrid.Visibility = Visibility.Visible;
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /* Este evento comprueba si se puede guardar el rol modificado, en caso negativo muestra un mensaje al usuario. */
        private async void aceptarMR_Click(object sender, RoutedEventArgs e)
        {
            if (mvRol.guardarRol() == -1) await this.ShowMessageAsync("CREAR ROL", "Por favor añade al menos un rol");
            else if (mvRol.guardarRol() == -2) await this.ShowMessageAsync("CREAR ROL", "Error al guardar el rol, por favor intentalo de nuevo");
            else this.Close();
        }

        private void añadirTodosPerm_Click(object sender, RoutedEventArgs e)
        {
            if (mvRol.rolSeleccionado.nombre != null)
            {
                mvRol.cambiarPermisosList(1);
                this.dgTablaPermisoIn.ItemsSource = mvRol.listaPermisosIn;
                this.dgTablaPermisoOut.ItemsSource = mvRol.listaPermisosOut;
            }
        }

        private void quitarTodosPerm_Click(object sender, RoutedEventArgs e)
        {
            if (mvRol.rolSeleccionado.nombre != null)
            {
                mvRol.cambiarPermisosList(2);
                this.dgTablaPermisoIn.ItemsSource = mvRol.listaPermisosIn;
                this.dgTablaPermisoOut.ItemsSource = mvRol.listaPermisosOut;
            }
        }

        private void añadirPerm_Click(object sender, RoutedEventArgs e)
        {
            mvRol.cambiarPermisos((permisos)this.dgTablaPermisoOut.SelectedItem, 1);
            this.dgTablaPermisoIn.ItemsSource = mvRol.listaPermisosIn;
            this.dgTablaPermisoOut.ItemsSource = mvRol.listaPermisosOut;
        }

        private void quitarPerm_Click(object sender, RoutedEventArgs e)
        {
            mvRol.cambiarPermisos((permisos)this.dgTablaPermisoIn.SelectedItem, 2);
            this.dgTablaPermisoIn.ItemsSource = mvRol.listaPermisosIn;
            this.dgTablaPermisoOut.ItemsSource = mvRol.listaPermisosOut;

        }

        private void comboFiltroPermiso_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.btnEliminarPermiso.Visibility = Visibility.Visible;
        }

        /* Este evento se encarga de primero, eliminar el permiso seleccionado y luego actualizar la lista que se muestra en el diálogo, estas acciones se ejecutan desde el mv que usa el diálogo. */
        private void btnEliminarPermiso_Click(object sender, RoutedEventArgs e)
        {
            mvRol.borrarPermiso();
            mvRol.editarPermisos();
            this.Close();
        }
    }
}
