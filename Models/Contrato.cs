using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Models;

public class Contrato
{
	public int IdContrato { get; set; }
	
	public int InmuebleId { get; set; }
	
	public Inmueble ? Inmueble { get; set; }
	
	public int InquilinoId { get; set; }
	
	public Inquilino ? Inquilino { get; set; }
	
	[Display(Name = "Fecha Inicio")]
	public DateTime ? FechaInicio { get; set; }
	
	[Display(Name = "Fecha Fin")]
	public DateTime ? FechaFin { get; set; }
	
	[Display(Name = "Monto Mensual")]
	public decimal ? MontoMensual { get; set; }
	
}