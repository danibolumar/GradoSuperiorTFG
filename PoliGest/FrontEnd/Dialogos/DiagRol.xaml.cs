using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using PoliGest.BackEnd.Modelo;
using PoliGest.MVVM;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PoliGest.FrontEnd.Dialogos
{
    /// <summary>
    /// Lógica de interacción para DiagRol.xaml
    /// </summary>
    public partial class DiagRol : MetroWindow
    {
        private GestionPolideportivaEntities gestionEnt;

        private MVRol mvRol;

        public DiagRol(GestionPolideportivaEntities gestion)
        {
            InitializeComponent();
            gestionEnt = gestion;
            mvRol = new MVRol(gestion);
            this.AddHandler(Validation.ErrorEvent, new RoutedEventHandler(mvRol.OnErrorEvent));
            DataContext = mvRol;
            mvRol.btnGuardar = aceptarNR;
        }

        /* Este evento llama al método “gaurdarNuevoRol” del archivo mv, en caso que el resultado del método usado sea  negativo se muestra un mensaje al usuario. */
        private async void aceptarNS_Click(object sender, RoutedEventArgs e)
        {
            int result = mvRol.guardarNuevoRol();
            if (result == -1) await this.ShowMessageAsync("CREAR ROL", "Este rol es repetido por favor introduce uno nuevo");
            else if (result == -2) await this.ShowMessageAsync("CREAR ROL", "Por favor añade al menos un permiso");
            else if (result == -3)
            {
                await this.ShowMessageAsync("CREAR ROL", "Error al guardar el rol, por favor intentalo de nuevo");
                this.nombreRol.Text = "";
            }
            else this.Close();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            mvRol.permisosRolSeleccionado.Add((permisos)this.dgTablaPermiso.SelectedItem);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            mvRol.permisosRolSeleccionado.Remove((permisos)this.dgTablaPermiso.SelectedItem);
        }
    }
}
