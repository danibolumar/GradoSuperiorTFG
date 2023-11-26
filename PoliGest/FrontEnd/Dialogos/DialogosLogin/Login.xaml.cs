using MahApps.Metro.Controls;
using PoliGest.BackEnd.Modelo;
using PoliGest.BackEnd.Servicios;
using PoliGest.MVVM;
using System;
using System.Linq;
using System.Windows;

namespace PoliGest.FrontEnd.Dialogos.DialogosLogin
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : MetroWindow
    {
        private GestionPolideportivaEntities gestionEnt;

        private MVUsuario mvUsuario;
        private MVReserva mvReserva;
        private ReservaServicio resServ;

        private usuario user;
        public Login()
        {

            InitializeComponent();
            if (!conectar())
            {
                MessageBox.Show("ERROR EN LA CONEXIÓN", "Problemas con la base de datos", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
            resServ = new ReservaServicio(gestionEnt);
            mvUsuario = new MVUsuario(gestionEnt);
            mvReserva = new MVReserva(gestionEnt);
            DataContext = mvUsuario;
        }

        private bool conectar()
        {
            bool con = true;
            try
            {
                gestionEnt = new GestionPolideportivaEntities();
                gestionEnt.Database.Connection.Open();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.StackTrace);
                System.Console.WriteLine(e.InnerException);
                con = false;
            }
            return con;
        }

        private void ConstraseñaOlvidada_Click(object sender, RoutedEventArgs e)
        {

            NuevaContraseña diag = new NuevaContraseña(gestionEnt);
            diag.ShowDialog();

        }

        private void CrearUsuario_Click(object sender, RoutedEventArgs e)
        {
            DiagUsuario diag = new DiagUsuario(gestionEnt, "Socio");
            diag.ShowDialog();
        }

        /* Este evento se activará cuando el usuario intente acceder a la aplicación principal. comprobará que el usuario no está penalizado y que los datos con los que el usuario quiere loguearse son correctos.*/

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {

            foreach (reserva res in mvReserva.listaRes)
            {
                mvReserva.nuevaReserva = res;
                if (!(res.fecha_reserva > DateTime.Now || (res.fecha_reserva == DateTime.Now && (res.hora_fin.Hours - mvReserva.time) >= 1)) && res.pagado != 1 && res.anulado == 0)
                {
                    res.pagado = 1;

                    mvReserva.guardar(2);
                }

                if (!(res.fecha_reserva > DateTime.Now || (res.fecha_reserva == DateTime.Now && (res.hora_fin.Hours - mvReserva.time) >= 0)) && res.no_presentado == 1 && res.anulado == 0)
                {
                    mvReserva.ponerPenalizacion();
                }
            }
            mvUsuario.usuarioDatos.dni = boxUsername.Text.ToUpper();
            mvUsuario.usuarioDatos.contraseña = boxContraseña.Password;
            this.user = mvUsuario.existeUsuario;
            socios socioUsu = new socios();
            if (mvUsuario.loginDatosUsu) socioUsu = gestionEnt.Set<socios>().Where(s => s.usuario_idusuario == user.idusuario).FirstOrDefault();
            if (this.user != null && mvUsuario.loginDatosUsu && user.fecha_baja == null && mvUsuario.estaSancionado(socioUsu, this.user) == false)
            {
                this.errorTxt.Visibility = Visibility.Collapsed;
                this.errorTxtContinua.Visibility = Visibility.Collapsed;

                if (user.rol.nombre.Equals("Socio"))
                {
                    MainWindoSocio ventana = new MainWindoSocio(gestionEnt, user);
                    ventana.Show();
                }
                else
                {
                    MainWindow ventana = new MainWindow(gestionEnt, user);
                    ventana.Show();
                }
                this.Close();
            }
            else
            {
                if (mvUsuario.loginDatosUsu && mvUsuario.estaSancionado(socioUsu, user))
                {
                    errorTxt.Text = "ESTE USUARIO ESTÁ SANCIONADO HASTA EL";
                    errorTxtContinua.Text = socioUsu.fecha_fin_penalizacion.Value.Date.ToString();
                    errorTxt.Visibility = Visibility.Visible;
                    errorTxtContinua.Visibility = Visibility.Visible;
                }
                else
                {
                    errorTxt.Text = "EL DNI/NIE O LA CONTRASEÑA NO COINCIDEN.";
                    errorTxtContinua.Text = "INTENTALO DE NUEVO";
                    errorTxt.Visibility = Visibility.Visible;
                    this.errorTxtContinua.Visibility = Visibility.Visible;
                }
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
