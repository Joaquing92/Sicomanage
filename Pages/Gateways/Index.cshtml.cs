using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace SICPERU.Pages.Gateways
{
    public class IndexModel : PageModel
    {
        public List<GatewayInfo> listgateways = new List<GatewayInfo>();

        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=W10LIMJG\\STACIONLIMA;Initial Catalog=SICPERU;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM gateways";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                GatewayInfo gatewayInfo = new GatewayInfo();
                                gatewayInfo.id_chip = reader.GetInt32(0).ToString();
                                gatewayInfo.numero_chip = reader.GetString(1);
                                gatewayInfo.nombre_gateway = reader.GetString(2);
                                gatewayInfo.ubicacion_gateway = reader.GetString(3);
                                gatewayInfo.fecha_colocacion = reader.GetString(4);
                                gatewayInfo.fecha_extraccion = reader.GetString(5);
                                gatewayInfo.numero_nuevo = reader.GetString(6);
                                gatewayInfo.estado = reader.GetString(7);

                                listgateways.Add(gatewayInfo);
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

    public class GatewayInfo
    {
        public string id_chip { get; set; }
        public string numero_chip { get; set; }
        public string nombre_gateway { get; set; }
        public string ubicacion_gateway { get; set; }
        public string fecha_colocacion { get; set; }
        public string fecha_extraccion { get; set; }
        public string numero_nuevo { get; set; }
        public string estado { get; set; }
    }
}