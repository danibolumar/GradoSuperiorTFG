using LiveCharts;
using LiveCharts.Wpf;
using PoliGest.BackEnd.Modelo;
using PoliGest.MVVM;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace PoliGest.FrontEnd.Dialogos
{
    /// <summary>
    /// Lógica de interacción para DiagChart.xaml
    /// </summary>
    public partial class DiagChart : Window
    {
        private MVReserva mvReserva;
        private List<String> meses = new List<string>();
        public DiagChart(GestionPolideportivaEntities gestion)
        {
            InitializeComponent();
            mvReserva = new MVReserva(gestion);
            meses.Add("Enero");
            meses.Add("Febrero");
            meses.Add("Marzo");
            meses.Add("Abril");
            meses.Add("Mayo");
            meses.Add("Junio");
            meses.Add("Julio");
            meses.Add("Agosto");
            meses.Add("Septiembre");
            meses.Add("Octubre");
            meses.Add("Noviembre");
            meses.Add("Diciembre");
            loadChart();
        }

        private void loadChart()
        {
            Dictionary<String, int> numResMes = new Dictionary<string, int>();
            foreach (string mes in meses)
            {
                numResMes.Add(mes, 0);
            }
            // Crea una lista de ChartValues para almacenar la cantidad de alumnos
            ChartValues<int> valores = new ChartValues<int>();
            // Crea una lista de string para almacenar los nombres de los grupos
            List<String> etiquetas = new List<string>();
            foreach (reserva res in mvReserva.listaRes)
            {
                numResMes[meses[res.fecha_reserva.Month - 1]]++;
            }

            foreach (var lista in numResMes)
            {
                valores.Add(lista.Value);
                etiquetas.Add(lista.Key);
            }

            // Creamos la Serie del gráfico. Contiene los valores a visualizar
            // Se corresponde al eje Y
            lvGrupos.Series = new SeriesCollection
            {
                new ColumnSeries
                    {
                        Title = "Número de Reservas", // Título
                        Values = valores, // Número de alumnos en cada grupo
                        DataLabels = true, // Visualizamos las etiquetas
                        Fill = Brushes.LightBlue // Lo visualizamos del color de la aplicación
                    }
            };
            // Valores que veremos en el ejeX
            lvGrupos.AxisX.Add(new Axis
            {
                Title = "Mes", // Titulo del eje X
                Labels = etiquetas, // Nombres de los grupos
                Unit = 1 // Separación entre valores
            });

        }
    }
}
