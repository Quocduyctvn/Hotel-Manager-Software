using AutoMapper;
using Hotel.Client.Areas.Admin.DTOs.BookingRoom;
using Hotel.Client.Areas.Admin.DTOs.IncurredFee;
using Hotel.Client.Areas.Admin.Interface;
using Hotel.Client.Areas.Admin.PDF;
using Hotel.Data;
using Hotel.Data.Entities;
using Hotel.Share.Const;
using Hotel.Share.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using SelectPdf;
using System.Security.Claims;
using System.Text.Json;

namespace Hotel.Client.Areas.Admin.Controllers
{
    public class AdminBookingRoomController : AdminControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RazorViewToStringRenderer _razorViewToStringRenderer;
        private readonly IPdfService _pdfService;
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        public AdminBookingRoomController(ApplicationDbContext DbContext, IMapper mapper,
            IRazorViewEngine razorViewEngine, ITempDataProvider tempDataProvider,
            IPdfService pdfService, RazorViewToStringRenderer razorViewToStringRenderer, IHttpContextAccessor httpContextAccessor) : base(DbContext, mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _razorViewToStringRenderer = razorViewToStringRenderer;
            _pdfService = pdfService;
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
        }

        public async Task<IActionResult> Index(int IdRoom, RoomStatus? status)
        {
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
            var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
            int IdGroup = int.Parse(IdGroupClaim);
            var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

            var roomCates = _HotelDbContext.AppRoomCates
                                     .Where(x => x.IdHotel == hotel.Id)
                                     .Include(x => x.appRooms)
                                         .ThenInclude(x => x.appFloor)
                                     .Include(x => x.appImages)
                                     .Include(x => x.appRentalPrices)
                                     .ToList();
            foreach (var item in roomCates)
            {
                item.appRooms = item.appRooms
                                     .Where(room => room.Status != RoomStatus.IS_DELETED && room.Status != RoomStatus.INACTIVE && room.UseStartDate <= DateTime.Now)
                                     .ToList();
            }
            if (status != null)
            {
                foreach (var item in roomCates)
                {
                    item.appRooms = item.appRooms.Where(x => x.Status == status).ToList();
                }
            }
            //TempData["DateTypeWeek"] = _HotelDbContext.AppDateTypeWeeks
            //                                .Where(x => x.IdHotel == hotel.Id).Include(x => x.appDayType).ToList();
            //TempData["Holiday"] = _HotelDbContext.AppHolidays.Where(x => x.IdHotel == hotel.Id)
            //                                 .Include(x => x.appDayType).ToList();

            var currentDate = DateTime.Now;
            var currentWeekDay = (int)currentDate.DayOfWeek;
            var holiday = _HotelDbContext.AppHolidays
                            .Where(h => h.IdHotel == hotel.Id && h.StartDate <= currentDate && h.EndDate >= currentDate)
                            .FirstOrDefault();
            int? dayTypeId;

            if (holiday != null)
            {
                // Nếu là ngày lễ, lấy DayType từ AppHolidays
                dayTypeId = holiday.IdDayType;
            }
            else
            {
                // Nếu không phải ngày lễ, lấy DayType dựa trên ngày trong tuần từ AppDayTypes
                dayTypeId = _HotelDbContext.AppDateTypeWeeks
                    .Where(d => d.IdHotel == hotel.Id && d.WeekDay == currentWeekDay)
                    .Select(d => d.IdDayType)
                    .FirstOrDefault();
            }

            var bookingRooms = roomCates.Select(roomCate => new BookingRoomDTOs
            {
                Name = roomCate.Name,
                EarlyCheckInFee = roomCate.EarlyCheckInFee,
                LateCheckOutFee = roomCate.LateCheckOutFee,
                StanderAdult = roomCate.StanderAdult,
                StanderChildren = roomCate.StanderChildren,
                MaxAdult = roomCate.MaxAdult,
                MaxChildren = roomCate.MaxChildren,
                Position = roomCate.Position,
                Desc = roomCate.Desc,
                Status = roomCate.Status,
                IdHotel = roomCate.IdHotel,

                HourlyRate = _HotelDbContext.AppRentalPrices.Where(x => x.IdDayType == dayTypeId && x.IdRoomCate == roomCate.Id)
                                                                                .Select(x => x.Price).FirstOrDefault(),
                OvernightRate = _HotelDbContext.AppRentalPrices.Where(x => x.IdDayType == dayTypeId && x.IdRoomCate == roomCate.Id)
                                                                                .Select(x => x.Price).Skip(1).FirstOrDefault(),
                FullDayRate = _HotelDbContext.AppRentalPrices.Where(x => x.IdDayType == dayTypeId && x.IdRoomCate == roomCate.Id)
                                                                                .Select(x => x.Price).Skip(2).FirstOrDefault(),

                appHotels = roomCate.appHotels,
                appImages = roomCate.appImages,
                appRentalPrices = roomCate.appRentalPrices,
                appRooms = roomCate.appRooms,
                appRoomsCateAmenity = roomCate.appRoomsCateAmenity

            }).ToList();

            #region Mở popup (Nhận phòng) trả phòng..)
            TempData["Popup"] = _HotelDbContext.AppBookingRooms.Where(x => x.IdRoom == IdRoom && x.Status == BookingStatus.SELECTED)
                                                    .Include(x => x.appCustHotel)
                                                    .Include(x => x.appRoom)
                                                    .ThenInclude(x => x.appRoomCate)
                                                    .Include(x => x.appRentalPrice).FirstOrDefault();
            if (TempData["Action_CheckOut"] as string == "CheckOutPopup")
            {
                var bookingRoomJson = HttpContext.Session.GetString("CheckOutPopup");
                if (bookingRoomJson != null)
                {
                    ViewData["CheckOutPopup"] = Newtonsoft.Json.JsonConvert.DeserializeObject<AppBookingRoom>(bookingRoomJson);
                }
            }


            #endregion


            #region Xóa Cookies khi quay lại trang Index
            var cookies = Request.Cookies;
            // Lặp qua tất cả các cookie và xóa các cookie có liên quan
            foreach (var cookie in cookies)
            {
                if (cookie.Key.Contains(CookieConst.KEY_COOKIES_BOOKING_ROOM))
                {
                    // Xóa cookie
                    Response.Cookies.Delete(cookie.Key);
                }
            }
            #endregion

            return View(bookingRooms);
        }
        public async Task<IActionResult> OpenPdf(string path)
        {
            //var path = await ExportConfirmBookingRoomPdf();
            //ViewBag.Path = path;
            // Trả về view OpenPdf

            //string filePath = await ExportConfirmBookingRoomPdf();
            TempData["Path"] = path;
            return View("OpenPdf");
        }

