using AutoMapper;
using Hotel.Data;
using Hotel.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using X.PagedList;

namespace Hotel.Client.Areas.Admin.Controllers
{
	public class AdminHolidaysController : AdminControllerBase
	{
		public AdminHolidaysController(ApplicationDbContext DbContext, IMapper mapper) : base(DbContext, mapper)
		{
		}

		public IActionResult Index(string keyword, int page = 1, int size = DEFAULT_PAGE_SIZE)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			var amenities = _HotelDbContext.AppHolidays.AsNoTracking().OrderBy(x => x.Position)
													.Where(x => x.IdHotel == hotel.Id).AsQueryable();

			if (keyword != null)
			{
				keyword = keyword.Trim().ToUpper();
				amenities = amenities.Where(x => x.Name.ToUpper().Contains(keyword));
				TempData["searched"] = "searched";
			}
			return View(amenities.ToPagedList());
		}

		[HttpPost]
		public IActionResult Create(string name, DateTime startDate, DateTime endDate)
		{
			if (string.IsNullOrEmpty(name))
			{
				SetErrorMesg("Tên ngày lễ là bắt buột");
				return RedirectToAction("Index");
			}
			if (startDate == default(DateTime))
			{
				SetErrorMesg("Ngày bắt đầu không hợp lệ.");
				return RedirectToAction("Index");
			}

			if (endDate == default(DateTime))
			{
				SetErrorMesg("Ngày kết thúc không hợp lệ.");
				return RedirectToAction("Index");
			}

			if (endDate < startDate)
			{
				SetErrorMesg("Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu.");
				return RedirectToAction("Index");
			}


			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			var check = _HotelDbContext.AppHolidays.Where(x => x.IdHotel == hotel.Id).FirstOrDefault(x => x.Name == name);
			if (check != null)
			{
				SetErrorMesg("Ngày lễ đã được thiết lặp");
				return RedirectToAction("Index");
			}
			var holidays = new AppHoliday()
			{
				Name = name,
				StartDate = startDate,
				EndDate = endDate,
				IdHotel = hotel.Id,
				IdDayType = 2,
				Position = 2000,
				CreatedDate = DateTime.Now
			};

			_HotelDbContext.Add(holidays);
			_HotelDbContext.SaveChanges();

			var holiday = _HotelDbContext.AppHolidays.Where(x => x.IdHotel == hotel.Id).ToList();

			if (holiday.Any())
			{
				var order = holiday.OrderBy(x => x.Position).ToList();
				for (int i = 0; i < order.Count; i++)
				{
					order[i].Position = i + 1;
				}
			}

			_HotelDbContext.SaveChanges();
			SetSuccessMesg($"Thêm mới thành công");
			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult Update(string name, DateTime startDate, DateTime endDate, int id)
		{
			if (string.IsNullOrEmpty(name))
			{
				SetErrorMesg("Tên ngày lễ là bắt buột");
				return RedirectToAction("Index");
			}
			if (startDate == default(DateTime))
			{
				SetErrorMesg("Ngày bắt đầu không hợp lệ.");
				return RedirectToAction("Index");
			}
			if (endDate == default(DateTime))
			{
				SetErrorMesg("Ngày kết thúc không hợp lệ.");
				return RedirectToAction("Index");
			}
			if (endDate < startDate)
			{
				SetErrorMesg("Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu.");
				return RedirectToAction("Index");
			}
			if (id == null || id <= 0)
			{
				SetErrorMesg("Cảnh báo truy cập không hợp lệ");
				return RedirectToAction("Index");
			}


			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);


			var holiday = _HotelDbContext.AppHolidays.Where(x => x.IdHotel == hotel.Id).FirstOrDefault(x => x.Id == id);
			if (holiday == null)
			{
				SetErrorMesg("Truy cập không hợp lệ");
				return RedirectToAction("Index");
			}

			holiday.Name = name;
			holiday.StartDate = startDate;
			holiday.EndDate = endDate;
			holiday.UpdatedDate = DateTime.Now;
			_HotelDbContext.Update(holiday);
			_HotelDbContext.SaveChanges();
			SetSuccessMesg("Cập nhật thành công");
			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult Delete(int id)
		{

			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			var holiday = _HotelDbContext.AppHolidays.Where(x => x.IdHotel == hotel.Id).FirstOrDefault(x => x.Id == id);
			if (holiday == null)
			{
				SetErrorMesg("Truy cập không hợp lệ");
				return RedirectToAction("Index");
			}
			_HotelDbContext.Remove(holiday);
			_HotelDbContext.SaveChanges();
			SetSuccessMesg("Xóa thành công");
			return RedirectToAction("Index");
		}

		public IActionResult DeleteList(List<int> ids)
		{

			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			if (ids == null || ids.Count == 0)
			{
				SetErrorMesg("Không có mục nào được chọn để xóa.");
				return RedirectToAction("Index");
			}

			var list = _HotelDbContext.AppHolidays
						  .Where(s => ids.Contains(s.Id) && s.IdHotel == hotel.Id)
						  .ToList();
			if (list.Any())
			{
				_HotelDbContext.RemoveRange(list);
				_HotelDbContext.SaveChanges();
				SetSuccessMesg("Xóa danh sách các ngày lễ thành công");
			}

			return RedirectToAction("Index");
		}

		public IActionResult Plus(int id)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			var holiday = _HotelDbContext.AppHolidays.Where(x => x.IdHotel == hotel.Id).OrderBy(x => x.Position).ToList();

			var currentItem = holiday.FirstOrDefault(x => x.Id == id);

			int currentIndex = holiday.IndexOf(currentItem);
			if (currentIndex == holiday.Count - 1)
			{
				// Nếu là phần tử cuối cùng, giữ nguyên
				return RedirectToAction("Index");
			}

			var nextItem = holiday[currentIndex + 1];

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


			var holiday = _HotelDbContext.AppHolidays.Where(x => x.IdHotel == hotel.Id).OrderBy(x => x.Position).ToList();

			// Tìm roomCate  hiện tại theo id
			var currentItem = holiday.FirstOrDefault(x => x.Id == id);

			int currentIndex = holiday.IndexOf(currentItem);

			// Kiểm tra nếu phần tử là đầu tiên, không giảm Position
			if (currentIndex == 0)
			{
				// Giữ nguyên nếu là phần tử đầu tiên
				return RedirectToAction("Index");
			}

			// Tìm package đứng ngay trước đó
			var previousItem = holiday[currentIndex - 1];

			// Hoán đổi Position giữa phần tử hiện tại và phần tử trước đó
			(currentItem.Position, previousItem.Position) = (previousItem.Position, currentItem.Position);

			// Lưu thay đổi vào cơ sở dữ liệu
			_HotelDbContext.SaveChanges();

			// Trả về trang danh sách
			return RedirectToAction("Index");
		}

	}
}
