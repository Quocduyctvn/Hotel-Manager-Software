using AutoMapper;
using Hotel.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hotel.Client.Areas.Admin.Controllers
{
	public class AdminHomeController : AdminControllerBase
	{
		public AdminHomeController(ApplicationDbContext DbContext, IMapper mapper) : base(DbContext, mapper)
		{
		}

		public IActionResult Index()
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			ViewBag.Room = _HotelDbContext.AppRooms.Include(x => x.appRoomCate)
										.Where(x => x.appRoomCate.IdHotel == hotel.Id).ToList();
			return View();
		}
	}
}
