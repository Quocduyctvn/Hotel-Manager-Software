using Hotel.Client.DTOs;
using Hotel.Data;
using Hotel.Data.Entities;
using Hotel.Share.Const;
using Hotel.Share.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Security.Claims;
using System.Text.Json;

namespace Hotel.Client.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static Random random = new Random();
        public HomeController(ApplicationDbContext DbContext, IHttpContextAccessor httpContextAccessor) : base(DbContext)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            //    SetErrorMesg("askfsdkjvcbk asfcdsb");
            //    SetSuccessMesg("askfsdkjvcbk asfcdsb 2");
            return View();
        }
        public IActionResult RentalPackage()
        {
            var rental_package = _HotelDbContext.AppRentalPackage.Where(x => x.Status == RentalStatus.ACTIVE).OrderBy(x => x.Position).ToList();
            return View(rental_package);
        }



        //public IActionResult Verssion()
        //{
        //    var rentalType = _HotelDbContext.AppHotelRentalType.ToList();
        //    ViewBag.Cookies = GetFromCookie<AppRentalPackageCate>(CookieConst.KEY_COOKIES_RENTAL_TYPE);
        //    return View(rentalType);
        //}

        //public IActionResult RentalType(int id)
        //{
        //    var cookies = GetFromCookie<AppRentalPackageCate>(CookieConst.KEY_COOKIES_RENTAL_TYPE);
        //    var item = cookies.FirstOrDefault(i => i.Id == id);

        //    if (item == null)
        //    {
        //        var rentalType = _HotelDbContext.AppHotelRentalType.FirstOrDefault(i => i.Id == id);
        //        if (rentalType != null)
        //        {
        //            item = new AppRentalPackageCate { Id = rentalType.Id };

        //            cookies.Clear();
        //            cookies.Add(item);
        //        }
        //    }

        //    SaveToCookie(CookieConst.KEY_COOKIES_RENTAL_TYPE, cookies);

        //    return RedirectToAction("Package");
        //}


        public IActionResult Package(int id)
        {
            var cookies = GetFromCookie<AppRentalPackage>(CookieConst.KEY_COOKIES_RENTAL_PACKAGE);
            var item = cookies.FirstOrDefault(i => i.Id == id);

            if (!(item != null && item.Id == id)) // nếu đúng dk là khác cái cũ 
            {
                var rentalPackage = _HotelDbContext.AppRentalPackage.FirstOrDefault(i => i.Id == id);
                if (rentalPackage != null)
                {
                    item = new AppRentalPackage { Id = rentalPackage.Id };

                    // Chỉ xóa các cookies liên quan đến RentalPackage, giữ lại các loại khác
                    cookies.Clear();
                    cookies.Add(item);
                }
            }

            SaveToCookie(CookieConst.KEY_COOKIES_RENTAL_PACKAGE, cookies);

            return RedirectToAction("ValidMail");
        }

        [HttpGet]
        public IActionResult ValidMail()
        {
            return View();
        }
        public IActionResult ValidatedByMail()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ValidMail(ValidMailDTOs model)
        {

            string content = $@"
							<div class=""container mt-5"" >
                                <div class=""card""style=""border-radius: 20px 20px 10px 10px;"" >
                                    <div class=""card-header text-white text-center  d-flex justify-content-center"" style=""border-radius: 20px 20px 0 0; background-color: #ffb964;"">
                                        <span style=""padding-left: 20px"">Xác nhận mail - Quy trình Đăng ký phần mềm quản trị khách sạn HMSoft.com!</span>
                                    </div>
                                    <div class=""card-body"">
                                        <p>Chào bạn, {model.Gmail}</p>
                                        <p>Cảm ơn bạn đã đăng ký sử dụng <strong>Phần mềm quản trị khách sạn HMSoft.com</strong>! Để hoàn tất quá trình đăng ký, Vui lòng nhấn vào nút bên dưới để xác nhận tài khoản và hoàn thành quá trình thiết lập:</p>
                                        <div class=""text-center my-4"">
                                            <label class=""golden-button"" style="" cursor: pointer; border-radius: 13px; "">
                                                <span class=""golden-text fw-bold ""><a href=""https://localhost:44362/Home/Register"" class="" text-decoration-none"">Xác nhận tài khoản</a></span>
                                            </label>
                    
                                        </div>
                
                                        <p><strong>Lưu ý:</strong> Việc xác nhận email là bước quan trọng để bảo vệ tài khoản của bạn và truy cập vào các tính năng đầy đủ của phần mềm.</p>

                                        <p>Nếu bạn không thực hiện đăng ký này, vui lòng bỏ qua email. Nếu cần hỗ trợ, hãy liên hệ với chúng tôi qua email <a href=""quocduyctvn@gmail.com.com"">quocduyctvn@gmail.com</a>.</p>
                
                                        <p>Trân trọng,<br><strong>Đội ngũ hỗ trợ HMSoft.com</strong></p>
                                    </div>
                                    <div class=""card-footer text-muted text-center"">
                                        &copy; 2024 HMSoft.com - No copy right 
                                    </div>
                                </div>
                            </div>
			";
            var Username = "quocduyctvn@gmail.com";                             // người gửi mail
            var Password = "ylya rfag tclg nhae";
            var Host = "smtp.gmail.com";
            var Port = 587;
            var FromEmail = "quocduyctvn@gmail.com";

            MailMessage message = new MailMessage
            {
                From = new MailAddress(FromEmail),
                Subject = "Xác thực mail - Quy trình Đăng ký phần mềm quản trị khách sạn HMSoft.com",
                IsBodyHtml = true,
                Body = content
            };
            message.To.Add(model.Gmail); // Người nhận email


            // Tạo đối tượng SmtpClient
            SmtpClient mailClient = new SmtpClient
            {
                Host = Host,
                Port = Port,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(Username, Password)
            };

            mailClient.Send(message);

            var cookies = GetFromCookie<ValidMailDTOs>(CookieConst.KEY_COOKIES_MAIL);
            cookies.Clear();
            var item = new ValidMailDTOs { Gmail = model.Gmail };
            cookies.Add(item);

            SaveToCookie(CookieConst.KEY_COOKIES_MAIL, cookies);


            return RedirectToAction("ValidatedByMail");
        }

        public IActionResult ValidByGoogle(string provider)
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("ExternalLoginCallback", "Home")
            };
            TempData["google"] = "google";
            return provider switch
            {
                "Google" => Challenge(properties, GoogleDefaults.AuthenticationScheme),
                _ => RedirectToAction("Index", "Home")
            };
        }

        public IActionResult Register()
        {
            var cookies = GetFromCookie<ValidMailDTOs>(CookieConst.KEY_COOKIES_MAIL);
            var item = cookies.FirstOrDefault();
            if (item == null && !User.Identity.IsAuthenticated)
            {
                SetWrnMesg("Truy cập không thành công");
                return RedirectToAction("Index", "Home");
            }
            var registerDTOs = new RegisterDTOs();
            var googleTempData = TempData["google"] as string; ;
            if (googleTempData == "google")
                registerDTOs.AdMail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            else
                registerDTOs.AdMail = item.Gmail;

            // lấy lại gói thuê 
            var rentalPackage = GetFromCookie<AppRentalPackage>(CookieConst.KEY_COOKIES_RENTAL_PACKAGE);
            var packageCookie = rentalPackage.FirstOrDefault();
            var package = _HotelDbContext.AppRentalPackage.FirstOrDefault(x => x.Id == packageCookie.Id);
            if (package != null)
            {
                TempData["package"] = package;
                TempData["Time"] = _HotelDbContext.AppTime.ToList();
            }
            return View(registerDTOs);
        }


        [HttpPost]
        public IActionResult Register(RegisterDTOs model)
        {
            if (!ModelState.IsValid)
            {
                SetErrorMesg("Vui lòng kiểm tra lại dữ liệu");
                var rentalPackage = GetFromCookie<AppRentalPackage>(CookieConst.KEY_COOKIES_RENTAL_PACKAGE);
                var packageCookie = rentalPackage.FirstOrDefault();
                var package = _HotelDbContext.AppRentalPackage.FirstOrDefault(x => x.Id == packageCookie.Id);
                TempData["package"] = package;
                TempData["Time"] = _HotelDbContext.AppTime.ToList();
                return View(model);
            }
            var checkMail = _HotelDbContext.AppUser.FirstOrDefault(x => x.Email == model.AdMail);
            if (checkMail != null)
            {
                SetErrorMesg("Email đã tồn tại - vui lòng chọn email khác ");
                return View(model);
            }
            var checkPhone = _HotelDbContext.AppUser.FirstOrDefault(x => x.Email == model.AdMail);
            if (checkPhone != null)
            {
                SetErrorMesg("Số điện thoại đã tồn tại!!");
                return View(model);
            }
            var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.Phone == model.HtelPhone);
            if (hotel != null)
            {
                SetErrorMesg("Số điện thoại khách sạn đã được đăng ký!!");
                return View(model);
            }

            var cookies = GetFromCookie<RegisterDTOs>(CookieConst.KEY_COOKIES_REGISTER);
            cookies.Clear();
            cookies.Add(model);

            SaveToCookie(CookieConst.KEY_COOKIES_REGISTER, cookies);
            return RedirectToAction("Payment", "Home");
        }

        [HttpGet]
        public IActionResult Payment()
        {
            // lay goi thue 
            var cookiesPackage = GetFromCookie<AppRentalPackage>(CookieConst.KEY_COOKIES_RENTAL_PACKAGE).FirstOrDefault();
            var package = _HotelDbContext.AppRentalPackage.FirstOrDefault(x => x.Id == cookiesPackage.Id);

            // Đơn đặt hàng 
            var cookiesHMSOrder = GetFromCookie<RegisterDTOs>(CookieConst.KEY_COOKIES_REGISTER).FirstOrDefault();


            // lấy côkie trang thanh toán == để giữ lại mã code 
            var cookiePayment = GetFromCookie<PaymentDTOs>(CookieConst.KEY_COOKIES_PAYMENT);


            var paymentDTOs = new PaymentDTOs();
            paymentDTOs.StartDate = DateTime.Now;
            paymentDTOs.PackageName = package.Name;
            paymentDTOs.HtelCode = GenerateHotelCode();
            paymentDTOs.Gmail = cookiesHMSOrder.AdMail;
            if (package.Price > 0)
            {
                switch (cookiesHMSOrder.IdTime)
                {
                    case 3:
                        paymentDTOs.TotalPrice = package.Price * cookiesHMSOrder.Time.Value;
                        paymentDTOs.EndDate = DateTime.Now.AddMonths(cookiesHMSOrder.Time.Value);
                        break;

                    case 4:
                        paymentDTOs.TotalPrice = package.Price * 12 * cookiesHMSOrder.Time.Value;
                        paymentDTOs.EndDate = DateTime.Now.AddYears(cookiesHMSOrder.Time.Value);
                        break;
                }
            }
            else
            {
                paymentDTOs.TotalPrice = 0;
                paymentDTOs.EndDate = DateTime.Now.AddMonths(package.TrialTime.Value);
            }


            // them vao cookie 
            cookiePayment.Clear();
            cookiePayment.Add(paymentDTOs);
            SaveToCookie(CookieConst.KEY_COOKIES_PAYMENT, cookiePayment);

            return View(paymentDTOs);
        }

        [HttpGet]
        public async Task<IActionResult> CompleteRegis()
        {
            // Đơn đặt hàng 
            var cookiesHMSOrder = GetFromCookie<RegisterDTOs>(CookieConst.KEY_COOKIES_REGISTER).FirstOrDefault();
            var cookiePayment = GetFromCookie<PaymentDTOs>(CookieConst.KEY_COOKIES_PAYMENT).FirstOrDefault();
            var cookiesPackage = GetFromCookie<AppRentalPackage>(CookieConst.KEY_COOKIES_RENTAL_PACKAGE).FirstOrDefault();



            using (var transaction = _HotelDbContext.Database.BeginTransaction())
            {
                try
                {
                    var group = new AppGroup();
                    group.Name = cookiePayment.HtelCode;
                    group.Desc = "Khách sạn " + cookiePayment.HtelCode;
                    group.CreatedDate = DateTime.Now;
                    _HotelDbContext.Add(group);
                    _HotelDbContext.SaveChanges();
                    var Group = _HotelDbContext.AppGroup.Find(group.Id);


                    var hotel = new AppHotels();
                    hotel.Code = cookiePayment.HtelCode;
                    hotel.Name = cookiesHMSOrder.HtelName;
                    hotel.Phone = cookiesHMSOrder.HtelPhone;
                    hotel.City = cookiesHMSOrder.HtelCity;
                    hotel.District = cookiesHMSOrder.HtelDistrict;
                    hotel.IdGroup = group.Id;
                    hotel.status = EnumStatusHotel.ACTIVE;
                    hotel.CreatedDate = DateTime.Now;
                    _HotelDbContext.AppHotel.Add(hotel);
                    _HotelDbContext.SaveChanges();

                    var svcCommoCates = new List<AppSvcCommoCate>()
                    {
                        new AppSvcCommoCate
                        {
                            Name = "Dịch vụ",
                            IdHotel= hotel.Id
                        },
                        new AppSvcCommoCate
                        {
                            Name = "Sản phẫm",
                            IdHotel= hotel.Id
                        }
                    };
                    _HotelDbContext.AppSvcCommoCates.AddRange(svcCommoCates);
                    _HotelDbContext.SaveChanges();


                    var role = new AppRole();
                    role.Name = "Quản trị hệ thống";
                    role.Desc = "Quản lý toàn bộ hệ thống";
                    role.IdGroup = group.Id;
                    role.CreatedDate = DateTime.Now;
                    _HotelDbContext.AppRole.Add(role);
                    _HotelDbContext.SaveChanges();


                    var now = DateTime.Now;
                    var permissions = new List<AppPermission>();

                    #region tạo các quyền cho admin 
                    // Quản lý tài khoản
                    var groupName = "Quản lý tài khoản";
                    permissions.AddRange(new List<AppPermission>()
            {
                new AppPermission { PerCode = AuthConst.AppUser.VIEW_LIST, Table= "AppUser", Code = "VIEW_LIST", GroupName = groupName, Desc = "Xem danh sách tài khoản", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppUser.CREATE,Table= "AppUser",  Code = "CREATE", GroupName = groupName, Desc = "Thêm tài khoản", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppUser.UPDATE,Table= "AppUser",  Code = "UPDATE", GroupName = groupName, Desc = "Cập nhật tài khoản", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppUser.DELETE,Table= "AppUser",  Code = "DELETE", GroupName = groupName, Desc = "Xóa tài khoản", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppUser.VIEW_DETAIL,Table= "AppUser",  Code = "VIEW_DETAIL", GroupName = groupName, Desc = "Xem chi tiết tài khoản", CreatedDate = now, IdGroup = Group.Id }
            });

                    // Quản lý vai trò
                    groupName = "Quản lý vai trò";
                    permissions.AddRange(new List<AppPermission>()
            {
                new AppPermission { PerCode = AuthConst.AppRole.VIEW_LIST,Table= "AppRole",  Code = "VIEW_LIST", GroupName = groupName, Desc = "Xem danh sách vai trò", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppRole.CREATE,Table= "AppRole", Code = "CREATE", GroupName = groupName, Desc = "Thêm vai trò", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppRole.UPDATE, Table= "AppRole", Code = "UPDATE", GroupName = groupName, Desc = "Cập nhật vai trò", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppRole.DELETE, Table= "AppRole",  Code = "DELETE", GroupName = groupName, Desc = "Xóa vai trò", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppRole.VIEW_DETAIL, Table= "AppRole",  Code = "VIEW_DETAIL", GroupName = groupName, Desc = "Xem chi tiết vai trò", CreatedDate = now, IdGroup = Group.Id }
            });

                    // Quản lý khách hàng
                    groupName = "Quản lý khách hàng";
                    permissions.AddRange(new List<AppPermission>()
            {
                new AppPermission { PerCode = AuthConst.AppCustommer.VIEW_LIST, Table= "AppCustommer",  Code = "VIEW_LIST", GroupName = groupName, Desc = "Xem danh sách khách hàng", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppCustommer.CREATE, Table= "AppCustommer",  Code = "CREATE", GroupName = groupName, Desc = "Thêm khách hàng", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppCustommer.UPDATE, Table= "AppCustommer",  Code = "UPDATE", GroupName = groupName, Desc = "Cập nhật khách hàng", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppCustommer.DELETE, Table= "AppCustommer",  Code = "DELETE", GroupName = groupName, Desc = "Xóa khách hàng", CreatedDate = now, IdGroup = group.Id },
                new AppPermission { PerCode = AuthConst.AppCustommer.VIEW_DETAIL, Table= "AppCustommer",  Code = "VIEW_DETAIL", GroupName = groupName, Desc = "Xem chi tiết khách hàng", CreatedDate = now, IdGroup = Group.Id }
            });

                    // Quản lý đặt phòng
                    groupName = "Quản lý đặt phòng";
                    permissions.AddRange(new List<AppPermission>()
            {
                new AppPermission { PerCode = AuthConst.AppBookingRoom.VIEW_LIST, Table= "AppBookingRoom",  Code = "VIEW_LIST", GroupName = groupName, Desc = "Xem danh sách đặt phòng", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppBookingRoom.CREATE, Table= "AppBookingRoom",  Code = "CREATE", GroupName = groupName, Desc = "Thêm đặt phòng", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppBookingRoom.UPDATE, Table= "AppBookingRoom",  Code = "UPDATE", GroupName = groupName, Desc = "Cập nhật đặt phòng", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppBookingRoom.DELETE, Table= "AppBookingRoom",  Code = "DELETE", GroupName = groupName, Desc = "Xóa đặt phòng", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppBookingRoom.VIEW_DETAIL, Table= "AppBookingRoom",  Code = "VIEW_DETAIL", GroupName = groupName, Desc = "Xem chi tiết đặt phòng", CreatedDate = now, IdGroup = Group.Id }
            });

                    // Quản lý tầng
                    groupName = "Quản lý tầng";
                    permissions.AddRange(new List<AppPermission>()
            {
                new AppPermission { PerCode = AuthConst.AppFloor.VIEW_LIST, Table= "AppFloor",  Code = "VIEW_LIST", GroupName = groupName, Desc = "Xem danh sách tầng", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppFloor.CREATE, Table= "AppFloor",  Code = "CREATE", GroupName = groupName, Desc = "Thêm tầng", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppFloor.UPDATE, Table= "AppFloor",  Code = "UPDATE", GroupName = groupName, Desc = "Cập nhật tầng", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppFloor.DELETE, Table= "AppFloor",  Code = "DELETE", GroupName = groupName, Desc = "Xóa tầng", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppFloor.VIEW_DETAIL, Table= "AppFloor",  Code = "VIEW_DETAIL", GroupName = groupName, Desc = "Xem chi tiết tầng", CreatedDate = now, IdGroup = Group.Id }
            });

                    // Quản lý loại phòng
                    groupName = "Quản lý loại phòng";
                    permissions.AddRange(new List<AppPermission>()
            {
                new AppPermission { PerCode = AuthConst.AppRoomCate.VIEW_LIST, Table= "AppRoomCate",  Code = "VIEW_LIST", GroupName = groupName, Desc = "Xem danh sách loại phòng", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppRoomCate.CREATE, Table= "AppRoomCate",  Code = "CREATE", GroupName = groupName, Desc = "Thêm loại phòng", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppRoomCate.UPDATE, Table= "AppRoomCate",  Code = "UPDATE", GroupName = groupName, Desc = "Cập nhật loại phòng", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppRoomCate.DELETE, Table= "AppRoomCate",  Code = "DELETE", GroupName = groupName, Desc = "Xóa loại phòng", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppRoomCate.VIEW_DETAIL, Table= "AppRoomCate",  Code = "VIEW_DETAIL", GroupName = groupName, Desc = "Xem chi tiết loại phòng", CreatedDate = now, IdGroup = Group.Id }
            });

                    // Quản lý phòng
                    groupName = "Quản lý phòng";
                    permissions.AddRange(new List<AppPermission>()
            {
                new AppPermission { PerCode = AuthConst.AppRoom.VIEW_LIST, Table= "AppRoom",  Code = "VIEW_LIST", GroupName = groupName, Desc = "Xem danh sách phòng", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppRoom.CREATE, Table= "AppRoom",  Code = "CREATE", GroupName = groupName, Desc = "Thêm phòng", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppRoom.UPDATE, Table= "AppRoom",  Code = "UPDATE", GroupName = groupName, Desc = "Cập nhật phòng", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppRoom.DELETE, Table= "AppRoom",  Code = "DELETE", GroupName = groupName, Desc = "Xóa phòng", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppRoom.VIEW_DETAIL, Table= "AppRoom",  Code = "VIEW_DETAIL", GroupName = groupName, Desc = "Xem chi tiết phòng", CreatedDate = now, IdGroup = Group.Id }
            });

                    // Quản lý tiện ích
                    groupName = "Quản lý tiện ích";
                    permissions.AddRange(new List<AppPermission>()
            {
                new AppPermission { PerCode = AuthConst.AppAmenity.VIEW_LIST, Table= "AppAmenity",  Code = "VIEW_LIST", GroupName = groupName, Desc = "Xem danh sách tiện ích", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppAmenity.CREATE, Table= "AppAmenity",  Code = "CREATE", GroupName = groupName, Desc = "Thêm tiện ích", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppAmenity.UPDATE, Table= "AppAmenity",  Code = "UPDATE", GroupName = groupName, Desc = "Cập nhật tiện ích", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppAmenity.DELETE, Table= "AppAmenity",  Code = "DELETE", GroupName = groupName, Desc = "Xóa tiện ích", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppAmenity.VIEW_DETAIL, Table= "AppAmenity",  Code = "VIEW_DETAIL", GroupName = groupName, Desc = "Xem chi tiết tiện ích", CreatedDate = now, IdGroup = Group.Id }
            });

                    groupName = "Quản lý giá cho thuê";
                    permissions.AddRange(new List<AppPermission>()
                {
                    new AppPermission { PerCode = AuthConst.AppRentalPrice.VIEW_LIST, Table= "AppRentalPrice",  Code = "VIEW_LIST", GroupName = groupName, Desc = "Xem danh sách giá cho thuê", CreatedDate = now, IdGroup = Group.Id },
                    new AppPermission { PerCode = AuthConst.AppRentalPrice.CREATE, Table= "AppRentalPrice",  Code = "CREATE", GroupName = groupName, Desc = "Thêm giá cho thuê", CreatedDate = now, IdGroup = Group.Id },
                    new AppPermission { PerCode = AuthConst.AppRentalPrice.UPDATE, Table= "AppRentalPrice",  Code = "UPDATE", GroupName = groupName, Desc = "Cập nhật giá cho thuê", CreatedDate = now, IdGroup = Group.Id },
                    new AppPermission { PerCode = AuthConst.AppRentalPrice.DELETE, Table= "AppRentalPrice",  Code = "DELETE", GroupName = groupName, Desc = "Xóa giá cho thuê", CreatedDate = now, IdGroup = Group.Id },
                    new AppPermission { PerCode = AuthConst.AppRentalPrice.VIEW_DETAIL, Table= "AppRentalPrice",  Code = "VIEW_DETAIL", GroupName = groupName, Desc = "Xem chi tiết giá cho thuê", CreatedDate = now, IdGroup = Group.Id }
                });

                    // Quản lý ngày trong tuần
                    groupName = "Quản lý ngày trong tuần";
                    permissions.AddRange(new List<AppPermission>()
            {
                new AppPermission { PerCode = AuthConst.AppDateTypeWeek.VIEW_LIST, Table= "AppDateTypeWeek",  Code = "VIEW_LIST", GroupName = groupName, Desc = "Xem danh sách ngày trong tuần", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppDateTypeWeek.CREATE, Table= "AppDateTypeWeek",  Code = "CREATE", GroupName = groupName, Desc = "Thêm ngày trong tuần", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppDateTypeWeek.UPDATE, Table= "AppDateTypeWeek",  Code = "UPDATE", GroupName = groupName, Desc = "Cập nhật ngày trong tuần", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppDateTypeWeek.DELETE, Table= "AppDateTypeWeek",  Code = "DELETE", GroupName = groupName, Desc = "Xóa ngày trong tuần", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppDateTypeWeek.VIEW_DETAIL, Table= "AppDateTypeWeek",  Code = "VIEW_DETAIL", GroupName = groupName, Desc = "Xem chi tiết ngày trong tuần", CreatedDate = now, IdGroup = Group.Id }
            });

                    // Quản lý ngày lễ
                    groupName = "Quản lý ngày lễ";
                    permissions.AddRange(new List<AppPermission>()
            {
                new AppPermission { PerCode = AuthConst.AppHoliday.VIEW_LIST, Table= "AppHoliday",  Code = "VIEW_LIST", GroupName = groupName, Desc = "Xem danh sách ngày lễ", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppHoliday.CREATE, Table= "AppHoliday",  Code = "CREATE", GroupName = groupName, Desc = "Thêm ngày lễ", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppHoliday.UPDATE, Table= "AppHoliday",  Code = "UPDATE", GroupName = groupName, Desc = "Cập nhật ngày lễ", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppHoliday.DELETE, Table= "AppHoliday",  Code = "DELETE", GroupName = groupName, Desc = "Xóa ngày lễ", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppHoliday.VIEW_DETAIL, Table= "AppHoliday",  Code = "VIEW_DETAIL", GroupName = groupName, Desc = "Xem chi tiết ngày lễ", CreatedDate = now, IdGroup = Group.Id }
            });
                    // Quản lý ngày lễ
                    groupName = "Quản lý sản phẫm";
                    permissions.AddRange(new List<AppPermission>()
            {
                new AppPermission { PerCode = AuthConst.AppCommodity.VIEW_LIST, Table= "AppCommodity",  Code = "VIEW_LIST", GroupName = groupName, Desc = "Xem danh sách sản phẫm", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppCommodity.CREATE, Table= "AppCommodity",  Code = "CREATE", GroupName = groupName, Desc = "Thêm sản phẫm", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppCommodity.UPDATE, Table= "AppCommodity",  Code = "UPDATE", GroupName = groupName, Desc = "Cập nhật sản phẫm", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppCommodity.DELETE, Table= "AppHoliday",  Code = "DELETE", GroupName = groupName, Desc = "Xóa sản phẫm", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppCommodity.VIEW_DETAIL, Table= "AppCommodity",  Code = "VIEW_DETAIL", GroupName = groupName, Desc = "Xem chi sản phẫm", CreatedDate = now, IdGroup = Group.Id }
            });
                    // Quản lý ngày lễ
                    groupName = "Quản lý dịch vụ";
                    permissions.AddRange(new List<AppPermission>()
            {
                new AppPermission { PerCode = AuthConst.AppServices.VIEW_LIST, Table= "AppServices",  Code = "VIEW_LIST", GroupName = groupName, Desc = "Xem danh sách dịch vụ", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppServices.CREATE, Table= "AppServices",  Code = "CREATE", GroupName = groupName, Desc = "Thêm dịch vụ", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppServices.UPDATE, Table= "AppServices",  Code = "UPDATE", GroupName = groupName, Desc = "Cập nhật dịch vụ", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppServices.DELETE, Table= "AppServices",  Code = "DELETE", GroupName = groupName, Desc = "Xóa dịch vụ", CreatedDate = now, IdGroup = Group.Id },
                new AppPermission { PerCode = AuthConst.AppServices.VIEW_DETAIL, Table= "AppServices",  Code = "VIEW_DETAIL", GroupName = groupName, Desc = "Xem chi dịch vụ", CreatedDate = now, IdGroup = Group.Id }
            });
                    #endregion


                    _HotelDbContext.AppPermissions.AddRange(permissions);
                    await _HotelDbContext.SaveChangesAsync();

                    var user = new AppUser();
                    user.Email = cookiesHMSOrder.AdMail;
                    user.Password = BCrypt.Net.BCrypt.HashPassword(cookiesHMSOrder.AdPass);
                    user.CreatedDate = DateTime.Now;
                    user.IdGroup = group.Id;
                    user.IdRole = role.Id;
                    _HotelDbContext.AppUser.Add(user);
                    _HotelDbContext.SaveChanges();


                    var rolePer = new List<AppRolePermission>();
                    // Lặp qua danh sách permissions từ trước
                    foreach (var permission in permissions)
                    {
                        rolePer.Add(new AppRolePermission
                        {
                            IdRole = role.Id,
                            IdPermission = permission.PerCode,
                            CreatedDate = DateTime.Now
                        });
                    }
                    _HotelDbContext.AddRange(rolePer);
                    _HotelDbContext.SaveChanges();


                    var HMSOrder = new AppHMSOrder();
                    HMSOrder.StartDate = now;
                    HMSOrder.EndDate = cookiePayment.EndDate;
                    if (cookiePayment.TotalPrice <= 0)
                    {
                        HMSOrder.TotalTime = (double)cookiesPackage.TrialTime!;
                        HMSOrder.IdTime = (int)cookiesPackage.IdTimeOfTrial!;
                    }
                    else
                    {
                        HMSOrder.TotalTime = (double)cookiesHMSOrder.Time!;
                        HMSOrder.IdTime = (int)cookiesHMSOrder.IdTime!;
                    }
                    HMSOrder.TotalFee = (double)cookiePayment.TotalPrice!;
                    HMSOrder.Status = EnumHMSOrder.SUCCESS;
                    HMSOrder.IdPaymentMethod = 2;  // Paypal 
                    HMSOrder.IdRentalPackage = cookiesPackage!.Id;
                    HMSOrder.IdHotel = hotel.Id;
                    HMSOrder.CreatedDate = DateTime.Now;
                    _HotelDbContext.Add(HMSOrder);
                    _HotelDbContext.SaveChanges();


                    SetSuccessMesg("Đăng ký thành công");
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // Rollback transaction nếu có lỗi xảy ra
                    transaction.Rollback();
                    // Xử lý lỗi (ví dụ: ghi log hoặc thông báo lỗi)
                    //SetErrorMesg($"Đã xảy ra lỗi trong quá trình xử lý: {ex.InnerException?.Message}");
                    throw;
                }
                return RedirectToAction("Index", "Home");
            }
        }

        public string GenerateHotelCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var randomPart = new string(Enumerable.Repeat(chars, 3)
                                     .Select(s => s[random.Next(s.Length)]).ToArray());
            return "KS00" + randomPart;
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
            _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieKey, cookieJson, options);
        }
        #endregion

        #region Valid By Google 
        public async Task<IActionResult> ExternalLoginCallback()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
                return RedirectToAction(nameof(ValidByGoogle));

            // Lấy thông tin người dùng từ token
            var claimsPrincipal = authenticateResult.Principal;

            // Tạo một claim identity mới cho người dùng
            var claimsIdentity = new ClaimsIdentity(claimsPrincipal.Claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Đăng nhập người dùng bằng cookie authentication
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    IsPersistent = true, // Lưu trữ cookie trên trình duyệt (ngay cả sau khi đóng)
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(60) // Thời hạn tồn tại của cookie
                });

            return RedirectToAction("Register", "Home");
        }
        #endregion
    }
}
