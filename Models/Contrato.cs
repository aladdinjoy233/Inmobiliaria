using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Models;

public class Contrato
{
	public int IdContrato { get; set; }
	

	[Required (ErrorMessage = "El inmueble es requerido")]
	public int InmuebleId { get; set; }
	public Inmueble ? Inmueble { get; set; }
	

	[Required (ErrorMessage = "El inquilino es requerido")]
	public int InquilinoId { get; set; }
	public Inquilino ? Inquilino { get; set; }
	

	[Display(Name = "Fecha Inicio")]
	[Required (ErrorMessage = "La fecha de inicio es requerido")]
	public DateTime ? FechaInicio { get; set; }
	

	[Display(Name = "Fecha Fin")]
	[Required (ErrorMessage = "La fecha del fin es requerido")]
	public DateTime ? FechaFin { get; set; }
	

	[Display(Name = "Monto Mensual")]
	[Required (ErrorMessage = "El monto mensual es requerido")]
	public decimal ? MontoMensual { get; set; }
	
}