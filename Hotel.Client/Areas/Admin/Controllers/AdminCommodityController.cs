using AutoMapper;
using Hotel.Client.Areas.Admin.DTOs.Commodity;
using Hotel.Data;
using Hotel.Data.Entities;
using Hotel.Share.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using X.PagedList;

namespace Hotel.Client.Areas.Admin.Controllers
{
	public class AdminCommodityController : AdminControllerBase
	{
		public AdminCommodityController(ApplicationDbContext DbContext, IMapper mapper) : base(DbContext, mapper)
		{
		}

		public IActionResult Index(string keyword, int page = 1, int size = DEFAULT_PAGE_SIZE)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			var svcCommo = _HotelDbContext.AppSvcCommoCates.Where(x => x.IdHotel == hotel.Id).Skip(1).FirstOrDefault();

			var commo = _HotelDbContext.AppCommodities.AsNoTracking().OrderBy(x => x.Position)
													.Include(x => x.appImages)
													.Where(x => x.IdSvcCommoCate == svcCommo.Id && x.Status != CommodityStatus.IS_DELETED).AsQueryable();

			if (keyword != null)
			{
				keyword = keyword.Trim().ToUpper();
				commo = commo.Where(x => x.Name.ToUpper().Contains(keyword)
														|| x.CostPrice.ToString().Contains(keyword)
														|| x.SellingPrice.ToString().Contains(keyword)
														|| x.Stock.ToString().Contains(keyword));
				TempData["searched"] = "searched";
			}
			return View(commo.ToPagedList());
		}

		public IActionResult Create(CommodityDTOs model, [FromServices] IWebHostEnvironment envi)
		{
			if (!ModelState.IsValid)
			{
				SetErrorMesg("Vui lòng kiểm tra lại dữ liệu đầu vào");
				return RedirectToAction("Index");
			}
			if (model.FormFile1 == null && model.FormFile2 == null)
			{
				SetErrorMesg("Vui lòng chọn ít nhất 1 ảnh");
				return RedirectToAction("Index");
			}
			if (model.Stock <= 0)
			{
				SetErrorMesg("Số lượng nhập tối thiểu là 1");
				return RedirectToAction("Index");
			}

			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);
			var svcCommo = _HotelDbContext.AppSvcCommoCates.Where(x => x.IdHotel == hotel.Id).Skip(1).FirstOrDefault();

			// kiem tra su ton tai
			var exist = _HotelDbContext.AppCommodities.Where(x => x.IdSvcCommoCate == svcCommo.Id).FirstOrDefault(x => x.Name.ToUpper().Trim() == model.Name.ToUpper().Trim());
			if (exist != null)
			{
				SetErrorMesg("Tên sản phẫm đã tồn tại");
				return RedirectToAction("Index");
			}

			var commo = new AppCommodity();
			commo.Name = model.Name;
			commo.CostPrice = (decimal)model.CostPrice!;
			commo.SellingPrice = (decimal)model.SellingPrice!;
			commo.Stock = (int)model.Stock!;
			commo.Desc = model.Desc;
			commo.Status = CommodityStatus.ACTIVE;
			commo.CreatedDate = DateTime.Now;
			commo.IdSvcCommoCate = svcCommo.Id;
			commo.Position = 10000;
			if (model.FormFile1 != null)
			{
				commo.appImages.Add(
						new AppImage
						{
							Path = UploadFile(model.FormFile1, envi.WebRootPath),
						}
					);
			}
			if (model.FormFile2 != null)
			{
				commo.appImages.Add(
						new AppImage
						{
							Path = UploadFile(model.FormFile2, envi.WebRootPath),
						}
					);
			}
			_HotelDbContext.Add(commo);
			_HotelDbContext.SaveChanges();


			var svcs = _HotelDbContext.AppCommodities.OrderBy(x => x.Position)
													.Include(x => x.appImages)
													.Where(x => x.IdSvcCommoCate == svcCommo.Id).ToList();
			if (svcs.Any())
			{
				var order = svcs.OrderBy(x => x.Position).ToList();
				for (int i = 0; i < order.Count; i++)
				{
					order[i].Position = i + 1;
				}
			}

