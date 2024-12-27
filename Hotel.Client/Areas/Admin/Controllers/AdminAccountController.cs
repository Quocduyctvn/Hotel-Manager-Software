using AutoMapper;
using Hotel.Client.Areas.Admin.DTOs.User;
using Hotel.Data;
using Hotel.Data.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using X.PagedList;

namespace Hotel.Client.Areas.Admin.Controllers
{
	public class AdminAccountController : AdminControllerBase
	{
		public AdminAccountController(ApplicationDbContext DbContext, IMapper mapper) : base(DbContext, mapper)
		{
		}
		public IActionResult Index(string keyword, int? page)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			var user = _HotelDbContext.AppUser.Include(x => x.appRole).Where(x => x.IdGroup == IdGroup).AsQueryable();
			if (!String.IsNullOrEmpty(keyword))
			{
				var keywordUpper = keyword.ToUpper();
				user = user.Where(i => i.Name.ToUpper().Contains(keywordUpper) || i.appRole.Name.ToUpper().Contains(keywordUpper) || i.Email.ToUpper().Contains(keywordUpper));
				TempData["searched"] = "searched";
			}

			return View(user.ToPagedList(page ?? DEFAULT_PAGE_NUMBER, DEFAULT_PAGE_SIZE));
		}

		public IActionResult Create()
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			ViewBag.ListRole = _HotelDbContext.AppRole.Where(x => x.IdGroup == IdGroup).ToList();
			return View();
		}

		[HttpPost]
		public IActionResult Create(CreateUserDTOs model, [FromServices] IWebHostEnvironment envi)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			if (model.Name == null)
			{
				SetErrorMesg("Tên tài khoản là bắt buột!!");
				return RedirectToAction("Create");
			}
			if (model.Password == null)
			{
				SetErrorMesg("Mật khẩu là bắt buột!!");
				return RedirectToAction("Create");
			}
			if (model.Cfm_Password == null)
			{
				SetErrorMesg("Mật khẩu xác nhận là bắt buột!!");
				return RedirectToAction("Create");
			}
			if (model.IdRole < 0)
			{
				SetErrorMesg("Vai trò là bắt buột!!");
				return RedirectToAction("Create");
			}

			var exit = _HotelDbContext.AppUser.Any(i => i.Name == model.Name);
			if (exit)
			{
				SetErrorMesg("Tài khoản đã tồn tại");
				return RedirectToAction("Create");
			}
			var user = new AppUser();
			user.Name = model.Name;
			user.Email = model.Email;
			user.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
			user.Phone = model.Phone;
			if (model.fileAvatar != null)
			{
				user.Avatar = UploadFile(model.fileAvatar, envi.WebRootPath);
			}
			user.CreatedDate = DateTime.Now;
			user.IdRole = model.IdRole;
			user.IdGroup = IdGroup;
			_HotelDbContext.AppUser.Add(user);
			_HotelDbContext.SaveChanges();
			SetSuccessMesg("Thêm tài khoản thành công");
			return RedirectToAction("Index");
		}

		public IActionResult Update(int? id)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			ViewBag.ListRole = _HotelDbContext.AppRole.Where(x => x.IdGroup == IdGroup).ToList();

			if (id == null)
			{
				SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
				return RedirectToAction("Update");
			}
			var user = _HotelDbContext.AppUser.FirstOrDefault(x => x.Id == id);
			if (user == null)
			{
				SetErrorMesg("Người dùng không tồn tại");
				return RedirectToAction("Update");
			}

			var userDTOs = new UpdateUserDTOs();
			userDTOs.StrAvatar = user.Avatar;
			userDTOs.Email = user.Email;
			userDTOs.Phone = user.Phone;
			userDTOs.Name = user.Name;
			userDTOs.Cfm_Password = user.Password;
			userDTOs.Password = user.Password;
			userDTOs.IdRole = user.IdRole;


			return View(userDTOs);
		}

		[HttpPost]
		public IActionResult Update(int id, UpdateUserDTOs model, [FromServices] IWebHostEnvironment envi)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);
			if (model.Name == null)
			{
				SetErrorMesg("Tên tài khoản là bắt buột!!");
				return RedirectToAction("Create");
			}
			if (model.IdRole < 0)
			{
				SetErrorMesg("Vai trò là bắt buột!!");
				return RedirectToAction("Create");
			}

			var user = _HotelDbContext.AppUser.FirstOrDefault(x => x.Id == id);
			if (user == null)
			{
				SetErrorMesg("Người dùng không tồn tại");
				return RedirectToAction("Update");
			}

			if (model.fileAvatar != null)
			{
				user.Avatar = UploadFile(model.fileAvatar, envi.WebRootPath);
			}
			user.Email = model.Email;
			user.Phone = model.Phone;
			user.Name = model.Name;
			user.IdRole = model.IdRole;

			_HotelDbContext.Update(user);
			_HotelDbContext.SaveChanges();
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Index", "Home", new { area = "" });
		}

		private string UploadFile(IFormFile file, string webRootPath)
		{
			var fName = file.FileName;
			fName = Path.GetFileNameWithoutExtension(fName)
				+ DateTime.Now.Ticks
				+ Path.GetExtension(fName);

			var directoryPath = Path.Combine(webRootPath, "images", "user");
			Directory.CreateDirectory(directoryPath); // Đảm bảo thư mục tồn tại

			var filePath = Path.Combine(directoryPath, fName);
			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				file.CopyTo(stream);
			}

			var relativePath = "/images/user/" + fName;
			return relativePath;
		}
	}
}
