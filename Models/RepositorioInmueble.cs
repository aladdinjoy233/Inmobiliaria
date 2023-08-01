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

	public List<Inmueble> GetInmueblesDesdeHasta(DateTime? desde, DateTime? hasta)
	{
		List<Inmueble> inmuebles = new List<Inmueble>();
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"SELECT i.id_inmueble, i.id_propietario, direccion, uso, tipo, ambientes, latitud, longitud, precio, activo, p.nombre, p.apellido
			FROM inmuebles i
			INNER JOIN propietarios p ON i.id_propietario = p.id_propietario
			WHERE NOT EXISTS (
				SELECT 1
				FROM contratos c
				WHERE c.id_inmueble = i.id_inmueble
				AND c.activo = 1 ";

			if (desde != DateTime.MinValue && hasta != DateTime.MinValue) // Si estan ambos valores
			{
				query += "AND fecha_inicio <= @hasta AND fecha_fin >= @desde";
			}
			else if (desde != DateTime.MinValue && hasta == DateTime.MinValue) // Si esta solo el "desde"
			{
				query += "AND fecha_fin >= @desde";
			}
			else if (hasta != DateTime.MinValue && desde == DateTime.MinValue) // Si esta solo el "hasta"
			{
				query += "AND fecha_inicio <= @hasta";
			}

			query += ");";

			using (var command = new MySqlCommand(query, connection))
			{
				if (desde.HasValue)
				{
					command.Parameters.AddWithValue("@desde", desde.Value);
				}

				if (hasta.HasValue)
				{
					command.Parameters.AddWithValue("@hasta", hasta.Value);
				}

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

	public List<Inmueble> GetDisponibles()
	{
		List<Inmueble> inmuebles = new List<Inmueble>();
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"SELECT id_inmueble, i.id_propietario, direccion, uso, tipo, ambientes, latitud, longitud, precio, activo, p.nombre, p.apellido
			FROM inmuebles i
			INNER JOIN propietarios p ON i.id_propietario = p.id_propietario
			WHERE activo = 1;";

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

	public Inmueble? GetDetalleInmueble(int id)
	{
		Inmueble? inmueble = null;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"SELECT i.id_inmueble, i.id_propietario, direccion, uso, tipo, ambientes, latitud, longitud, precio, i.activo, p.nombre, p.apellido, p.dni, c.id_contrato, c.fecha_inicio, c.fecha_fin, c.id_inquilino, c.monto_mensual, iq.nombre as iq_nombre, iq.apellido as iq_apellido
			FROM inmuebles i
			INNER JOIN propietarios p ON i.id_propietario = p.id_propietario
			LEFT JOIN contratos c ON i.id_inmueble = c.id_inmueble
			LEFT JOIN inquilinos iq ON c.id_inquilino = iq.id_inquilino
			WHERE i.id_inmueble = @id;";

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
							IdInmueble    = reader.GetInt32("id_inmueble"),
							PropietarioId = reader.GetInt32("id_propietario"),
							Propietario   = new Propietario
							{
								IdPropietario = reader.GetInt32("id_propietario"),
								Nombre        = reader.GetString("nombre"),
								Apellido      = reader.GetString("apellido"),
								Dni           = reader.GetString("dni")
							},
							Direccion = reader.GetString("direccion"),
							Uso       = reader.GetInt32("uso"),
							Tipo      = reader.GetInt32("tipo"),
							Ambientes = reader.GetInt32("ambientes"),
							Latitud   = reader.GetDecimal("latitud"),
							Longitud  = reader.GetDecimal("longitud"),
							Precio    = reader.GetDecimal("precio"),
							Activo    = reader.GetBoolean("activo"),
							Contratos = new List<Contrato>()
						};

						do
						{
							if (!reader.IsDBNull(reader.GetOrdinal("id_contrato")))
							{
								inmueble.Contratos.Add(new Contrato
								{
									IdContrato = reader.GetInt32("id_contrato"),
									FechaInicio = reader.GetDateTime("fecha_inicio"),
									FechaFin = reader.GetDateTime("fecha_fin"),
									MontoMensual = reader.GetDecimal("monto_mensual"),
									InquilinoId = reader.GetInt32("id_inquilino"),
									Inquilino = new Inquilino
									{
										Nombre = reader.GetString("iq_nombre"),
										Apellido = reader.GetString("iq_apellido")
									}
								});
							}
						} while (reader.Read());
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