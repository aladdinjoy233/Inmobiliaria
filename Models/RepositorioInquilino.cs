using Inmobiliaria.Models;
using MySql.Data.MySqlClient;

public class RepositorioInquilino
{
	string connectionString = "Server=localhost;Database=inmobiliaria;User=root;Password=;";

	public RepositorioInquilino() {}

	public int Alta(Inquilino inquilino)
	{
		int res = 0;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			string query = @"INSERT INTO inquilinos (dni, apellido, nombre, telefono, correo)
			VALUES (@dni, @apellido, @nombre, @telefono, @correo);
			SELECT LAST_INSERT_ID();";

			using (var command = new MySqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@dni", inquilino.Dni);
				command.Parameters.AddWithValue("@apellido", inquilino.Apellido);
				command.Parameters.AddWithValue("@nombre", inquilino.Nombre);
				command.Parameters.AddWithValue("@telefono", inquilino.Telefono);
				command.Parameters.AddWithValue("@correo", inquilino.Correo);
				connection.Open();
				res = Convert.ToInt32(command.ExecuteScalar());
				inquilino.IdInquilino = res;
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
			var query = @"DELETE FROM inquilinos WHERE id_inquilino = @id;";

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

	public int Modificar(Inquilino inquilino)
	{
		int res = 0;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"UPDATE inquilinos SET dni = @dni, apellido = @apellido, nombre = @nombre, telefono = @telefono, correo = @correo
			WHERE id_inquilino = @id;";

			using (var command = new MySqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@id", inquilino.IdInquilino);
				command.Parameters.AddWithValue("@dni", inquilino.Dni);
				command.Parameters.AddWithValue("@apellido", inquilino.Apellido);
				command.Parameters.AddWithValue("@nombre", inquilino.Nombre);
				command.Parameters.AddWithValue("@telefono", inquilino.Telefono);
				command.Parameters.AddWithValue("@correo", inquilino.Correo);
				connection.Open();
				res = command.ExecuteNonQuery();
				connection.Close();
			}
		}
		return res;
	}

	public List<Inquilino> GetInquilinos()
	{
		List<Inquilino> inquilinos = new List<Inquilino>();
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"SELECT id_inquilino, dni, apellido, nombre, telefono, correo
			FROM inquilinos;";

			using (var command = new MySqlCommand(query, connection))
			{
				connection.Open();
				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						var correo = reader["correo"];
						Inquilino inquilino = new Inquilino
						{
							IdInquilino = reader.GetInt32("id_inquilino"),
							Dni         = reader.GetString("dni"),
							Apellido    = reader.GetString("apellido"),
							Nombre      = reader.GetString("nombre"),
							Telefono    = reader.GetString("telefono"),
							Correo      = correo == DBNull.Value ? null : correo.ToString()
						};
						inquilinos.Add(inquilino);
					}
				}
			}
			connection.Close();
		}
		return inquilinos;
	}

	public Inquilino? GetInquilinoPorId(int id)
	{
		Inquilino? inquilino = null;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"SELECT id_inquilino, dni, apellido, nombre, telefono, correo
			FROM inquilinos
			WHERE id_inquilino = @id;";

			using (var command = new MySqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@id", id);
				connection.Open();
				using (var reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
						var correo = reader["correo"];
						inquilino = new Inquilino
						{
							IdInquilino = reader.GetInt32("id_inquilino"),
							Dni         = reader.GetString("dni"),
							Apellido    = reader.GetString("apellido"),
							Nombre      = reader.GetString("nombre"),
							Telefono    = reader.GetString("telefono"),
							Correo      = correo == DBNull.Value ? null : correo.ToString()
						};
					}
				}
			}
			connection.Close();
		}
		return inquilino;
	}

	public List<object> Buscar(string searchQuery)
	{
		var result = new List<object>();

		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"SELECT id_inquilino, nombre, apellido, dni
			FROM inquilinos
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
						result.Add(new { outputString, id = reader.GetInt32("id_inquilino") });
					}
				}
			}
			connection.Close();
		}
		return result;
	}
}