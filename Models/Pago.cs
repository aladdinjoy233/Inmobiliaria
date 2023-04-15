using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Models;

public class Pago
{
	public int IdPago { get; set; }
	
	
	[Display(Name = "Contrato")]
	[Required (ErrorMessage = "El inmueble es requerido")]
	public int ContratoId { get; set; }
	public Contrato ? Contrato { get; set; }
	

	[Display(Name = "Número de pago")]
	public int Numero { get; set; }
	

	[Required (ErrorMessage = "El inmueble es requerido")]
	public DateTime Fecha { get; set; }


	[Required (ErrorMessage = "El inmueble es requerido")]	
	public decimal Importe { get; set; }
	
}