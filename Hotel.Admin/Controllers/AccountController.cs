using Hotel.Admin.DTOs;
using Hotel.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hotel.Admin.Controllers
{
	public class AccountController : ControllerBase
	{
		public AccountController(ApplicationDbContext DbContext) : base(DbContext)
		{
		}

		public IActionResult Login()
		{
			return View();
		}


		[HttpPost]
		public IActionResult Login(LoginDTOs model)
		{
			if (!ModelState.IsValid)
			{
				SetWrnMesg("Thông tin đăng nhập không hợp lệ");
				return View(model);
			}

			var user = _HotelDbContext.AppUser.FirstOrDefault(x => x.Phone == model.Phone);
			if (user == null)
			{
				SetErrorMesg("Thông tin đăng nhập không hợp lệ");
				return View(model);
			}
			var checkPass = BCrypt.Net.BCrypt.Verify(model?.Password, user.Password);

			// Nếu mật khẩu không đúng
			if (!checkPass)
			{
				SetErrorMesg("Thông tin đăng nhập không hợp lệ");
				return View(model);
			}

			var claims = new List<Claim>
							{
								new Claim("UserId", user.Id.ToString()),
								new Claim(ClaimTypes.Name, user.Name),
								new Claim(ClaimTypes.Email, user.Email),
								new Claim(ClaimTypes.HomePhone, user.Phone),
                                //claim - role động
                                new Claim(ClaimTypes.Role , user.IdRole.ToString())
							};
			var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
			HttpContext.SignInAsync(claimsPrincipal);
			return RedirectToAction("Index", "AdminHome", new { area = "Admin" });
		}
	}
}
