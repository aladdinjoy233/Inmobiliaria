using Inmobiliaria.Models;
using MySql.Data.MySqlClient;

public class RepositorioInmueble
{
	string connectionString = "Server=localhost;Database=inmobiliaria;User=root;Password=;";

	public RepositorioInmueble() {}

	public int Alta(Inmueble inmueble)
	{
		int res = 0;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			string query = @"INSERT INTO inmuebles
			(id_propietario, direccion, uso, tipo, ambientes, latitud, longitud, precio, activo)
			VALUES (@id_propietario, @direccion, @uso, @tipo, @ambientes, @latitud, @longitud, @precio, @activo);
			SELECT LAST_INSERT_ID();";

			using (var command = new MySqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@id_propietario", inmueble.PropietarioId);
				command.Parameters.AddWithValue("@direccion", inmueble.Direccion);
				command.Parameters.AddWithValue("@uso", inmueble.Uso);
				command.Parameters.AddWithValue("@tipo", inmueble.Tipo);
				command.Parameters.AddWithValue("@ambientes", inmueble.Ambientes);
				command.Parameters.AddWithValue("@latitud", inmueble.Latitud);
				command.Parameters.AddWithValue("@longitud", inmueble.Longitud);
				command.Parameters.AddWithValue("@precio", inmueble.Precio);
				command.Parameters.AddWithValue("@activo", inmueble.Activo);

				connection.Open();
				res = Convert.ToInt32(command.ExecuteScalar());
				inmueble.IdInmueble = res;
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
			var query = @"DELETE FROM inmuebles WHERE id_inmueble = @id;";

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

	public int Modificar(Inmueble inmueble)
	{
		int res = 0;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"UPDATE inmuebles
			SET id_propietario = @id_propietario, direccion = @direccion, uso = @uso, tipo = @tipo, ambientes = @ambientes, latitud = @latitud, longitud = @longitud, precio = @precio, activo = @activo
			WHERE id_inmueble = @id;";

			using (var command = new MySqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@id_propietario", inmueble.PropietarioId);
				command.Parameters.AddWithValue("@direccion", inmueble.Direccion);
				command.Parameters.AddWithValue("@uso", inmueble.Uso);
				command.Parameters.AddWithValue("@tipo", inmueble.Tipo);
				command.Parameters.AddWithValue("@ambientes", inmueble.Ambientes);
				command.Parameters.AddWithValue("@latitud", inmueble.Latitud);
				command.Parameters.AddWithValue("@longitud", inmueble.Longitud);
				command.Parameters.AddWithValue("@precio", inmueble.Precio);
				command.Parameters.AddWithValue("@activo", inmueble.Activo);
				command.Parameters.AddWithValue("@id", inmueble.IdInmueble);

				connection.Open();
				res = command.ExecuteNonQuery();
				connection.Close();
			}
		}
		return res;
	}

	public List<Inmueble> GetInmuebles()
	{
		List<Inmueble> inmuebles = new List<Inmueble>();
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"SELECT id_inmueble, i.id_propietario, direccion, uso, tipo, ambientes, latitud, longitud, precio, activo, p.nombre, p.apellido
			FROM inmuebles i
			INNER JOIN propietarios p ON i.id_propietario = p.id_propietario;";

			using (var command = new MySqlCommand(query, connection))
			{
				connection.Open();
				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						Inmueble inmueble = new Inmueble
						{
							IdInmueble = reader.GetInt32("id_inmueble"),
							PropietarioId = reader.GetInt32("id_propietario"),
							Propietario = new Propietario
							{
								IdPropietario = reader.GetInt32("id_propietario"),
								Nombre = reader.GetString("nombre"),
								Apellido = reader.GetString("apellido")
							},
							Direccion = reader.GetString("direccion"),
							Uso = reader.GetInt32("uso"),
							Tipo = reader.GetInt32("tipo"),
							Ambientes = reader.GetInt32("ambientes"),
							Latitud = reader.GetDecimal("latitud"),
							Longitud = reader.GetDecimal("longitud"),
							Precio = reader.GetDecimal("precio"),
							Activo = reader.GetBoolean("activo")
						};
						inmuebles.Add(inmueble);
					}
				}
			}
			connection.Close();
		}
		return inmuebles;
	}

	public List<Inmueble> GetInmueblesParaAlquilar()
	{
		List<Inmueble> inmuebles = new List<Inmueble>();
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"SELECT i.id_inmueble, i.id_propietario, i.direccion, i.uso, i.tipo, i.ambientes, i.latitud, i.longitud, i.precio, i.activo, p.nombre, p.apellido
			FROM inmuebles i
			INNER JOIN propietarios p ON i.id_propietario = p.id_propietario
			LEFT JOIN contratos c ON i.id_inmueble = c.id_inmueble
			WHERE i.activo = TRUE
			AND c.id_contrato IS NULL;";

			using (var command = new MySqlCommand(query, connection))
			{
				connection.Open();
				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						Inmueble inmueble = new Inmueble
						{
							IdInmueble = reader.GetInt32("id_inmueble"),
							PropietarioId = reader.GetInt32("id_propietario"),
							Propietario = new Propietario
							{
								IdPropietario = reader.GetInt32("id_propietario"),
								Nombre = reader.GetString("nombre"),
								Apellido = reader.GetString("apellido")
							},
							Direccion = reader.GetString("direccion"),
							Uso = reader.GetInt32("uso"),
							Tipo = reader.GetInt32("tipo"),
							Ambientes = reader.GetInt32("ambientes"),
							Latitud = reader.GetDecimal("latitud"),
							Longitud = reader.GetDecimal("longitud"),
							Precio = reader.GetDecimal("precio"),
							Activo = reader.GetBoolean("activo")
						};
						inmuebles.Add(inmueble);
					}
				}
			}
			connection.Close();
		}
		return inmuebles;
	}

	public Inmueble? GetInmueblePorId(int id)
	{
		Inmueble? inmueble = null;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"SELECT id_inmueble, i.id_propietario, direccion, uso, tipo, ambientes, latitud, longitud, precio, activo, p.nombre, p.apellido, p.dni
			FROM inmuebles i
			INNER JOIN propietarios p ON i.id_propietario = p.id_propietario
			WHERE id_inmueble = @id;";

			using (var command = new MySqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@id", id);
				connection.Open();
				using (var reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
						inmueble = new Inmueble
						{
							IdInmueble = reader.GetInt32("id_inmueble"),
							PropietarioId = reader.GetInt32("id_propietario"),
							Propietario = new Propietario
							{
								IdPropietario = reader.GetInt32("id_propietario"),
								Nombre = reader.GetString("nombre"),
								Apellido = reader.GetString("apellido"),
								Dni = reader.GetString("dni")
							},
							Direccion = reader.GetString("direccion"),
							Uso = reader.GetInt32("uso"),
							Tipo = reader.GetInt32("tipo"),
							Ambientes = reader.GetInt32("ambientes"),
							Latitud = reader.GetDecimal("latitud"),
							Longitud = reader.GetDecimal("longitud"),
							Precio = reader.GetDecimal("precio"),
							Activo = reader.GetBoolean("activo")
						};
					}
				}
			}
			connection.Close();
		}
		return inmueble;
	}

	public List<object> Buscar(string searchQuery)
	{
		var result = new List<object>();

		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"SELECT i.id_inmueble, i.direccion, i.tipo, i.precio, i.activo
			FROM inmuebles i
			LEFT JOIN contratos c ON i.id_inmueble = c.id_inmueble
			LEFT JOIN enum_tipos t ON i.tipo = t.id_tipo
			WHERE i.activo = TRUE
			AND (c.id_contrato IS NULL OR !c.activo)
			AND (i.direccion LIKE @searchQuery
			OR t.nombre_tipo LIKE @searchQuery
			OR i.precio LIKE @searchQuery);";

			using (var command = new MySqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@searchQuery", $"%{searchQuery}%");
				connection.Open();
				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						var direccion = reader.GetString("direccion");
						var tipoInmueble = new Inmueble
						{
							Tipo = reader.GetInt32("tipo"),
						};
						var precio = reader.GetString("precio");
						var outputString = $"{direccion} ({tipoInmueble.TipoNombre}) - ${precio}";
						result.Add(new { outputString, id = reader.GetInt32("id_inmueble") });
					}
				}
			}
			connection.Close();
		}
		return result;
	}

	public List<object> BuscarConId(int id, string searchQuery)
	{
		var result = new List<object>();

		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"SELECT i.id_inmueble, i.direccion, i.tipo, i.precio, i.activo
			FROM inmuebles i
			LEFT JOIN contratos c ON i.id_inmueble = c.id_inmueble
			LEFT JOIN enum_tipos t ON i.tipo = t.id_tipo
			WHERE i.activo = TRUE
			AND (c.id_contrato IS NULL OR c.id_contrato = @id OR !c.activo)
			AND (i.direccion LIKE @searchQuery
			OR t.nombre_tipo LIKE @searchQuery
			OR i.precio LIKE @searchQuery);";

			using (var command = new MySqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@id", id);
				command.Parameters.AddWithValue("@searchQuery", $"%{searchQuery}%");
				connection.Open();
				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						var direccion = reader.GetString("direccion");
						var tipoInmueble = new Inmueble
						{
							Tipo = reader.GetInt32("tipo"),
						};
						var precio = reader.GetString("precio");
						var outputString = $"{direccion} ({tipoInmueble.TipoNombre}) - ${precio}";
						result.Add(new { outputString, id = reader.GetInt32("id_inmueble") });
					}
				}
			}
			connection.Close();
		}
		return result;
	}
}