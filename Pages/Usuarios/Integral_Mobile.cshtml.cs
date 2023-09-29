using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace SICPERU.Pages.Inventarios
{
    public class Integral_MobileModel : PageModel
    {
        public List<IntegralInfo> listusuariosintegral = new List<IntegralInfo>();
        public void OnGet()
        {

            try
            {
                string connectionString = "Data Source = 172.16.24.15\\instbdd02; Initial Catalog = Seguridad; Integrated Security = True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"SELECT 
    nickname, 
    CONCAT(nombres, ' ', apellidos) AS Nombre,
    IMEI,  
    CASE
        WHEN firma IS NOT NULL THEN 'SI'
        ELSE 'NO'
    END AS Firma,
	fechacreacion
FROM IntegralMobile..geogestor WITH (NOLOCK)
WHERE SysStatus = '1' AND Eliminado = '0'
order by fechacreacion desc;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                IntegralInfo integralInfo = new IntegralInfo();

                                integralInfo.nickname = reader["nickname"].ToString();
                                integralInfo.Nombre = reader["Nombre"].ToString();
                                integralInfo.IMEI = reader["IMEI"].ToString();
                                integralInfo.Firma = reader["Firma"].ToString();
                                integralInfo.fechacreacion = reader["fechacreacion"].ToString();
                             


                                listusuariosintegral.Add(integralInfo);
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

    public class IntegralInfo
    {
        public string nickname { get; set; }
        public string Nombre { get; set; }
        public string IMEI { get; set; }
        public string Firma { get; set; }
        public string fechacreacion { get; set; }
        
    }
}