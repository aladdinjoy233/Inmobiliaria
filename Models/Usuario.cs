using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Models;

public enum enRoles
{
	Empleado = 1,
	Administrador = 2,
}

public class Usuario
{
	public int IdUsuario { get; set; }

	[Required]
	public string? Nombre { get; set; }

	[Required]
	public string? Apellido { get; set; }

	[Required]
	[EmailAddress]
	public string? Email { get; set; }

	[Required(ErrorMessage = "La contraseña es opcional en edición.")]
	public string Password { get; set; } = "";

	[Display(Name = "Confirmar contraseña")]
	public string ConfirmPassword { get; set; } = "";

	public string? Avatar { get; set; }

	public IFormFile? AvatarFile { get; set; }

	public int Rol { get; set; }

	public string RolNombre => Rol > 0 ? ((enRoles)Rol).ToString() : "";

	public static IDictionary<int, string> ObtenerRoles()
	{
		SortedDictionary<int, string> roles = new SortedDictionary<int, string>();
		Type tipoEnumRol = typeof(enRoles);
		foreach (var valor in Enum.GetValues(tipoEnumRol))
		{
			roles.Add((int)valor, Enum.GetName(tipoEnumRol, valor) ?? "");
		}
		return roles;
	}
}