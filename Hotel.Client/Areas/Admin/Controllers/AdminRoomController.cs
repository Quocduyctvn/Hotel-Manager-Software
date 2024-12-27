using AutoMapper;
using Hotel.Client.Areas.Admin.DTOs.Room;
using Hotel.Data;
using Hotel.Data.Entities;
using Hotel.Share.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using X.PagedList;

namespace Hotel.Client.Areas.Admin.Controllers
{
	public class AdminRoomController : AdminControllerBase
	{
		public AdminRoomController(ApplicationDbContext DbContext, IMapper mapper) : base(DbContext, mapper)
		{
		}
		public IActionResult Index(string keyword, int? roomCateId = null, int? floorId = null, int? page = 1, int size = DEFAULT_PAGE_SIZE)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			TempData["RoomCate"] = _HotelDbContext.AppRoomCates.Where(x => x.IdHotel == hotel.Id && x.Status == RoomCateStatus.ACTIVE).ToList();
			TempData["Floor"] = _HotelDbContext.appFloors.Where(x => x.IdHotel == hotel.Id).ToList();

			var roomCate = _HotelDbContext.AppRoomCates
								.Where(x => x.IdHotel == hotel.Id).Select(x => x.Id).ToList();

			var roomsQuery = _HotelDbContext.AppRooms
								.Where(x => roomCate.Contains(x.IdRoomCate) && x.Status != RoomStatus.IS_DELETED)
								.Include(x => x.appRoomCate)
								.Include(x => x.appFloor)
								.AsQueryable();

			// Áp dụng sắp xếp dựa trên tham số
			if (roomCateId >= 0 || roomCateId != null)
			{
				roomsQuery = roomsQuery.Where(x => x.IdRoomCate == roomCateId);
				TempData["searched"] = "searched";
			}
			else if (floorId >= 0 || floorId != null)
			{
				roomsQuery = roomsQuery.Where(x => x.IdFloor == floorId);
				TempData["searched"] = "searched";
			}
			else
			{
				roomsQuery = roomsQuery.OrderBy(x => x.Position);
			}

			if (!string.IsNullOrEmpty(keyword))
			{
				keyword = keyword.Trim().ToUpper();
				roomsQuery = roomsQuery.Where(x => x.Name.ToUpper().Contains(keyword) ||
												   x.appRoomCate.Name.ToUpper().Contains(keyword) ||
												   x.appFloor.FloorNumber.ToUpper().Contains(keyword));
				TempData["searched"] = "searched";
			}

			return View(roomsQuery.ToPagedList(page ?? DEFAULT_PAGE_NUMBER, DEFAULT_PAGE_SIZE));
		}



