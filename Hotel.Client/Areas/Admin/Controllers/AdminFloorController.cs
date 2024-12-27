using AutoMapper;
using Hotel.Client.Areas.Admin.DTOs.Floor;
using Hotel.Data;
using Hotel.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using X.PagedList;

namespace Hotel.Client.Areas.Admin.Controllers
{
	public class AdminFloorController : AdminControllerBase
	{
		public AdminFloorController(ApplicationDbContext DbContext, IMapper mapper) : base(DbContext, mapper)
		{
		}

		public IActionResult Index(string keyword, int page = 1, int size = DEFAULT_PAGE_SIZE)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			var floor = _HotelDbContext.appFloors.AsNoTracking()
													.Where(x => x.IdHotel == hotel.Id).
													OrderBy(x => x.Position).AsQueryable();

			if (keyword != null)
			{
				keyword = keyword.Trim().ToUpper();
				floor = floor.Where(x => x.FloorNumber.ToUpper().Contains(keyword) ||
														x.Desc!.ToUpper().Contains(keyword));
				TempData["searched"] = "searched";
			}
			return View(floor.ToPagedList());
		}

		public IActionResult Create(FloorDTOs model)
		{
			if (!ModelState.IsValid)
			{
				SetErrorMesg("Vui lòng kiểm tra lại giá trị đầu vào");
				return RedirectToAction("Index");
			}

			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			var check_floor = _HotelDbContext.appFloors.Where(x => x.IdHotel == hotel.Id).FirstOrDefault(x => x.FloorNumber.ToUpper() == model.FloorNumber.ToUpper());
			if (check_floor != null)
			{
				SetErrorMesg("Tên tầng đã tồn tại");
				return RedirectToAction("Index");
			}
			var floor = new AppFloor();
			floor.FloorNumber = model.FloorNumber;
			floor.Desc = model.Desc;
			floor.CreatedDate = DateTime.Now;
			floor.IdHotel = hotel.Id;
			floor.Position = 1000;

			_HotelDbContext.Add(floor);
			_HotelDbContext.SaveChanges();


			var floors = _HotelDbContext.appFloors.Where(x => x.IdHotel == hotel.Id).ToList();

			if (floors.Any())
			{
				var order = floors.OrderBy(x => x.Position).ToList();
				for (int i = 0; i < order.Count; i++)
				{
					order[i].Position = i + 1;
				}
			}

			_HotelDbContext.SaveChanges();
			SetSuccessMesg("Thêm tầng thành công");
			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult Update(FloorDTOs model)
		{
			if (!ModelState.IsValid)
			{
				SetErrorMesg("Vui lòng kiểm tra lại giá trị đầu vào");
				return RedirectToAction("Index");
			}

			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			//Kiểm tra số tầng có tồn tại hay ch
			var numberFloor = _HotelDbContext.appFloors.Where(x => x.IdHotel == hotel.Id).FirstOrDefault(x => x.Id == model.Id);
			if (numberFloor == null)
			{
				SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
				return RedirectToAction("Index");
			}
			numberFloor.FloorNumber = model.FloorNumber;
			numberFloor.Desc = model.Desc;
			numberFloor.UpdatedDate = DateTime.Now;
			_HotelDbContext.Update(numberFloor);
			_HotelDbContext.SaveChanges();
			SetSuccessMesg("Cập nhật thành công");
			return RedirectToAction("Index");
		}

		[HttpGet]
		public IActionResult Delete(int id)
		{
			if (id == null || id <= 0)
			{
				SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
				return RedirectToAction("Index");
			}

			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim!);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);
			var floor = _HotelDbContext.appFloors.Where(x => x.IdHotel == hotel!.Id)
												.Include(x => x.appRooms).FirstOrDefault(x => x.Id == id);
			if (floor == null)
			{
				SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
				return RedirectToAction("Index");
			}


			return View(floor);
		}

		public IActionResult DeleteFloor(int id)
		{
			if (id == null || id <= 0)
			{
				SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
				return RedirectToAction("Index");
			}


			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim!);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);
			var floor = _HotelDbContext.appFloors.Where(x => x.IdHotel == hotel.Id).FirstOrDefault(x => x.Id == id);
			if (floor == null)
			{
				SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
				return RedirectToAction("Index");
			}

			_HotelDbContext.Remove(floor);
			_HotelDbContext.SaveChanges();
			SetSuccessMesg("Xóa thành công");
			return RedirectToAction("Index");
		}


		public IActionResult Plus(int id)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			var floor = _HotelDbContext.appFloors.Where(x => x.IdHotel == hotel.Id).OrderBy(x => x.Position).ToList();

			var currentItem = floor.FirstOrDefault(x => x.Id == id);

			int currentIndex = floor.IndexOf(currentItem);
			if (currentIndex == floor.Count - 1)
			{
				// Nếu là phần tử cuối cùng, giữ nguyên
				return RedirectToAction("Index");
			}

			var nextItem = floor[currentIndex + 1];

			// Hoán đổi Position
			(currentItem.Position, nextItem.Position) = (nextItem.Position, currentItem.Position);

			// Cập nhật và lưu thay đổi
			_HotelDbContext.SaveChanges();
			return RedirectToAction("Index");
		}

		public IActionResult Subtr(int id)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);


			var floor = _HotelDbContext.appFloors.Where(x => x.IdHotel == hotel.Id).OrderBy(x => x.Position).ToList();

			// Tìm roomCate  hiện tại theo id
			var currentItem = floor.FirstOrDefault(x => x.Id == id);

			int currentIndex = floor.IndexOf(currentItem);

			// Kiểm tra nếu phần tử là đầu tiên, không giảm Position
			if (currentIndex == 0)
			{
				// Giữ nguyên nếu là phần tử đầu tiên
				return RedirectToAction("Index");
			}

			// Tìm package đứng ngay trước đó
			var previousItem = floor[currentIndex - 1];

			// Hoán đổi Position giữa phần tử hiện tại và phần tử trước đó
			(currentItem.Position, previousItem.Position) = (previousItem.Position, currentItem.Position);

			// Lưu thay đổi vào cơ sở dữ liệu
			_HotelDbContext.SaveChanges();

			// Trả về trang danh sách
			return RedirectToAction("Index");
		}

	}
}
