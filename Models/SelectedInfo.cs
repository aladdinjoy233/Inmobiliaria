namespace Inmobiliaria.Models;

public class SelectedInfo
{
	public int Id { get; set; }
	public string OutputString { get; set; }

	public SelectedInfo(int id, string outputString)
	{
		Id = id;
		OutputString = outputString;
	}
}