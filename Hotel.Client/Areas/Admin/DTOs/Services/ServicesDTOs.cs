using Hotel.Data.Entities;
using Hotel.Share.Enums;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Client.Areas.Admin.DTOs.Services
{
	public class ServicesDTOs
	{
		public int? Id { get; set; }
		[Required(ErrorMessage = "Thuộc tính là bắt buột")]
		public string? Name { get; set; }
		[Required(ErrorMessage = "Thuộc tính là bắt buột")]
		public decimal? Price { get; set; } // Giá thuê
		public string? Desc { get; set; }
		public ServicesStatus? Status { get; set; }
		public int? IdSvcCommocate { get; set; }
		public AppSvcCommoCate? appSvcCommoCate { get; set; }
		public ICollection<AppServicesOrder>? appServicesOrders { get; set; }
		public ICollection<AppImage>? appImages { get; set; }


		public IFormFile? FormFile1 { get; set; }
		public IFormFile? FormFile2 { get; set; }
		public List<string>? FileStrings { get; set; }
	}
}
