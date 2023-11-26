using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using PoliGest.BackEnd.Modelo;
using PoliGest.MVVM;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PoliGest.FrontEnd.Dialogos
{
    /// <summary>
    /// Lógica de interacción para DiagTipoInstalacion.xaml
    /// </summary>
    public partial class DiagTipoInstalacion : MetroWindow
    {
        private GestionPolideportivaEntities gestionEnt;
        private MVTipoInstalacion mvTipoInstalacion;
        private tipo_instalacion tipoSeleccionado = new tipo_instalacion();
        private horario hora = null;
        private int cod;

        public DiagTipoInstalacion(GestionPolideportivaEntities gestion)
        {
            inicializa(gestion);
        }

        public DiagTipoInstalacion(GestionPolideportivaEntities gestion, tipo_instalacion tipoInstalacion)
        {
            inicializa(gestion);
            tipoSeleccionado = tipoInstalacion;
            mvTipoInstalacion.bindingTipoInstalacion = tipoSeleccionado;
            this.comboHorario.SelectedItem = gestionEnt.Set<horario>().Where(h => h.idhorario == tipoSeleccionado.tiempo_max).FirstOrDefault();
            this.precioHora.Text = tipoSeleccionado.precio_hora.ToString();
            mvTipoInstalacion.cargarListaSemanal();
        }

        private void inicializa(GestionPolideportivaEntities gestion)
        {
            gestionEnt = gestion;
            mvTipoInstalacion = new MVTipoInstalacion(gestion);
            this.AddHandler(Validation.ErrorEvent, new RoutedEventHandler(mvTipoInstalacion.OnErrorEvent));
            InitializeComponent();
            DataContext = mvTipoInstalacion;
            mvTipoInstalacion.btnGuardar = this.aceptar;
        }

        private void comboDias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mvTipoInstalacion.diaSeleccionado != null)
            {
                mvTipoInstalacion.guardarHorario();

                mvTipoInstalacion.cargarHorario();
                mvTipoInstalacion.diaGuardado = (dia)this.comboDias.SelectedItem;

            }
            recargarListas();
        }

        private void comboHora_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            hora = (horario)this.comboHorario.SelectedItem;
            mvTipoInstalacion.bindingTipoInstalacion.tiempo_max = hora.idhorario;
            this.comboHorario.Foreground = Brushes.Gray;
        }

        private void quitarPerm_Click(object sender, RoutedEventArgs e)
        {
            mvTipoInstalacion.cambiarHorario((horario)this.dgTablaHorarioIn.SelectedItem, 2);
            recargarListas();
        }

        private void añadirPerm_Click(object sender, RoutedEventArgs e)
        {
            mvTipoInstalacion.cambiarHorario((horario)this.dgTablaHorarioOut.SelectedItem, 1);
            recargarListas();
        }

        private void quitarTodosPerm_Click(object sender, RoutedEventArgs e)
        {
            if (mvTipoInstalacion.listaSemanal.Count != 0)
            {
                mvTipoInstalacion.cambiarHorarioList(2);
                recargarListas();
            }
        }

        private void añadirTodosPerm_Click(object sender, RoutedEventArgs e)
        {
            if (mvTipoInstalacion.listaSemanal.Count != 0)
            {
                mvTipoInstalacion.cambiarHorarioList(1);
                recargarListas();
            }
        }

        private void recargarListas()
        {
            this.dgTablaHorarioIn.ItemsSource = mvTipoInstalacion.listaHorarioIn;
            this.dgTablaHorarioOut.ItemsSource = mvTipoInstalacion.listaHorarioOut;
        }

        /* Este evento se encarga de comprobar que el tipo instalación que queremos guardar tiene al menos un horario y usando el método “validarDatos” compruebo que todos los datos puesto sean correctos, en caso que todo esté correcto se guarda el tipo de instalación. */

        private async void aceptar_Click(object sender, RoutedEventArgs e)
        {
            if (mvTipoInstalacion.listaSemanal.Count == 0)
            {
                this.comboHorario.Foreground = Brushes.Black;
                this.comboDias.Foreground = Brushes.Red;
            }
            else
            {
                this.comboDias.Foreground = Brushes.Black;
                mvTipoInstalacion.bindingTipoInstalacion.tiempo_max = hora.idhorario;

                tipo_instalacion cogerId = gestionEnt.Set<tipo_instalacion>().Where(t => t.descripcion == mvTipoInstalacion.bindingTipoInstalacion.descripcion &&
                    (mvTipoInstalacion.bindingTipoInstalacion.idtipo == 0 || mvTipoInstalacion.bindingTipoInstalacion.idtipo != t.idtipo)).FirstOrDefault();

                if (cogerId != null)
                {
                    await this.ShowMessageAsync("CREAR TIPO INSTALACION", "Este tipo de instalacion ya existe");
                    mvTipoInstalacion.bindingTipoInstalacion.descripcion = null;
                    this.txtDescripcion.Text = null;
                }
                else if (validarDatos())
                {

                    mvTipoInstalacion.bindingTipoInstalacion.precio_hora = Double.Parse(this.precioHora.Text);
                    int resultGuardarTI;
                    if (tipoSeleccionado.descripcion == null)
                    {
                        cod = 1;
                        resultGuardarTI = mvTipoInstalacion.guardarTipoInstalacion(1);
                    }
                    else
                    {
                        cod = 2;
                        resultGuardarTI = mvTipoInstalacion.guardarTipoInstalacion(2);
                    }

                    if (resultGuardarTI == -1)
                    {
                        await this.ShowMessageAsync("CREAR TIPO INSTALACIÓN", "Por favor introduce al menos un rango de horas en al menos un día de la semana");
                        this.comboDias.Foreground = Brushes.Red;
                    }

                    if (resultGuardarTI == -2) await this.ShowMessageAsync("CREAR TIPO INSTALACIÓN", "Error al guardar el tipo de instalación, por favor intentalo de nuevo");

                    if (resultGuardarTI == 1)
                    {
                        if (cod == 2) await this.ShowMessageAsync("AVISO MODIFICACIÓN TIPO INSTALACIÓN",
                             "Los cambios realizados sobre el horario se aplicarán en las reservas que se creen a partide este momento, NO en las ya realizadas");
                        DialogResult = true;
                    }
                }
            }
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        /* Este método comprueba que el tipo de instalación que queremos crear o modificar contiene un tiempo máximo para las reservas y que contiene una cantidad a pagar por hora. */
        private bool validarDatos()
        {
            if (hora == null)
            {
                comboHorario.Foreground = Brushes.Red;
                return false;
            }
            else if (this.precioHora.Text.Contains("_") || this.precioHora.Text.Contains("_"))
            {
                this.precioHora.Foreground = Brushes.Red;
                return false;
            }
            else
            {
                precioHora.Foreground = Brushes.Black;
            }

            return true;
        }
    }
}
