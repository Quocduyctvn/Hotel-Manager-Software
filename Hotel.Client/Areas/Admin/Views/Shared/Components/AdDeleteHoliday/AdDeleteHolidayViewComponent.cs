using Microsoft.AspNetCore.Mvc;

namespace Hotel.Client.Areas.Admin.Views.Shared.Components.AdDeleteHoliday
{
	public class AdDeleteHolidayViewComponent : ViewComponent
	{
		public AdDeleteHolidayViewComponent()
		{
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
