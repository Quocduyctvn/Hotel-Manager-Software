using Microsoft.AspNetCore.Mvc;

namespace Hotel.Client.Areas.Admin.Views.Shared.Components.AdDeleteAmenity
{
	public class AdDeleteAmenityViewComponent : ViewComponent
	{
		public AdDeleteAmenityViewComponent()
		{
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}