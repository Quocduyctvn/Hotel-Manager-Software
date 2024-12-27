using Microsoft.AspNetCore.Mvc;

namespace Hotel.Client.Areas.Admin.Views.Shared.Components.AdCreateAmenity
{
	public class AdCreateAmenityViewComponent : ViewComponent
	{
		public AdCreateAmenityViewComponent()
		{
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
