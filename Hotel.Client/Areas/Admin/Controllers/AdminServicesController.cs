using AutoMapper;
using Hotel.Client.Areas.Admin.DTOs.Services;
using Hotel.Data;
using Hotel.Data.Entities;
using Hotel.Share.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using X.PagedList;

namespace Hotel.Client.Areas.Admin.Controllers
{
	public class AdminServicesController : AdminControllerBase
	{
		public AdminServicesController(ApplicationDbContext DbContext, IMapper mapper) : base(DbContext, mapper)
		{
		}

		public IActionResult Index(string keyword, int page = 1, int size = DEFAULT_PAGE_SIZE)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			var svcCommo = _HotelDbContext.AppSvcCommoCates.Where(x => x.IdHotel == hotel.Id).FirstOrDefault();

			var services = _HotelDbContext.AppServices.AsNoTracking().OrderBy(x => x.Position)
													.Include(x => x.appImages)
													.Where(x => x.IdSvcCommocate == svcCommo.Id && x.Status != ServicesStatus.IS_DELETED).AsQueryable();

			if (keyword != null)
			{
				keyword = keyword.Trim().ToUpper();
				services = services.Where(x => x.Name.ToUpper().Contains(keyword) || x.Price.ToString().Contains(keyword));
				TempData["searched"] = "searched";
			}
			return View(services.ToPagedList());
		}
		public IActionResult Create(ServicesDTOs model, [FromServices] IWebHostEnvironment envi)
		{
			if (!ModelState.IsValid)
			{
				SetErrorMesg("Vui lòng kiểm tra dữ liệu đầu vào");
				return RedirectToAction("Index");
			}
			if (model.FormFile1 == null && model.FormFile2 == null)
			{
				SetErrorMesg("Vui lòng chọn ít nhất 1 ảnh");
				return RedirectToAction("Index");
			}

			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);
			var svcCommo = _HotelDbContext.AppSvcCommoCates.Where(x => x.IdHotel == hotel.Id).FirstOrDefault();

			// kiem tra su ton tai
			var exist = _HotelDbContext.AppServices.Where(x => x.IdSvcCommocate == svcCommo.Id).FirstOrDefault(x => x.Name.ToUpper().Trim() == model.Name.ToUpper().Trim());
			if (exist != null)
			{
				SetErrorMesg("Tên dịch vụ đã tồn tại");
				return RedirectToAction("Index");
			}


			var svc = new AppServices();
			svc.appImages = svc.appImages ?? new List<AppImage>();

			svc.Name = model.Name!;
			svc.Desc = model.Desc!;
			svc.Price = (decimal)model.Price!;
			svc.IdSvcCommocate = svcCommo.Id;
			svc.Position = 10000;
			svc.CreatedDate = DateTime.Now;
			if (model.FormFile1 != null)
			{
				svc.appImages.Add(
						new AppImage
						{
							Path = UploadFile(model.FormFile1, envi.WebRootPath),
						}
					);
			}
			if (model.FormFile2 != null)
			{
				svc.appImages.Add(
						new AppImage
						{
							Path = UploadFile(model.FormFile2, envi.WebRootPath),
						}
					);
			}
			_HotelDbContext.Add(svc);
			_HotelDbContext.SaveChanges();


			var svcs = _HotelDbContext.AppServices.OrderBy(x => x.Position)
													.Include(x => x.appImages)
													.Where(x => x.IdSvcCommocate == svcCommo.Id).ToList();

			if (svcs.Any())
			{
				var order = svcs.OrderBy(x => x.Position).ToList();
				for (int i = 0; i < order.Count; i++)
				{
					order[i].Position = i + 1;
				}
			}

			_HotelDbContext.SaveChanges();
			SetSuccessMesg("Thêm dịch vụ thành công");
			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult Update(ServicesDTOs model, [FromServices] IWebHostEnvironment envi)
		{
			if (!ModelState.IsValid)
			{
				SetErrorMesg("Vui lòng kiểm tra dữ liệu đầu vào");
				return RedirectToAction("Index");
			}


			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);
			var svcCommo = _HotelDbContext.AppSvcCommoCates.Where(x => x.IdHotel == hotel.Id).FirstOrDefault();

			// kiem tra su ton tai
			var svc = _HotelDbContext.AppServices.Include(x => x.appImages).Where(x => x.IdSvcCommocate == svcCommo.Id).FirstOrDefault(x => x.Id == model.Id);
			if (svc == null)
			{
				SetErrorMesg("Dịch vụ không tồn tại");
				return RedirectToAction("Index");
			}

