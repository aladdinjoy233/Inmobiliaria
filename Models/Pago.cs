using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Models;

public class Pago
{
	public int IdPago { get; set; }
	
	[Display(Name = "Contrato")]
	public int ContratoId { get; set; }
	
	public Contrato ? Contrato { get; set; }
	
	[Display(Name = "Número de pago")]
	public int Numero { get; set; }
	
	public DateTime Fecha { get; set; }
	
	public decimal Importe { get; set; }
	
}