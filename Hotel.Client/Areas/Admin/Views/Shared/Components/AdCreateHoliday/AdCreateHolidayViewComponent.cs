using Microsoft.AspNetCore.Mvc;

namespace Hotel.Client.Areas.Admin.Views.Shared.Components.AdCreateHoliday
{
	public class AdCreateHolidayViewComponent : ViewComponent
	{
		public AdCreateHolidayViewComponent()
		{
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
