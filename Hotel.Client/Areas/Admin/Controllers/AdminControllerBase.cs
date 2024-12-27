using AutoMapper;
using Hotel.Data;
using Hotel.Share.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hotel.Client.Areas.Admin.Controllers
{
	[Authorize(Roles = "admin")]
	public class AdminControllerBase : Controller
	{
		protected readonly ApplicationDbContext _HotelDbContext;  // khai báo protected quy định không cho phép truy cập ngoài lớp - trừ kế thừa 
		protected const int DEFAULT_PAGE_SIZE = 10; // Số phần tử trên 1 trang
		protected const int DEFAULT_PAGE_NUMBER = 1;
		protected const int DEFAULT_PAGE_NUMBER_QUERY_STRING = 1;

		protected readonly IMapper _mapper;
		public AdminControllerBase(ApplicationDbContext DbContext, IMapper mapper)
		{
			_HotelDbContext = DbContext;
			_mapper = mapper;
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			var roomCates = _HotelDbContext.AppRoomCates
									 .Where(x => x.IdHotel == hotel.Id)
									 .Include(x => x.appRooms)
										 .ThenInclude(x => x.appFloor)
									 .Include(x => x.appImages)
									 .Include(x => x.appRentalPrices)
									 .ToList();
			foreach (var item in roomCates)
			{
				item.appRooms = item.appRooms
									 .Where(room => room.Status != RoomStatus.IS_DELETED && room.Status != RoomStatus.INACTIVE && room.UseStartDate <= DateTime.Now)
									 .ToList();
			}
			foreach (var item in roomCates)
			{
				foreach (var room in item.appRooms)
				{
					var booking = _HotelDbContext.AppBookingRooms.Where(x => x.IdRoom == room.Id && x.Status == BookingStatus.SELECTED).FirstOrDefault();
					if (booking != null)
					{
						// kiem tra "đang chờ - đang sử dụng - sắp trả - quá giờ)

						// quá giờ nhận phòng
						//if (room.Status == RoomStatus.CHECKING_IN && booking.CheckOutExpectual > DateTime.Now)
						//{
						//    // đặt phòng về phòng trống
						//    room.Status = RoomStatus.AVAILABLE;
						//    _HotelDbContext.Update(room);
						//    _HotelDbContext.SaveChanges();
						//    booking.Status = BookingStatus.CANCEL;
						//    _HotelDbContext.Update(booking);
						//    _HotelDbContext.SaveChanges();
						//    break;
						//}
						// nếu trạng tháo phòng đang sd & sắp trả
						if (room.Status == RoomStatus.OCCUPIED &&
											 (booking.CheckOutExpectual.HasValue &&
											  (booking.CheckOutExpectual.Value - DateTime.Now).TotalMinutes < 10))
						{
							room.Status = RoomStatus.CHECKING_OUT;
							_HotelDbContext.Update(room);
							_HotelDbContext.SaveChanges();
							break;
						}
						// quá giờ trả
						if ((room.Status == RoomStatus.OCCUPIED || room.Status == RoomStatus.CHECKING_OUT) &&
											 (booking.CheckOutExpectual.HasValue &&
											  booking.CheckOutExpectual.Value < DateTime.Now))
						{
							room.Status = RoomStatus.OVERDUE;
							_HotelDbContext.Update(room);
							_HotelDbContext.SaveChanges();
							break;
						}
					}
				}
			}
		}

		protected void SetErrorMesg(string mesg, bool modelStateIsInvalid = false)
		{
			TempData["Err"] = mesg;
		}
		protected void SetWrnMesg(string mesg, bool modelStateIsInvalid = false)
		{
			TempData["Wrn"] = mesg;
		}

		protected void SetSuccessMesg(string mesg) => TempData["Success"] = mesg;

	}
}
