using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace SICPERU.Pages.Gateways
{


    public class EditarModel : PageModel
    {
        public GatewayInfo gatewayinfo = new GatewayInfo();
       
        public String errorMessage = "";
        public String successMessage = "";
        
      

        public void OnGet()
        {
            

            string id_chip = Request.Query["id_chip"];

            try
            {

                string connectionString = "Data Source=W10LIMJG\\STACIONLIMA;Initial Catalog=SICPERU;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "select * from gateways where id_chip=@id_chip";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id_chip", id_chip);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                gatewayinfo.id_chip = "" + reader.GetInt32(0);
                                gatewayinfo.numero_chip = reader.GetString(1);
                                gatewayinfo.nombre_gateway = reader.GetString(2);
                                gatewayinfo.ubicacion_gateway = reader.GetString(3);
                                gatewayinfo.fecha_colocacion = reader.GetString(4);
                                gatewayinfo.fecha_extraccion = reader.GetString(5);
                                gatewayinfo.numero_nuevo = reader.GetString(6);
                                gatewayinfo.estado = reader.GetString(7);
                            }
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            

        }

        public void OnPost()
        {
            gatewayinfo.id_chip = Request.Form["id_chip"];
            gatewayinfo.numero_chip = Request.Form["numero_chip"];
            gatewayinfo.nombre_gateway = Request.Form["nombre_gateway"];
            gatewayinfo.ubicacion_gateway = Request.Form["ubicacion_gateway"];
            gatewayinfo.fecha_colocacion = Request.Form["fecha_colocacion"];
            gatewayinfo.fecha_extraccion = Request.Form["fecha_extraccion"];
            gatewayinfo.numero_nuevo = Request.Form["numero_nuevo"];
            gatewayinfo.estado = Request.Form["estado"];

            if (gatewayinfo.id_chip.Length == 0 || gatewayinfo.numero_chip.Length == 0 || gatewayinfo.nombre_gateway.Length == 0 ||
               gatewayinfo.ubicacion_gateway.Length == 0 || gatewayinfo.fecha_colocacion.Length == 0 ||
               gatewayinfo.fecha_extraccion.Length == 0 || gatewayinfo.numero_nuevo.Length == 0 || gatewayinfo.estado.Length == 0)
            {
                errorMessage = "Debe llenar todos los campos";
                return;
            }

            try
            {
                String connectionString = "Data Source=W10JLIMJG\\WORKSTATIONISO;Initial Catalog=SICPERU;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE gateways " +
                                "SET numero_chip=@numero_chip, nombre_gateway=@nombre_gateway, ubicacion_gateway=@ubicacion_gateway, fecha_colocacion=@fecha_colocacion, fecha_extraccion=@fecha_extraccion, numero_nuevo=@numero_nuevo, estado=@estado " +
                                "WHERE id_chip=@id_chip";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@numero_chip", gatewayinfo.numero_chip);
                        command.Parameters.AddWithValue("@nombre_gateway", gatewayinfo.nombre_gateway);
                        command.Parameters.AddWithValue("@ubicacion_gateway", gatewayinfo.ubicacion_gateway);
                        command.Parameters.AddWithValue("@fecha_colocacion", gatewayinfo.fecha_colocacion);
                        command.Parameters.AddWithValue("@fecha_extraccion", gatewayinfo.fecha_extraccion);
                        command.Parameters.AddWithValue("@numero_nuevo", gatewayinfo.numero_nuevo);
                        command.Parameters.AddWithValue("@estado", gatewayinfo.estado);
                        command.Parameters.AddWithValue("@id_chip", gatewayinfo.id_chip);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Gateways/chips");
        }
    }
}
