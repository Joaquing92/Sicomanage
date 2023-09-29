using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace SICPERU.Pages.Lineas
{
    public class ChipsModel : PageModel
    {
        public List<ChipsInfo> listachips = new List<ChipsInfo>();
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
                                ChipsInfo chipinfo = new ChipsInfo();
                                chipinfo.ID_CHIP = reader.GetInt32(0).ToString();
                                chipinfo.NUMERO_CHIP = reader.GetString(1);
                                chipinfo.GATEWAY = reader.GetString(2);
                                chipinfo.SLOT = reader.GetString(3); // Corrección aquí

                                listachips.Add(chipinfo);
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

    public class ChipsInfo
    {
        public string ID_CHIP { get; set; }
        public string NUMERO_CHIP { get; set; }
        public string GATEWAY { get; set; }
        public string SLOT { get; set; }
    
    }
}