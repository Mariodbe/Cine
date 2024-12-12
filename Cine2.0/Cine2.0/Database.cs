using MySql.Data.MySqlClient;
using System;

namespace CineApp
{
    public static class Database
    {
        // Cadena de conexión de MySQL
        private static string connectionString = "Server=dam.clp4c2egnzkb.us-east-1.rds.amazonaws.com;Database=cine;User ID=admin;Password=MagicCut;Port=3306;";

        // Método para validar el inicio de sesión
        public static bool ValidateLogin(string username, string password)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();  // Intento de conexión

                    // Consulta para verificar si existe el usuario y la contraseña
                    string query = "SELECT COUNT(*) FROM Usuarios WHERE NombreUsuario = @username AND Contrasena = @password";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Parámetros para evitar inyecciones SQL
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);  // Aquí sería ideal usar un hash

                        var result = cmd.ExecuteScalar();
                        if (int.TryParse(result?.ToString(), out int count))
                        {
                            return count > 0;  // Si el conteo es mayor a 0, la validación es exitosa
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    // Aquí puedes manejar el error, por ejemplo:
                    Console.WriteLine("Error de conexión: " + ex.Message);
                }
            }
            return false;  // Si hubo algún problema en la validación
        }

        // Método para registrar un nuevo usuario
        public static bool RegisterUser(string username, string password)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();  // Intento de conexión

                    // Consulta para insertar un nuevo usuario
                    string query = "INSERT INTO Usuarios (NombreUsuario, Contrasena) VALUES (@username, @password)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Parámetros para evitar inyecciones SQL
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);  // Aquí también es recomendable usar un hash

                        int result = cmd.ExecuteNonQuery();
                        return result > 0;  // Si el número de filas afectadas es mayor a 0, significa que el registro fue exitoso
                    }
                }
                catch (MySqlException ex)
                {
                    // Manejo de error (como si el usuario ya existe, por ejemplo)
                    Console.WriteLine("Error al registrar el usuario: " + ex.Message);
                }
            }
            return false;  // Si hubo algún error en el proceso de registro
        }
    }
}
