using AutoMapper;
using Hotel.Admin.Areas.Admin.DTOs;
using Hotel.Admin.Areas.Admin.DTOs.RentalPackage;
using Hotel.Data;
using Hotel.Data.Entities;
using Hotel.Share.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace Hotel.Admin.Areas.Admin.Controllers
{
	public class AdminRentalPackageController : AdminControllerBase
	{
		public AdminRentalPackageController(ApplicationDbContext DbContext, IMapper mapper) : base(DbContext, mapper)
		{
		}
		public IActionResult Index()
		{
			ViewBag.RentalPackageCate = _HotelDbContext.AppRentalPackageCate.ToList();
			ViewBag.Times = _HotelDbContext.AppTime.ToList();
			ViewBag.Status = new List<Status> {
				new Status { Id = 0, Name = "Đang hoạt động" },
				new Status { Id = 1, Name = "Tạm ngưng hoạt động" }
			};
			var rentalPackage = _HotelDbContext.AppRentalPackage.OrderBy(x => x.Position).AsQueryable();

			return View(rentalPackage.ToPagedList());
		}

		public IActionResult Create()
		{
			ViewBag.RentalPackageCate = _HotelDbContext.AppRentalPackageCate.ToList();
			ViewBag.Status = new List<Status> {
				new Status { Id = 0, Name = "Đang hoạt động" },
				new Status { Id = 1, Name = "Tạm ngưng hoạt động" }
			};
			ViewBag.Times = _HotelDbContext.AppTime.ToList();
			return View();
		}

		[HttpPost]
		public IActionResult Create(CreateRentalPackageDTOs model)
		{
			ModelState.Remove("Status");
			if (!ModelState.IsValid)
			{
				SetErrorMesg("Dữ liệu không hợp lệ");
				return RedirectToAction("Create", model);
			}

			var rentalPackage = _HotelDbContext.AppRentalPackage.FirstOrDefault(x => x.Name.Trim().ToUpper() == model.Name.Trim().ToUpper());
			if (rentalPackage != null)
			{
				SetErrorMesg("Hình thức cho thuê đã tồn tại");
				return RedirectToAction("Create", model);
			}
			if (model.IdTimeOfPrice <= 0)
			{
				SetErrorMesg("Vui lòng chọn loại thời gian cho giá tiền");
				return RedirectToAction("Create", model);
			}
			if (model.IdRentalPackageCate <= 0)
			{
				SetErrorMesg("Vui lòng chọn loại hình cho thuê");
				return RedirectToAction("Create", model);
			}
			if (model.Status == null)
			{
				SetErrorMesg("Trạng thái không được để trống");
				return RedirectToAction("Create", model);
			}
			if (model.IdRentalPackageCate == 1)
			{
				if (model.TrialTime == null)
				{
					SetErrorMesg("Thời gian cho thuê không được để trống đối với gói free");
					return RedirectToAction("Update", model);
				}
				if (model.IdTimeOfTrial == null || model.IdTimeOfTrial <= 0)
				{
					SetErrorMesg("Vui lòng chọn loại thời gian cho thuê");
					return RedirectToAction("Update", model);
				}
			}
			else
			{
				model.TrialTime = null;
				model.IdTimeOfTrial = null;
			}

			AppRentalPackage package = _mapper.Map<AppRentalPackage>(model);
			package.Position = 1000;  // set default 
			package.CreatedDate = DateTime.Now;

			//package.Name = model.Name;
			//package.Price = model.Price;
			//package.IdTimeOfPrice = (int)model.IdTimeOfPrice!;
			//package.TrialTime = model.TrialTime;
			//package.IdTimeOfTrial = model.IdTimeOfTrial;
			//package.Desc = model.Desc;
			//package.AccountLimit = model.AccountLimit;
			//package.RoomLimit = model.RoomLimit;
			//package.UsageTraining = model.UsageTraining;
			//package.VisualReport = model.VisualReport;
			//package.Status = (RentalStatus)model.Status;
			//package.IdRentalPackageCate = (int)model.IdRentalPackageCate!;
			//package.CreatedDate = DateTime.Now;
			_HotelDbContext.AppRentalPackage.Add(package);
			_HotelDbContext.SaveChanges();


			var package_list = _HotelDbContext.AppRentalPackage.ToList();

			if (package_list.Any())
			{
				var order = package_list.OrderBy(x => x.Position).ToList();
				for (int i = 0; i < order.Count; i++)
				{
					order[i].Position = i + 1;
				}
			}

			_HotelDbContext.SaveChanges();
			SetSuccessMesg("Thêm gói cho thuê thành công");
			return RedirectToAction("Index");
		}

		public IActionResult Update(int id)
		{
			if (id == null || id <= 0)
			{
				SetErrorMesg("Giá trị id không được để trống");
				return RedirectToAction("Index");
			}
			var package = _HotelDbContext.AppRentalPackage.FirstOrDefault(o => o.Id == id);
			if (package == null)
			{
				SetErrorMesg("Hình thức cho thuê không tồn tại");
				return RedirectToAction("Index");
			}

			ViewBag.RentalPackageCate = _HotelDbContext.AppRentalPackageCate.ToList();
			ViewBag.Status = new List<Status> {
				new Status { Id = 0, Name = "Đang hoạt động" },
				new Status { Id = 1, Name = "Tạm ngưng hoạt động" }
			};
			ViewBag.Times = _HotelDbContext.AppTime.ToList();

			UpdateRentalPackageDTOs update = _mapper.Map<UpdateRentalPackageDTOs>(package);
			//var update = new UpdateRentalPackageDTOs();
			//update.Price = package.Price;
			//update.IdTimeOfPrice = package.IdTimeOfPrice;
			//update.TrialTime = package.TrialTime;
			//update.IdTimeOfTrial = package.IdTimeOfTrial;
			//update.Desc = package.Desc;
			//update.AccountLimit = package.AccountLimit;
			//update.RoomLimit = package.RoomLimit;
			//update.UsageTraining = package.UsageTraining;
			//update.UsageTraining = package.UsageTraining;
			//update.VisualReport = package.VisualReport;
			//update.Status = package.Status;
			//update.IdRentalPackageCate = package.IdRentalPackageCate;
			return View(update);
		}

		[HttpPost]
		public IActionResult Update(int id, UpdateRentalPackageDTOs model)
		{
			if (id == null || id <= 0)
			{
				SetErrorMesg("Giá trị id không được để trống");
				return RedirectToAction("Index");
			}
			if (model.Status == null)
			{
				SetErrorMesg("Trạng thái không được null");
				return View(model);
			}
			var package = _HotelDbContext.AppRentalPackage
										.FirstOrDefault(o => o.Id == id);
			if (package == null)
			{
				SetErrorMesg("Gói cho thuê không tồn tại");
				return RedirectToAction("Index");
			}
			if (model.IdTimeOfPrice <= 0)
			{
				SetErrorMesg("Vui lòng chọn loại thời gian cho giá tiền");
				return RedirectToAction("Update", model);
			}
			if (model.IdRentalPackageCate <= 0)
			{
				SetErrorMesg("Vui lòng chọn loại hình cho thuê");
				return RedirectToAction("Update", model);
			}
			if (model.Status == null)
			{
				SetErrorMesg("Trạng thái không được để trống");
				return RedirectToAction("Update", model);
			}
			if (model.IdRentalPackageCate == 1)
			{
				if (model.TrialTime == null)
				{
					SetErrorMesg("Thời gian cho thuê không được để trống đối với gói free");
					return RedirectToAction("Update", model);
				}
				if (model.IdTimeOfTrial == null || model.IdTimeOfTrial <= 0)
				{
					SetErrorMesg("Vui lòng chọn loại thời gian cho thuê");
					return RedirectToAction("Update", model);
				}
			}
			else
			{
				model.TrialTime = null;
				model.IdTimeOfTrial = null;
			}
			_mapper.Map(model, package);
			package.UpdatedDate = DateTime.Now;

			_HotelDbContext.Update(package);
			_HotelDbContext.SaveChanges();
			SetSuccessMesg("Cập nhật hình thức cho thuê thành công");
			return RedirectToAction("Index");
		}

		public IActionResult Delete(int id)
		{
			if (id <= 0 || id == null)
			{
				SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
				return RedirectToAction("Index");
			}
			var package = _HotelDbContext.AppRentalPackage.Include(x => x.appHMSOrders)
													.ThenInclude(x => x.appHotels)
													.FirstOrDefault(x => x.Id == id);
			if (package == null)
			{
				SetErrorMesg("Gói cho thuê không tồn tại");
				return RedirectToAction("Index");
			}

			ViewBag.Status = new List<Status> {
				new Status { Id = 0, Name = "Đang hoạt động" },
				new Status { Id = 1, Name = "Tạm ngưng hoạt động" }
			};
			ViewBag.Times = _HotelDbContext.AppTime.ToList();

			return View(package);
		}

		public IActionResult DeletePackage(int id)
		{
			if (id <= 0 || id == null)
			{
				SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
				return RedirectToAction("Delete");
			}
			var package = _HotelDbContext.AppRentalPackage.Include(x => x.appHMSOrders)
													.ThenInclude(x => x.appHotels)
													.FirstOrDefault(x => x.Id == id);
			if (package == null)
			{
				SetErrorMesg("Gói cho thuê không tồn tại");
				return RedirectToAction("Delete");
			}
			_HotelDbContext.Remove(package);
			_HotelDbContext.SaveChanges();
			SetSuccessMesg("Xóa thành công");
			return RedirectToAction("Index");
		}

		public IActionResult UpdateStatus(int id)
		{
			if (id <= 0 || id == null)
			{
				SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
				return RedirectToAction("Delete");
			}
			var package = _HotelDbContext.AppRentalPackage.Include(x => x.appHMSOrders)
													.ThenInclude(x => x.appHotels)
													.FirstOrDefault(x => x.Id == id);
			if (package == null)
			{
				SetErrorMesg("Gói cho thuê không tồn tại");
				return RedirectToAction("Delete");
			}

			package.Status = RentalStatus.INACTIVE;
			_HotelDbContext.Update(package);
			_HotelDbContext.SaveChanges();
			SetSuccessMesg("Cập nhật thành công");
			return RedirectToAction("Index");
		}

		public IActionResult Plus(int id)
		{
			// Lấy danh sách các package đã sắp xếp theo Position
			var packages = _HotelDbContext.AppRentalPackage.OrderBy(x => x.Position).ToList();

			// Tìm package hiện tại và package kế tiếp
			var currentItem = packages.FirstOrDefault(x => x.Id == id);

			int currentIndex = packages.IndexOf(currentItem);
			if (currentIndex == packages.Count - 1)
			{
				// Nếu là phần tử cuối cùng, giữ nguyên
				return RedirectToAction("Index");
			}

			var nextItem = packages[currentIndex + 1];

			// Hoán đổi Position
			(currentItem.Position, nextItem.Position) = (nextItem.Position, currentItem.Position);

			// Cập nhật và lưu thay đổi
			_HotelDbContext.SaveChanges();

			return RedirectToAction("Index");
		}

		public IActionResult Subtr(int id)
		{
			// Lấy danh sách các package sắp xếp theo Position
			var packages = _HotelDbContext.AppRentalPackage.OrderBy(x => x.Position).ToList();

			// Tìm package hiện tại theo id
			var currentItem = packages.FirstOrDefault(x => x.Id == id);

			int currentIndex = packages.IndexOf(currentItem);

			// Kiểm tra nếu phần tử là đầu tiên, không giảm Position
			if (currentIndex == 0)
			{
				// Giữ nguyên nếu là phần tử đầu tiên
				return RedirectToAction("Index");
			}

			// Tìm package đứng ngay trước đó
			var previousItem = packages[currentIndex - 1];

			// Hoán đổi Position giữa phần tử hiện tại và phần tử trước đó
			(currentItem.Position, previousItem.Position) = (previousItem.Position, currentItem.Position);

			// Lưu thay đổi vào cơ sở dữ liệu
			_HotelDbContext.SaveChanges();

			// Trả về trang danh sách
			return RedirectToAction("Index");
		}

	}
}