			svc.Name = model.Name!;
			svc.Price = (decimal)model.Price!;
			svc.UpdatedDate = DateTime.Now;
			svc.Desc = model.Desc!;
			if (model.FormFile1 != null)
			{
				if (svc.appImages.FirstOrDefault() != null)
				{
					var filePath = Path.Combine(envi.WebRootPath, svc.appImages.FirstOrDefault()!.Path.TrimStart('/'));
					if (System.IO.File.Exists(filePath))
					{
						System.IO.File.Delete(filePath);
					}
					svc.appImages.FirstOrDefault()!.Path = UploadFile(model.FormFile1, envi.WebRootPath);
				}
				else
				{
					svc.appImages.Add(
						new AppImage
						{
							Path = UploadFile(model.FormFile1, envi.WebRootPath),
						}
					);
				}
			}
			if (model.FormFile2 != null)
			{
				if (svc.appImages.Skip(1).FirstOrDefault() != null)
				{
					var filePath = Path.Combine(envi.WebRootPath, svc.appImages.Skip(1).FirstOrDefault()!.Path.TrimStart('/'));
					if (System.IO.File.Exists(filePath))
					{
						System.IO.File.Delete(filePath);
					}
					svc.appImages.Skip(1).FirstOrDefault()!.Path = UploadFile(model.FormFile2, envi.WebRootPath);
				}
				else
				{
					svc.appImages.Add(
						new AppImage
						{
							Path = UploadFile(model.FormFile2, envi.WebRootPath),
						}
					);
				}
			}
			_HotelDbContext.Update(svc);
			_HotelDbContext.SaveChanges();
			SetSuccessMesg("Cập nhập dịch vụ thành công");
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
			var svc = _HotelDbContext.AppServices.FirstOrDefault(x => x.Id == id);
			svc.Status = ServicesStatus.IS_DELETED;
			svc.DeletedDate = DateTime.Now;
			_HotelDbContext.Update(svc);
			_HotelDbContext.SaveChanges();
			SetSuccessMesg("Xóa dịch vụ thành công");
			return RedirectToAction("Index");
		}


		public IActionResult Plus(int id)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			var svcCommo = _HotelDbContext.AppSvcCommoCates.Where(x => x.IdHotel == hotel.Id).FirstOrDefault();

			// Removed AsNoTracking() to allow tracking of entities
			var services = _HotelDbContext.AppServices
				.OrderBy(x => x.Position)
				.Include(x => x.appImages)
				.Where(x => x.IdSvcCommocate == svcCommo.Id && x.Status != ServicesStatus.IS_DELETED)
				.ToList();

			var currentItem = services.FirstOrDefault(x => x.Id == id);
			int currentIndex = services.IndexOf(currentItem);

			if (currentIndex == services.Count - 1)
			{
				// If it's the last item, do nothing
				return RedirectToAction("Index");
			}

			var nextItem = services[currentIndex + 1];

			// Swap Position
			(currentItem.Position, nextItem.Position) = (nextItem.Position, currentItem.Position);

			// Update and save changes
			_HotelDbContext.SaveChanges();
			return RedirectToAction("Index");
		}

		public IActionResult Subtr(int id)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			var svcCommo = _HotelDbContext.AppSvcCommoCates.Where(x => x.IdHotel == hotel.Id).FirstOrDefault();

			// Removed AsNoTracking() to allow tracking of entities
			var services = _HotelDbContext.AppServices
				.OrderBy(x => x.Position)
				.Include(x => x.appImages)
				.Where(x => x.IdSvcCommocate == svcCommo.Id && x.Status != ServicesStatus.IS_DELETED)
				.ToList();

			var currentItem = services.FirstOrDefault(x => x.Id == id);
			int currentIndex = services.IndexOf(currentItem);

			if (currentIndex == 0)
			{
				// If it's the first item, do nothing
				return RedirectToAction("Index");
			}

			var previousItem = services[currentIndex - 1];

			// Swap Position
			(currentItem.Position, previousItem.Position) = (previousItem.Position, currentItem.Position);

			// Save changes to the database
			_HotelDbContext.SaveChanges();
			return RedirectToAction("Index");
		}



		// kiểm tra dịch vụ có phòng nào đang sử dụng hay không 
		public JsonResult CheckDelete(int id)
		{
			bool canDelete = false; // Assume delete is allowed by default
			var svc = _HotelDbContext.AppServices.Include(x => x.appServicesOrders)
												 .ThenInclude(x => x.appBookingRoom)
												 .FirstOrDefault(x => x.Id == id);

			if (svc?.appServicesOrders != null && svc.appServicesOrders.Any())
			{
				foreach (var item in svc.appServicesOrders)
				{
					// Check if the current date is within the booking period
					if (DateTime.Now > item.appBookingRoom.CheckInExpectual && DateTime.Now < item.appBookingRoom.CheckOutExpectual)
					{
						canDelete = true;
						break;
					}
				}
			}

			return Json(new { CanDelete = canDelete });
		}

		private string UploadFile(IFormFile file, string webRootPath)
		{
			var fName = file.FileName;
			fName = Path.GetFileNameWithoutExtension(fName)
				+ DateTime.Now.Ticks
				+ Path.GetExtension(fName);

			var directoryPath = Path.Combine(webRootPath, "images", "services");
			Directory.CreateDirectory(directoryPath); // Đảm bảo thư mục tồn tại

			var filePath = Path.Combine(directoryPath, fName);
			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				file.CopyTo(stream);
			}

			var relativePath = "/images/services/" + fName;
			return relativePath;
		}
	}
}
