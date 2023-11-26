using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using PoliGest.BackEnd.Modelo;
using PoliGest.MVVM;
using System.Windows;
using System.Windows.Controls;

namespace PoliGest.FrontEnd.Dialogos.DialogosLogin
{
    /// <summary>
    /// Lógica de interacción para NuevaContraseña.xaml
    /// </summary>
    public partial class NuevaContraseña : MetroWindow
    {
        private string randomContraseña = string.Empty;
        private GestionPolideportivaEntities gestionEnt;

        private MVUsuario mvUsuario;

        public NuevaContraseña(GestionPolideportivaEntities gestEnt)
        {
            InitializeComponent();
            gestionEnt = gestEnt;
            mvUsuario = new MVUsuario(gestionEnt);
            DataContext = mvUsuario;
            this.AddHandler(Validation.ErrorEvent, new RoutedEventHandler(mvUsuario.OnErrorEvent));
            mvUsuario.btnGuardar = this.contrasenya_solicitada;
        }

        /* Este método comprueba si el usuario existe, en caso afirmativo usa el método “cambioContraseña” del mv y mostrará la nueva contraseña en el diálogo.*/

        private void Contrasenya_solicitada_Click(object sender, RoutedEventArgs e)
        {
            this.errorTxt.Visibility = Visibility.Collapsed;

            if (mvUsuario.existeUsuario != null)
            {
                randomContraseña = mvUsuario.cambioContraseña();
                mvUsuario.usuarioDatos = mvUsuario.existeUsuario;
                mvUsuario.usuarioDatos.contraseña = randomContraseña;

                this.separartorNC.Visibility = Visibility.Visible;
                this.textoNuevaContraseaña.Visibility = Visibility.Visible;
                this.mostrarContraseña.Visibility = Visibility.Visible;
                this.textoMostrarContraseña.Text = randomContraseña;
                this.aceptar.Visibility = Visibility.Visible;
            }
            else this.errorTxt.Visibility = Visibility.Visible;
        }

        /* Usa el método editarUsuario para guardar la nueva contraseña del usuario. */
        private async void aceptar_Click(object sender, RoutedEventArgs e)
        {
            if (!mvUsuario.editarUsuario())
            {
                await this.ShowMessageAsync("GUARDADO", "No se ha guardado correctamente, intentalo de nuevo");
            }
            else Close();

        }

        /* Evento que borra el portapapeles y carga en la nueva contraseña en el portapapeles. */
        private async void copiarCont_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.Clear();

            Clipboard.SetText(randomContraseña);

            await this.ShowMessageAsync("CONTRASEÑA", "CONTRSEÑA GUARDADA EN EL PORTAPAPELES");
        }
    }
}
