using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace SICPERU.Pages.Inventarios
{
    public class FenixModel : PageModel
    {
        public List<FenixInfo> listusuariosFenix = new List<FenixInfo>();
        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=172.16.24.18\\instbdd05;Initial Catalog=Seguridad;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"SELECT
    u.NumeroIdentificacion,
    CONCAT(u.PrimerNombre, ' ', u.SegundoNombre, ' ', u.ApellidoPaterno, ' ', u.ApellidoMaterno) AS NombreCompleto,
    CASE
        WHEN LEN(u.NumeroIdentificacion) = 8 THEN 'DNI'
        ELSE 'CEDULA'
    END AS TipoIdentificacion,
    CASE
        WHEN u.Estado = 'act' THEN 'ACTIVO'
        ELSE u.Estado
    END AS Estado,
    STUFF((
        SELECT DISTINCT ', ' + p.Nombre
        FROM seguridad..usuarioperfil AS up2 WITH(NOLOCK)
        JOIN seguridad..perfil AS p WITH(NOLOCK) ON up2.IdPerfil = p.IdPerfil
        WHERE up2.IdUsuario = u.IdUsuario and up2.esactivo='1'
        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS Perfiles,
    u.UserName,
    u.FechaCreacion,
    u.FechaUltimoAcceso,
    u.EstacionUltimoAcceso
FROM seguridad..usuario AS u WITH(NOLOCK)
LEFT JOIN seguridad..usuarioperfil AS up ON u.IdUsuario = up.IdUsuario
LEFT JOIN seguridad..perfil AS p ON up.IdPerfil = p.IdPerfil
WHERE u.Estado = 'ACT'
GROUP BY u.IdUsuario, u.NumeroIdentificacion, u.PrimerNombre, u.SegundoNombre, u.ApellidoPaterno, u.ApellidoMaterno, u.UserName, u.FechaCreacion, u.FechaUltimoAcceso, u.EstacionUltimoAcceso, u.Estado
ORDER BY u.IdUsuario ASC";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                FenixInfo fenixInfo = new FenixInfo();

                                fenixInfo.NumeroIdentificacion = reader["NumeroIdentificacion"].ToString();
                                fenixInfo.NombreCompleto = reader["NombreCompleto"].ToString();
                                fenixInfo.TipoIdentificacion = reader["TipoIdentificacion"].ToString();
                                fenixInfo.Estado = reader["Estado"].ToString();
                                fenixInfo.Perfiles = reader["Perfiles"].ToString();
                                fenixInfo.UserName = reader["UserName"].ToString();
                                fenixInfo.FechaCreacion = reader["FechaCreacion"].ToString();
                                fenixInfo.FechaUltimoAcceso = reader["FechaUltimoAcceso"].ToString();
                                fenixInfo.EstacionUltimoAcceso = reader["EstacionUltimoAcceso"].ToString();

                                listusuariosFenix.Add(fenixInfo);
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

    public class FenixInfo
    {
        public string NumeroIdentificacion { get; set; }
        public string NombreCompleto { get; set; }
        public string TipoIdentificacion { get; set; }
        public string Estado { get; set; }
        public string Perfiles { get; set; }
        public string UserName { get; set; }
        public string FechaCreacion { get; set; }
        public string FechaUltimoAcceso { get; set; }
        public string EstacionUltimoAcceso { get; set; }

    }
}
  