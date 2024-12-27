using AutoMapper;
using Hotel.Client.Areas.Admin.DTOs.SettingTime;
using Hotel.Data;
using Hotel.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hotel.Client.Areas.Admin.Controllers
{
    public class AdminSettingTimeController : AdminControllerBase
    {
        public AdminSettingTimeController(ApplicationDbContext DbContext, IMapper mapper) : base(DbContext, mapper)
        {
        }

        public IActionResult Index()
        {
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
            var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
            int IdGroup = int.Parse(IdGroupClaim);
            var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

            var regularDays = _HotelDbContext.AppDateTypeWeeks
                                .Where(x => x.IdHotel == hotel.Id && x.IdDayType == 1)
                                .Select(x => x.WeekDay) // Select only the WeekDay values
                                .ToList();

            var weekendDays = _HotelDbContext.AppDateTypeWeeks
                                .Where(x => x.IdHotel == hotel.Id && x.IdDayType == 2)
                                .Select(x => x.WeekDay) // Select only the WeekDay values
                                .ToList();

            // Convert list of integers to a comma-separated string
            var times = new Times
            {
                IdDays = string.Join(",", regularDays), // Convert to comma-separated string
                IdHolidays = string.Join(",", weekendDays) // Convert to comma-separated string
            };
            return View(times);
        }

        [HttpPost]
        public IActionResult Update(Times model)
        {
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
            var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
            int IdGroup = int.Parse(IdGroupClaim);
            var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);


            // Chuyển đổi chuỗi CSV thành danh sách các số nguyên
            var regularDays = model.IdDays.Split(',').Select(int.Parse).ToList();
            var weekendDays = model.IdHolidays.Split(',').Select(int.Parse).ToList();

            // Kiểm tra trùng lặp giữa các ngày
            var overlappingDays = regularDays.Intersect(weekendDays).ToList();

            if (overlappingDays.Any())
            {
                SetErrorMesg("Lỗi: Ngày đầu tuần và ngày cuối tuần không được trùng nhau.");
                // Trả về lại View với thông báo lỗi
                return RedirectToAction("Index");
            }

            var listDay = _HotelDbContext.AppDateTypeWeeks.Where(x => x.IdHotel == hotel.Id).ToList();
            if (listDay.Any())
            {
                _HotelDbContext.AppDateTypeWeeks.RemoveRange(listDay);
                _HotelDbContext.SaveChanges();
            }
            foreach (var item in regularDays)
            {
                var day = new AppDateTypeWeek
                {
                    IdDayType = 1,
                    IdHotel = hotel.Id,
                    WeekDay = item
                };
                _HotelDbContext.Add(day);
                _HotelDbContext.SaveChanges();
            }

            foreach (var item in weekendDays)
            {
                var day = new AppDateTypeWeek
                {
                    IdDayType = 2,
                    IdHotel = hotel.Id,
                    WeekDay = item
                };
                _HotelDbContext.Add(day);
                _HotelDbContext.SaveChanges();
            }
            SetSuccessMesg("Cập nhật thành công");
            return RedirectToAction("Index");
        }
    }
}
