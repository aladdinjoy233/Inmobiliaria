using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Inmobiliaria.Components
{
	public class SearchViewComponent : ViewComponent
	{
		public IViewComponentResult Invoke(string searchUrl, string inputId, object selected)
		{
			return View(new { SearchUrl = searchUrl, InputId = inputId, Selected = selected });
		}
	}
}