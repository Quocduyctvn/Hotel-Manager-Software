using Hotel.Client.Areas.Admin.DTOs.BookingRoom;
using Hotel.Data;
using Hotel.Share.Const;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace Hotel.Client.Areas.Admin.Views.Components.AdBookingRoom
{
    [ViewComponent(Name = "AdBookingRoom")]
    public class AdBookingRoomViewComponent : ViewComponent
    {
        protected readonly ApplicationDbContext _HotelDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AdBookingRoomViewComponent(IHttpContextAccessor httpContextAccessor, ApplicationDbContext DbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _HotelDbContext = DbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Sử dụng _httpContextAccessor để lấy HttpContext và Claims
            var identity = (ClaimsIdentity)_httpContextAccessor.HttpContext?.User.Identity!;

            if (identity == null || !identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
            if (string.IsNullOrEmpty(IdGroupClaim))
            {
                throw new Exception("IdGroup claim is missing.");
            }
            int IdGroup = int.Parse(IdGroupClaim);

            var IdUserClaim = identity.FindFirst("IdUser")?.Value;
            if (string.IsNullOrEmpty(IdUserClaim))
            {
                throw new Exception("IdUser claim is missing.");
            }
            int IdUser = int.Parse(IdUserClaim);

            var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

            var bookingRoomCookies = GetFromCookie<AddBookingRoomDTOs>(CookieConst.KEY_COOKIES_BOOKING_ROOM) ?? new List<AddBookingRoomDTOs>();
            SaveToCookie(CookieConst.KEY_COOKIES_BOOKING_ROOM, bookingRoomCookies);

            var item = bookingRoomCookies.FirstOrDefault();

            TempData["Hotel"] = hotel;
            TempData.Keep("Hotel");

            TempData["Room"] = _HotelDbContext.AppRooms.Include(x => x.appRoomCate).FirstOrDefault(x => x.Id == item.IdRoom);
            TempData.Keep("Room");

            TempData["BookingRoom"] = _HotelDbContext.AppBookingRooms
                .Where(x => x.CheckInExpectual == item.CheckInExpectual && x.CheckOutExpectual == item.CheckOutExpectual)
                .FirstOrDefault();
            TempData.Keep("BookingRoom");

            TempData["User"] = _HotelDbContext.AppUser.FirstOrDefault(x => x.Id == IdUser);
            TempData.Keep("User");

            return View(item);
        }


        #region Add Cookies
        private List<T> GetFromCookie<T>(string cookieKey)
        {
            var cookieJson = _httpContextAccessor.HttpContext!.Request.Cookies[cookieKey];
            return string.IsNullOrEmpty(cookieJson) ? new List<T>() : JsonSerializer.Deserialize<List<T>>(cookieJson)!;
        }

        private void SaveToCookie<T>(string cookieKey, List<T> data)
        {
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(30) // Thiết lập thời gian sống của cookie là 30 ngày
            };

            var cookieJson = JsonSerializer.Serialize(data);
            _httpContextAccessor.HttpContext!.Response.Cookies.Append(cookieKey, cookieJson, options);
        }
        #endregion
    }
}
