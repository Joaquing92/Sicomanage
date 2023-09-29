using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace SICPERU.Pages.Usuarios
{
    public class C2VModel : PageModel
    {


        public int SumaUsuarios => listusuariosC2V.Count;

        public List<C2VInfo> listusuariosC2V = new List<C2VInfo>();
        public void OnGet()
        {

            try
            {
                string connectionString = "Data Source=10.10.8.12\\INSTBDD01;Initial Catalog=MarcadorSic;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"SELECT 
    ISNULL(u.Username, '-') AS Username,
    CONCAT(
        COALESCE(u.Nombre1, '-'), ' ',
        COALESCE(u.Nombre2, '-'), ' ',
        COALESCE(u.Apellido1, '-'), ' ',
        COALESCE(u.Apellido2, '-')
    ) AS NombreCompleto,
    COALESCE(r.nombre, '-') AS Perfil,
    COALESCE(x.extnumber, 'No asignada') AS Extension,
    CASE WHEN u.Activo = '1' THEN 'activo' ELSE 'inactivo' END AS Estado,
    u.fecharegistro
FROM marcadorsic..xf_usuarios u WITH (NOLOCK)
LEFT JOIN MarcadorSic..XF_roles r WITH (NOLOCK) ON u.idroles = r.id
LEFT JOIN marcadorsic..t_pbx_ext x WITH (NOLOCK) ON u.iduser = x.iduser
ORDER BY Extension ASC;
";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                C2VInfo c2vInfo = new C2VInfo();

                                c2vInfo.username = reader["username"].ToString();
                                c2vInfo.NombreCompleto = reader["NombreCompleto"].ToString();
                                c2vInfo.Perfil = reader["Perfil"].ToString();
                                c2vInfo.Extension = reader["Extension"].ToString();
                                c2vInfo.Estado = reader["Estado"].ToString();
                                c2vInfo.Fecharegistro = reader["Fecharegistro"].ToString();
                               

                                listusuariosC2V.Add(c2vInfo);
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

    public class C2VInfo
    {
        public string username { get; set; }
        public string NombreCompleto { get; set; }
        public string Perfil { get; set; }
        public string Extension { get; set; }
        public string Estado { get; set; }
        public string Fecharegistro { get; set; }


    }
}
