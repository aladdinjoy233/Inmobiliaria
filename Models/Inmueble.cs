using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Models;

public class Inmueble
{
	public int IdInmueble { get; set; }

	
	[Required (ErrorMessage = "El propietario es requerido")]
	public int PropietarioId { get; set; }

	[Display(Name = "Dueño")]
	public Propietario? Propietario { get; set; }


	[Required (ErrorMessage = "La direccion es requerido")]
	public string ? Direccion { get; set; }


	[Required (ErrorMessage = "El uso es requerido")]
	public string ? Uso { get; set; }


	[Required (ErrorMessage = "El tipo es requerido")]
	public string ? Tipo { get; set; }


	[Required (ErrorMessage = "La cantidad de ambientes es requerido")]
	public int ? Ambientes { get; set; }


	[Required (ErrorMessage = "El latitud es requerido")]
	public decimal ? Latitud { get; set; }


	[Required (ErrorMessage = "El longitud es requerido")]
	public decimal ? Longitud { get; set; }


	[Required (ErrorMessage = "El precio es requerido")]
	public decimal ? Precio { get; set; }

	public bool Activo { get; set; }
}