using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace SICPERU.Pages.Gateways
{
    public class CreateModel : PageModel
    {
        public GatewayInfo gatewayinfo = new GatewayInfo();
        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            gatewayinfo.numero_chip = Request.Form["numero_chip"];
            gatewayinfo.nombre_gateway = Request.Form["nombre_gateway"];
            gatewayinfo.ubicacion_gateway = Request.Form["ubicacion_gateway"];
            gatewayinfo.fecha_colocacion = Request.Form["fecha_colocacion"];
            gatewayinfo.fecha_extraccion = Request.Form["fecha_extraccion"];
            gatewayinfo.numero_nuevo = Request.Form["numero_nuevo"];
            gatewayinfo.estado = Request.Form["estado"];

            if (gatewayinfo.numero_chip.Length == 0 || gatewayinfo.nombre_gateway.Length == 0 ||
                gatewayinfo.ubicacion_gateway.Length == 0 || gatewayinfo.fecha_colocacion.Length == 0 ||
                gatewayinfo.fecha_extraccion.Length == 0 || gatewayinfo.numero_nuevo.Length == 0 || gatewayinfo.estado.Length == 0)
            {
                errorMessage = "Debe llenar todos los campos";
                return;
            }

            // Guardar nuevo chip
            try
            {
                string connectionString = "Data Source=W10LIMJG\\STACIONLIMA;Initial Catalog=SICPERU;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO gateways " +
                                 "(numero_chip, nombre_gateway, ubicacion_gateway, fecha_colocacion, fecha_extraccion, numero_nuevo, estado) VALUES" +
                                 "(@numero_chip, @nombre_gateway, @ubicacion_gateway, @fecha_colocacion, @fecha_extraccion, @numero_nuevo, @estado);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@numero_chip", gatewayinfo.numero_chip);
                        command.Parameters.AddWithValue("@nombre_gateway", gatewayinfo.nombre_gateway);
                        command.Parameters.AddWithValue("@ubicacion_gateway", gatewayinfo.ubicacion_gateway);
                        command.Parameters.AddWithValue("@fecha_colocacion", gatewayinfo.fecha_colocacion);
                        command.Parameters.AddWithValue("@fecha_extraccion", gatewayinfo.fecha_extraccion);
                        command.Parameters.AddWithValue("@numero_nuevo", gatewayinfo.numero_nuevo);
                        command.Parameters.AddWithValue("@estado", gatewayinfo.estado);

                        command.ExecuteNonQuery();
                    }
                }

				successMessage = "Nuevo chip agregado correctamente";
			}
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            gatewayinfo.numero_chip = "";
            gatewayinfo.nombre_gateway = "";
            gatewayinfo.ubicacion_gateway = "";
            gatewayinfo.fecha_colocacion = "";
            gatewayinfo.fecha_extraccion = "";
            gatewayinfo.numero_nuevo = "";
            gatewayinfo.estado = "";

            Response.Redirect("/Gateways");
        }
    }
}
