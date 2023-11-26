using CrystalDecisions.CrystalReports.Engine;
using PoliGest.BackEnd.Servicios;
using System;
using System.Windows;

namespace PoliGest.FrontEnd.Dialogos
{
    /// <summary>
    /// Lógica de interacción para DiagInforme.xaml
    /// </summary>
    public partial class DiagInforme : Window
    {
        public DiagInforme(int cod)
        {
            InitializeComponent();
            cargaInforme(cod);
        }

        /* En este método dependiendo del valor del parámetro carga un informe con su consulta sql u otra. */
        private void cargaInforme(int cod)
        {
            try
            {
                ReportDocument rd = new ReportDocument();
                ServicioSQL sqlServ = new ServicioSQL();

                if (cod == 1)
                {
                    rd.Load("../../Informes/CRReservaSocioTipo.rpt");
                    rd.SetDataSource(sqlServ.getDatos(
                      "select u.nombre, u.apellido1, u.dni, count(*), t.descripcion " +
                      "from reserva r, tipo_instalacion t, instalacion i, socios s, usuario u " +
                      "where r.instalacion_idinstalacion = i.idinstalacion and i.idtipo = t.idtipo " +
                      "and r.usuario_idusuario = u.idusuario group by t.descripcion, u.dni"));

                }
                else
                {
                    rd.Load("../../Informes/CRInformeAnual.rpt");
                    rd.SetDataSource(sqlServ.getDatos(
                            "select count(*) as NumeroReservas, t.descripcion as TipoInstalacion " +
                            "from reserva r, instalacion i, tipo_instalacion t " +
                            "where r.instalacion_idinstalacion = i.idinstalacion and i.idtipo = t.idtipo " +
                            "and month(r.fecha_reserva) >= month(sysdate()) and year(r.fecha_reserva) >= year(sysdate()) - 1 " + 
                            "group by t.descripcion"));
                }
                this.crInforme.ViewerCore.ReportSource = rd;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.StackTrace);
                System.Console.WriteLine(ex.InnerException);
            }
        }
    }
}
