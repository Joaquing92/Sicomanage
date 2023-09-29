using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;

namespace SICPERU.Pages.Lineas
{
    public class EliminarModel : PageModel
    {
        public string SuccessMessage { get; set; }

        public IActionResult OnGet(string id_chip)
        {
            // Validar si el ID del chip se proporcionó correctamente
            if (string.IsNullOrEmpty(id_chip))
            {
                return RedirectToPage("/Lineas/Chips");
            }

            try
            {
                // Realizar la eliminación del elemento con el ID especificado
                string connectionString = "Data Source=W10LIMJG\\STACIONLIMA;Initial Catalog=SICPERU;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "delete from gateways where id_chip=@id_chip";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id_chip", id_chip);
                        command.ExecuteNonQuery();
                    }
                }

                // Establecer el mensaje de éxito
                SuccessMessage = "El Chip fue eliminado correctamente";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
                // En caso de error, puedes mostrar un mensaje de error o redireccionar a una página de error
                return RedirectToPage("/Error");
            }

            return RedirectToPage("/Lineas/Chips");
        }
    }
}
