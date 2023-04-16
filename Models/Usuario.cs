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

	[Required(ErrorMessage = "El nombre es requerido")]
	public string? Nombre { get; set; }

	[Required(ErrorMessage = "El apellido es requerido")]
	public string? Apellido { get; set; }

	[Required(ErrorMessage = "El correo es requerido")]
	[EmailAddress]
	public string? Email { get; set; }

	[Required(ErrorMessage = "La contraseña es requerida")]
	public string Password { get; set; } = "";

	[Display(Name = "Confirmar password")]
	[Required(ErrorMessage = "La confirmacion es requerida")]
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