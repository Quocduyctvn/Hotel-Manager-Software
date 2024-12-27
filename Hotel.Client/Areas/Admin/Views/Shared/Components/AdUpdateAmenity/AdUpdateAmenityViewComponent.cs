using Microsoft.AspNetCore.Mvc;

namespace Hotel.Client.Areas.Admin.Views.Shared.Components.AdUpdateAmenity
{
	public class AdUpdateAmenityViewComponent : ViewComponent
	{
		public AdUpdateAmenityViewComponent()
		{

		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
