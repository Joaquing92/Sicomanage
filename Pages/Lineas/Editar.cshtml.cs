using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace SICPERU.Pages.Lineas
{
    public class EditarModel : PageModel
    {
        public ChipsInfo chipsInfo = new ChipsInfo(); 

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
                                chipsInfo.ID_CHIP = "" + reader.GetInt32(0);
                                chipsInfo.NUMERO_CHIP = reader.GetString(1);
                                chipsInfo.GATEWAY = reader.GetString(2);
                                chipsInfo.SLOT = reader.GetString(3);
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
            chipsInfo.ID_CHIP = Request.Form["ID_CHIP"];
            chipsInfo.NUMERO_CHIP = Request.Form["NUMERO_CHIP"];
            chipsInfo.GATEWAY = Request.Form["GATEWAY"];
            chipsInfo.SLOT = Request.Form["SLOT"];


            if (chipsInfo.ID_CHIP.Length == 0 || chipsInfo.NUMERO_CHIP.Length == 0 || chipsInfo.GATEWAY.Length == 0 || chipsInfo.SLOT.Length == 0)
            {
                errorMessage = "Debe llenar todos los campos";
                return;
            }

            try
            {
                String connectionString = "Data Source=W10LIMJG\\STACIONLIMA;Initial Catalog=SICPERU;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE gateways " +
                                "SET NUMERO_CHIP=@NUMERO_CHIP, GATEWAY=@GATEWAY, SLOT=@SLOT " +
                                "WHERE ID_CHIP=@ID_CHIP";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ID_CHIP", chipsInfo.ID_CHIP);
                        command.Parameters.AddWithValue("@NUMERO_CHIP", chipsInfo.NUMERO_CHIP);
                        command.Parameters.AddWithValue("@GATEWAY", chipsInfo.GATEWAY);
                        command.Parameters.AddWithValue("@SLOT", chipsInfo.SLOT);


                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Lineas/chips");
        }
    }
}
