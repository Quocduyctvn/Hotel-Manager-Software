using Hotel.Data.Entities;
using Hotel.Share.Const;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Client.Areas.Admin.DTOs.Commodity
{
	public class CommodityDTOs
	{
		public int? Id { get; set; }

		[Required(ErrorMessage = "Thuộc tính là bắt buột")]
		public string Name { get; set; }
		[Required(ErrorMessage = "Thuộc tính là bắt buột")]
		[RegularExpression(RegexConst.INT_REGEX, ErrorMessage = "Giá trị phải là số nguyên dương")]
		public decimal? CostPrice { get; set; } // Giá vốn
		[Required(ErrorMessage = "Thuộc tính là bắt buột")]
		[RegularExpression(RegexConst.INT_REGEX, ErrorMessage = "Giá trị phải là số nguyên dương")]
		public decimal? SellingPrice { get; set; } // Giá bán
		[Required(ErrorMessage = "Thuộc tính là bắt buột")]
		[RegularExpression(RegexConst.INT_REGEX, ErrorMessage = "Giá trị phải là số nguyên dương")]
		public int? Stock { get; set; } // Tồn kho
		public string? Desc { get; set; }
		public ICollection<AppComodityOrder>? appComodityOrders { get; set; }
		public int? IdSvcCommoCate { get; set; }
		public AppSvcCommoCate? appSvcCommoCate { get; set; }
		public ICollection<AppImage>? appImages { get; set; }



		public IFormFile? FormFile1 { get; set; }
		public IFormFile? FormFile2 { get; set; }
	}
}
