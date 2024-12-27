using Hotel.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hotel.Client.Areas.Admin.Views.Shared.Components.AdRoomCateAmenity
{
	public class AdRoomCateAmenityViewComponent : ViewComponent
	{
		protected readonly ApplicationDbContext _HotelDbContext;
		public AdRoomCateAmenityViewComponent(ApplicationDbContext DbContext)
		{
			_HotelDbContext = DbContext;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{

			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			var amenities = _HotelDbContext.AppAmenities.Where(x => x.IdHotel == hotel.Id).ToList();
			return View(amenities);
		}
	}
}