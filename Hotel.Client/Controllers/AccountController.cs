using Hotel.Client.DTOs;
using Hotel.Data;
using Hotel.Share.Const;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hotel.Client.Controllers
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
        public IActionResult Sigup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginDTOs model)
        {
            if (!ModelState.IsValid)
            {
                SetErrorMesg("Dữ liệu không hợp lệ!!");
                return View(model);
            }

            var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.Code == model.HtelCode);
            if (hotel == null)
            {
                SetErrorMesg("Thông tin đăng nhập không hợp lệ!");
                return View(model);
            }
            var group = _HotelDbContext.AppGroup.FirstOrDefault(x => x.Id == hotel.IdGroup);
            var user = _HotelDbContext.AppUser
                                .Include(x => x.appRole).ThenInclude(x => x.appRolePers)
                                .FirstOrDefault(x => x.Email == model.Email && x.IdGroup == hotel.IdGroup);
            if (user == null)
            {
                SetErrorMesg("Thông tin đăng nhập không hợp lệ!!");
                return View(model);
            }
            var checkPass = BCrypt.Net.BCrypt.Verify(model?.Pass, user.Password);
            // Nếu mật khẩu không đúng
            if (!checkPass)
            {
                SetErrorMesg("Thông tin đăng nhập không hợp lệ!!!");
                return View(model);
            }

            var per = string.Join(',', user.appRole.appRolePers.Select(rp => rp.IdPermission));
            var claims = new List<Claim>
                            {
                                new Claim("IdUser", user.Id.ToString()),
                                new Claim(ClaimTypes.Email, user.Email),
                                new Claim("IdGroup", user.IdGroup.ToString()),
                                //claim - role động
                                new Claim(ClaimTypes.Role , "admin"),
                                new Claim(AppClaimTypes.Permissions,  per),
                            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            HttpContext.SignInAsync(claimsPrincipal);
            return RedirectToAction("Index", "AdminHome", new { area = "Admin" });
        }
    }
}
