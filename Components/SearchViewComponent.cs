using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Inmobiliaria.Components
{
	public class SearchViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync(string searchUrl, string inputId)
		{
			return View(new { SearchUrl = searchUrl, InputId = inputId });
		}
	}
}