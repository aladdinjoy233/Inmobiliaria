using Inmobiliaria.Models;
using MySql.Data.MySqlClient;

public class RepositorioPropietario
{
	string connectionString = "Server=localhost;Database=inmobiliaria;User=root;Password=;";

	public RepositorioPropietario() {}

	// public int Alta(Propietario propietario)
	// {
	// 	int red = 0;
	// 	using (MySqlConnection connection = new MySqlConnection(connectionString))
	// 	{
	// 		string query = @"INSERT INTO propietarios (dni, apellido, nombre, telefono, correo)
	// 		VALUES (@dni, @apellido, @nombre, @telefono, @correo);
	// 		SELECT LAST_INSERT_ID();";
	// 	}
	// }

	public List<Propietario> GetPropietarios()
	{
		List<Propietario> propietarios = new List<Propietario>();
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"SELECT id_propietario, dni, apellido, nombre, telefono, correo
			FROM propietarios;";

			using (var command = new MySqlCommand(query, connection))
			{
				connection.Open();
				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						var correo = reader["correo"];
						Propietario propietario = new Propietario
						{
							IdPropietario = reader.GetInt32("id_propietario"),
							Dni           = reader.GetString("dni"),
							Apellido      = reader.GetString("apellido"),
							Nombre        = reader.GetString("nombre"),
							Telefono      = reader.GetString("telefono"),
							Correo        = correo == DBNull.Value ? null : correo.ToString()
						};
						propietarios.Add(propietario);
					}
				}
			}
			connection.Close();
		}
		return propietarios;
	}
}