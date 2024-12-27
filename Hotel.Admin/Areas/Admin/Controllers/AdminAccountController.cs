using AutoMapper;
using Hotel.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Admin.Areas.Admin.Controllers
{
	public class AdminAccountController : AdminControllerBase
	{
		public AdminAccountController(ApplicationDbContext DbContext, IMapper mapper) : base(DbContext, mapper)
		{
		}

		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("", "", new { area = "" });
		}
	}
}
