using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Models;

public enum enUsos
{
	Residencial = 1,
	Comercial = 2,
	Industrial = 3,
	Vacacional = 4,
	Educativo = 5,
	Deportivo = 6,
	Sanitario = 7,
	Cultural = 8
}

public enum enTipos
{
	Casa = 1,
	Departamento = 2,
	Oficina = 3,
	Local = 4,
	Terreno = 5,
	Galpon = 6,
	Edificio = 7,
	Hotel = 8,
	Quinta = 9
}

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
	public int Uso { get; set; }


	[Required (ErrorMessage = "El tipo es requerido")]
	public int Tipo { get; set; }


	[Required (ErrorMessage = "La cantidad de ambientes es requerido")]
	public int ? Ambientes { get; set; }


	[Required (ErrorMessage = "El latitud es requerido")]
	public decimal ? Latitud { get; set; }


	[Required (ErrorMessage = "El longitud es requerido")]
	public decimal ? Longitud { get; set; }


	[Required (ErrorMessage = "El precio es requerido")]
	public decimal ? Precio { get; set; }

	public bool Activo { get; set; }

	public string UsoNombre => Uso > 0 ? ((enUsos)Uso).ToString() : "";
	public string TipoNombre => Tipo !> 0 ? ((enTipos)Tipo).ToString() : "";

	public static IDictionary<int, string> ObtenerUsos()
	{
		SortedDictionary<int, string> usos = new SortedDictionary<int, string>();
		Type tipoEnumUso = typeof(enUsos);
		foreach (var valor in Enum.GetValues(tipoEnumUso))
		{
			usos.Add((int)valor, Enum.GetName(tipoEnumUso, valor) ?? "");
		}
		return usos;
	}

	public static IDictionary<int, string> ObtenerTipos()
	{
		SortedDictionary<int, string> tipos = new SortedDictionary<int, string>();
		Type tipoEnumTipo = typeof(enTipos);
		foreach (var valor in Enum.GetValues(tipoEnumTipo))
		{
			tipos.Add((int)valor, Enum.GetName(tipoEnumTipo, valor) ?? "");
		}
		return tipos;
	}
}