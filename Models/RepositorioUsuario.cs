using Inmobiliaria.Models;
using MySql.Data.MySqlClient;

public class RepositorioUsuario
{
	string connectionString = "Server=localhost;Database=inmobiliaria;User=root;Password=;";

	public RepositorioUsuario() {}

	public int Alta(Usuario usuario)
	{
		int res = 0;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			String query = @"INSERT INTO usuarios
			(nombre, apellido, email, avatar, rol)
			VALUES (@nombre, @apellido, @email, @avatar, @rol);
			SELECT LAST_INSERT_ID();";

			using (var command = new MySqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@nombre", usuario.Nombre);
				command.Parameters.AddWithValue("@apellido", usuario.Apellido);
				command.Parameters.AddWithValue("@email", usuario.Email);
				if (String.IsNullOrEmpty(usuario.Avatar))
					command.Parameters.AddWithValue("@avatar", DBNull.Value);
				else
					command.Parameters.AddWithValue("@avatar", usuario.Avatar);
				command.Parameters.AddWithValue("@rol", usuario.Rol);

				connection.Open();
				res = Convert.ToInt32(command.ExecuteScalar());
				usuario.IdUsuario = res;
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
			var query = @"DELETE FROM usuarios WHERE id_usuario = @id;";

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

	public int Modificar(Usuario usuario)
	{
		int res = 0;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			String query = @"UPDATE usuarios
			SET nombre = @nombre, apellido = @apellido, email = @email, rol = @rol
			WHERE id_usuario = @id;";

			using (var command = new MySqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@nombre", usuario.Nombre);
				command.Parameters.AddWithValue("@apellido", usuario.Apellido);
				command.Parameters.AddWithValue("@email", usuario.Email);
				command.Parameters.AddWithValue("@rol", usuario.Rol);
				command.Parameters.AddWithValue("@id", usuario.IdUsuario);

				connection.Open();
				res = command.ExecuteNonQuery();
				connection.Close();
			}
		}
		return res;
	}

	public int ModificarContraseña(Usuario usuario)
	{
		int res = 0;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			String query = @"UPDATE usuarios
			SET password = @password
			WHERE id_usuario = @id;";

			using (var command = new MySqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@password", usuario.Password);
				command.Parameters.AddWithValue("@id", usuario.IdUsuario);

				connection.Open();
				res = command.ExecuteNonQuery();
				connection.Close();
			}
		}
		return res;
	}

	public int ModificarAvatar(Usuario usuario)
	{
		int res = 0;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			String query = @"UPDATE usuarios
			SET avatar = @avatar
			WHERE id_usuario = @id;";

			using (var command = new MySqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@avatar", usuario.Avatar);
				command.Parameters.AddWithValue("@id", usuario.IdUsuario);

				connection.Open();
				res = command.ExecuteNonQuery();
				connection.Close();
			}
		}
		return res;
	}

	public IList<Usuario> GetUsuarios()
	{
		List<Usuario> usuarios = new List<Usuario>();
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"SELECT id_usuario, nombre, apellido, email, avatar, rol
			FROM usuarios;";

			using (var command = new MySqlCommand(query, connection))
			{
				connection.Open();
				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						var usuario = new Usuario
						{
							IdUsuario = reader.GetInt32("id_usuario"),
							Nombre = reader.GetString("nombre"),
							Apellido = reader.GetString("apellido"),
							Email = reader.GetString("email"),
							Avatar = !reader.IsDBNull(reader.GetOrdinal("avatar")) ? reader.GetString("avatar") : null,
							Rol = reader.GetInt32("rol")
						};
						usuarios.Add(usuario);
					}
				}
				connection.Close();
			}
		}
		return usuarios;
	}

	public Usuario GetUsuarioPorId(int id)
	{
		Usuario usuario = new Usuario();
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			var query = @"SELECT id_usuario, nombre, apellido, email, avatar, rol
			FROM usuarios
			WHERE id_usuario = @id;";

			using (var command = new MySqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@id", id);
				connection.Open();
				using (var reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
						usuario.IdUsuario = reader.GetInt32("id_usuario");
						usuario.Nombre = reader.GetString("nombre");
						usuario.Apellido = reader.GetString("apellido");
						usuario.Email = reader.GetString("email");
						usuario.Avatar = reader.IsDBNull(reader.GetOrdinal("avatar")) ? null : reader.GetString("avatar");
						usuario.Rol = reader.GetInt32("rol");
					}
				}
				connection.Close();
			}
		}
		return usuario;
	}
}