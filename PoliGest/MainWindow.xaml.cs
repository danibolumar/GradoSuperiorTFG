using CrystalDecisions.CrystalReports.Engine;
using MahApps.Metro.Controls;
using PoliGest.BackEnd.Modelo;
using PoliGest.BackEnd.Servicios;
using PoliGest.FrontEnd.ControlUsuario;
using PoliGest.FrontEnd.Dialogos;
using PoliGest.FrontEnd.Dialogos.DialogosLogin;
using PoliGest.Informes;
using PoliGest.MVVM;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts.Defaults;

namespace PoliGest
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        private GestionPolideportivaEntities gestionEnt;
        private MVUsuario mvUsuario;
        private usuario user;
        private RolServicio rolServ;
        public MainWindow(GestionPolideportivaEntities gestion, usuario usuario)
        {
            InitializeComponent();
            gestionEnt = gestion;
            rolServ = new RolServicio(gestion);
            this.user = usuario;
            mvUsuario = new MVUsuario(gestion);

            if (!rolServ.tienePerm(this.user.rol, "Realizar informes") && !rolServ.tienePerm(this.user.rol, "Visualizar gráficos") && !rolServ.tienePerm(this.user.rol, "Alta/Baja/Modificacion de roles y permisos"))
            {
                this.administracion.Visibility = Visibility.Collapsed;
            }
            if (!rolServ.tienePerm(this.user.rol, "Realizar informes"))
            {
                this.realizarInformes.Visibility = Visibility.Collapsed;
            }
            if (!rolServ.tienePerm(this.user.rol, "Visualizar gráficos")) {
                this.verGraficos.Visibility = Visibility.Collapsed;
            }
            if (!rolServ.tienePerm(this.user.rol, "Alta/Baja/Modificacion de roles y permisos"))
            {
                this.crearPermiso.Visibility = Visibility.Collapsed;
                this.crearRoles.Visibility = Visibility.Collapsed;
                this.modificarRolPerm.Visibility = Visibility.Collapsed;
            }

            if(!rolServ.tienePerm(this.user.rol, "Crear/Modificar/Borrar instalacion") && !rolServ.tienePerm(this.user.rol, "Crear/Modificar/Borrar tipo instalación"))
            {
                this.instalacionesMenu.Visibility = Visibility.Collapsed;
            }

            if(!rolServ.tienePerm(this.user.rol, "Crear/Modificar/Borrar instalacion"))
            {
                this.administrarInstalaciones.Visibility = Visibility.Collapsed;
                this.crearInstalacion.Visibility = Visibility.Collapsed;
            }
            if(!rolServ.tienePerm(this.user.rol, "Crear/Modificar/Borrar tipo instalación"))
            {
                this.administrarTipoInstalaciones.Visibility = Visibility.Collapsed;
                this.crearTipoInstalacion.Visibility = Visibility.Collapsed;
            }
            if (!rolServ.tienePerm(this.user.rol, "Ver reservas"))
            {
                this.todasReservas.Visibility = Visibility.Collapsed;
                this.reservasActivas.Visibility = Visibility.Collapsed;
            }
            else
            {
                
                UCListaReservas uc = new UCListaReservas(gestionEnt, user, 2);
                this.cargarControlUsu.Children.Clear();
                this.cargarControlUsu.Children.Add(uc);
            }

            if (!rolServ.tienePerm(this.user.rol, "Realizar reserva"))
            {
                this.realizaReserva.Visibility = Visibility.Collapsed;
            }

            if(!rolServ.tienePerm(this.user.rol, "Alta/Modificacion socios") && !rolServ.tienePerm(this.user.rol, "Baja de socios"))
            {
                this.usuarios.Visibility = Visibility.Collapsed;
            }else if(!rolServ.tienePerm(this.user.rol, "Alta/Modificacion socios"))
            {
                this.crearUser.Visibility = Visibility.Collapsed;
            }

        }

        private void cerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("¿Estás seguro de cerrar las sesión?", "SESIÓN", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                Login login = new Login();
                login.Show();
                this.Close();
            }
        }

        private void miPerfil_Click(object sender, RoutedEventArgs e)
        {
            
            usuario usu = user;
            usuario original = new usuario();
            mvUsuario.crearCopia(usu, original);
            DiagUsuario diag = new DiagUsuario(gestionEnt, usu);
            diag.ShowDialog();
            if (diag.DialogResult == false)
            {
                mvUsuario.crearCopia(original, usu);
            }
            MainWindow main = new MainWindow(gestionEnt, usu);
            main.Show();
            this.Close();
        }

        private void crearUser_Click(object sender, RoutedEventArgs e)
        {
            DiagUsuario diag = new DiagUsuario(gestionEnt, "");
            diag.ShowDialog();
            UCListaUsuarios uc = new UCListaUsuarios(gestionEnt, user);
            this.cargarControlUsu.Children.Clear();
            this.cargarControlUsu.Children.Add(uc);
        }

        private void listaUsers_Click(object sender, RoutedEventArgs e)
        {
            UCListaUsuarios uc = new UCListaUsuarios(gestionEnt, user);
            this.cargarControlUsu.Children.Clear();
            this.cargarControlUsu.Children.Add(uc);
        }

        private void modificarRolPerm_Click(object sender, RoutedEventArgs e)
        {
            DiagModificarPermisos diag = new DiagModificarPermisos(gestionEnt);
            diag.ShowDialog();
            MainWindow main = new MainWindow(gestionEnt, user);
            main.Show();
            this.Close();
            this.Close();
        }

        private void crearRoles_Click(object sender, RoutedEventArgs e)
        {
            DiagRol diag = new DiagRol(gestionEnt);
            diag.ShowDialog();
        }

        private void crearPermiso_Click(object sender, RoutedEventArgs e)
        {
            DiagPermiso diag = new DiagPermiso(gestionEnt);
            diag.ShowDialog();
        }

        private void crearTipoInstalacion_Click(object sender, RoutedEventArgs e)
        {
            DiagTipoInstalacion diag = new DiagTipoInstalacion(gestionEnt);
            diag.ShowDialog();

            UCTipoInstalaciones uc = new UCTipoInstalaciones(gestionEnt);
            this.cargarControlUsu.Children.Clear();
            this.cargarControlUsu.Children.Add(uc);
        }

        private void crearInstalacion_Click(object sender, RoutedEventArgs e)
        {
            DiagInstalacion diag = new DiagInstalacion(gestionEnt);
            diag.ShowDialog();

            UCInstalaciones uc = new UCInstalaciones(gestionEnt);
            this.cargarControlUsu.Children.Clear();
            this.cargarControlUsu.Children.Add(uc);
        }

        private void administrarInstalaciones_Click(object sender, RoutedEventArgs e)
        {
            UCInstalaciones uc = new UCInstalaciones(gestionEnt);

            this.cargarControlUsu.Children.Clear();
            this.cargarControlUsu.Children.Add(uc);
        }

        private void administrarTipoInstalaciones_Click(object sender, RoutedEventArgs e)
        {
            UCTipoInstalaciones uc = new UCTipoInstalaciones(gestionEnt);
            this.cargarControlUsu.Children.Clear();
            this.cargarControlUsu.Children.Add(uc);
        }

        private void realizaReserva_Click(object sender, RoutedEventArgs e)
        {
            DiagRealizarReserva diag = new DiagRealizarReserva(gestionEnt, user);
            diag.ShowDialog();

            if (rolServ.tienePerm(this.user.rol, "Ver reservas"))
            {
                UCListaReservas uc = new UCListaReservas(gestionEnt, user, 2);
                this.cargarControlUsu.Children.Clear();
                this.cargarControlUsu.Children.Add(uc);
            }
        }

        private void todasReservas_Click(object sender, RoutedEventArgs e)
        {
            
            UCListaReservas uc = new UCListaReservas(gestionEnt, user, 1);
            this.cargarControlUsu.Children.Clear();
            this.cargarControlUsu.Children.Add(uc);
        }

        private void reservasActivas_Click(object sender, RoutedEventArgs e)
        {
           
            UCListaReservas uc = new UCListaReservas(gestionEnt, user, 2);
            this.cargarControlUsu.Children.Clear();
            this.cargarControlUsu.Children.Add(uc);
        }

        private void verGraficos_Click(object sender, RoutedEventArgs e)
        {
            DiagChart diag = new DiagChart(gestionEnt);
            diag.Show();
        }

        private void informePistaSocios_Click(object sender, RoutedEventArgs e)
        {
            DiagInforme diag = new DiagInforme(1);
            diag.ShowDialog();
        }

        private void informeResumenAnual_Click(object sender, RoutedEventArgs e)
        {
            DiagInforme diag = new DiagInforme(2);
            diag.ShowDialog();
        }
    }
}
