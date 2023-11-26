using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using PoliGest.BackEnd.Modelo;
using PoliGest.MVVM;
using System.Windows;
using System.Windows.Controls;

namespace PoliGest.FrontEnd.Dialogos
{
    /// <summary>
    /// Lógica de interacción para DiagPermiso.xaml
    /// </summary>
    public partial class DiagPermiso : MetroWindow
    {
        private GestionPolideportivaEntities gestionEnt;

        private MVPermiso mvPermiso;
        public DiagPermiso(GestionPolideportivaEntities gestion)
        {
            InitializeComponent();
            gestionEnt = gestion;
            mvPermiso = new MVPermiso(gestionEnt);
            this.AddHandler(Validation.ErrorEvent, new RoutedEventHandler(mvPermiso.OnErrorEvent));
            DataContext = mvPermiso;
            mvPermiso.btnGuardar = aceptarNP;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            mvPermiso.rolPermisoSeleccionado.Add((rol)this.dgTablaRol.SelectedItem);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            mvPermiso.rolPermisoSeleccionado.Remove((rol)this.dgTablaRol.SelectedItem);
        }

        /* Este evento llama al método “guardarNuevoPermiso” del archivo mv y en caso que el resultado del método usado sea  negativo se muestra un mensaje al usuario. */
        private async void aceptarNP_Click(object sender, RoutedEventArgs e)
        {
            int result = mvPermiso.guardarNuevoPermiso();
            if (result == -1) await this.ShowMessageAsync("CREAR PERMISOS", "Este permiso es repetido por favor introduce uno nuevo");
            else if (result == -2) await this.ShowMessageAsync("CREAR PERMISOS", "Por favor añade al menos un rol");
            else if (result == -3)
            {
                await this.ShowMessageAsync("CREAR PERMISOS", "Error al guardar el permiso, por facor intentalo de nuevo");
                this.nombrePermiso.Text = "";
            }
            else this.Close();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
