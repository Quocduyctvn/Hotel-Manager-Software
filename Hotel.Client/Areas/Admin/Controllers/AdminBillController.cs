using AutoMapper;
using Hotel.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace Hotel.Client.Areas.Admin.Controllers
{
	public class AdminBillController : AdminControllerBase
	{
		public AdminBillController(ApplicationDbContext DbContext, IMapper mapper) : base(DbContext, mapper)
		{
		}

		public IActionResult Index(string keyword, int page = 1, int size = DEFAULT_PAGE_SIZE)
		{
			var bill = _HotelDbContext.AppBill
											.Include(x => x.appBookingRoom)
												.ThenInclude(x => x.appCustHotel)
											.Include(x => x.appBookingRoom)
											.ThenInclude(x => x.appRoom)
												.ThenInclude(x => x.appRoomCate).AsQueryable();
			return View(bill.ToPagedList());
		}
	}
}
