using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using PoliGest.BackEnd.Modelo;
using PoliGest.MVVM;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PoliGest.FrontEnd.Dialogos
{
    /// <summary>
    /// Lógica de interacción para DiagRealizarReserva.xaml
    /// </summary>
    public partial class DiagRealizarReserva : MetroWindow
    {
        private GestionPolideportivaEntities gestionEnt;
        private MVReserva mvReserva;
        private int horaMaxTipoInst;
        private usuario usu;

        public DiagRealizarReserva(GestionPolideportivaEntities gestion, usuario usu)
        {
            InitializeComponent();
            this.usu = usu;
            gestionEnt = gestion;
            mvReserva = new MVReserva(gestionEnt);
            DataContext = mvReserva;
            this.fechaReserva.DisplayDateStart = DateTime.Now;
            this.fechaReserva.SelectedDate = DateTime.Now;
            if(usu.rol.nombre.Equals("Socio")) this.fechaReserva.DisplayDateEnd = DateTime.Now.AddDays(14);
            this.comboHoraInicial.IsEnabled = false;
            this.comboHoraFin.IsEnabled = false;
        }

        /* 
         *El propósito de este método consiste en buscar una o varias instalaciones del tipo que haya seleccionado el usuario que tengan disponible algún horario
         *en la fecha que el usuario ha seleccionado, para ello usa el método explicado arriba “validarFechaYTipo” y si este método devuelve “true” 
         *usa el método reservasDelTipoInst de la clase mv usada, en caso que el método de la clase mv devuelve “false” se le indica al usuario, 
         *en caso contrario se carga en el comboBox del horario las horas disponibles.
         */
        public async void disponibilidad()
        {
            if (validarFechaYTipo())
            {
                mvReserva.nuevaReserva.hora_fin = TimeSpan.Zero;
                mvReserva.nuevaReserva.hora_inicio = TimeSpan.Zero;
                this.comboHoraInicial.SelectedItem = null;
                this.comboHoraFin.SelectedItem = null;
                if (!mvReserva.reservasDelTipoInst())
                {
                    await this.ShowMessageAsync("REALIZAR RESERVA", "No hay instalaciones disponibles");
                    this.comboHoraInicial.IsEnabled = false;
                    this.comboHoraFin.IsEnabled = false;
                    this.botones.IsEnabled = false;
                    this.precioTotal.Text = "";
                    this.instalacionSel.Text = "";
                    
                }

                else
                {
                    this.comboHoraInicial.ItemsSource = mvReserva.listaHoraInicio;
                    this.comboHoraFin.ItemsSource = mvReserva.listaHoraFin;
                    this.comboHoraInicial.IsEnabled = true;
                    this.comboHoraFin.IsEnabled = true;
                }
            }
        }

        private void fechaReserva_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            this.fechaReserva.Foreground = Brushes.Black;
            mvReserva.nuevaReserva.fecha_reserva = (DateTime)this.fechaReserva.SelectedDate;
            disponibilidad();
        }

        private void comboTipos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.comboTipos.Foreground = Brushes.Black;
            disponibilidad();
            this.horaMaxTipoInst = this.mvReserva.tipoInstalacionSeleccionado.tiempo_max;
        }

        /* Este método comprueba que el usuario haya seleccionado una fecha y un tipo de instalación */
        private bool validarFechaYTipo()
        {
            if (this.fechaReserva.SelectedDate == null)
            {
                this.fechaReserva.Foreground = Brushes.Red;
                this.fechaReserva.Focus();
                return false;
            }

            if (this.comboTipos.SelectedItem == null)
            {
                this.comboTipos.Foreground = Brushes.Red;
                this.comboTipos.Focus();
                return false;
            }

            return true;
        }

        private async void comboHoraInicial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!comprobarHorario(this.comboHoraInicial, this.comboHoraFin))
            {
                await this.ShowMessageAsync("REALIZAR RESERVA", "Esa hora de inicio no es correcta, Selecciona otra.");
                this.precioTotal.Text = "";
                this.instalacionSel.Text = "";
                this.botones.IsEnabled = false;
            }
            else if (this.comboHoraFin.SelectedItem != null)
            {
                cargarDatosNReserva();
            }

        }

        private async void comboHoraFin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!comprobarHorario(this.comboHoraFin, this.comboHoraInicial))
            {
                await this.ShowMessageAsync("REALIZAR RESERVA", "Esa hora de fin no es correcta, Selecciona otra.");
                this.precioTotal.Text = "";
                this.instalacionSel.Text = "";
                this.botones.IsEnabled = false;
            }
            else if (this.comboHoraInicial.SelectedItem != null)
            {
                cargarDatosNReserva();
            }
        }

        /* Este método comprueba que el rango de horas seleccionadas por el usuario sean correctas. */
        private bool comprobarHorario(ComboBox combo, ComboBox comboH)
        {
            int min = this.horaMaxTipoInst * 60;
            if (comboH.SelectedItem != null)
            {
                if (mvReserva.nuevaReserva.hora_fin.TotalMinutes < mvReserva.nuevaReserva.hora_inicio.TotalMinutes || (usu.rol.nombre.Equals("Socio") && (mvReserva.nuevaReserva.hora_fin.TotalMinutes - mvReserva.nuevaReserva.hora_inicio.TotalMinutes) > min) || (mvReserva.nuevaReserva.hora_fin.TotalMinutes - mvReserva.nuevaReserva.hora_inicio.TotalMinutes) == 0)
                {
                    combo.Foreground = Brushes.Red;
                    return false;
                }
            }
            combo.Foreground = Brushes.Black;
            comboH.Foreground = Brushes.Black;
            return true;
        }

        /* Este método comprueba mediante el método “asignarInstalacion” de la clase mv si hay instalaciones disponibles que cumplan los requisitos que el usuario
         * selecciona, en caso afirmativo se muestra en el diálogo la instalación asignada, de lo contrario se muestra un mensaje al usuario indicando que en la
         * franja horaria que ha seleccionado no hay instalaciones disponibles.
         */
        private async void cargarDatosNReserva()
        {
            double hora = mvReserva.nuevaReserva.hora_fin.TotalMinutes - mvReserva.nuevaReserva.hora_inicio.TotalMinutes;
            mvReserva.nuevaReserva.precio = (hora / 60) * mvReserva.tipoInstalacionSeleccionado.precio_hora;
            this.precioTotal.Text = "El precio de la reserva es: " + mvReserva.nuevaReserva.precio + " €";
            if (mvReserva.asignarInstalacion())
            {
                this.instalacionSel.Text = "La instalación de tu reserva es: " + mvReserva.nuevaReserva.instalacion.nombre;
                this.botones.IsEnabled = true;
            }
            else
            {
                this.precioTotal.Text = "";
                await this.ShowMessageAsync("REALIZAR RESERVA", "No se puede hacer una reserva con ese franjo de horas, por favor selecciona otras");
                this.botones.IsEnabled = false;
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /* En este evento solo se le pregunta al usuario si quiere pagar la reserva online y se guarda la nueva reserva. */
        private async void aceptar_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult res = MessageBox.Show("¿Pagar la reserva online?", "RESERVA", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                mvReserva.nuevaReserva.pagado = 1;
            }
            else mvReserva.nuevaReserva.pagado = 0;

            mvReserva.nuevaReserva.no_presentado = 0;
            mvReserva.nuevaReserva.anulado = 0;
            mvReserva.nuevaReserva.usuario = usu;

            await this.ShowMessageAsync("REALIZAR RESERVA", "Tiene hasta una hora antes para cancelar la reserva, si no la cancela y no se presenta será sancionado sin poder entrar a la aplicación por un periodo de 15 días a partir de la fecha de la reserva");

            mvReserva.guardar(1);
            this.Close();
        }
    }
}
