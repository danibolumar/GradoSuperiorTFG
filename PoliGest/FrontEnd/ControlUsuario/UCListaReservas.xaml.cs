using PoliGest.BackEnd.Modelo;
using PoliGest.MVVM;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PoliGest.FrontEnd.ControlUsuario
{
    /// <summary>
    /// Lógica de interacción para UCListaReservas.xaml
    /// </summary>
    public partial class UCListaReservas : UserControl
    {
        private GestionPolideportivaEntities gestionEnt;
        private MVReserva mvReserva;
        private usuario user;
        private int cod;

        /* El constructor se le pasa como parámetro un código y un usuario que hace referencia al usuario que está ahora mismo en la aplicación
         * dependiendo del rol del usuario y del código que se le pase se cargarán unas reservas u otras y se mostrarán más o menos acciones a realizar en la lista
         */
        public UCListaReservas(GestionPolideportivaEntities gestion, usuario usu, int codigo)
        {
            gestionEnt = gestion;
            user = usu;
            cod = codigo;
            mvReserva = new MVReserva(gestionEnt, user, codigo);
            DataContext = mvReserva;
            InitializeComponent();
            quitarFiltro();
            if (cod == 1)
            {
                this.opciones.Visibility = Visibility.Collapsed;
                this.titulo.Text = "Todas Las Reservas";
            }
            else if (cod == 2)
            {
                if (usu.rol.nombre.Equals("Socio"))
                {
                    this.noPresent.Visibility = Visibility.Collapsed;
                    this.pago.Visibility = Visibility.Collapsed;
                }

                this.noPresentCheck.Visibility = Visibility.Collapsed;
                this.anulado.Visibility = Visibility.Collapsed;

                this.titulo.Text = "Reservas Activas";
            }

            this.fechaInicio.DisplayDateStart = mvReserva.fechaInicioSeleccinada;
            this.fechaInicio.DisplayDateEnd = mvReserva.fechaFinSeleccionada;
            this.fechaFin.DisplayDateStart = mvReserva.fechaInicioSeleccinada;
            this.fechaFin.DisplayDateEnd = mvReserva.fechaFinSeleccionada;
        }

        private void filtro()
        {
            mvReserva.criterios.Clear();
            mvReserva.addCriterios();
            dgListaTodasReservas.Items.Filter = new Predicate<object>(mvReserva.filtroCombinadoLista);
        }

        private void quitarFiltro()
        {
            mvReserva.quitarFiltros();
            dgListaTodasReservas.Items.Filter = null;
        }



        private void btnQuitarFiltro_Click(object sender, RoutedEventArgs e)
        {
            quitarFiltro();
        }

        private void btnFiltrar_Click(object sender, RoutedEventArgs e)
        {
            filtro();
        }

        /* Este método se encarga de comprobar si se puede cancelar la reserva o no, en caso que se pueda cancelar la reserva llama  al método “cancelarReserva”.*/

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            mvReserva.nuevaReserva = (reserva)this.dgListaTodasReservas.SelectedItem;
            if (!user.rol.nombre.Equals("Administrador"))
            {
                if (mvReserva.nuevaReserva.fecha_reserva >= DateTime.Now || (mvReserva.nuevaReserva.fecha_reserva == DateTime.Now && (mvReserva.nuevaReserva.hora_fin.Hours - mvReserva.time) >= 1))
                {
                    cancelarReserva();
                }
                else MessageBox.Show("Ya no se puede cancelar esta reserva", "RESERVA", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else cancelarReserva();
        }

        /* En este método se pregunta al usuario si está seguro de cancelar la reserva, en caso afirmativo se anula la reserva usando la clase mv y se actualiza la lista del xaml. */
        private void cancelarReserva()
        {
            MessageBoxResult res = MessageBox.Show("¿De verdad quieres cancelar esta reserva?", "RESERVA", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (res == MessageBoxResult.Yes)
            {
                mvReserva.nuevaReserva.anulado = 1;

                mvReserva.guardar(2);
                mvReserva = new MVReserva(gestionEnt, user, cod);
                DataContext = mvReserva;
                InitializeComponent();
            }
        }

        /* En este método primero se consulta al usuario si quiere marcar la reserva como no presentada, en caso afirmativo se usa el método “noPresentado” de la clase mv y se actualiza la lista del xaml. */
        private void noPresent_Click(object sender, RoutedEventArgs e)
        {
            mvReserva.nuevaReserva = (reserva)this.dgListaTodasReservas.SelectedItem;
            MessageBoxResult res = MessageBox.Show("¿Marcar esta reserva como usuario no presentado?", "RESERVA", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (res == MessageBoxResult.Yes)
            {
                mvReserva.noPresentado();
                mvReserva = new MVReserva(gestionEnt, user, cod);
                DataContext = mvReserva;
                InitializeComponent();
            }
        }

        /* Este método consulta al usuario si quiere marcar la reserva como pagada, en caso afirmativo marca la reserva como pagada y guarda los cambios realizados en la reserva y actualiza la lista del xaml. */
        private void pago_Click(object sender, RoutedEventArgs e)
        {
            mvReserva.nuevaReserva = (reserva)this.dgListaTodasReservas.SelectedItem;
            MessageBoxResult res = MessageBox.Show("¿Marcar esta reserva como pagada?", "RESERVA", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (res == MessageBoxResult.Yes)
            {
                mvReserva.nuevaReserva.pagado = 1;

                mvReserva.guardar(2);
                mvReserva = new MVReserva(gestionEnt, user, cod);
                DataContext = mvReserva;
                InitializeComponent();

            }
        }
    }
}
