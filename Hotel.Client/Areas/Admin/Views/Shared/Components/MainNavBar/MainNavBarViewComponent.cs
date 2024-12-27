using Hotel.Data;
using Hotel.Data.Entities;
using Hotel.Share.Const;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Client.Areas.Admin.Views.Shared.Components.MainNavBar
{
	public class MainNavBarViewComponent : ViewComponent
	{
		protected readonly ApplicationDbContext _HotelDbContext;
		public MainNavBarViewComponent(ApplicationDbContext DbContext)
		{
			_HotelDbContext = DbContext;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			var idGroupClaim = HttpContext.User.FindFirst("IdGroup")?.Value;

			var navBar = new NavBarViewModel();
			navBar.Items.AddRange(new MenuItem[]
			{
				//new MenuItem
				//{
				//	DisplayText = "Tiếp tân",
				//	Icon = "fa-folder-open",
				//	ChildrenItems = new MenuItem[]
				//	{
				//		new MenuItem
				//		{
				//			Action = "Index",
				//			Controller = "User",
				//			DisplayText = "Quản lý tài khoản",
				//			Icon = "fa-user-cog"
				//		},
				//		new MenuItem
				//		{
				//			Action = "Index",
				//			Controller = "User",
				//			DisplayText = "Quản lý tài khoản",
				//			Icon = "fa-user-cog"
				//		}
				//	}
				//},
				
				new MenuItem
				{
					DisplayText = "Lễ tân",
					Icon = "fas fa-user-tie",
					ChildrenItems = new MenuItem[]
					{
						new MenuItem
						{
							Action = "Index",
							Controller = "AdminBookingRoom",
							DisplayText = "Lịch đặt phòng",
							Icon = "fas fa-calendar-check"
						}
					}
				},
				new MenuItem
				{
					DisplayText = "Báo cáo",
					Icon = "far fa-chart-bar",
					ChildrenItems = new MenuItem[]
					{
						new MenuItem
						{
							Action = "BookingRoom",
							Controller = "AdminReport",
							DisplayText = "Đặt phòng",
							Icon = "fas fa-calendar-check"
						},
						new MenuItem
						{
							Action = "Revenue",
							Controller = "AdminReport",
							DisplayText = "Doanh thu",
							Icon = "fas fa-dollar-sign"
						}
					}
				},new MenuItem
				{
					DisplayText = "Quản lý hóa đơn",
					Icon = "fas fa-file-medical-alt",
					ChildrenItems = new MenuItem[]
					{
						new MenuItem
						{
							Action = "Index",
							Controller = "AdminBill",
							DisplayText = "Đặt phòng",
							Icon = "fas fa-calendar-check"
						}
					}
				},
				new MenuItem
				{
					Action = "Index",
					Controller = "AdminRoomCate",
					DisplayText = "Quản lý hạng phòng",
					Icon = "fas fa-stream",
					Permission = 0
				},
				new MenuItem
				{
					Action = "Index",
					Controller = "AdminRoom",
					DisplayText = "Quản lý phòng",
					Icon = "fas fa-bed",
					Permission = 0
				},
				new MenuItem
				{
					Action = "Index",
					Controller = "AdminFloor",
					DisplayText = "Quản lý tầng",
					Icon = "fas fa-boxes",
					Permission = 0
				},
				new MenuItem
				{
					Action = "Index",
					Controller = "AdminAmenity",
					DisplayText = "Quản lý tiện nghi",
					Icon = "fas fa-bath",
					Permission = AuthConst.AppAmenity.VIEW_LIST,
				},
				new MenuItem
				{
					DisplayText = "Dịch vụ/ Sản phẫm",
					Icon = "fas fa-dumpster",
					ChildrenItems = new MenuItem[]
					{
						new MenuItem
						{
							Action = "Index",
							Controller = "AdminServices",
							DisplayText = "Quản lý Dịch vụ",
							Icon = "fas fa-skiing",
						},
						new MenuItem
						{
							Action = "Index",
							Controller = "AdminCommodity",
							DisplayText = "Quản lý sản phẫm",
							Icon = "fas fa-utensils",
						}
					}
				},
				new MenuItem
				{
					DisplayText = "Thời gian thuê",
					Icon = "fas fa-business-time",
					ChildrenItems = new MenuItem[]
					{
						new MenuItem
						{
							Action = "Index",
							Controller = "AdminSettingTime",
							DisplayText = "Ngày trong tuần",
							Icon = "far fa-calendar-alt"
						},
						new MenuItem
						{
							Action = "Index",
							Controller = "AdminHolidays",
							DisplayText = "Ngày lễ",
							Icon = "far fa-glass-cheers"
						}
					}
				},
				new MenuItem
				{
					DisplayText = "Quản lý tài khoản",
					Icon = "fas fa-users-cog",
					ChildrenItems = new MenuItem[]
					{
						new MenuItem
						{
							Action = "Index",
							Controller = "AdminAccount",
							DisplayText = "Tài khoản",
							Icon = "fas fa-user-cog"
						},
						new MenuItem
						{
							Action = "Index",
							Controller = "AdminRole",
							DisplayText = "Vai trò trên trang",
							Icon = "fas fa-user-shield"
						}
					}
				},
				new MenuItem
				{
					Action = "Index",
					Controller = "AdminHotel",
					DisplayText = "Quản lý Website",
					Icon = "fas fa-tools",
					Permission = AuthConst.AppServices.VIEW_LIST,
				},
				//new MenuItem 
				//{
				//	Action = "Index",
				//	Controller = "CompanyPatientExcel",
				//	DisplayText = "Thông tin đã nhập (Excel)",
				//	Icon = "fa-file-excel",
				//	Permission = AuthConst.AppCompanyPatientImported.VIEW_LIST,
				//},
				//new MenuItem
				//{
				//	Action = "Index",
				//	Controller = "User",
				//	DisplayText = "Quản lý tài khoản",
				//	Icon = "fa-user-cog",
				//	Permission = AuthConst.AppUser_Auth.VIEW_LIST,
				//},
				//new MenuItem
				//{
				//	Action = "Index",
				//	Controller = "Role",
				//	DisplayText = "Quản lý phân quyền",
				//	Icon = "fa-user-shield",
				//	Permission = AuthConst.AppRole_Auth.VIEW_LIST,
				//},
				//new MenuItem
				//{
				//	Action = "Index",
				//	Controller = "FileManager",
				//	DisplayText = "Quản lý tệp",
				//	Icon = "fa-folder-open",
				//	Permission = AuthConst.FileManager.MANAGE_ALL_USER_FILES,
				//},

			});
			TempData["Hotel"] = _HotelDbContext.AppHotel.Where(x => x.IdGroup == int.Parse(idGroupClaim)).FirstOrDefault();
			if (TempData["Hotel"] as AppHotels == null)
			{
				return View("Index");
			}
			return View(navBar);
		}
	}
}
