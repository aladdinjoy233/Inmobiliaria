using Inmobiliaria.Models;
using MySql.Data.MySqlClient;

public class RepositorioInquilino
{
	string connectionString = "Server=localhost;Database=inmobiliaria;User=root;Password=;";

	public RepositorioInquilino() {}

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
}