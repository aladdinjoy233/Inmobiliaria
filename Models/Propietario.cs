using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Models;

public class Propietario
{
	public int IdPropietario { get; set; }

	[Required (ErrorMessage = "El DNI es requerido")]
	public string ? Dni { get; set; }

	[Required (ErrorMessage = "El apellido es requerido")]
	public string ? Apellido { get; set; }

	[Required (ErrorMessage = "El nombre es requerido")]
	public string ? Nombre { get; set; }

	[Required (ErrorMessage = "El telefono es requerido")]
	public string ? Telefono { get; set; }

	public string ? Correo { get; set; }


	public override string ToString()
	{
		return $"{Apellido}, {Nombre}";
	}
}