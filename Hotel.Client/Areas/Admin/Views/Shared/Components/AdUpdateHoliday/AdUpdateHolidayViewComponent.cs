using Microsoft.AspNetCore.Mvc;

namespace Hotel.Client.Areas.Admin.Views.Shared.Components.AdUpdateHoliday
{
	public class AdUpdateHolidayViewComponent : ViewComponent
	{
		public AdUpdateHolidayViewComponent()
		{

		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
