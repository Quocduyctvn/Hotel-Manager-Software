using Hotel.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hotel.Client.Areas.Admin.Views.Shared.Components.Permission
{
    public class PermissionViewComponent : ViewComponent
    {
        protected readonly ApplicationDbContext _HotelDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PermissionViewComponent(IHttpContextAccessor httpContextAccessor, ApplicationDbContext DbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _HotelDbContext = DbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Sử dụng _httpContextAccessor để lấy HttpContext và Claims
            var identity = (ClaimsIdentity)_httpContextAccessor.HttpContext?.User.Identity!;
            var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
            int IdGroup = int.Parse(IdGroupClaim);
            var IdUserClaim = identity.FindFirst("IdUser")?.Value;
            int IdUser = int.Parse(IdUserClaim);
            var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);


            var per = _HotelDbContext.AppPermissions
                            .Where(x => x.IdGroup == IdGroup)
                            .AsEnumerable()
                            .GroupBy(x => x.GroupName)
                            .ToList();

            return View(per);
        }
    }
}
