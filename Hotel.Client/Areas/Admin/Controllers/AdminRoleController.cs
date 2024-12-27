using AutoMapper;
using Hotel.Client.Areas.Admin.DTOs.Role;
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
	public class AdminRoleController : AdminControllerBase
	{
		public AdminRoleController(ApplicationDbContext DbContext, IMapper mapper) : base(DbContext, mapper)
		{
		}

		public IActionResult Index(string keyword, int? page)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			var roles = _HotelDbContext.AppRole.Where(x => x.IdGroup == IdGroup).ToList();

			return View(roles.ToPagedList(page ?? DEFAULT_PAGE_NUMBER, DEFAULT_PAGE_SIZE));
		}

		public IActionResult AddRole()
		{
			return View();
		}

		[HttpPost]
		public IActionResult AddRole(RoleDTOs model)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			if (!ModelState.IsValid)
			{
				SetErrorMesg("Vui lòng nhập đúng yêu cầu");
				return View(model);
			}
			if (model.IdPermission == null)
			{
				SetErrorMesg("Xảy ra lỗi trong quá trình xử lí");
				return View(model);
			}

			var arrIdPermission = model.IdPermission.Split(',');

			var role = new AppRole();

			role.Name = model.Name;
			role.Desc = model.Desc;
			role.CreatedDate = DateTime.Now;
			role.appRolePers = new List<AppRolePermission>();
			role.IdGroup = IdGroup;

			foreach (var item in arrIdPermission)
			{
				var idPer = Convert.ToInt32(item);
				role.appRolePers.Add(new AppRolePermission
				{
					IdPermission = idPer,
					IdGroup = IdGroup
                });
			}
			_HotelDbContext.Add(role);
			_HotelDbContext.SaveChanges();
			SetSuccessMesg("Thêm vai trò thành công");
			return RedirectToAction("Index");
		}


		[HttpGet]
		public async Task<IActionResult> Update(int? id)
		{
			var data = _HotelDbContext.AppRole.Include(x => x.appRolePers).FirstOrDefault(x => x.Id == id);
			if (data == null)
			{
				return NotFound();
			}
			var model = new UpdateRoleDTOs
			{
				Id = data.Id,
				Name = data.Name,
				Desc = data.Desc,
				IdPermission = string.Join(',', data.appRolePers.Select(rp => rp.IdPermission)),
			};

			return View(model);
		}


		[HttpPost]
		public IActionResult Update(UpdateRoleDTOs model)
		{
			if (String.IsNullOrEmpty(model.IdPermission))
			{
				SetErrorMesg("Vai trò ít nhất 1 hành động");
				return RedirectToAction("Update", new { id = model.Id });
			}

			var role = _HotelDbContext.AppRole.FirstOrDefault(x => x.Id == model.Id);
			// kiểm tra có XÓA bớt hay không 
			string[] deletedIdPermission = new string[200];
			if (model.DeletedIdPermission != null)
			{
				if (model.DeletedIdPermission.Contains(","))  // kiểm tra xem có nhiều hơn 1 hay không
				{
					deletedIdPermission = model.DeletedIdPermission.Split(',');
				}
				else
				{
					deletedIdPermission[0] = model.DeletedIdPermission;
				}
			}

			if (deletedIdPermission.Length > 0)
			{
				foreach (var item in deletedIdPermission)
				{
					if (item != null)
					{
						var idPer = Convert.ToInt32(item);
						var rolePer = _HotelDbContext.AppRolePermissions
										.Where(i => i.IdRole == role.Id && i.IdPermission == idPer)
										.FirstOrDefault();
						_HotelDbContext.AppRolePermissions.Remove(rolePer);
						_HotelDbContext.SaveChanges();
					}
				}
			}

			// kiểm tra có thêm mới hay không 
			string[] addedIdPermission = new string[200];
			if (model.AddedIdPermission != null)
			{
				if (model.AddedIdPermission.Contains(",")) // kiểm tra xem có nhiều hơn 1 hay không 
				{
					addedIdPermission = model.AddedIdPermission.Split(',');
				}
				else
				{
					addedIdPermission[0] = model.AddedIdPermission;
				}
			}

			if (addedIdPermission.Length > 0)
			{
				role.appRolePers = new List<AppRolePermission>();
				foreach (var item in addedIdPermission)
				{
					if (item != null)
					{
						var idPer = Convert.ToInt32(item);
						role.appRolePers.Add(new AppRolePermission
						{
							IdPermission = idPer
						});
					}
				}
			}
			role.Name = model.Name;
			role.Desc = model.Desc;



			_HotelDbContext.AppRole.Update(role);
			_HotelDbContext.SaveChanges();
			HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Login", "Account", new { area = "" });
		}


		public IActionResult Delete(int id)
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

			var role = _HotelDbContext.AppRole
								.Include(i => i.appUsers)
								.FirstOrDefault(i => i.Id == id);
			if (role == null)
			{
				TempData["ToastMessageWrg"] = "Đã xảy ra lỗi trong quá trình xử lí";
				return RedirectToAction("Index");
			}
			var listUser = _HotelDbContext.AppUser
					.Where(i => i.IdRole == role.Id)
					.ToList();
			DeleteRoleDTOs deleteRoloVM = new DeleteRoleDTOs();
			deleteRoloVM.Name = role.Name;
			deleteRoloVM.Desc = role.Desc;
			deleteRoloVM.CreateDate = (DateTime)role.CreatedDate!;
			deleteRoloVM.IdRole = id;
			deleteRoloVM.appUsers = new List<RoleDeleteVM_User>();

			foreach (var item in listUser)
			{
				var user = new RoleDeleteVM_User();
				user.IdUser = item.Id;
				user.Name = item.Name;
				deleteRoloVM.appUsers.Add(user);
			}
			ViewBag.ListRole = _HotelDbContext.AppRole.Where(x => x.IdGroup == IdGroup).ToList();

			return View(deleteRoloVM);
		}



		[HttpPost]
		public IActionResult Delete(DeleteRoleDTOs model, int id)
		{
			var role = _HotelDbContext.AppRole
								.Include(x => x.appRolePers)
								.FirstOrDefault(i => i.Id == id);
			if (role == null)
			{
				TempData["ToastMessageWrg"] = "Đã xảy ra lỗi trong quá trình xử lí";
				return RedirectToAction("Index");
			}


			// Cập nhật Role 
			var listUser = _HotelDbContext.AppUser
					.Where(i => i.IdRole == role.Id)
					.ToList();
			if (listUser.Count > 0 && listUser != null)
			{
				foreach (var item in listUser)
				{
					item.IdRole = model.IdNewRole;
					_HotelDbContext.AppUser.Update(item);
					_HotelDbContext.SaveChanges();
				}
			}

			// Xóa item Role ở bảng RolePermission  
			var RolePer = _HotelDbContext.AppRolePermissions
												.Where(i => i.IdRole == id)
												.ToList();
			if (RolePer.Count > 0 && RolePer != null)
			{
				foreach (var item in RolePer)
				{
					_HotelDbContext.AppRolePermissions.Remove(item);
					_HotelDbContext.SaveChanges();
				}
			}
			// xóa Role 
			_HotelDbContext.AppRole.Remove(role);
			_HotelDbContext.SaveChanges();
			SetSuccessMesg("Xóa vai trò thành công");
			return RedirectToAction("Index");
		}
	}
}