		[HttpPost]
		public IActionResult Create(RoomDTOs model)
		{
			if (!ModelState.IsValid)
			{
				SetErrorMesg("Vui lòng kiểm tra dữ liệu đầu vào");
				return RedirectToAction("Index");
			}

			var exist = _HotelDbContext.AppRooms.Where(x => x.IdRoomCate == model.IdRoomCate).FirstOrDefault(x => x.Name == model.Name);
			if (exist != null)
			{
				SetErrorMesg("Tên phòng đã tồn tại");
				return RedirectToAction("Index");
			}

			var room = new AppRoom();
			room.Name = model.Name;
			room.IdFloor = model.IdFloor;
			room.IdRoomCate = model.IdRoomCate;
			room.UseStartDate = model.UseStartDate;
			room.Position = 1000;
			room.Desc = model.Desc;
			room.CleanStatus = CleanRoomStatus.Clean;
			room.Status = RoomStatus.AVAILABLE;
			room.CreatedDate = DateTime.Now;

			_HotelDbContext.Add(room);
			_HotelDbContext.SaveChanges();


			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			var rooms = _HotelDbContext.AppRooms.Where(x => x.appRoomCate.IdHotel == hotel.Id).ToList();

			if (rooms.Any())
			{
				var order = rooms.OrderBy(x => x.Position).ToList();
				for (int i = 0; i < order.Count; i++)
				{
					order[i].Position = i + 1;
				}
			}

			_HotelDbContext.SaveChanges();
			SetSuccessMesg("Thêm phòng thành công");
			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult Update(RoomDTOs model)
		{
			if (model.Id <= 0)
			{
				SetErrorMesg("Xảy ra lỗi trong quá trình xử lí");
				return RedirectToAction("Index");
			}
			if (model.Name == null)
			{
				SetErrorMesg("Tên phòng không được để trống");
				return RedirectToAction("Index");
			}
			if (model.IdRoomCate <= 0)
			{
				SetErrorMesg("Hạng phòng không được để trống");
				return RedirectToAction("Index");
			}
			if (model.IdFloor <= 0)
			{
				SetErrorMesg("Tầng không được để trống");
				return RedirectToAction("Index");
			}
			//if (model.Status <= 0 || model.Status == null)
			//{
			//	SetErrorMesg("Trạng thái không được để trống");
			//	return RedirectToAction("Index");
			//}

			var room = _HotelDbContext.AppRooms.FirstOrDefault(x => x.Id == model.Id);
			if (room == null)
			{
				SetErrorMesg("Phòng không tồn tại");
				return RedirectToAction("Index");
			}
			if (room.UseStartDate > model.UseStartDate)
			{
				SetErrorMesg("Ngày bắt đầu sử dụng mới không được bé hơn ngày bắt đầu sử dụng cũ");
				return RedirectToAction("Index");
			}

			// Kiểm tra trạng thái - nếu là INACTIVE nhưng phòng đang sử dụng thì không cho phép
			//if ((RoomStatus)model.Status == RoomStatus.INACTIVE)
			//{
			//	// Danh sách các trạng thái không cho phép chuyển sang INACTIVE
			//	var restrictedStatuses = new List<RoomStatus>
			//	{
			//		RoomStatus.OCCUPIED,
			//		RoomStatus.CHECKING_IN,
			//		RoomStatus.CHECKING_OUT,
			//		RoomStatus.OVERDUE
			//	};

			//	// Kiểm tra nếu trạng thái phòng hiện tại thuộc danh sách trạng thái hạn chế
			//	if (restrictedStatuses.Contains(room.Status))
			//	{
			//		string errorMessage = room.Status switch
			//		{
			//			RoomStatus.OCCUPIED => "Không thể chuyển trạng thái sang tạm ngưng vì 'Phòng' đang được sử dụng",
			//			RoomStatus.CHECKING_IN => "Không thể chuyển trạng thái sang tạm ngưng vì 'Phòng' đang trong quá trình nhận phòng",
			//			RoomStatus.CHECKING_OUT => "Không thể chuyển trạng thái sang tạm ngưng vì 'Phòng' đang trong quá trình trả phòng",
			//			RoomStatus.OVERDUE => "Không thể chuyển trạng thái sang tạm ngưng vì 'Phòng' đang được sử dụng",
			//			_ => "Không thể chuyển trạng thái sang tạm ngưng do phòng đang ở trạng thái không hợp lệ"
			//		};

			//		SetErrorMesg(errorMessage);
			//		return RedirectToAction("Index");
			//	}
			//}


			room.Name = model.Name;
			room.IdRoomCate = model.IdRoomCate;
			room.IdFloor = model.IdFloor;
			room.UseStartDate = model.UseStartDate;
			room.Desc = model.Desc;
			//room.Status = (RoomStatus)model.Status;
			room.UpdatedDate = DateTime.Now;

			_HotelDbContext.Update(room);
			_HotelDbContext.SaveChanges();
			SetSuccessMesg("Cập nhật thành công");
			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult Delete(int id)
		{
			if (id <= 0 || id == null)
			{
				SetErrorMesg("Xảy ra lỗi trong quá trình xử lí");
				return RedirectToAction("Index");
			}
			var room = _HotelDbContext.AppRooms.FirstOrDefault(r => r.Id == id);
			room.Status = RoomStatus.IS_DELETED;

			_HotelDbContext.Update(room);
			_HotelDbContext.SaveChanges();



			SetSuccessMesg("Xóa phòng thành công");
			return RedirectToAction("Index");
		}

		public IActionResult Plus(int id)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			// Lấy danh sách hạng phòng thuộc về khách sạn hiện tại
			var roomCateIds = _HotelDbContext.AppRoomCates
								.Where(x => x.IdHotel == hotel.Id)
								.Select(x => x.Id)
								.ToList();

			// Lấy danh sách phòng dựa trên hạng phòng và trạng thái
			var rooms = _HotelDbContext.AppRooms
							.Where(x => roomCateIds.Contains(x.IdRoomCate) && x.Status != RoomStatus.IS_DELETED)
							.Include(x => x.appRoomCate)
							.Include(x => x.appFloor)
							.OrderBy(x => x.Position) // Giả sử Position xác định thứ tự sắp xếp
							.ToList();

			// Tìm phòng hiện tại và phòng tiếp theo dựa trên Position
			var currentItem = rooms.FirstOrDefault(x => x.Id == id);
			if (currentItem == null)
			{
				// Nếu không tìm thấy phòng hiện tại, chuyển hướng về Index
				return RedirectToAction("Index");
			}

			int currentIndex = rooms.IndexOf(currentItem);
			if (currentIndex == rooms.Count - 1)
			{
				// Nếu là phần tử cuối cùng, giữ nguyên và chuyển hướng về Index
				return RedirectToAction("Index");
			}

			var nextItem = rooms[currentIndex + 1];

			// Hoán đổi Position của phòng hiện tại và phòng tiếp theo
			(currentItem.Position, nextItem.Position) = (nextItem.Position, currentItem.Position);

			// Lưu các thay đổi vào cơ sở dữ liệu
			_HotelDbContext.SaveChanges();

			return RedirectToAction("Index");
		}

		public IActionResult Subtr(int id)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			// Lấy danh sách hạng phòng thuộc về khách sạn hiện tại
			var roomCateIds = _HotelDbContext.AppRoomCates
								.Where(x => x.IdHotel == hotel.Id)
								.Select(x => x.Id)
								.ToList();

			// Lấy danh sách phòng dựa trên hạng phòng và trạng thái
			var rooms = _HotelDbContext.AppRooms
							.Where(x => roomCateIds.Contains(x.IdRoomCate) && x.Status != RoomStatus.IS_DELETED)
							.Include(x => x.appRoomCate)
							.Include(x => x.appFloor)
							.OrderBy(x => x.Position) // Giả sử Position xác định thứ tự sắp xếp
							.ToList();
			// Tìm roomCate  hiện tại theo id
			var currentItem = rooms.FirstOrDefault(x => x.Id == id);

			int currentIndex = rooms.IndexOf(currentItem);

			// Kiểm tra nếu phần tử là đầu tiên, không giảm Position
			if (currentIndex == 0)
			{
				// Giữ nguyên nếu là phần tử đầu tiên
				return RedirectToAction("Index");
			}

			// Tìm package đứng ngay trước đó
			var previousItem = rooms[currentIndex - 1];

			// Hoán đổi Position giữa phần tử hiện tại và phần tử trước đó
			(currentItem.Position, previousItem.Position) = (previousItem.Position, currentItem.Position);

			// Lưu thay đổi vào cơ sở dữ liệu
			_HotelDbContext.SaveChanges();

			// Trả về trang danh sách
			return RedirectToAction("Index");
		}
	}
}
