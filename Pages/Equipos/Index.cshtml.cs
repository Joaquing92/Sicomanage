using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;


namespace SICPERU.Pages.Equipos
{
    public class IndexModel : PageModel
    {

        public List<ReportePagosInfo> listreporte = new List<ReportePagosInfo>();

        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=172.16.24.15\\INSTBDD02;Integrated Security=True;Initial Catalog=CRMPlanificacion;MultipleActiveResultSets=True;Initial Catalog=CRMInformacion;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"
                IF OBJECT_ID('tempdb..#pagosenprocesofaltante') IS NOT NULL
                    DROP TABLE #pagosenprocesofaltante;

                IF OBJECT_ID('tempdb..#Operaciones') IS NOT NULL
                    DROP TABLE #Operaciones;


                SELECT o.IdOperacion, NumeroOperacion, PagoEnProceso, SaldoInicial, o.CodigoProductoGestion, OC.CodigoEtapa, o.CodigoProducto, E.Nombre, SaldoInicial as ValorTotalCuota, SaldoInicial as Diferencia
                INTO #pagosenprocesofaltante
                FROM CRMInformacion.dbo.operacioncobranza oc WITH (NOLOCK)
                INNER JOIN CRMInformacion.dbo.Operacion o WITH (NOLOCK) ON oc.IdOperacion = o.IdOperacion
                INNER JOIN CRMInformacion..DetalleAsignacionRecurso d WITH (NOLOCK) ON d.IdOperacion = o.IdOperacion AND d.EsActivo = 1
                INNER JOIN CRMPlanificacion..estrategia e WITH (NOLOCK) ON d.IdEstrategia = e.idestrategia AND ClaseCampania = 'TIPCAMPAD' AND e.EsActivo = 1 AND CodigoCanal = 'CALLCENTER'
                WHERE o.EsActivo = 1 AND PagoEnProceso > 0
                ORDER BY PagoEnProceso DESC;


                SELECT DISTINCT IdOperacion INTO #Operaciones FROM #pagosenprocesofaltante;


                UPDATE #pagosenprocesofaltante
                SET ValorTotalCuota = ab.ValorTotalCuota, Diferencia = PagoEnProceso - ab.ValorTotalCuota
                FROM #pagosenprocesofaltante i
                INNER JOIN (
                    SELECT SUM(ValorAbono) Abonos, do.IdOperacion, SUM(do.ValorTotalCuota) ValorTotalCuota
                    FROM #Operaciones i
                    INNER JOIN CRMInformacion.dbo.DetalleOperacion do WITH (NOLOCK) ON i.IdOperacion = do.IdOperacion AND do.EsActivo = 1
                    GROUP BY do.IdOperacion
                ) AS ab ON i.IdOperacion = ab.IdOperacion;

                SELECT * FROM #pagosenprocesofaltante;";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                

                                ReportePagosInfo reporteinfo = new ReportePagosInfo();
                                reporteinfo.idOperacion = reader["IdOperacion"].ToString();
                                reporteinfo.NumeroOperacion = reader["NumeroOperacion"].ToString();
                                reporteinfo.PagoEnProceso = reader["PagoEnProceso"].ToString();
                                reporteinfo.SaldoInicial = reader["SaldoInicial"].ToString();
                                reporteinfo.CodigoProductoGestion = reader["CodigoProductoGestion"].ToString();
                                reporteinfo.CodigoEtapa = reader["CodigoEtapa"].ToString();
                                reporteinfo.CodigoProducto = reader["CodigoProducto"].ToString();
                                reporteinfo.Nombre = reader["Nombre"].ToString();
                                reporteinfo.ValorTotalCuota = reader["ValorTotalCuota"].ToString();
                                reporteinfo.Diferencia = reader["Diferencia"].ToString();



                                listreporte.Add(reporteinfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }

    public class ReportePagosInfo
    {
        public string idOperacion { get; set; }
        public string NumeroOperacion { get; set; }
        public string PagoEnProceso { get; set; }
        public string SaldoInicial { get; set; }
        public string CodigoProductoGestion { get; set; }
        public string CodigoEtapa { get; set; }
        public string CodigoProducto { get; set; }
        public string Nombre { get; set; }
        public string ValorTotalCuota { get; set; }
        public string Diferencia { get; set; }

    }
}


