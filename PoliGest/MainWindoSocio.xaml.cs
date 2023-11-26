using MahApps.Metro.Controls;
using PoliGest.BackEnd.Modelo;
using PoliGest.BackEnd.Servicios;
using PoliGest.FrontEnd.ControlUsuario;
using PoliGest.FrontEnd.Dialogos;
using PoliGest.FrontEnd.Dialogos.DialogosLogin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PoliGest
{
    /// <summary>
    /// Lógica de interacción para MainWindoSocio.xaml
    /// </summary>
    public partial class MainWindoSocio : MetroWindow
    {
        private GestionPolideportivaEntities gestionEnt;
        private usuario user;
        private RolServicio rolServ;

        public MainWindoSocio(GestionPolideportivaEntities gestion, usuario usuario)
        {
            InitializeComponent();
            gestionEnt = gestion;
            this.user = usuario;
            rolServ = new RolServicio(gestionEnt);
            if (!rolServ.tienePerm(this.user.rol, "Ver reservas"))
            {
                this.misReservas.Visibility = Visibility.Collapsed;
                this.reservasActivas.Visibility = Visibility.Collapsed;
            }
            else
            {
                UCListaReservas uc = new UCListaReservas(gestionEnt, user, 2);
                this.gridCentral.Children.Clear();
                this.gridCentral.Children.Add(uc);
            }

            if (!rolServ.tienePerm(this.user.rol, "Realizar reserva"))
            {
                this.realizarReservas.Visibility = Visibility.Collapsed;
            }
        }

        private void cerrarSesion_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("¿Estás seguro de cerrar las sesión?", "SESIÓN", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                Login login = new Login();
                login.Show();
                this.Close();
            }
        }

        private void miPerfil_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DiagUsuario diag = new DiagUsuario(gestionEnt, user);
            diag.ShowDialog();
        }

        private void misReservas_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UCListaReservas uc = new UCListaReservas(gestionEnt, user, 1);
            this.gridCentral.Children.Clear();
            this.gridCentral.Children.Add(uc);
        }

        private void reservasActivas_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UCListaReservas uc = new UCListaReservas(gestionEnt, user, 2);
            this.gridCentral.Children.Clear();
            this.gridCentral.Children.Add(uc);
        }

        private void realizarReservas_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DiagRealizarReserva diag = new DiagRealizarReserva(gestionEnt, user);
            diag.ShowDialog();

            UCListaReservas uc = new UCListaReservas(gestionEnt, user, 2);
            this.gridCentral.Children.Clear();
            this.gridCentral.Children.Add(uc);
        }
    }
}
