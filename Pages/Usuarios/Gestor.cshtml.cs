using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace SICPERU.Pages.Inventarios
{
    public class GestorModel : PageModel
    {
        public List<GestorInfo> listusuariosgestor = new List<GestorInfo>();
        public void OnGet()
        {


            try
            {
                string connectionString = "Data Source = 172.16.24.15\\instbdd02; Initial Catalog = Seguridad; Integrated Security = True";
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
                                GestorInfo gestorInfo = new GestorInfo();

                                gestorInfo.NumeroIdentificacion = reader["NumeroIdentificacion"].ToString();
                                gestorInfo.NombreCompleto = reader["NombreCompleto"].ToString();
                                gestorInfo.TipoIdentificacion = reader["TipoIdentificacion"].ToString();
                                gestorInfo.Estado = reader["Estado"].ToString();
                                gestorInfo.Perfiles = reader["Perfiles"].ToString();
                                gestorInfo.UserName = reader["UserName"].ToString();
                                gestorInfo.FechaCreacion = reader["FechaCreacion"].ToString();
                                gestorInfo.FechaUltimoAcceso = reader["FechaUltimoAcceso"].ToString();
                                gestorInfo.EstacionUltimoAcceso = reader["EstacionUltimoAcceso"].ToString();

                                listusuariosgestor.Add(gestorInfo);
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

    public class GestorInfo
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