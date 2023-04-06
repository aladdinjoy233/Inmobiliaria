using Inmobiliaria.Models;
using MySql.Data.MySqlClient;

public class RepositorioPropietario
{
	string connectionString = "Server=localhost;Database=inmobiliaria;User=root;Password=;";

	public RepositorioPropietario() {}

	public int Alta(Propietario propietario)
	{
		int res = 0;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			string query = @"INSERT INTO propietarios (dni, apellido, nombre, telefono, correo)
			VALUES (@dni, @apellido, @nombre, @telefono, @correo);
			SELECT LAST_INSERT_ID();";

			using (var command = new MySqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@dni", propietario.Dni);
				command.Parameters.AddWithValue("@apellido", propietario.Apellido);
				command.Parameters.AddWithValue("@nombre", propietario.Nombre);
				command.Parameters.AddWithValue("@telefono", propietario.Telefono);
				command.Parameters.AddWithValue("@correo", propietario.Correo);
				connection.Open();
				res = Convert.ToInt32(command.ExecuteScalar());
				propietario.IdPropietario = res;
				connection.Close();
			}
		}

		return res;
	}

	public int Eliminar(int id)
	{
		int res = 0;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"DELETE FROM propietarios WHERE id_propietario = @id;";

			using (var command = new MySqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@id", id);
				connection.Open();
				res = command.ExecuteNonQuery();
				connection.Close();
			}
		}
		return res;
	}

	public int Modificar(Propietario propietario)
	{
		int res = 0;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"UPDATE propietarios SET dni = @dni, apellido = @apellido, nombre = @nombre, telefono = @telefono, correo = @correo
			WHERE id_propietario = @id;";

			using (var command = new MySqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@id", propietario.IdPropietario);
				command.Parameters.AddWithValue("@dni", propietario.Dni);
				command.Parameters.AddWithValue("@apellido", propietario.Apellido);
				command.Parameters.AddWithValue("@nombre", propietario.Nombre);
				command.Parameters.AddWithValue("@telefono", propietario.Telefono);
				command.Parameters.AddWithValue("@correo", propietario.Correo);
				connection.Open();
				res = command.ExecuteNonQuery();
				connection.Close();
			}
		}
		return res;
	}
	

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

	public Propietario? GetPropietarioPorId(int id)
	{
		Propietario? propietario = null;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"SELECT id_propietario, dni, apellido, nombre, telefono, correo
			FROM propietarios
			WHERE id_propietario = @id;";

			using (var command = new MySqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@id", id);
				connection.Open();
				using (var reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
						var correo = reader["correo"];
						propietario = new Propietario
						{
							IdPropietario = reader.GetInt32("id_propietario"),
							Dni           = reader.GetString("dni"),
							Apellido      = reader.GetString("apellido"),
							Nombre        = reader.GetString("nombre"),
							Telefono      = reader.GetString("telefono"),
							Correo        = correo == DBNull.Value ? null : correo.ToString()
						};
					}
				}
			}
			connection.Close();
		}
		return propietario;
	}

	public List<object> Buscar(string searchQuery)
	{
		var result = new List<object>();

		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"SELECT id_propietario, nombre, apellido, dni
			FROM propietarios
			WHERE CONCAT(nombre, ' ', apellido) LIKE @searchQuery
			OR dni LIKE @searchQuery;";

			using (var command = new MySqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@searchQuery", $"%{searchQuery}%");
				connection.Open();
				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						var nombre = reader.GetString("nombre");
						var apellido = reader.GetString("apellido");
						var dni = reader.GetString("dni");
						var outputString = $"{nombre} {apellido} ({dni})";
						result.Add(new { outputString, id = reader.GetInt32("id_propietario") });
					}
				}
			}
			connection.Close();
		}
		return result;
	}
}