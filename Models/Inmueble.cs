using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Models;

public class Inmueble
{
	public int IdInmueble { get; set; }
	public int ? PropietarioId { get; set; }
	[Display(Name = "Dueño")]
	public Propietario ? Propietario { get; set; }
	public string ? Direccion { get; set; }
	public string ? Uso { get; set; }
	public string ? Tipo { get; set; }
	public int ? Ambientes { get; set; }
	public decimal ? Latitud { get; set; }
	public decimal ? Longitud { get; set; }
	public decimal ? Precio { get; set; }
	public bool Activo { get; set; }
}