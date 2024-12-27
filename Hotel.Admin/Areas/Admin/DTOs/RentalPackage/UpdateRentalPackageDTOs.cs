using Hotel.Data.Entities;
using Hotel.Share.Const;
using Hotel.Share.Enums;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Admin.Areas.Admin.DTOs.RentalPackage
{
	public class UpdateRentalPackageDTOs
	{
		[Required(ErrorMessage = "Thuộc tính là bắt buộc")]
		public string Name { get; set; }
		[Required(ErrorMessage = "Thuộc tính là bắt buộc")]
		[RegularExpression(RegexConst.INT_REGEX, ErrorMessage = "Dữ liệu phải là số nguyên dương")]
		public double Price { get; set; }
		public int? IdTimeOfPrice { get; set; }
		[RegularExpression(RegexConst.INT_REGEX, ErrorMessage = "Dữ liệu phải là số nguyên dương")]
		public int? TrialTime { get; set; }
		public int? IdTimeOfTrial { get; set; }
		public string? Desc { get; set; }
		[Required(ErrorMessage = "Thuộc tính là bắt buộc")]
		[RegularExpression(RegexConst.INT_REGEX, ErrorMessage = "Dữ liệu phải là số nguyên dương")]
		public int AccountLimit { get; set; }           // giới hạn tài khoản
		[Required(ErrorMessage = "Thuộc tính là bắt buộc")]
		[RegularExpression(RegexConst.INT_REGEX, ErrorMessage = "Dữ liệu phải là số nguyên dương")]
		public int RoomLimit { get; set; }              // giới hạn phòng
		[Required(ErrorMessage = "Thuộc tính là bắt buộc")]
		public string UsageTraining { get; set; }       // đào tạo sử dụng
		[Required(ErrorMessage = "Thuộc tính là bắt buộc")]
		public bool VisualReport { get; set; }         // báo cáo trực quan
		public RentalStatus? Status { get; set; }        // Active, Inactive
		public int? IdRentalPackageCate { get; set; }
		public AppRentalPackageCate? appRentalPackageCate { get; set; }
		public ICollection<AppHMSOrder>? appHMSOrders { get; set; }
	}
}
