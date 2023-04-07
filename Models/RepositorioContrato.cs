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
			(id_inmueble, id_inquilino, fecha_inicio, fecha_fin, monto_mensual)
			VALUES (@id_inmueble, @id_inquilino, @fecha_inicio, @fecha_fin, @monto_mensual);
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

	public List<Contrato> GetContratos()
	{
		List<Contrato> contratos = new List<Contrato>();
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"SELECT id_contrato, c.id_inmueble, c.id_inquilino, fecha_inicio, fecha_fin, monto_mensual, i.direccion, iq.nombre, iq.apellido
			FROM contratos c
			INNER JOIN inmuebles i ON c.id_inmueble = i.id_inmueble
 			INNER JOIN inquilinos iq ON c.id_inquilino = iq.id_inquilino;";

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
			var query = @"SELECT id_contrato, c.id_inmueble, c.id_inquilino, fecha_inicio, fecha_fin, monto_mensual, i.direccion, i.tipo, i.precio, iq.nombre, iq.apellido, iq.dni
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
								Tipo = reader.GetString("tipo"),
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
							MontoMensual = reader.GetDecimal("monto_mensual")
						};
					}
				}
			}
			connection.Close();
		}
		return contrato;
	}
}