using Microsoft.AspNetCore.Mvc;

namespace Hotel.Client.Areas.Admin.Views.Shared.Components.AdCreateRoom
{
	public class AdCreateRoomViewComponent : ViewComponent
	{
		public AdCreateRoomViewComponent()
		{
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
