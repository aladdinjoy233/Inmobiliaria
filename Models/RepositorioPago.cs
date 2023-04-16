using Inmobiliaria.Models;
using MySql.Data.MySqlClient;

public class RepositorioPago
{
	string connectionString = "Server=localhost;Database=inmobiliaria;User=root;Password=;";

	public RepositorioPago() {}

	public int Alta(Pago pago)
	{
		int res = 0;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			String query = @"INSERT INTO pagos
			(id_contrato, numero, fecha, importe)
			VALUES (@id_contrato, @numero, @fecha, @importe);
			SELECT LAST_INSERT_ID();";

			using (var command = new MySqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@id_contrato", pago.ContratoId);
				command.Parameters.AddWithValue("@numero", pago.Numero);
				command.Parameters.AddWithValue("@fecha", pago.Fecha);
				command.Parameters.AddWithValue("@importe", pago.Importe);

				connection.Open();
				res = Convert.ToInt32(command.ExecuteScalar());
				pago.IdPago = res;
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
			var query = @"DELETE FROM pagos WHERE id_pago = @id;";

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

	public int Modificar(Pago pago)
	{
		int res = 0;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"UPDATE pagos
			SET id_contrato = @id_contrato, numero = @numero, fecha = @fecha, importe = @importe
			WHERE id_pago = @id;";

			using (var command = new MySqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@id", pago.IdPago);
				command.Parameters.AddWithValue("@id_contrato", pago.ContratoId);
				command.Parameters.AddWithValue("@numero", pago.Numero);
				command.Parameters.AddWithValue("@fecha", pago.Fecha);
				command.Parameters.AddWithValue("@importe", pago.Importe);

				connection.Open();
				res = command.ExecuteNonQuery();
				connection.Close();
			}
		}
		return res;
	}

	public List<Pago> GetPagosValidos()
	{
		List<Pago> pagos = new List<Pago>();
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"SELECT id_pago, p.id_contrato, numero, fecha, importe, i.direccion, iq.nombre, iq.apellido
			FROM pagos p
			INNER JOIN contratos c ON p.id_contrato = c.id_contrato
			INNER JOIN inmuebles i ON c.id_inmueble = i.id_inmueble
			INNER JOIN inquilinos iq ON c.id_inquilino = iq.id_inquilino
			WHERE c.activo = 1;";

			using (var command = new MySqlCommand(query, connection))
			{
				connection.Open();
				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					pagos.Add(new Pago
					{
						IdPago = Convert.ToInt32(reader["id_pago"]),
						ContratoId = Convert.ToInt32(reader["id_contrato"]),
						Contrato = new Contrato
						{
							Inmueble = new Inmueble
							{
								Direccion = reader.GetString("direccion"),
							},
							Inquilino = new Inquilino
							{
								Nombre = reader.GetString("nombre"),
								Apellido = reader.GetString("apellido"),
							}
						},
						Numero = Convert.ToInt32(reader["numero"]),
						Fecha = Convert.ToDateTime(reader["fecha"]),
						Importe = Convert.ToDecimal(reader["importe"])
					});
				}
			}
			connection.Close();
		}
		return pagos;
	}

	public List<Pago> GetPagosExpirados()
	{
		List<Pago> pagos = new List<Pago>();
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"SELECT id_pago, p.id_contrato, numero, fecha, importe, i.direccion, iq.nombre, iq.apellido
			FROM pagos p
			INNER JOIN contratos c ON p.id_contrato = c.id_contrato
			INNER JOIN inmuebles i ON c.id_inmueble = i.id_inmueble
			INNER JOIN inquilinos iq ON c.id_inquilino = iq.id_inquilino
			WHERE c.activo = 0;";

			using (var command = new MySqlCommand(query, connection))
			{
				connection.Open();
				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					pagos.Add(new Pago
					{
						IdPago = Convert.ToInt32(reader["id_pago"]),
						ContratoId = Convert.ToInt32(reader["id_contrato"]),
						Contrato = new Contrato
						{
							Inmueble = new Inmueble
							{
								Direccion = reader.GetString("direccion"),
							},
							Inquilino = new Inquilino
							{
								Nombre = reader.GetString("nombre"),
								Apellido = reader.GetString("apellido"),
							}
						},
						Numero = Convert.ToInt32(reader["numero"]),
						Fecha = Convert.ToDateTime(reader["fecha"]),
						Importe = Convert.ToDecimal(reader["importe"])
					});
				}
			}
			connection.Close();
		}
		return pagos;
	}

	public Pago GetPagoPorId(int id)
	{
		Pago pago = new Pago();
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"SELECT id_pago, p.id_contrato, numero, fecha, importe, i.direccion, iq.nombre, iq.apellido, c.monto_mensual
			FROM pagos p
			INNER JOIN contratos c ON p.id_contrato    = c.id_contrato
			INNER JOIN inmuebles i ON c.id_inmueble    = i.id_inmueble
			INNER JOIN inquilinos iq ON c.id_inquilino = iq.id_inquilino
			WHERE id_pago = @id;";

			using (var command = new MySqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@id", id);
				connection.Open();
				using (var reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
						pago = new Pago
						{
							IdPago = Convert.ToInt32(reader["id_pago"]),
							ContratoId = Convert.ToInt32(reader["id_contrato"]),
							Contrato = new Contrato
							{
								IdContrato = Convert.ToInt32(reader["id_contrato"]),
								MontoMensual = Convert.ToDecimal(reader["monto_mensual"]),
								Inmueble = new Inmueble
								{
									Direccion = reader.GetString("direccion"),
								},
								Inquilino = new Inquilino
								{
									Nombre = reader.GetString("nombre"),
									Apellido = reader.GetString("apellido"),
								}
							},
							Numero = Convert.ToInt32(reader["numero"]),
							Fecha = Convert.ToDateTime(reader["fecha"]),
							Importe = Convert.ToDecimal(reader["importe"])
						};
					}
				}
			}
			connection.Close();
		}
		return pago;
	}

	public int ObtenerUltimoPago(int contratoId)
	{
		int res = 0;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"SELECT COALESCE(MAX(numero), 0) AS last_pago_number
			FROM pagos
			WHERE id_contrato = @id_contrato;";

			using (var command = new MySqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@id_contrato", contratoId);
				connection.Open();
				using (var reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
						res = reader.GetInt32("last_pago_number");
					}
				}
				connection.Close();
			}
		}
		return res;
	}
}