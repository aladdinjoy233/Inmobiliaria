using Inmobiliaria.Models;
using MySql.Data.MySqlClient;

public class RepositorioContrato
{
	string connectionString = "Server=localhost;Database=inmobiliaria;User=root;Password=;";

	public RepositorioContrato() {}

	public int Alta(Contrato contrato)
	{
		int res = 0;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			String query = @"INSERT INTO contratos
			(id_inmueble, id_inquilino, fecha_inicio, fecha_fin, monto_mensual, activo)
			VALUES (@id_inmueble, @id_inquilino, @fecha_inicio, @fecha_fin, @monto_mensual, 1);
			SELECT LAST_INSERT_ID();";

			using (var command = new MySqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@id_inmueble", contrato.InmuebleId);
				command.Parameters.AddWithValue("@id_inquilino", contrato.InquilinoId);
				command.Parameters.AddWithValue("@fecha_inicio", contrato.FechaInicio);
				command.Parameters.AddWithValue("@fecha_fin", contrato.FechaFin);
				command.Parameters.AddWithValue("@monto_mensual", contrato.MontoMensual);

				connection.Open();
				res = Convert.ToInt32(command.ExecuteScalar());
				contrato.IdContrato = res;
				connection.Close();
			}
		}

		return res;
	}

	public int Renovar(Contrato contrato)
	{
		int res = 0;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			String query = @"INSERT INTO contratos
			(id_inmueble, id_inquilino, fecha_inicio, fecha_fin, monto_mensual, activo)
			VALUES (@id_inmueble, @id_inquilino, @fecha_inicio, @fecha_fin, @monto_mensual, 1);
			SELECT LAST_INSERT_ID();";

			using (var command = new MySqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@id_inmueble", contrato.InmuebleId);
				command.Parameters.AddWithValue("@id_inquilino", contrato.InquilinoId);
				command.Parameters.AddWithValue("@fecha_inicio", contrato.FechaInicio);
				command.Parameters.AddWithValue("@fecha_fin", contrato.FechaFin);
				command.Parameters.AddWithValue("@monto_mensual", contrato.MontoMensual);

				connection.Open();
				res = Convert.ToInt32(command.ExecuteScalar());
				contrato.IdContrato = res;
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
			var query = @"DELETE FROM contratos WHERE id_contrato = @id;";

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

	public decimal Terminar(int id)
	{
		decimal multa = 0;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"SELECT fecha_inicio, fecha_fin, monto_mensual
			FROM contratos
			WHERE id_contrato = @id;

			UPDATE contratos
			SET activo = 0, fecha_fin = @fecha_fin
			WHERE id_contrato = @id;";

			using (var command = new MySqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@id", id);
				command.Parameters.AddWithValue("@fecha_fin", DateTime.Now);
				connection.Open();

				using (var reader = command.ExecuteReader())
				{

					if (reader.Read())
					{
						var fechaInicio = Convert.ToDateTime(reader["fecha_inicio"]);
						var fechaFin = Convert.ToDateTime(reader["fecha_fin"]);
						var montoMensual = Convert.ToDecimal(reader["monto_mensual"]);

						int totalDays = (fechaFin - fechaInicio).Days;
						int daysInContract = totalDays < 0 ? 0 : totalDays;
						int daysElapsed = (DateTime.Now - fechaInicio).Days;

						if (daysElapsed < daysInContract / 2)
						{
							multa = montoMensual * 2;
						}
						else
						{
							multa = montoMensual;
						}
					}
				}
				connection.Close();
			}
		}
		return multa;
	}

	public int Modificar(Contrato contrato)
	{
		int res = 0;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			String query = @"UPDATE contratos
			SET id_inmueble = @id_inmueble, id_inquilino = @id_inquilino, fecha_inicio = @fecha_inicio, fecha_fin = @fecha_fin, monto_mensual = @monto_mensual
			WHERE id_contrato = @id;";

			using (var command = new MySqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@id", contrato.IdContrato);
				command.Parameters.AddWithValue("@id_inmueble", contrato.InmuebleId);
				command.Parameters.AddWithValue("@id_inquilino", contrato.InquilinoId);
				command.Parameters.AddWithValue("@fecha_inicio", contrato.FechaInicio);
				command.Parameters.AddWithValue("@fecha_fin", contrato.FechaFin);
				command.Parameters.AddWithValue("@monto_mensual", contrato.MontoMensual);

				connection.Open();
				res = command.ExecuteNonQuery();
				connection.Close();
			}
		}
		return res;
	}

	public List<Contrato> GetContratosValidos()
	{
		List<Contrato> contratos = new List<Contrato>();
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"SELECT id_contrato, c.id_inmueble, c.id_inquilino, fecha_inicio, fecha_fin, monto_mensual, i.direccion, iq.nombre, iq.apellido
			FROM contratos c
			INNER JOIN inmuebles i ON c.id_inmueble = i.id_inmueble
 			INNER JOIN inquilinos iq ON c.id_inquilino = iq.id_inquilino
			WHERE fecha_fin >= CURDATE() AND c.activo = 1;";

			using (var command = new MySqlCommand(query, connection))
			{
				connection.Open();
				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						Contrato contrato = new Contrato
						{
							IdContrato = reader.GetInt32("id_contrato"),
							InmuebleId = reader.GetInt32("id_inmueble"),
							Inmueble = new Inmueble
							{
								Direccion = reader.GetString("direccion"),
							},
							InquilinoId = reader.GetInt32("id_inquilino"),
							Inquilino = new Inquilino
							{
								Nombre = reader.GetString("nombre"),
								Apellido = reader.GetString("apellido"),
							},
							FechaInicio = reader.GetDateTime("fecha_inicio"),
							FechaFin = reader.GetDateTime("fecha_fin"),
							MontoMensual = reader.GetDecimal("monto_mensual")
						};
						contratos.Add(contrato);
					}
				}
			}
			connection.Close();
		}
		return contratos;
	}

	public List<Contrato> GetContratosValidosDesdeHasta(DateTime? desde, DateTime? hasta)
	{
		List<Contrato> contratos = new List<Contrato>();
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			string query = @"SELECT id_contrato, c.id_inmueble, c.id_inquilino, fecha_inicio, fecha_fin, monto_mensual, i.direccion, iq.nombre, iq.apellido
				FROM contratos c
				INNER JOIN inmuebles i ON c.id_inmueble = i.id_inmueble
				INNER JOIN inquilinos iq ON c.id_inquilino = iq.id_inquilino
				WHERE c.activo = 1 ";

			if (desde != DateTime.MinValue && hasta != DateTime.MinValue) // Si estan ambos valores
			{
				query += "AND fecha_inicio >= @desde AND fecha_fin <= @hasta;";
			}
			else if (desde != DateTime.MinValue && hasta == DateTime.MinValue) // Si esta solo el "desde"
			{
				query += "AND fecha_inicio >= @desde;";
			}
			else if (hasta != DateTime.MinValue && desde == DateTime.MinValue) // Si esta solo el "hasta"
			{
				query += "AND fecha_fin <= @hasta;";
			}

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
						Contrato contrato = new Contrato
						{
							IdContrato = reader.GetInt32("id_contrato"),
							InmuebleId = reader.GetInt32("id_inmueble"),
							Inmueble = new Inmueble
							{
								Direccion = reader.GetString("direccion"),
							},
							InquilinoId = reader.GetInt32("id_inquilino"),
							Inquilino = new Inquilino
							{
								Nombre = reader.GetString("nombre"),
								Apellido = reader.GetString("apellido"),
							},
							FechaInicio = reader.GetDateTime("fecha_inicio"),
							FechaFin = reader.GetDateTime("fecha_fin"),
							MontoMensual = reader.GetDecimal("monto_mensual")
						};
						contratos.Add(contrato);
					}
				}
			}
			connection.Close();
		}
		return contratos;
	}

	public List<Contrato> GetContratosExpirados()
	{
		List<Contrato> contratos = new List<Contrato>();
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"SELECT id_contrato, c.id_inmueble, c.id_inquilino, fecha_inicio, fecha_fin, monto_mensual, i.direccion, iq.nombre, iq.apellido
			FROM contratos c
			INNER JOIN inmuebles i ON c.id_inmueble = i.id_inmueble
 			INNER JOIN inquilinos iq ON c.id_inquilino = iq.id_inquilino
			WHERE fecha_fin < CURDATE() OR c.activo = 0;";

			using (var command = new MySqlCommand(query, connection))
			{
				connection.Open();
				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						Contrato contrato = new Contrato
						{
							IdContrato = reader.GetInt32("id_contrato"),
							InmuebleId = reader.GetInt32("id_inmueble"),
							Inmueble = new Inmueble
							{
								Direccion = reader.GetString("direccion"),
							},
							InquilinoId = reader.GetInt32("id_inquilino"),
							Inquilino = new Inquilino
							{
								Nombre = reader.GetString("nombre"),
								Apellido = reader.GetString("apellido"),
							},
							FechaInicio = reader.GetDateTime("fecha_inicio"),
							FechaFin = reader.GetDateTime("fecha_fin"),
							MontoMensual = reader.GetDecimal("monto_mensual")
						};
						contratos.Add(contrato);
					}
				}
			}
			connection.Close();
		}
		return contratos;
	}

	public Contrato GetContratoPorId(int id)
	{
		Contrato contrato = new Contrato();
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"SELECT id_contrato, c.id_inmueble, c.id_inquilino, fecha_inicio, fecha_fin, monto_mensual, i.direccion, i.tipo, i.precio, iq.nombre, iq.apellido, iq.dni, c.activo
			FROM contratos c
			INNER JOIN inmuebles i ON c.id_inmueble = i.id_inmueble
 			INNER JOIN inquilinos iq ON c.id_inquilino = iq.id_inquilino
			WHERE id_contrato = @id;";

			using (var command = new MySqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@id", id);
				connection.Open();
				using (var reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
						contrato = new Contrato
						{
							IdContrato = reader.GetInt32("id_contrato"),
							InmuebleId = reader.GetInt32("id_inmueble"),
							Inmueble = new Inmueble
							{
								IdInmueble = reader.GetInt32("id_inmueble"),
								Direccion = reader.GetString("direccion"),
								Tipo = reader.GetInt32("tipo"),
								Precio = reader.GetDecimal("precio")
							},
							InquilinoId = reader.GetInt32("id_inquilino"),
							Inquilino = new Inquilino
							{
								IdInquilino = reader.GetInt32("id_inquilino"),
								Nombre = reader.GetString("nombre"),
								Apellido = reader.GetString("apellido"),
								Dni = reader.GetString("dni")
							},
							FechaInicio = reader.GetDateTime("fecha_inicio"),
							FechaFin = reader.GetDateTime("fecha_fin"),
							MontoMensual = reader.GetDecimal("monto_mensual"),
							Activo = reader.GetBoolean("activo")
						};
					}
				}
			}
			connection.Close();
		}
		return contrato;
	}

	public List<int> GetContratosIdsPorInmueble(int idInmueble)
	{
		var result = new List<int>();
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"SELECT id_contrato
			FROM contratos c
			INNER JOIN inmuebles i ON c.id_inmueble = i.id_inmueble
			WHERE c.id_inmueble = @idInmueble
			AND c.activo = 1;";

			using (var command = new MySqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@idInmueble", idInmueble);
				connection.Open();

				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						result.Add(reader.GetInt32("id_contrato"));
					}
				}
				connection.Close();
			}
			return result;
		}
	}

	public List<object> Buscar(string searchQuery)
	{
		var result = new List<object>();

		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"SELECT id_contrato, c.id_inmueble, c.id_inquilino, fecha_inicio, fecha_fin, monto_mensual, i.direccion, i.tipo, i.precio, iq.nombre, iq.apellido, iq.dni
			FROM contratos c
			INNER JOIN inmuebles i ON c.id_inmueble = i.id_inmueble
 			INNER JOIN inquilinos iq ON c.id_inquilino = iq.id_inquilino
			WHERE c.activo = 1
			AND (i.direccion LIKE @searchQuery
			OR iq.nombre LIKE @searchQuery
			OR iq.apellido LIKE @searchQuery
			OR iq.dni LIKE @searchQuery);";

			using (var command = new MySqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@searchQuery", $"%{searchQuery}%");
				connection.Open();
				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						var direccion = reader.GetString("direccion");
						var inquilinoNombre = $"{reader.GetString("nombre")} {reader.GetString("apellido")}";
						var montoMensual = reader.GetDecimal("monto_mensual");
						var outputString = $"{direccion} ({inquilinoNombre}) - ${montoMensual}";
						result.Add(new { outputString, id = reader.GetInt32("id_contrato") });
					}
				}
			}
			connection.Close();
		}
		return result;
	}
}