        public async Task<IActionResult> OpenPdfCheckIn(string path)
        {
            //var path = await ExportConfirmBookingRoomPdf();
            //ViewBag.Path = path;
            // Trả về view OpenPdf

            //string filePath = await ExportConfirmCheckInPdf(id);
            TempData["Path"] = path;
            return View("OpenPdf");
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult BookingRoom(int? id)
        {
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity!; //IdUser
            var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
            int IdGroup = int.Parse(IdGroupClaim);
            var IdUserClaim = identity.FindFirst("IdUser")?.Value;
            int IdUser = int.Parse(IdUserClaim);
            var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

            var room = new AppRoom();
            var bookingRoomCookies = GetFromCookie<AddBookingRoomDTOs>(CookieConst.KEY_COOKIES_BOOKING_ROOM) ?? new List<AddBookingRoomDTOs>();
            SaveToCookie(CookieConst.KEY_COOKIES_BOOKING_ROOM, bookingRoomCookies);
            if (id != null && id > 0)
            {
                var item = bookingRoomCookies.FirstOrDefault(i => i.IdRoom == id);

                room = _HotelDbContext.AppRooms.Include(x => x.appRoomCate).FirstOrDefault(x => x.Id == id);

                if (item == null)
                {
                    #region Lấy ra ID loại ngày hiện tại (đầu tuần - cuối tuần - lễ)
                    var currentDate = DateTime.Now;
                    var currentWeekDay = (int)currentDate.DayOfWeek;
                    var holiday = _HotelDbContext.AppHolidays
                                    .Where(h => h.IdHotel == hotel.Id && h.StartDate <= currentDate && h.EndDate >= currentDate)
                                    .FirstOrDefault();
                    int? dayTypeId;

                    if (holiday != null)
                    {
                        // Nếu là ngày lễ, lấy DayType từ AppHolidays
                        dayTypeId = holiday.IdDayType;
                    }
                    else
                    {
                        // Nếu không phải ngày lễ, lấy DayType dựa trên ngày trong tuần từ AppDayTypes
                        dayTypeId = _HotelDbContext.AppDateTypeWeeks
                            .Where(d => d.IdHotel == hotel.Id && d.WeekDay == currentWeekDay)
                            .Select(d => d.IdDayType)
                            .FirstOrDefault();
                    }
                    #endregion

                    #region Lấy ra loại hình thuê
                    var rentalType = _HotelDbContext.AppRentalTypes.FirstOrDefault();
                    #endregion

                    var rentalPrice = _HotelDbContext.AppRentalPrices.Where(x => x.IdRoomCate == room.appRoomCate.Id
                                                                            && x.IdRentalType == rentalType.Id
                                                                            && x.IdDayType == dayTypeId).FirstOrDefault();
                    var booking = new AddBookingRoomDTOs();
                    booking.RoomCateName = room.appRoomCate.Name;
                    booking.IdRoom = room.Id;
                    booking.IdUser = IdUser;
                    booking.IdRentalPrice = rentalPrice.Id;
                    booking.IdRentalType = rentalType.Id;
                    booking.CheckInExpectual = DateTime.Now;
                    booking.CheckOutExpectual = DateTime.Now.AddHours(1);
                    booking.RoomPrice = rentalPrice.Price;
                    booking.Desc = "";
                    booking.Adult = 1;
                    booking.Children = 0;
                    // khách tối da dựa trên hạng phòng
                    booking.MaxAdult = room.appRoomCate.MaxAdult;
                    booking.MaxChildren = room.appRoomCate.MaxChildren;
                    booking.StanderAdult = room.appRoomCate.StanderAdult;
                    booking.StanderChildren = room.appRoomCate.StanderChildren;

                    //cookies.Clear();
                    _httpContextAccessor.HttpContext!.Response.Cookies.Delete(CookieConst.KEY_COOKIES_BOOKING_ROOM);
                    bookingRoomCookies.Add(booking);
                    SaveToCookie(CookieConst.KEY_COOKIES_BOOKING_ROOM, bookingRoomCookies);
                }


                SaveToCookie(CookieConst.KEY_COOKIES_BOOKING_ROOM, bookingRoomCookies);
                // Lưu dữ liệu vào TempData (trong Controller)

            }
            if (id != null && id > 0)
            {
                var rooms = _HotelDbContext.AppRooms.Include(x => x.appRoomCate).FirstOrDefault(x => x.Id == id);
                if (rooms != null)
                {
                    TempData["Room"] = _HotelDbContext.AppRooms
                        .AsNoTracking()
                        .Where(x => x.IdRoomCate == rooms.appRoomCate.Id && x.Status != RoomStatus.IS_DELETED && x.UseStartDate < DateTime.Now)
                        .ToList();
                }
            }
            TempData["RentalType"] = _HotelDbContext.AppRentalTypes.ToList();
            return View(bookingRoomCookies.FirstOrDefault());
        }

        public async Task<IActionResult> ConfirmBooking()
        {
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity!; //IdUser
            var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
            int IdGroup = int.Parse(IdGroupClaim);
            var IdUserClaim = identity.FindFirst("IdUser")?.Value;
            int IdUser = int.Parse(IdUserClaim);
            var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);


            var bookingRoomCookies = GetFromCookie<AddBookingRoomDTOs>(CookieConst.KEY_COOKIES_BOOKING_ROOM) ?? new List<AddBookingRoomDTOs>();
            SaveToCookie(CookieConst.KEY_COOKIES_BOOKING_ROOM, bookingRoomCookies);
            if (bookingRoomCookies.FirstOrDefault() != null)
            {
                var item = bookingRoomCookies.FirstOrDefault();

                using (var transaction = _HotelDbContext.Database.BeginTransaction())
                {
                    try
                    {
                        // Kiểm tra lần cuối - xem có phòng nào đang đc sử dụng hay không (Status SELECTED~đang chờ-đangSd- đang quá giờ
                        var checkBooking = _HotelDbContext.AppBookingRooms.Where(x => x.Status == BookingStatus.SELECTED && x.IdRoom == item.IdRoom).FirstOrDefault();
                        if (checkBooking != null)
                        {
                            SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
                            return RedirectToAction("BookingRoom", new { id = item.IdRoom });
                        }

                        var customers = _HotelDbContext.AppCustHotels.Where(x => x.IdHotel == hotel.Id).ToList();
                        var customer = new AppCustHotel();
                        customer.appImages = new List<AppImage>();
                        customer.Code = "KH0" + (customers.Count + 1);
                        customer.Name = item.CusName;
                        customer.BirthDay = item.BirthDay;
                        customer.Email = item.Email;
                        customer.Phone = item.Phone;
                        customer.Country = item.Country;
                        customer.Address = item.Address;
                        customer.Desc = item.CusDesc;
                        customer.IdHotel = hotel.Id;
                        if (item.stringFile1 != null)
                        {
                            customer.appImages.Add(new AppImage
                            {
                                Path = item.stringFile1,
                            });
                        }
                        if (item.stringFile2 != null)
                        {
                            customer.appImages.Add(new AppImage
                            {
                                Path = item.stringFile2,
                            });
                        }
                        _HotelDbContext.Add(customer);
                        _HotelDbContext.SaveChanges();


                        #region Tạo bảng booking_room && Cập nhậtj hàng phòng
                        var room = _HotelDbContext.AppRooms.FirstOrDefault(x => x.Id == item.IdRoom);
                        room.Status = RoomStatus.CHECKING_IN;
                        _HotelDbContext.Update(room);
                        _HotelDbContext.SaveChanges();


                        var booking = new AppBookingRoom();
                        booking.CheckInExpectual = item.CheckInExpectual;
                        booking.CheckOutExpectual = item.CheckOutExpectual;
                        booking.Status = BookingStatus.SELECTED;
                        booking.CreatedDate = DateTime.Now;
                        booking.Adult = item.Adult;
                        booking.Children = item.Children;
                        booking.Price = item.RoomPrice;
                        booking.IdCustommer = customer.Id;
                        booking.IdRentalPrice = item.IdRentalPrice;
                        booking.IdRoom = item.IdRoom;
                        booking.IdUser = IdUser;
                        _HotelDbContext.Add(booking);
                        _HotelDbContext.SaveChanges();


                        string filePath = await ExportConfirmBookingRoomPdf();

                        booking.Code = "DP0" + booking.Id;
                        booking.BookingConfirm = filePath;
                        _HotelDbContext.Update(booking); // Update giá trị mới của booking
                        _HotelDbContext.SaveChanges();
                        #endregion



                        transaction.Commit();
                        return RedirectToAction("OpenPdf", new { path = filePath });
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction nếu có lỗi xảy ra
                        transaction.Rollback();
                        SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
                        return RedirectToAction("BookingRoom", new { id = item.IdRoom });
                    }
                }

            }
            SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
            return RedirectToAction("BookingRoom");
        }

