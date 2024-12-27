using AutoMapper;
using Hotel.Data;
using Hotel.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using X.PagedList;

namespace Hotel.Client.Areas.Admin.Controllers
{
	public class AdminAmenityController : AdminControllerBase
	{
		public AdminAmenityController(ApplicationDbContext DbContext, IMapper mapper) : base(DbContext, mapper)
		{
		}

		public IActionResult Index(string keyword, int page = 1, int size = DEFAULT_PAGE_SIZE)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			var amenities = _HotelDbContext.AppAmenities.AsNoTracking()
													.Where(x => x.IdHotel == hotel.Id).
													OrderBy(x => x.Position).AsQueryable();

			if (keyword != null)
			{
				keyword = keyword.Trim().ToUpper();
				amenities = amenities.Where(x => x.Name.ToUpper().Contains(keyword) ||
														x.Desc!.ToUpper().Contains(keyword));
				TempData["searched"] = "searched";
			}
			return View(amenities.ToPagedList());
		}


		[HttpPost]
		public async Task<IActionResult> Create(string name, string desc)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);


			if (string.IsNullOrEmpty(name))
			{
				SetWrnMesg("Tên tiện nghi là bắt buột");
				return RedirectToAction("Index");
			}

			var checkName = _HotelDbContext.AppAmenities.Where(x => x.IdHotel == hotel.Id).FirstOrDefault(x => x.Name == name);
			if (checkName != null)
			{
				SetErrorMesg("Tên tiện nghi đã tồn tại");
				return RedirectToAction("Index");
			}

			var amenity = new AppAmenity()
			{
				Name = name,
				Desc = desc,
				Position = 1000,
				CreatedDate = DateTime.Now,
				IdHotel = hotel.Id
			};

			_HotelDbContext.Add(amenity);
			await _HotelDbContext.SaveChangesAsync();



			var amenities = _HotelDbContext.AppAmenities.Where(x => x.IdHotel == hotel.Id).ToList();

			if (amenities.Any())
			{
				var order = amenities.OrderBy(x => x.Position).ToList();
				for (int i = 0; i < order.Count; i++)
				{
					order[i].Position = i + 1;
				}
			}

			_HotelDbContext.SaveChanges();
			SetSuccessMesg("Thêm mới tiện nghi thành công");
			return RedirectToAction("Index");
		}


		[HttpPost]
		public async Task<IActionResult> Update(int id, string name, string desc)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);


			if (string.IsNullOrEmpty(name))
			{
				SetWrnMesg("Tên tiện nghi là bắt buột");
				return RedirectToAction("Index");
			}

			var amenities = _HotelDbContext.AppAmenities.Where(x => x.IdHotel == hotel.Id).FirstOrDefault(x => x.Id == id);
			if (amenities == null)
			{
				SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lý");
				return RedirectToAction("Index");
			}

			amenities.UpdatedDate = DateTime.Now;
			amenities.Name = name;
			amenities.Desc = desc;

			_HotelDbContext.Update(amenities);
			_HotelDbContext.SaveChanges();
			SetSuccessMesg("Cập nhật tiện nghi thành công");
			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);


			var amenities = await _HotelDbContext.AppAmenities.Where(x => x.IdHotel == hotel.Id).FirstOrDefaultAsync(x => x.Id == id);
			if (amenities == null)
			{
				SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lý");
				return RedirectToAction("Index");
			}
			var amenityRoom = _HotelDbContext.AppRoomCateAmenities.Where(x => x.IdAmenity == id).ToList();
			if (amenityRoom.Any())
			{
				_HotelDbContext.AppRoomCateAmenities.RemoveRange(amenityRoom);
			}

			_HotelDbContext.Remove(amenities);
			_HotelDbContext.SaveChanges(true);
			SetSuccessMesg("Xóa tiện nghi thành công");
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


			var list = _HotelDbContext.AppAmenities
									  .Where(s => ids.Contains(s.Id) && s.IdHotel == hotel.Id)
									  .ToList();

			// xóa danh sách các RoomCateAmenity 
			foreach (var item in list)
			{
				var amenityRoom = _HotelDbContext.AppRoomCateAmenities.Where(x => x.IdAmenity == item.Id).ToList();
				if (amenityRoom.Any())
				{
					_HotelDbContext.AppRoomCateAmenities.RemoveRange(amenityRoom);
				}
			}

			// xóa amenity 
			foreach (var service in list)
			{
				_HotelDbContext.AppAmenities.Remove(service);
			}
			_HotelDbContext.SaveChanges();

			SetSuccessMesg($"Xóa {ids.Count} mục thành công");
			return RedirectToAction("Index");
		}


		public IActionResult Plus(int id)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			var amenities = _HotelDbContext.AppAmenities.Where(x => x.IdHotel == hotel.Id).OrderBy(x => x.Position).ToList();

			var currentItem = amenities.FirstOrDefault(x => x.Id == id);

			int currentIndex = amenities.IndexOf(currentItem);
			if (currentIndex == amenities.Count - 1)
			{
				// Nếu là phần tử cuối cùng, giữ nguyên
				return RedirectToAction("Index");
			}

			var nextItem = amenities[currentIndex + 1];

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


			var amenities = _HotelDbContext.AppAmenities.Where(x => x.IdHotel == hotel.Id).OrderBy(x => x.Position).ToList();

			// Tìm roomCate  hiện tại theo id
			var currentItem = amenities.FirstOrDefault(x => x.Id == id);

			int currentIndex = amenities.IndexOf(currentItem);

			// Kiểm tra nếu phần tử là đầu tiên, không giảm Position
			if (currentIndex == 0)
			{
				// Giữ nguyên nếu là phần tử đầu tiên
				return RedirectToAction("Index");
			}

			// Tìm package đứng ngay trước đó
			var previousItem = amenities[currentIndex - 1];

			// Hoán đổi Position giữa phần tử hiện tại và phần tử trước đó
			(currentItem.Position, previousItem.Position) = (previousItem.Position, currentItem.Position);

			// Lưu thay đổi vào cơ sở dữ liệu
			_HotelDbContext.SaveChanges();

			// Trả về trang danh sách
			return RedirectToAction("Index");
		}

	}
}