			_HotelDbContext.SaveChanges();
			SetSuccessMesg("Thêm sản phẫm thành công");
			return RedirectToAction("Index");
		}

		public IActionResult AddQuantity(AddQuantityCommoDTOs model)
		{
			if (model.Id <= 0)
			{
				SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
				return RedirectToAction("Index");
			}
			if (model.Quantity <= 0)
			{
				SetErrorMesg("Số lượng nhập tối thiểu là 1");
				return RedirectToAction("Index");
			}
			var commo = _HotelDbContext.AppCommodities.FirstOrDefault(x => x.Id == model.Id);
			if (commo == null)
			{
				SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
				return RedirectToAction("Index");
			}
			commo.Stock += model.Quantity;
			_HotelDbContext.Update(commo);
			_HotelDbContext.SaveChanges();
			SetSuccessMesg("Nhập kho thành công");
			return RedirectToAction("Index");
		}

		public IActionResult Update(CommodityDTOs model, [FromServices] IWebHostEnvironment envi)
		{

			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);
			var svcCommo = _HotelDbContext.AppSvcCommoCates.Where(x => x.IdHotel == hotel.Id).Skip(1).FirstOrDefault();

			// kiem tra su ton tai
			var commo = _HotelDbContext.AppCommodities.Include(x => x.appImages).Where(x => x.IdSvcCommoCate == svcCommo.Id).FirstOrDefault(x => x.Id == model.Id);
			if (commo == null)
			{
				SetErrorMesg("Dịch vụ không tồn tại");
				return RedirectToAction("Index");
			}
			commo.Name = model.Name;
			commo.CostPrice = (decimal)model.CostPrice!;
			commo.SellingPrice = (decimal)model.SellingPrice!;
			commo.Desc = model.Desc;
			commo.UpdatedDate = DateTime.Now;

			if (model.FormFile1 != null)
			{
				if (commo.appImages.FirstOrDefault() != null)
				{
					var filePath = Path.Combine(envi.WebRootPath, commo.appImages.FirstOrDefault()!.Path.TrimStart('/'));
					if (System.IO.File.Exists(filePath))
					{
						System.IO.File.Delete(filePath);
					}
					commo.appImages.FirstOrDefault()!.Path = UploadFile(model.FormFile1, envi.WebRootPath);
				}
				else
				{
					commo.appImages.Add(
						new AppImage
						{
							Path = UploadFile(model.FormFile1, envi.WebRootPath),
						}
					);
				}
			}
			if (model.FormFile2 != null)
			{
				if (commo.appImages.Skip(1).FirstOrDefault() != null)
				{
					var filePath = Path.Combine(envi.WebRootPath, commo.appImages.Skip(1).FirstOrDefault()!.Path.TrimStart('/'));
					if (System.IO.File.Exists(filePath))
					{
						System.IO.File.Delete(filePath);
					}
					commo.appImages.Skip(1).FirstOrDefault()!.Path = UploadFile(model.FormFile2, envi.WebRootPath);
				}
				else
				{
					commo.appImages.Add(
						new AppImage
						{
							Path = UploadFile(model.FormFile2, envi.WebRootPath),
						}
					);
				}
			}
			_HotelDbContext.Update(commo);
			_HotelDbContext.SaveChanges();
			SetSuccessMesg("Cập nhập thành công");
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
			var svc = _HotelDbContext.AppCommodities.FirstOrDefault(x => x.Id == id);
			svc.Status = CommodityStatus.IS_DELETED;
			svc.DeletedDate = DateTime.Now;
			_HotelDbContext.Update(svc);
			_HotelDbContext.SaveChanges();
			SetSuccessMesg("Xóa sản phẫm thành công");
			return RedirectToAction("Index");
		}


		public IActionResult Plus(int id)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			var svcCommo = _HotelDbContext.AppSvcCommoCates.Where(x => x.IdHotel == hotel.Id).Skip(1).FirstOrDefault();

			var commo = _HotelDbContext.AppCommodities
				.OrderBy(x => x.Position)
				.Include(x => x.appImages)
				.Where(x => x.IdSvcCommoCate == svcCommo.Id && x.Status != CommodityStatus.IS_DELETED)
				.ToList();

			var currentItem = commo.FirstOrDefault(x => x.Id == id);
			int currentIndex = commo.IndexOf(currentItem);

			if (currentIndex == commo.Count - 1)
			{
				// If it's the last item, do nothing
				return RedirectToAction("Index");
			}

			var nextItem = commo[currentIndex + 1];

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

			var svcCommo = _HotelDbContext.AppSvcCommoCates.Where(x => x.IdHotel == hotel.Id).Skip(1).FirstOrDefault();

			// Removed AsNoTracking() to allow tracking of entities
			var commo = _HotelDbContext.AppCommodities
				.OrderBy(x => x.Position)
				.Include(x => x.appImages)
				.Where(x => x.IdSvcCommoCate == svcCommo.Id && x.Status != CommodityStatus.IS_DELETED)
				.ToList();

			var currentItem = commo.FirstOrDefault(x => x.Id == id);
			int currentIndex = commo.IndexOf(currentItem);

			if (currentIndex == 0)
			{
				// If it's the first item, do nothing
				return RedirectToAction("Index");
			}

			var previousItem = commo[currentIndex - 1];

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
			var svc = _HotelDbContext.AppCommodities.Include(x => x.appComodityOrders)
												 .ThenInclude(x => x.appBookingRoom)
												 .FirstOrDefault(x => x.Id == id);

			if (svc?.appComodityOrders != null && svc.appComodityOrders.Any())
			{
				foreach (var item in svc.appComodityOrders)
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

			var directoryPath = Path.Combine(webRootPath, "images", "commodity");
			Directory.CreateDirectory(directoryPath); // Đảm bảo thư mục tồn tại

			var filePath = Path.Combine(directoryPath, fName);
			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				file.CopyTo(stream);
			}

			var relativePath = "/images/commodity/" + fName;
			return relativePath;
		}
	}
}