        public async Task<IActionResult> CheckIn(int id)
        {
            if (id <= 0)
            {
                SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
                return RedirectToAction("Index");
            }

            // cập nhật thoigw gian nhạn phòng thực tế 
            var booking = _HotelDbContext.AppBookingRooms.FirstOrDefault(x => x.Id == id);
            if (booking == null)
            {
                SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
                return RedirectToAction("Index");
            }
            string filePath = await ExportConfirmCheckInPdf(id);
            booking.CheckinConfirm = filePath;
            booking.CheckInActual = DateTime.Now;
            _HotelDbContext.Update(booking);
            _HotelDbContext.SaveChanges();

            // Cập nhật trạng thái phòng sang đang sử dụng 
            var room = _HotelDbContext.AppRooms.FirstOrDefault(x => x.Id == booking.IdRoom);
            if (room == null)
            {
                SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
                return RedirectToAction("Index");
            }
            room.Status = RoomStatus.OCCUPIED;
            _HotelDbContext.Update(room);
            _HotelDbContext.SaveChanges();

            return RedirectToAction("OpenPdfCheckIn", new { path = filePath });
        }

        public IActionResult CheckOut(int id)
        {
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity!; //IdUser
            var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
            int IdGroup = int.Parse(IdGroupClaim);
            var IdUserClaim = identity.FindFirst("IdUser")?.Value;
            int IdUser = int.Parse(IdUserClaim);
            var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

            var SvcCommo = _HotelDbContext.AppSvcCommoCates.Where(x => x.IdHotel == hotel.Id).ToList();
            var svc = SvcCommo.FirstOrDefault();
            var Commo = SvcCommo.Skip(1).FirstOrDefault();
            TempData["Services"] = _HotelDbContext.AppServices.Include(x => x.appImages)
                                            .Where(x => x.IdSvcCommocate == svc.Id && x.Status == ServicesStatus.ACTIVE).ToList();
            TempData["Commodity"] = _HotelDbContext.AppCommodities.Include(x => x.appImages)
                                            .Where(x => x.IdSvcCommoCate == Commo.Id && x.Status == CommodityStatus.ACTIVE && x.Stock > 0).ToList();

            TempData["BookingRooms"] = _HotelDbContext.AppBookingRooms
                    .Where(x => x.Id == id && x.Status == BookingStatus.SELECTED)
                    .Include(x => x.appCustHotel)
                    .Include(x => x.appRoom)
                    .ThenInclude(x => x.appRoomCate)
                    .Include(x => x.appRentalPrice)
                    .ThenInclude(x => x.appRentalType)
                    .FirstOrDefault();

            TempData["SvcOrder"] = _HotelDbContext.AppServicesOrders
                                                    .Where(x => x.IdBookingRoom == id)
                                                    .Include(x => x.appServices).ToList();
            TempData["CommoOrder"] = _HotelDbContext.AppComodityOrders
                                                                .Where(x => x.IdBookingRoom == id)
                                                                .Include(x => x.appCommodity).ToList();
            TempData["PTNS"] = _HotelDbContext.AppIncurredFee
                                        .Where(x => x.IdBookingRoom == id && x.Name == "PTNS").FirstOrDefault();
            TempData["PTTM"] = _HotelDbContext.AppIncurredFee
                                        .Where(x => x.IdBookingRoom == id && x.Name == "PTTM").FirstOrDefault();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(int id, string discountPrice)
        {
            var bill = _HotelDbContext.AppBill.FirstOrDefault(x => x.IdBooking == id);
            if (bill == null)
            {

                var booking = _HotelDbContext.AppBookingRooms
                    .Where(x => x.Id == id)
                    .Include(x => x.appCustHotel)
                    .Include(x => x.appRoom)
                    .ThenInclude(x => x.appRoomCate)
                    .Include(x => x.appRentalPrice)
                    .ThenInclude(x => x.appRentalType)
                    .Include(x => x.appServicesOrders)
                    .Include(x => x.appComodityOrders)
                    .FirstOrDefault();
                booking.CheckOutActual = DateTime.Now;
                booking.Status = BookingStatus.SUCCESS;
                _HotelDbContext.Update(booking);
                _HotelDbContext.SaveChanges();

                var room = _HotelDbContext.AppRooms.FirstOrDefault(x => x.Id == booking.IdRoom);
                room.Status = RoomStatus.AVAILABLE;
                _HotelDbContext.Update(room);
                _HotelDbContext.SaveChanges();


                double TotalPrice = booking.Price;
                foreach (var item in booking.appComodityOrders)
                {
                    TotalPrice += item.Price;
                }
                foreach (var item in booking.appServicesOrders)
                {
                    TotalPrice += item.Price;
                }

                var appBill = new AppBill();
                appBill.Desc = "Thanh toán ngày " + DateTime.Now;
                appBill.RoomPrice = TotalPrice;
                appBill.DiscountPrice = double.TryParse(discountPrice, out double parsedDiscount) ? parsedDiscount : 0;
                appBill.FinalPrice = TotalPrice - appBill.DiscountPrice;
                appBill.CreatedDate = DateTime.Now;
                appBill.IdBooking = id;
                _HotelDbContext.Add(appBill); // Update giá trị mới của booking
                _HotelDbContext.SaveChanges();

                string filePath = await ExportCheckOutPdf(id);

                appBill.Path = filePath;
                _HotelDbContext.Update(appBill); // Update giá trị mới của booking
                _HotelDbContext.SaveChanges();

                booking.CheckoutConfirm = filePath;
                _HotelDbContext.Update(booking);
                _HotelDbContext.SaveChanges();

                return RedirectToAction("OpenPdf", new { path = filePath });
            }
            return View(id);
        }

        public IActionResult AddSvcCommo(int? SvcId, int? CommoId, int bookingId)
        {
            if (SvcId <= 0)
            {
                SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
                return RedirectToAction("CheckOut", new { id = bookingId });
            }
            if (SvcId > 0)
            {
                var svcOrder = _HotelDbContext.AppServicesOrders
                                                .Where(x => x.IdServices == SvcId && x.IdBookingRoom == bookingId)
                                                .FirstOrDefault();
                var svc = _HotelDbContext.AppServices.FirstOrDefault(x => x.Id == SvcId);
                if (svcOrder == null)
                {
                    var newSvc = new AppServicesOrder();
                    newSvc.IdServices = (int)SvcId!;
                    newSvc.IdBookingRoom = bookingId;
                    newSvc.Quantity = 1;
                    newSvc.Price = (double)svc.Price;
                    newSvc.CreatedDate = DateTime.Now;
                    _HotelDbContext.Add(newSvc);
                    _HotelDbContext.SaveChanges();
                }
                else
                {
                    svcOrder.Price = (double)((svcOrder.Quantity + 1) * svc.Price);
                    svcOrder.Quantity = svcOrder.Quantity + 1;
                    _HotelDbContext.Update(svcOrder);
                    _HotelDbContext.SaveChanges();
                }
            }
            if (CommoId <= 0)
            {
                SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
                return RedirectToAction("CheckOut", new { id = bookingId });
            }
            if (CommoId > 0)
            {
                var commoOrder = _HotelDbContext.AppComodityOrders
                                                .Where(x => x.IdCommodities == CommoId && x.IdBookingRoom == bookingId)
                                                .FirstOrDefault();
                var commo = _HotelDbContext.AppCommodities.FirstOrDefault(x => x.Id == CommoId);
                // kiêm tra hạng tồn tối thỉu 1 
                if (commo.Stock >= 1)
                {
                    if (commoOrder == null)
                    {
                        var newCommo = new AppComodityOrder();
                        newCommo.IdBookingRoom = bookingId;
                        newCommo.IdCommodities = commo.Id;
                        newCommo.Quantity = 1;
                        newCommo.Price = (double)commo.SellingPrice; // giá đã giảm
                        newCommo.CreatedDate = DateTime.Now;
                        _HotelDbContext.Add(newCommo);
                        _HotelDbContext.SaveChanges();
                    }
                    else
                    {
                        commoOrder.Price = (double)((commoOrder.Quantity + 1) * commo.SellingPrice);
                        commoOrder.Quantity = commoOrder.Quantity + 1;
                        _HotelDbContext.Update(commoOrder);
                        _HotelDbContext.SaveChanges();
                    }
                    // giảm tồn kho ( -1 ) 
                    commo.Stock -= 1;
                    _HotelDbContext.Update(commo);
                    _HotelDbContext.SaveChanges();
                }
                else
                {
                    SetWrnMesg("Sản phẫm đã hết hàng - vui lòng chọn mặt hàng khác");
                    return RedirectToAction("CheckOut", new { id = bookingId });
                }
            }

            return RedirectToAction("CheckOut", new { id = bookingId });
        }

        #region Tăng - Giảm Services -- Commo 
        public IActionResult PlusServices(int id, int OrderId)
        {
            if (id <= 0)
            {
                SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
                return RedirectToAction("Index");
            }
            if (OrderId <= 0)
            {
                SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
                return RedirectToAction("Index");
            }
            var svcOrder = _HotelDbContext.AppServicesOrders.FirstOrDefault(x => x.Id == OrderId);
            var svc = _HotelDbContext.AppServices.FirstOrDefault(x => x.Id == svcOrder.IdServices);

            if (svcOrder != null)
            {
                svcOrder.Price = (double)((svcOrder.Quantity + 1) * svc.Price);
                svcOrder.Quantity = svcOrder.Quantity + 1;
                _HotelDbContext.Update(svcOrder);
                _HotelDbContext.SaveChanges();
            }
            return RedirectToAction("CheckOut", new { id = id });
        }

        public IActionResult SubServices(int id, int OrderId)
        {
            if (id <= 0)
            {
                SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
                return RedirectToAction("Index");
            }
            if (OrderId <= 0)
            {
                SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
                return RedirectToAction("Index");
            }
            var svcOrder = _HotelDbContext.AppServicesOrders.FirstOrDefault(x => x.Id == OrderId);
            var svc = _HotelDbContext.AppServices.FirstOrDefault(x => x.Id == svcOrder.IdServices);
            if (svcOrder.Quantity == 1)
            {
                return RedirectToAction("CheckOut", new { id = id });
            }
            if (svcOrder != null)
            {
                svcOrder.Price = (double)((svcOrder.Quantity - 1) * svc.Price);
                svcOrder.Quantity = svcOrder.Quantity - 1;
                _HotelDbContext.Update(svcOrder);
                _HotelDbContext.SaveChanges();
            }
            return RedirectToAction("CheckOut", new { id = id });
        }

        public IActionResult PlusCommo(int id, int OrderId)
        {
            if (id <= 0)
            {
                SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
                return RedirectToAction("Index");
            }
            if (OrderId <= 0)
            {
                SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
                return RedirectToAction("Index");
            }
            var CommoOrder = _HotelDbContext.AppComodityOrders.FirstOrDefault(x => x.Id == OrderId);
            var commo = _HotelDbContext.AppCommodities.FirstOrDefault(x => x.Id == CommoOrder.IdCommodities);
            if (commo.Stock <= 0)
            {
                SetErrorMesg("Sản phẫm đã hết - Vui lòng chọn mặt hàng khác");
                return RedirectToAction("CheckOut", new { id = id });
            }

            if (CommoOrder != null)
            {
                CommoOrder.Price = (double)((CommoOrder.Quantity + 1) * commo.SellingPrice);
                CommoOrder.Quantity = CommoOrder.Quantity + 1;
                _HotelDbContext.Update(CommoOrder);
                _HotelDbContext.SaveChanges();

                commo.Stock -= 1;
                _HotelDbContext.Update(commo);
                _HotelDbContext.SaveChanges();
            }
            return RedirectToAction("CheckOut", new { id = id });
        }

        public IActionResult SubCommo(int id, int OrderId)
        {
            if (id <= 0)
            {
                SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
                return RedirectToAction("Index");
            }
            if (OrderId <= 0)
            {
                SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
                return RedirectToAction("Index");
            }
            var CommoOrder = _HotelDbContext.AppComodityOrders.FirstOrDefault(x => x.Id == OrderId);
            var svc = _HotelDbContext.AppCommodities.FirstOrDefault(x => x.Id == CommoOrder.IdCommodities);
            if (CommoOrder.Quantity == 1)
            {
                return RedirectToAction("CheckOut", new { id = id });
            }
            if (CommoOrder != null)
            {
                CommoOrder.Price = (double)((CommoOrder.Quantity - 1) * svc.SellingPrice);
                CommoOrder.Quantity = CommoOrder.Quantity - 1;
                _HotelDbContext.Update(CommoOrder);
                _HotelDbContext.SaveChanges();

                svc.Stock += 1;
                _HotelDbContext.Update(svc);
                _HotelDbContext.SaveChanges();
            }
            return RedirectToAction("CheckOut", new { id = id });
        }

        public IActionResult DeleteSvcOrder(int id, int OrderId)
        {
            var svcOrder = _HotelDbContext.AppServicesOrders.FirstOrDefault(x => x.Id == OrderId);
            var svc = _HotelDbContext.AppServices.FirstOrDefault(x => x.Id == svcOrder.IdServices);

            var quantity = svcOrder.Quantity;
            if (svcOrder != null)
            {
                _HotelDbContext.Remove(svcOrder);
                _HotelDbContext.SaveChanges();
            }
            return RedirectToAction("CheckOut", new { id = id });
        }

        public IActionResult DeleteCommoOrder(int id, int OrderId)
        {
            var commoOrder = _HotelDbContext.AppComodityOrders.FirstOrDefault(x => x.Id == OrderId);
            var commo = _HotelDbContext.AppCommodities.FirstOrDefault(x => x.Id == commoOrder.IdCommodities);

            var quantity = commoOrder.Quantity;
            if (commoOrder != null)
            {
                _HotelDbContext.Remove(commoOrder);
                _HotelDbContext.SaveChanges();

                commo.Stock += quantity;
                _HotelDbContext.Update(commo);
                _HotelDbContext.SaveChanges();
            }
            return RedirectToAction("CheckOut", new { id = id });
        }

        // Phụ thu 
        public IActionResult IncurredFee([FromBody] IncurredFeeRequest request)
        {
            if (request.Type == 1)
            {
                // Xử lý logic cho "Phụ thu vào sớm"
                if (request.IsChecked == true)
                {
                    var booking = _HotelDbContext.AppBookingRooms
                                                .Include(x => x.appRoom)
                                                .ThenInclude(x => x.appRoomCate)
                                                .Include(x => x.appRentalPrice)
                                                .FirstOrDefault(x => x.Id == request.BookingId);
                    var check = _HotelDbContext.AppIncurredFee
                                        .Where(x => x.IdBookingRoom == request.BookingId && x.Name == "PTNS").FirstOrDefault();
                    if (check == null)
                    {
                        var incurred = new AppIncurredFee();
                        if (booking.CheckInExpectual.HasValue && booking.CheckInActual.HasValue)
                        {
                            if (booking.CheckInActual.Value < booking.CheckInExpectual)
                            {
                                incurred.IdBookingRoom = request.BookingId;
                                incurred.Name = "PTNS"; // Phụ thu nhận sớm
                                var timeSpan = booking.CheckInExpectual.Value - booking.CheckInActual.Value;

                                // Nếu có >= 30 phút, làm tròn giờ lên
                                var roundedHours = (int)Math.Ceiling(timeSpan.TotalHours);

                                incurred.Quantity = roundedHours;
                                incurred.Price = booking.appRoom.appRoomCate.EarlyCheckInFee * incurred.Quantity;
                                incurred.CreatedDate = DateTime.Now;
                                _HotelDbContext.Add(incurred);
                                _HotelDbContext.SaveChanges();
                            }
                        }
                    }
                }
                else
                {
                    var incurred = _HotelDbContext.AppIncurredFee
                                        .Where(x => x.IdBookingRoom == request.BookingId && x.Name == "PTNS").FirstOrDefault();
                    _HotelDbContext.Remove(incurred);
                    _HotelDbContext.SaveChanges();
                }
            }
            else if (request.Type == 2)
            {
                if (request.IsChecked == true)
                {
                    var booking = _HotelDbContext.AppBookingRooms
                                                .Include(x => x.appRoom)
                                                .ThenInclude(x => x.appRoomCate)
                                                .Include(x => x.appRentalPrice)
                                                .FirstOrDefault(x => x.Id == request.BookingId);
                    var check = _HotelDbContext.AppIncurredFee
                                        .Where(x => x.IdBookingRoom == request.BookingId && x.Name == "PTTM").FirstOrDefault();
                    if (check == null)
                    {
                        var incurred = new AppIncurredFee();
                        if (booking.CheckOutExpectual.HasValue && booking.CheckOutExpectual < DateTime.Now)
                        {
                            incurred.IdBookingRoom = request.BookingId;
                            incurred.Name = "PTTM"; // Phụ thu nhận sớm
                            var timeSpan = DateTime.Now - booking.CheckInExpectual.Value;

                            // Nếu có >= 30 phút, làm tròn giờ lên
                            var roundedHours = (int)Math.Ceiling(timeSpan.TotalHours);

                            incurred.Quantity = roundedHours;
                            incurred.Price = booking.appRoom.appRoomCate.LateCheckOutFee * incurred.Quantity;
                            incurred.CreatedDate = DateTime.Now;
                            _HotelDbContext.Add(incurred);
                            _HotelDbContext.SaveChanges();
                        }
                    }
                }
                else
                {
                    var incurred = _HotelDbContext.AppIncurredFee
                                        .Where(x => x.IdBookingRoom == request.BookingId && x.Name == "PTTM").FirstOrDefault();
                    _HotelDbContext.Remove(incurred);
                    _HotelDbContext.SaveChanges();
                }
            }
            return Ok(new { message = "Xử lý thành công" });
        }
        #endregion

        // mở popup lấy thông tin phòng đang đc sử dụng
        public IActionResult CheckOutPopup(int id)
        {
            var bookingRoom = _HotelDbContext.AppBookingRooms
                    .Where(x => x.IdRoom == id && x.Status == BookingStatus.SELECTED)
                    .Include(x => x.appCustHotel)
                    .Include(x => x.appRoom)
                    .ThenInclude(x => x.appRoomCate)
                    .Include(x => x.appRentalPrice)
                    .FirstOrDefault();

            if (bookingRoom != null)
            {
                var bookingRoomJson = Newtonsoft.Json.JsonConvert.SerializeObject(bookingRoom, new Newtonsoft.Json.JsonSerializerSettings
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

                HttpContext.Session.SetString("CheckOutPopup", bookingRoomJson);
            }
            TempData["Action_CheckOut"] = "CheckOutPopup";

            return RedirectToAction("Index");
        }

        public IActionResult CancelBookingRoom(int id)
        {
            if (id <= 0)
            {
                SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
                return RedirectToAction("Index");
            }
            var booking = _HotelDbContext.AppBookingRooms.FirstOrDefault(x => x.Id == id);
            if (booking == null)
            {
                SetErrorMesg("Đã xảy ra lỗi trong quá trình xử lí");
                return RedirectToAction("Index");
            }
            booking.Status = BookingStatus.CANCEL;
            _HotelDbContext.Update(booking);
            _HotelDbContext.SaveChanges();

            var room = _HotelDbContext.AppRooms.FirstOrDefault(x => x.Id == booking.IdRoom);
            room.Status = RoomStatus.AVAILABLE;
            _HotelDbContext.Update(room);
            _HotelDbContext.SaveChanges();
            SetSuccessMesg("Hủy đặt Phòng thành công");
            return RedirectToAction("Index");
        }

        #region Nhóm Update booking_room by Cookies 
        [HttpPost]
        public IActionResult UpdateRentalType([FromBody] UpdateRentalTypeDTOs request)
        {
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity!; //IdUser
            var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
            int IdGroup = int.Parse(IdGroupClaim);
            var IdUserClaim = identity.FindFirst("IdUser")?.Value;
            int IdUser = int.Parse(IdUserClaim);
            var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

            var cookies = GetFromCookie<AddBookingRoomDTOs>(CookieConst.KEY_COOKIES_BOOKING_ROOM);
            if (request.RentalTypeId > 0 && request.RoomId > 0)
            {
                var room = _HotelDbContext.AppRooms.Include(x => x.appRoomCate).FirstOrDefault(x => x.Id == request.RoomId);
                var item = cookies.FirstOrDefault(i => i.IdRoom == request.RoomId);
                if (item != null && room != null)
                {

                    #region Lấy ra ID loại ngày hiện tại (đầu tuần - cuối tuần - lễ)
                    var currentDate = DateTime.Now;
                    var currentWeekDay = (int)currentDate.DayOfWeek;
                    var holiday = _HotelDbContext.AppHolidays
                                    .Where(h => h.IdHotel == hotel.Id && h.StartDate <= currentDate && h.EndDate >= currentDate)
                                    .FirstOrDefault();
                    int? dayTypeId;

                    if (holiday != null)
                    {
                        // Nếu là ngày lễ, lấy DayType từ AppHolidays
                        dayTypeId = holiday.IdDayType;
                    }
                    else
                    {
                        // Nếu không phải ngày lễ, lấy DayType dựa trên ngày trong tuần từ AppDayTypes
                        dayTypeId = _HotelDbContext.AppDateTypeWeeks
                            .Where(d => d.IdHotel == hotel.Id && d.WeekDay == currentWeekDay)
                            .Select(d => d.IdDayType)
                            .FirstOrDefault();
                    }
                    #endregion
                    var rentalPrice = _HotelDbContext.AppRentalPrices.Where(x => x.IdRentalType == request.RentalTypeId
                                                    && x.IdRoomCate == room.appRoomCate.Id && x.IdDayType == dayTypeId).FirstOrDefault();
                    item.RoomPrice = rentalPrice!.Price;
                    item.IdRentalType = rentalPrice.IdRentalType;
                    item.IdRentalPrice = rentalPrice.Id;
                    item.CheckInExpectual = DateTime.Now;
                    if (rentalPrice.IdRentalType == 1)
                    {
                        item.CheckOutExpectual = DateTime.Now.AddHours(1);
                    }
                    else if (rentalPrice.IdRentalType == 2)
                    {
                        item.CheckOutExpectual = DateTime.Now.AddHours(12);
                    }
                    else
                    {
                        item.CheckOutExpectual = DateTime.Now.AddHours(22);
                    }
                    SaveToCookie(CookieConst.KEY_COOKIES_BOOKING_ROOM, cookies);
                }
            }
            return Ok();
        }

        [HttpPost]
        public IActionResult UpdateCheckInDate([FromBody] CheckInTimeDTOs request)
        {
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity!; //IdUser
            var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
            int IdGroup = int.Parse(IdGroupClaim);
            var IdUserClaim = identity.FindFirst("IdUser")?.Value;
            int IdUser = int.Parse(IdUserClaim);
            var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

            var cookies = GetFromCookie<AddBookingRoomDTOs>(CookieConst.KEY_COOKIES_BOOKING_ROOM);
            if (request.RoomId > 0)
            {
                var room = _HotelDbContext.AppRooms.Include(x => x.appRoomCate).FirstOrDefault(x => x.Id == request.RoomId);
                var item = cookies.FirstOrDefault(i => i.IdRoom == request.RoomId);
                if (item != null && room != null)
                {
                    TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // Múi giờ UTC+7
                    DateTime localCheckInTime = TimeZoneInfo.ConvertTimeFromUtc(request.CheckInTime, vnTimeZone);

                    #region Lấy ra ID loại ngày hiện tại (đầu tuần - cuối tuần - lễ)
                    var currentDate = localCheckInTime;
                    var currentWeekDay = (int)currentDate.DayOfWeek;
                    var holiday = _HotelDbContext.AppHolidays
                                    .Where(h => h.IdHotel == hotel.Id && h.StartDate <= currentDate && h.EndDate >= currentDate)
                                    .FirstOrDefault();
                    int? dayTypeId;

                    if (holiday != null)
                    {
                        // Nếu là ngày lễ, lấy DayType từ AppHolidays
                        dayTypeId = holiday.IdDayType;
                    }
                    else
                    {
                        // Nếu không phải ngày lễ, lấy DayType dựa trên ngày trong tuần từ AppDayTypes
                        dayTypeId = _HotelDbContext.AppDateTypeWeeks
                            .Where(d => d.IdHotel == hotel.Id && d.WeekDay == currentWeekDay)
                            .Select(d => d.IdDayType)
                            .FirstOrDefault();
                    }
                    #endregion

                    var rentalPrice = _HotelDbContext.AppRentalPrices.Where(x => x.IdRentalType == item.IdRentalType
                                                    && x.IdRoomCate == room.appRoomCate.Id && x.IdDayType == dayTypeId).FirstOrDefault();
                    var timeDifference = item.CheckOutExpectual!.Value - localCheckInTime;

                    // Tính số ngày, giờ, và phút
                    int days = timeDifference.Days; // Số ngày
                    int hours = timeDifference.Hours; // Số giờ
                    int minutes = timeDifference.Minutes; // Số phút


                    if (rentalPrice.IdRentalType == 1)
                    {
                        item.CheckOutExpectual = localCheckInTime.AddHours(1);
                        item.RoomPrice = rentalPrice!.Price;
                        //if (item.CheckOutExpectual.HasValue)
                        //{
                        //	// Tính số giờ thuê
                        //	var rentalHours = (item.CheckOutExpectual.Value - localCheckInTime).TotalHours;

                        //	// Kiểm tra nếu thời gian thuê dưới 1 giờ
                        //	if (rentalHours < 0)
                        //	{
                        //		SetErrorMesg("Tổng thời gian thuê tối thiểu là 1 giờ");
                        //		return Ok();
                        //	}
                        //}
                    }
                    if (rentalPrice.IdRentalType == 2)
                    {
                        item.CheckOutExpectual = localCheckInTime.AddHours(12);
                        item.RoomPrice = rentalPrice!.Price;
                    }
                    if (rentalPrice.IdRentalType == 3)
                    {
                        item.CheckOutExpectual = localCheckInTime.AddHours(22);
                        item.RoomPrice = rentalPrice!.Price;
                    }

                    item.CheckInExpectual = localCheckInTime;
                    SaveToCookie(CookieConst.KEY_COOKIES_BOOKING_ROOM, cookies);
                }
                return Ok(new { Price = item.RoomPrice });
            }
            return BadRequest("Không tìm thấy phòng hoặc dữ liệu không hợp lệ.");
        }

        [HttpPost]
        public IActionResult UpdateCheckOutDate([FromBody] CheckOutTimeDTOs request)
        {
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity!; //IdUser
            var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
            int IdGroup = int.Parse(IdGroupClaim);
            var IdUserClaim = identity.FindFirst("IdUser")?.Value;
            int IdUser = int.Parse(IdUserClaim);
            var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

            var cookies = GetFromCookie<AddBookingRoomDTOs>(CookieConst.KEY_COOKIES_BOOKING_ROOM);
            if (request.RoomId > 0)
            {
                var room = _HotelDbContext.AppRooms.Include(x => x.appRoomCate).FirstOrDefault(x => x.Id == request.RoomId);
                var item = cookies.FirstOrDefault(i => i.IdRoom == request.RoomId);
                if (item != null && room != null)
                {
                    TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // Múi giờ UTC+7
                    DateTime localCheckOutTime = TimeZoneInfo.ConvertTimeFromUtc(request.CheckOutTime, vnTimeZone);


                    var rentalPrice = _HotelDbContext.AppRentalPrices.Where(x => x.IdRentalType == item.IdRentalType
                                                    && x.IdRoomCate == room.appRoomCate.Id && x.Price == item.RoomPrice).FirstOrDefault();
                    var timeDifference = localCheckOutTime - item.CheckInExpectual!.Value;

                    // Tính số ngày, giờ, và phút
                    int days = timeDifference.Days; // Số ngày
                    int hours = timeDifference.Hours; // Số giờ
                    int minutes = timeDifference.Minutes; // Số phút


                    if (rentalPrice.IdRentalType == 1)
                    {
                        item.CheckOutExpectual = localCheckOutTime;
                        if (minutes >= 30)
                        {
                            hours += 1;
                        }
                        item.RoomPrice = rentalPrice!.Price * hours;
                        //if (item.CheckOutExpectual.HasValue)
                        //{
                        //	// Tính số giờ thuê
                        //	var rentalHours = (localCheckOutTime - item.CheckInExpectual!.Value).TotalHours;

                        //	// Kiểm tra nếu thời gian thuê dưới 1 giờ
                        //	if (rentalHours < 0)
                        //	{
                        //		SetErrorMesg("Tổng thời gian thuê tối thiểu là 1 giờ");
                        //		return Ok();
                        //	}
                        //}
                    }
                    if (rentalPrice.IdRentalType == 3)
                    {
                        item.CheckOutExpectual = localCheckOutTime;
                        if (hours < 22)
                        {
                            item.RoomPrice = rentalPrice!.Price;
                        }
                        else
                        {
                            int dates = (int)Math.Ceiling(hours / 22.0);
                            item.RoomPrice = rentalPrice!.Price * dates;
                        }
                    }
                    SaveToCookie(CookieConst.KEY_COOKIES_BOOKING_ROOM, cookies);
                }
            }
            return Ok();
        }

        [HttpPost]
        public IActionResult UpdateQuantity([FromBody] UpdateQuantityDTOs request)
        {
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity!; //IdUser
            var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
            int IdGroup = int.Parse(IdGroupClaim);
            var IdUserClaim = identity.FindFirst("IdUser")?.Value;
            int IdUser = int.Parse(IdUserClaim);
            var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

            var cookies = GetFromCookie<AddBookingRoomDTOs>(CookieConst.KEY_COOKIES_BOOKING_ROOM);
            if (request.RoomId > 0)
            {
                var room = _HotelDbContext.AppRooms.Include(x => x.appRoomCate).FirstOrDefault(x => x.Id == request.RoomId);
                var item = cookies.FirstOrDefault(i => i.IdRoom == request.RoomId);
                if (item != null && room != null)
                {
                    if (request.Field == "AdultCount")
                    {
                        item.Adult = request.Value;
                    }
                    else if (request.Field == "ChildrenCount")
                    {
                        item.Children = request.Value;
                    }

                    SaveToCookie(CookieConst.KEY_COOKIES_BOOKING_ROOM, cookies);
                }
            }
            return Json(new { success = true });
        }

        public IActionResult UpdateStatus(int id, CleanRoomStatus status)
        {
            // Update the room status in your data store
            var room = _HotelDbContext.AppRooms.FirstOrDefault(x => x.Id == id);
            if (room != null)
            {
                room.CleanStatus = status;
                _HotelDbContext.Update(room);
                _HotelDbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateCustomer(AddBookingRoomDTOs model, [FromServices] IWebHostEnvironment envi)
        {
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity!; //IdUser
            var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
            int IdGroup = int.Parse(IdGroupClaim);
            var IdUserClaim = identity.FindFirst("IdUser")?.Value;
            int IdUser = int.Parse(IdUserClaim);
            var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

            // Xử lý KEY_COOKIES_CUSTOMER
            var bookingRoomCookies = GetFromCookie<AddBookingRoomDTOs>(CookieConst.KEY_COOKIES_BOOKING_ROOM);
            var bookingRoom = bookingRoomCookies.FirstOrDefault();

            if (bookingRoom == null)
            {
                // Thêm mới nếu chưa có
                var newCustomer = new AddBookingRoomDTOs
                {
                    Address = model.Address ?? "",
                    Email = model.Email ?? "",
                    Phone = model.Phone ?? "",
                    CusName = model.CusName ?? "",
                    BirthDay = model.BirthDay,
                    CusDesc = model.CusDesc ?? "",
                    Country = model.Country ?? "",
                    stringFile1 = model.FormFile1 != null ? UploadFile(model.FormFile1, envi.WebRootPath) : null,
                    stringFile2 = model.FormFile2 != null ? UploadFile(model.FormFile2, envi.WebRootPath) : null
                };

                bookingRoomCookies.Add(newCustomer);
            }
            else
            {
                // Cập nhật thông tin khách hàng
                bookingRoom.Address = model.Address ?? "";
                bookingRoom.Email = model.Email ?? "";
                bookingRoom.Phone = model.Phone ?? "";
                bookingRoom.CusName = model.CusName ?? "";
                bookingRoom.BirthDay = model.BirthDay;
                bookingRoom.CusDesc = model.CusDesc ?? "";
                bookingRoom.Country = model.Country ?? "";
                bookingRoom.stringFile1 = model.FormFile1 != null ? UploadFile(model.FormFile1, envi.WebRootPath) : bookingRoom.stringFile1;
                bookingRoom.stringFile2 = model.FormFile2 != null ? UploadFile(model.FormFile2, envi.WebRootPath) : bookingRoom.stringFile2;
            }

            SaveToCookie(CookieConst.KEY_COOKIES_BOOKING_ROOM, bookingRoomCookies);

            TempData["BookingRoomCookies"] = JsonSerializer.Serialize(bookingRoomCookies);

            // Chuyển hướng
            return RedirectToAction("BookingRoom", new { id = model.IdRoom });
        }

        #endregion

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

        #region Upload File 
        private string UploadFile(IFormFile file, string webRootPath)
        {
            var fName = file.FileName;
            fName = Path.GetFileNameWithoutExtension(fName)
                + DateTime.Now.Ticks
                + Path.GetExtension(fName);

            var directoryPath = Path.Combine(webRootPath, "images", "customer");
            Directory.CreateDirectory(directoryPath); // Đảm bảo thư mục tồn tại

            var filePath = Path.Combine(directoryPath, fName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            var relativePath = "/images/customer/" + fName;
            return relativePath;
        }
        #endregion



        private async Task<string> GenerateBookingConfirmationPDF()
        {
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity!; // IdUser
            var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
            int IdGroup = int.Parse(IdGroupClaim);
            var IdUserClaim = identity.FindFirst("IdUser")?.Value;
            int IdUser = int.Parse(IdUserClaim);
            var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);

            var bookingRoomCookies = GetFromCookie<AddBookingRoomDTOs>(CookieConst.KEY_COOKIES_BOOKING_ROOM) ?? new List<AddBookingRoomDTOs>();
            SaveToCookie(CookieConst.KEY_COOKIES_BOOKING_ROOM, bookingRoomCookies);
            string filePath = "";

            if (bookingRoomCookies.FirstOrDefault() != null)
            {
                // Đọc file HTML từ Views
                var htmlPath = Path.Combine("Areas/Admin/Views/Components/AdBookingRoom/Default.cshtml");
                var htmlContent = await System.IO.File.ReadAllTextAsync(htmlPath);

                // Sử dụng SelectPdf để chuyển HTML thành PDF
                HtmlToPdf converter = new HtmlToPdf();
                SelectPdf.PdfDocument doc = converter.ConvertHtmlString(htmlContent);

                // Lưu file PDF vào thư mục wwwroot/pdf
                filePath = Path.Combine("wwwroot", "pdf", "BookingConfirmation.pdf");
                doc.Save(filePath);
                doc.Close();
            }

            return filePath;
        }

        public async Task<string> ExportConfirmBookingRoomPdf()
        {
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity!; // IdUser
            var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
            int IdGroup = int.Parse(IdGroupClaim);
            var IdUserClaim = identity.FindFirst("IdUser")?.Value;
            int IdUser = int.Parse(IdUserClaim);
            var hotel = _HotelDbContext.AppHotel.Select(x => new
            {
                x.Id,
                x.Name,
                x.Email,
                x.Location,
                x.Phone,
                x.City,
                x.IdGroup
            }).FirstOrDefault(x => x.IdGroup == IdGroup);

            var bookingRoomCookies = GetFromCookie<AddBookingRoomDTOs>(CookieConst.KEY_COOKIES_BOOKING_ROOM) ?? new List<AddBookingRoomDTOs>();
            SaveToCookie(CookieConst.KEY_COOKIES_BOOKING_ROOM, bookingRoomCookies);

            var item = bookingRoomCookies.FirstOrDefault();


            TempData["Hotel"] = Newtonsoft.Json.JsonConvert.SerializeObject(hotel);

            var jsonSettings = new Newtonsoft.Json.JsonSerializerSettings
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects
            };
            var room = _HotelDbContext.AppRooms.Select(x => new
            {
                x.Id,
                x.Name, // Chỉ chọn các trường cần thiết
                x.IdRoomCate
            }).FirstOrDefault(x => x.Id == item.IdRoom);
            TempData["Room"] = Newtonsoft.Json.JsonConvert.SerializeObject(room, jsonSettings);
            TempData.Keep("Room");

            var roomCate = _HotelDbContext.AppRoomCates.Select(x => new
            {
                x.Id,
                x.Name // Chỉ chọn các trường cần thiết
            }).FirstOrDefault(x => x.Id == room.IdRoomCate);
            TempData["RoomCate"] = Newtonsoft.Json.JsonConvert.SerializeObject(roomCate);
            TempData.Keep("RoomCate");

            var bookingRoom = _HotelDbContext.AppBookingRooms
                .Where(x => x.CheckInExpectual == item.CheckInExpectual && x.CheckOutExpectual == item.CheckOutExpectual)
                .Select(x => new { x.Id, x.Code }).FirstOrDefault();
            TempData["BookingRoom"] = Newtonsoft.Json.JsonConvert.SerializeObject(bookingRoom);
            TempData.Keep("BookingRoom");

            var user = _HotelDbContext.AppUser.Select(x => new { x.Id, x.Name }).FirstOrDefault(x => x.Id == IdUser);
            TempData["User"] = Newtonsoft.Json.JsonConvert.SerializeObject(user);
            TempData.Keep("User");

            // Render Partial View thành HTML
            string htmlContent = await RenderViewHelper.RenderPartialViewToString(this, "_BookingRoomPDF", item);

            HtmlToPdf converter = new HtmlToPdf();
            PdfDocument doc = converter.ConvertHtmlString(htmlContent);

            // Lưu file PDF vào thư mục wwwroot/pdf
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss"); // Định dạng thời gian hợp lệ
            string fileName = $"Xac_nhan_dat_phong_{bookingRoom.Code}_{timestamp}.pdf"; // Tạo tên file an toàn
            string filePath = Path.Combine("wwwroot", "pdf", fileName); // Lưu file trong thư mục pdf
            doc.Save(filePath);
            doc.Close();

            // Đường dẫn URL mà trình duyệt có thể truy cập được
            string urlPath = $"/pdf/{fileName}";
            TempData["Path"] = urlPath; // Lưu đường dẫn URL vào TempData
            return urlPath;
        }

        public async Task<string> ExportConfirmCheckInPdf(int id)
        {
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity!; // IdUser
            var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
            int IdGroup = int.Parse(IdGroupClaim);
            var IdUserClaim = identity.FindFirst("IdUser")?.Value;
            int IdUser = int.Parse(IdUserClaim);
            var hotel = _HotelDbContext.AppHotel.Select(x => new
            {
                x.Id,
                x.Name,
                x.Email,
                x.Location,
                x.Phone,
                x.City,
                x.IdGroup
            }).FirstOrDefault(x => x.IdGroup == IdGroup);

            var booking = _HotelDbContext.AppBookingRooms.Include(x => x.appCustHotel)
                                                            .Include(x => x.appUser)
                                                            .Include(x => x.appRoom)
                                                            .ThenInclude(x => x.appRoomCate)
                                                            .FirstOrDefault(x => x.Id == id);

            TempData["Hotel"] = Newtonsoft.Json.JsonConvert.SerializeObject(hotel);

            // Render Partial View thành HTML
            string htmlContent = await RenderViewHelper.RenderPartialViewToString(this, "_ConfirmCheckInPDF", booking);

            HtmlToPdf converter = new HtmlToPdf();
            PdfDocument doc = converter.ConvertHtmlString(htmlContent);

            // Lưu file PDF vào thư mục wwwroot/pdf
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss"); // Định dạng thời gian hợp lệ
            string fileName = $"Xac_nhan_nhan_phong_{booking.Code}_{timestamp}.pdf"; // Tạo tên file an toàn
            string filePath = Path.Combine("wwwroot", "pdf", fileName); // Lưu file trong thư mục pdf
            doc.Save(filePath);
            doc.Close();

            // Đường dẫn URL mà trình duyệt có thể truy cập được
            string urlPath = $"/pdf/{fileName}";
            TempData["Path"] = urlPath; // Lưu đường dẫn URL vào TempData
            return urlPath;
        }
        public async Task<string> ExportCheckOutPdf(int id)
        {
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity!; // IdUser
            var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
            int IdGroup = int.Parse(IdGroupClaim);
            var IdUserClaim = identity.FindFirst("IdUser")?.Value;
            int IdUser = int.Parse(IdUserClaim);
            var hotel = _HotelDbContext.AppHotel.Select(x => new
            {
                x.Id,
                x.Name,
                x.Email,
                x.Location,
                x.Phone,
                x.City,
                x.IdGroup
            }).FirstOrDefault(x => x.IdGroup == IdGroup);


            TempData["Hotel"] = Newtonsoft.Json.JsonConvert.SerializeObject(hotel);

            var booking = _HotelDbContext.AppBookingRooms
                    .Where(x => x.Id == id)
                    .Include(x => x.appCustHotel)
                    .Include(x => x.appRoom)
                    .ThenInclude(x => x.appRoomCate)
                    .Include(x => x.appRentalPrice)
                    .ThenInclude(x => x.appRentalType)
                    .Include(x => x.appUser)
                    .Include(x => x.appServicesOrders)
                    .ThenInclude(x => x.appServices)
                    .Include(x => x.appComodityOrders)
                    .ThenInclude(x => x.appCommodity)
                    .Include(x => x.appIncurredFees)
                    .FirstOrDefault();

            var bill = _HotelDbContext.AppBill
                                        .Select(x => new
                                        {
                                            x.DiscountPrice,
                                            x.RoomPrice,
                                            x.FinalPrice,
                                            x.IdBooking

                                        }).FirstOrDefault(x => x.IdBooking == id);
            TempData["Bill"] = Newtonsoft.Json.JsonConvert.SerializeObject(bill);

            //TempData["SvcOrder"] = _HotelDbContext.AppServicesOrders
            //										.Where(x => x.IdBookingRoom == id)
            //										.Include(x => x.appServices).ToList();
            //TempData["CommoOrder"] = _HotelDbContext.AppComodityOrders
            //													.Where(x => x.IdBookingRoom == id)
            //													.Include(x => x.appCommodity).ToList();
            //var PTNS = _HotelDbContext.AppIncurredFee
            //							.Where(x => x.IdBookingRoom == id && x.Name == "PTNS").FirstOrDefault();
            //TempData["PTNS"] = Newtonsoft.Json.JsonConvert.SerializeObject(PTNS);

            //var PTTM = _HotelDbContext.AppIncurredFee
            //							.Where(x => x.IdBookingRoom == id && x.Name == "PTTM").FirstOrDefault();
            //TempData["PTTM"] = Newtonsoft.Json.JsonConvert.SerializeObject(PTNS);
            // Render Partial View thành HTML
            string htmlContent = await RenderViewHelper.RenderPartialViewToString(this, "_CheckOutPDF", booking);

            HtmlToPdf converter = new HtmlToPdf();
            PdfDocument doc = converter.ConvertHtmlString(htmlContent);

            // Lưu file PDF vào thư mục wwwroot/pdf
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss"); // Định dạng thời gian hợp lệ
            string fileName = $"Hoa_don_{booking.Code}_{timestamp}.pdf"; // Tạo tên file an toàn
            string filePath = Path.Combine("wwwroot", "pdf", fileName); // Lưu file trong thư mục pdf
            doc.Save(filePath);
            doc.Close();

            // Đường dẫn URL mà trình duyệt có thể truy cập được
            string urlPath = $"/pdf/{fileName}";
            TempData["Path"] = urlPath; // Lưu đường dẫn URL vào TempData
            return urlPath;
        }
    }


    public class RazorViewToStringRenderer
    {
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;

        public RazorViewToStringRenderer(
            IRazorViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }

        public async Task<string> RenderViewToStringAsync(string viewName)
        {
            var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            using (var sw = new StringWriter())
            {
                var viewResult = _viewEngine.FindView(actionContext, viewName, false);
                if (viewResult.View == null)
                {
                    throw new ArgumentNullException($"{viewName} does not match any available view");
                }

                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = ""
                };

                var viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    viewDictionary,
                    new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return sw.ToString();
            }
        }
    }
}
