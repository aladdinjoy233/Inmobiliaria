namespace Inmobiliaria.Models;

public class Propietario
{
	public int IdPropietario { get; set; }
	public string ? Dni { get; set; }
	public string ? Apellido { get; set; }
	public string ? Nombre { get; set; }
	public string ? Telefono { get; set; }
	public string ? Correo { get; set; }

	public override string ToString()
	{
		return $"{Apellido}, {Nombre}";
	}
}