using MahApps.Metro.Controls;
using NLog;
using PoliGest.BackEnd.Modelo;
using PoliGest.BackEnd.Servicios;
using PoliGest.MVVM;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PoliGest.FrontEnd.Dialogos
{
    /// <summary>
    /// Lógica de interacción para DiagUsuario.xaml
    /// </summary>
    public partial class DiagUsuario : MetroWindow
    {
        private GestionPolideportivaEntities gestionEnt;

        private MVUsuario mvUsuario;

        private RolServicio rolServ;

        // private usuario user = null;

        private usuario usuCambio = null;

        private usuario usuarioLogeado = null;


        private String esSocio = "";

        /* Creamos el log para guardar los posibles errores */
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public DiagUsuario(GestionPolideportivaEntities gestion)
        {
            InitializeComponent();
            gestionEnt = gestion;
            inicializa();
        }

        public DiagUsuario(GestionPolideportivaEntities gestion, String socio)
        {
            InitializeComponent();
            gestionEnt = gestion;
            esSocio = socio;
            this.btnMod.Visibility = Visibility.Collapsed;
            inicializa();
        }

        public DiagUsuario(GestionPolideportivaEntities gestion, usuario user)
        {
            InitializeComponent();
            gestionEnt = gestion;
            inicializa();
            this.usuCambio = user;
            inicializa(user);
            esSocio = user.rol.nombre;
            if (!rolServ.tienePerm(user.rol, "Alta/Modificacion socios"))
            {
                this.modificar.Visibility = Visibility.Collapsed;
            }

        }

        public DiagUsuario(GestionPolideportivaEntities gestion, usuario usuarioList, usuario user)
        {
            gestionEnt = gestion;
            InitializeComponent();
            inicializa();
            inicializa(usuarioList);
            if (!rolServ.tienePerm(user.rol, "Baja de socios"))
                this.baja.Visibility = Visibility.Collapsed;
            this.usuCambio = usuarioList;
            this.usuarioLogeado = user;
            esSocio = user.rol.nombre;
            if (!rolServ.tienePerm(user.rol, "Alta/Modificacion socios"))
            {
                this.modificar.Visibility = Visibility.Collapsed;
            }
        }

        private void inicializa(usuario usuario)
        {
            this.gridDatos.IsEnabled = false;
            this.btnsNU.Visibility = Visibility.Collapsed;
            mvUsuario.usuarioDatos = usuario;
            if (usuario.rol.nombre.Equals("Socio"))
            {
                socios socio = gestionEnt.Set<socios>().Where(s => s.usuario_idusuario == usuario.idusuario).FirstOrDefault();
                mvUsuario.socioNuevo = socio;
            }
            this.contraseñaNs.Password = mvUsuario.usuarioDatos.contraseña;
            this.repiteContraseñaNs.Password = mvUsuario.usuarioDatos.contraseña;
            this.titulo.Text = "Perfil de usuario";
        }

        private void inicializa()
        {
            this.rolServ = new RolServicio(gestionEnt);
            mvUsuario = new MVUsuario(gestionEnt);
            DataContext = mvUsuario;
            this.AddHandler(Validation.ErrorEvent, new RoutedEventHandler(mvUsuario.OnErrorEvent));
            mvUsuario.btnGuardar = this.aceptarNS;
            if (esSocio.Equals("Socio") && rolServ.tienePerm(mvUsuario.cogerRolSocio, "Alta/Modificacion socios"))
            {
                this.comboRoles.Visibility = Visibility.Collapsed;
                mvUsuario.usuarioDatos.rol = mvUsuario.cogerRolSocio;
            }
        }

        private void comboRoles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mvUsuario.usuarioDatos.rol.nombre.Equals("Socio"))
            {
                this.ObservacionesNS.Visibility = Visibility.Visible;
            }
            else
            {
                this.ObservacionesNS.Visibility = Visibility.Collapsed;
            }
        }

        /* Este evento guarda un nuevo usuario si el método “validarDatos” devuelve “true”. */
        private void aceptarNS_Click(object sender, RoutedEventArgs e)
        {
            if (validarDatos())
            {
                mvUsuario.usuarioDatos.contraseña = this.contraseñaNs.Password;
                mvUsuario.guardarUsuario();

                Close();
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        /* Este método sirve para comprobar que los datos insertados por el usuario en la parte xaml son correctos. */
        private bool validarDatos()
        {
            if (!checkearIban(this.ibanNS.Text))
            {
                this.ibanNS.Text = "";
                this.ibanNS.Focus();
                return false;
            }

            if (!calculardniNie(mvUsuario.usuarioDatos.dni) && mvUsuario.usuarioDatos.dni != null)
            {
                this.dniNieNS.Text = "";
                this.dniNieNS.Focus();
                return false;
            }

            if (this.contraseñaNs.Password == "" || this.repiteContraseñaNs.Password == "")
            {
                this.repiteContraseñaNs.Password = "";
                this.repiteContraseñaNs.Foreground = Brushes.Red;
                this.contraseñaNs.Password = "";
                this.contraseñaNs.Foreground = Brushes.Red;
                return false;
            }
            else
            {
                this.repiteContraseñaNs.Foreground = Brushes.Gray;
                this.contraseñaNs.Foreground = Brushes.Gray;
            }

            if (this.telefonoNS.Text.Contains("_"))
            {
                this.telefonoNS.Text = "";
                this.telefonoNS.Foreground = Brushes.Red;
                this.telefonoNS.Focus();
                return false;
            }
            else this.telefonoNS.Foreground = Brushes.Gray;

            if (!esSocio.Equals("Socio") && mvUsuario.usuarioDatos.rol == null && rolServ.tienePerm(mvUsuario.usuarioDatos.rol, "Alta/Modificacion socios"))
            {
                this.comboRoles.Foreground = Brushes.Red;
                return false;
            }
            else this.comboRoles.Foreground = Brushes.Gray;

            return comparaContraseña(this.contraseñaNs.Password, this.repiteContraseñaNs.Password);
        }

        /* Con este método se compara que el campo contraseña y el campo donde se tiene que repetir la contraseña en el xaml contengan el mismo valor. */
        private bool comparaContraseña(String contraseña, String confirmar)
        {
            if (contraseña.Equals(confirmar)) return true;
            else
            {
                MessageBoxResult res = System.Windows.MessageBox.Show("Las contraseñas tienen que ser identicas", "CONTRASEÑA", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.repiteContraseñaNs.Password = "";
                this.repiteContraseñaNs.Focus();
                this.repiteContraseñaNs.Foreground = Brushes.Red;
                return false;
            }
        }
        /* Este método se encarga de comprobar que el IBAN que el usuario ha añadido es correcto. */
        public bool checkearIban(string iban)
        {
            try
            {
                if (iban.Length < 4 || iban[0] == ' ' || iban[1] == ' ' || iban[2] == ' ' || iban[3] == ' ') throw new InvalidOperationException();

                var checksum = 0;
                var ibanLength = iban.Length;
                for (int charIndex = 0; charIndex < ibanLength; charIndex++)
                {
                    var c = iban[(charIndex + 4) % ibanLength];
                    if (c == ' ') continue;

                    int value;
                    if (c >= '0' && c <= '9')
                    {
                        value = c - '0';
                    }
                    else if (c >= 'A' && c <= 'Z')
                    {
                        value = c - 'A';
                        checksum = (checksum * 10 + value / 10 + 1) % 97;
                        value %= 10;
                    }
                    else if (c >= 'a' && c <= 'z')
                    {
                        value = c - 'a';
                        checksum = (checksum * 10 + value / 10 + 1) % 97;
                        value %= 10;
                    }
                    else throw new InvalidOperationException();

                    checksum = (checksum * 10 + value) % 97;
                }
                return checksum == 1;
            }
            catch (Exception e)
            {
                logger.Error("\n" + e.InnerException + "\n" + e.StackTrace);
                return false;
            }
        }

        /* Este método privado comprueba que el dni que el usuario indica no esté siendo usado en la aplicación y que cumple con los requisitos para ser un dni válido. */
        private bool calculardniNie(string texto)
        {
            try
            {
                if (texto.Length > 9 || mvUsuario.existeUsuario != null && mvUsuario.existeUsuario.dni != this.usuCambio.dni) throw new Exception();

                string[] letras = { "T", "R", "W", "A", "G", "M", "Y", "F", "P", "D", "X", "B",
                                    "N", "J", "Z", "S", "Q", "V", "H", "L", "C", "K", "E" };
                int numero = 0;
                int resto = 0;
                switch (texto.Substring(0, 1))
                {
                    case "X":
                        texto = 0 + texto.Substring(1);
                        break;
                    case "Y":
                        texto = 1 + texto.Substring(1);
                        break;
                    case "Z":
                        texto = 2 + texto.Substring(1);
                        break;
                }
                numero = Convert.ToInt32(texto.Substring(0, 8));
                resto = numero % 23;
                if (letras[resto].Equals(texto.Substring((texto.Length) - 1))) return true;
                else return false;
            }
            catch (Exception e)
            {
                logger.Error("\n" + e.InnerException + "\n" + e.StackTrace);
                return false;
            }
        }

        /* Este evento comprueba que el método “validarDatos” devuelve “true”, en ese caso guarda las modificaciones del usuario. */
        private void aceptar_Click(object sender, RoutedEventArgs e)
        {
            if (validarDatos())
            {
                mvUsuario.usuarioDatos.contraseña = this.contraseñaNs.Password;
                mvUsuario.editarUsuario();
                DialogResult = true;
            }
        }

        /* Este evento comprueba si el usuario logueado tiene permisos para modificar un usuario, de ser así podrá modificar los datos del usuario. */
        private void modificar_Click(object sender, RoutedEventArgs e)
        {
            if (this.usuarioLogeado != null && !rolServ.tienePerm(this.usuarioLogeado.rol, "Modificar la contraseña de otros"))
            {
                this.contraseñaNs.Visibility = Visibility.Collapsed;
                this.repiteContraseñaNs.Visibility = Visibility.Collapsed;
            }
            this.titulo.Text = "Modificar usuario";
            this.gridDatos.IsEnabled = true;
            this.modificar.Visibility = Visibility.Collapsed;
            if (esSocio.Equals("Socio")) this.comboRoles.IsEnabled = false;
        }

        /* Este evento pregunta al usuario si quiere dar de baja al usuario que está en el diálogo, en caso afirmativo usa el método “borrarUsuario”
         * de la clase mv y comprueba si el usuario dado de baja es el mismo que el usuario logueado, en ese caso se cierra la aplicación.
         */
        private void baja_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = System.Windows.MessageBox.Show("¿Estás seguro dar de baja al usuario? Se perderán todas las reservas", "USUARIO", MessageBoxButton.YesNo, MessageBoxImage.Error);
            if (res == MessageBoxResult.Yes)
            {
                mvUsuario.borrarUsuario();
                if (this.usuarioLogeado == null) Environment.Exit(0);
                else DialogResult = true;
            }
        }
    }
}
