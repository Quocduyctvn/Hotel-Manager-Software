using AutoMapper;
using Hotel.Data;
using Hotel.Share.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Security.Claims;

namespace Hotel.Client.Areas.Admin.Controllers
{
	public class AdminReportController : AdminControllerBase
	{
		public AdminReportController(ApplicationDbContext DbContext, IMapper mapper) : base(DbContext, mapper)
		{
		}

		public IActionResult BookingRoom()
		{
			return View();
		}

		public IActionResult BookingRoomChart(string timeRange, bool isExport = false)
		{
			// Nếu không có tham số, mặc định là "thisWeek"
			if (string.IsNullOrEmpty(timeRange))
			{
				timeRange = "thisWeek";
			}
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);
			if (hotel == null)
			{
				return BadRequest("Không tìm thấy khách sạn.");
			}

			DateTime startDate, endDate;
			switch (timeRange)
			{
				case "today":
					startDate = DateTime.Now.Date;
					endDate = startDate.AddDays(1);
					break;
				case "yesterday":
					startDate = DateTime.Now.Date.AddDays(-1);
					endDate = startDate.AddDays(1);
					break;
				case "thisWeek":
					startDate = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek + 1); // Bắt đầu từ Thứ Hai
					endDate = startDate.AddDays(7);
					break;
				case "lastWeek":
					startDate = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek - 6); // Tuần trước từ Thứ Hai
					endDate = startDate.AddDays(7);
					break;
				case "last7Days":
					startDate = DateTime.Now.Date.AddDays(-7);
					endDate = DateTime.Now.Date.AddDays(1);
					break;
				case "thisMonth":
					startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
					endDate = startDate.AddMonths(1);
					break;
				case "lastMonth":
					startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
					endDate = startDate.AddMonths(1);
					break;
				case "last30Days":
					startDate = DateTime.Now.Date.AddDays(-30);
					endDate = DateTime.Now.Date.AddDays(1);
					break;
				case "thisQuarter":
					int currentQuarter = (DateTime.Now.Month - 1) / 3 + 1;
					startDate = new DateTime(DateTime.Now.Year, (currentQuarter - 1) * 3 + 1, 1);
					endDate = startDate.AddMonths(3);
					break;
				case "lastQuarter":
					int lastQuarter = (DateTime.Now.Month - 1) / 3;
					startDate = new DateTime(DateTime.Now.Year, (lastQuarter - 1) * 3 + 1, 1);
					if (lastQuarter == 0) // Nếu là quý đầu tiên của năm
					{
						startDate = new DateTime(DateTime.Now.Year - 1, 10, 1);
					}
					endDate = startDate.AddMonths(3);
					break;
				case "thisYear":
					startDate = new DateTime(DateTime.Now.Year, 1, 1);
					endDate = startDate.AddYears(1);
					break;
				case "lastYear":
					startDate = new DateTime(DateTime.Now.Year - 1, 1, 1);
					endDate = startDate.AddYears(1);
					break;
				default:
					return BadRequest("Khoảng thời gian không hợp lệ.");
			}

			var bookings = _HotelDbContext.AppBookingRooms
					.Where(x => x.appRoom.appRoomCate.IdHotel == hotel.Id &&
								(x.Status == BookingStatus.SUCCESS || x.Status == BookingStatus.SELECTED) &&
								x.CheckInExpectual >= startDate &&
								x.CheckInExpectual < endDate)
								.Include(x => x.appRoom)
								.ThenInclude(x => x.appRoomCate)
								.Include(x => x.appRentalPrice)
								.ThenInclude(x => x.appRentalType)
								.Include(x => x.appUser)
								.Include(x => x.appServicesOrders)
								.ThenInclude(x => x.appServices)
								.Include(x => x.appComodityOrders)
								.ThenInclude(x => x.appCommodity)
								.Include(x => x.appBill)
								.Include(x => x.appCustHotel)
								.ToList(); // Retrieve the full list of bookings

			// Prepare the data for the chart
			var bookingChartData = bookings
				.GroupBy(x => x.CheckInExpectual.Value.Date)
				.Select(g => new
				{
					Date = g.Key.ToString("dd/MM/yyyy"), // Convert date to string
					Count = g.Count() // Count the number of bookings for this date
				})
				.ToList();

			// If the user requested export, generate the Excel file using the full list of bookings
			if (isExport)
			{
				var bookingExportData = bookings.Select(b => new
				{
					b.CheckInExpectual,  // Thời gian dự kiến nhận phòng
					b.CheckOutActual,    // Thời gian thực tế trả phòng
					b.Code,              // Mã đặt phòng
					RoomName = b.appRoom?.Name,  // Tên phòng
					b.appRoom?.appRoomCate.Name, // Loại phòng
					Price = b.Price,     // Giá phòng
					b.CreatedDate,       // Thời gian đặt
					CustomerName = b.appCustHotel?.Name, // Tên khách hàng
					TotalAmount = b.appBill?.RoomPrice, // Tổng tiền phòng
					Discount = b.appBill?.DiscountPrice, // Giảm giá
					FinalPrice = b.appBill?.FinalPrice, // Tổng thanh toán

				}).Cast<dynamic>().ToList(); // Cast to dynamic

				return GenerateExcelFile(bookingExportData, timeRange);

			}


			var dateRange = Enumerable.Range(0, (endDate - startDate).Days)
									   .Select(i => startDate.AddDays(i))
									   .ToList();

			var chartData = dateRange.Select(day =>
			{
				var booking = bookingChartData.FirstOrDefault(x => x.Date == day.ToString("dd/MM/yyyy"));
				return new
				{
					Date = day.ToString("dd/MM/yyyy"),
					Count = booking?.Count ?? 0
				};
			}).ToList();

			var result = new
			{
				labels = chartData.Select(x => x.Date).ToList(),
				data = chartData.Select(x => x.Count).ToList()
			};

			return Json(result);
		}

		private IActionResult GenerateExcelFile(List<dynamic> bookings, string timeRange)
		{
			using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add("Booking Report");

				// Thêm tiêu đề cột
				worksheet.Cells[1, 1].Value = "Mã đặt phòng";
				worksheet.Cells[1, 2].Value = "Tên phòng";
				worksheet.Cells[1, 3].Value = "Loại phòng";
				worksheet.Cells[1, 4].Value = "Thời gian đặt";
				worksheet.Cells[1, 5].Value = "Thời gian nhận phòng";
				worksheet.Cells[1, 6].Value = "Thời gian trả phòng";
				worksheet.Cells[1, 7].Value = "Giá phòng";
				worksheet.Cells[1, 8].Value = "Tên khách hàng";
				worksheet.Cells[1, 9].Value = "Tổng phòng và Dịch vụ";
				worksheet.Cells[1, 10].Value = "Giảm giá";
				worksheet.Cells[1, 11].Value = "Tổng thanh toán";

				// Định dạng tiêu đề
				worksheet.Row(1).Style.Font.Bold = true;
				worksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

				// Add booking data to the worksheet
				for (int i = 0; i < bookings.Count; i++)
				{
					var booking = bookings[i];

					worksheet.Cells[i + 2, 1].Value = booking.Code ?? "";
					worksheet.Cells[i + 2, 2].Value = booking.RoomName ?? "";
					worksheet.Cells[i + 2, 3].Value = booking.Name ?? "";
					worksheet.Cells[i + 2, 4].Value = booking.CreatedDate?.ToString("dd/MM/yyyy HH:mm") ?? "";
					worksheet.Cells[i + 2, 5].Value = booking.CheckInExpectual?.ToString("dd/MM/yyyy") ?? "";
					worksheet.Cells[i + 2, 6].Value = booking.CheckOutActual?.ToString("dd/MM/yyyy HH:mm") ?? "";
					worksheet.Cells[i + 2, 7].Value = booking.Price ?? 0;
					worksheet.Cells[i + 2, 8].Value = booking.CustomerName ?? "";
					worksheet.Cells[i + 2, 9].Value = booking.TotalAmount ?? 0;
					worksheet.Cells[i + 2, 10].Value = booking.Discount ?? 0;
					worksheet.Cells[i + 2, 11].Value = booking.FinalPrice ?? "Đang sử dụng";
				}

				// Adjust column width
				worksheet.Cells.AutoFitColumns();

				// Generate Excel file
				var stream = new MemoryStream(package.GetAsByteArray());
				string fileName = $"Bao_cao_dat_phong_{timeRange}_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
				return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
			}
		}


		public IActionResult Revenue()
		{
			return View();
		}

		public IActionResult RevenueChart(string timeRange, bool isExport = false)
		{
			// Nếu không có tham số, mặc định là "thisWeek"
			if (string.IsNullOrEmpty(timeRange))
			{
				timeRange = "thisWeek";
			}
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity!;
			var IdGroupClaim = identity.FindFirst("IdGroup")?.Value;
			int IdGroup = int.Parse(IdGroupClaim);
			var hotel = _HotelDbContext.AppHotel.FirstOrDefault(x => x.IdGroup == IdGroup);
			if (hotel == null)
			{
				return BadRequest("Không tìm thấy khách sạn.");
			}

			DateTime startDate, endDate;
			switch (timeRange)
			{
				case "today":
					startDate = DateTime.Now.Date;
					endDate = startDate.AddDays(1);
					break;
				case "yesterday":
					startDate = DateTime.Now.Date.AddDays(-1);
					endDate = startDate.AddDays(1);
					break;
				case "thisWeek":
					startDate = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek + 1); // Bắt đầu từ Thứ Hai
					endDate = startDate.AddDays(7);
					break;
				case "lastWeek":
					startDate = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek - 6); // Tuần trước từ Thứ Hai
					endDate = startDate.AddDays(7);
					break;
				case "last7Days":
					startDate = DateTime.Now.Date.AddDays(-7);
					endDate = DateTime.Now.Date.AddDays(1);
					break;
				case "thisMonth":
					startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
					endDate = startDate.AddMonths(1);
					break;
				case "lastMonth":
					startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
					endDate = startDate.AddMonths(1);
					break;
				case "last30Days":
					startDate = DateTime.Now.Date.AddDays(-30);
					endDate = DateTime.Now.Date.AddDays(1);
					break;
				case "thisQuarter":
					int currentQuarter = (DateTime.Now.Month - 1) / 3 + 1;
					startDate = new DateTime(DateTime.Now.Year, (currentQuarter - 1) * 3 + 1, 1);
					endDate = startDate.AddMonths(3);
					break;
				case "lastQuarter":
					int lastQuarter = (DateTime.Now.Month - 1) / 3;
					startDate = new DateTime(DateTime.Now.Year, (lastQuarter - 1) * 3 + 1, 1);
					if (lastQuarter == 0) // Nếu là quý đầu tiên của năm
					{
						startDate = new DateTime(DateTime.Now.Year - 1, 10, 1);
					}
					endDate = startDate.AddMonths(3);
					break;
				case "thisYear":
					startDate = new DateTime(DateTime.Now.Year, 1, 1);
					endDate = startDate.AddYears(1);
					break;
				case "lastYear":
					startDate = new DateTime(DateTime.Now.Year - 1, 1, 1);
					endDate = startDate.AddYears(1);
					break;
				default:
					return BadRequest("Khoảng thời gian không hợp lệ.");
			}


			// Lấy tất cả hóa đơn
			var bills = _HotelDbContext.AppBill
				.Where(b => b.appBookingRoom.appRoom.appRoomCate.IdHotel == hotel.Id && b.appBookingRoom.Status == BookingStatus.SUCCESS)
				.Where(b => b.appBookingRoom.CheckInExpectual >= startDate && b.appBookingRoom.CheckInExpectual < endDate)
				.Include(x => x.appBookingRoom)
				.ThenInclude(x => x.appRoom)
				.ToList();

			// Tính toán doanh thu theo ngày
			var revenueChartData = bills
				.GroupBy(b => b.appBookingRoom.CheckOutActual.Value.Date)
				.Select(g => new
				{
					Date = g.Key.ToString("dd/MM/yyyy"), // Ngày
					Revenue = g.Sum(b => b.FinalPrice) // Tổng doanh thu
				})
				.ToList();

			// Nếu yêu cầu xuất Excel
			if (isExport)
			{
				var revenueExportData = bills.Select(b => new
				{
					b.appBookingRoom.CheckInExpectual, // Ngày nhận phòng
					b.appBookingRoom.CheckOutActual, // Ngày trả phòng
					b.appBookingRoom.Code,           // Mã đặt phòng
					RoomName = b.appBookingRoom.appRoom?.Name, // Tên phòng
					b.appBookingRoom.appRoom.appRoomCate.Name, // Loại phòng
					Revenue = b.FinalPrice,           // Doanh thu
				}).Cast<dynamic>().ToList();

				return GenerateExcelFileForRevenue(revenueExportData, timeRange);
			}

			// Dữ liệu cho biểu đồ
			var dateRange = Enumerable.Range(0, (endDate - startDate).Days)
				.Select(i => startDate.AddDays(i))
				.ToList();

			var chartData = dateRange.Select(day =>
			{
				var revenue = revenueChartData.FirstOrDefault(x => x.Date == day.ToString("dd/MM/yyyy"));
				return new
				{
					Date = day.ToString("dd/MM/yyyy"),
					Revenue = revenue?.Revenue ?? 0
				};
			}).ToList();

			var result = new
			{
				labels = chartData.Select(x => x.Date).ToList(),
				data = chartData.Select(x => x.Revenue).ToList()
			};

			return Json(result);
		}

		private IActionResult GenerateExcelFileForRevenue(List<dynamic> revenueData, string timeRange)
		{
			using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add("Revenue Report");

				// Thêm tiêu đề cột
				worksheet.Cells[1, 1].Value = "Ngày nhận phòng";
				worksheet.Cells[1, 2].Value = "Mã đặt phòng";
				worksheet.Cells[1, 3].Value = "Tên phòng";
				worksheet.Cells[1, 4].Value = "Loại phòng";
				worksheet.Cells[1, 5].Value = "Doanh thu";

				worksheet.Row(1).Style.Font.Bold = true;
				worksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

				// Thêm dữ liệu doanh thu vào bảng
				for (int i = 0; i < revenueData.Count; i++)
				{
					var item = revenueData[i];
					worksheet.Cells[i + 2, 1].Value = item.CheckInExpectual?.ToString("dd/MM/yyyy") ?? "";
					worksheet.Cells[i + 2, 2].Value = item.Code ?? "";
					worksheet.Cells[i + 2, 3].Value = item.RoomName ?? "";
					worksheet.Cells[i + 2, 4].Value = item.Name ?? "";
					worksheet.Cells[i + 2, 5].Value = item.Revenue ?? 0;
				}

				worksheet.Cells.AutoFitColumns();

				var stream = new MemoryStream(package.GetAsByteArray());
				string fileName = $"Bao_cao_doanh_thu_{timeRange}_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
				return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
			}
		}

	}
}
