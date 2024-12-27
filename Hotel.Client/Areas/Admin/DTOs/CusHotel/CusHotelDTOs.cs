using Hotel.Data.Entities;
using Hotel.Share.Const;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Client.Areas.Admin.DTOs.CusHotel
{
	public class CusHotelDTOs
	{
		public int? IdRoom { get; set; }
		public string? CusName { get; set; }
		public DateTime? BirthDay { get; set; }
		[RegularExpression(RegexConst.EMAIL, ErrorMessage = "Vui lòng nhập đúng định dạng mail (vd: hotel@gmail.com)")]
		public string? Email { get; set; }
		[RegularExpression(RegexConst.PHONE_NUMBER, ErrorMessage = "Vui lòng nhập đúng định dạng (vd: 0925886544)")]
		public string? Phone { get; set; }
		public string? Country { get; set; }
		public string? Address { get; set; }
		public string? Desc { get; set; }

		public IFormFile? FormFile1 { get; set; }
		public IFormFile? FormFile2 { get; set; }
		public string? stringFile1 { get; set; }
		public string? stringFile2 { get; set; }

		public ICollection<AppBookingRoom>? appBookingRooms { get; set; }

		public ICollection<AppImage>? appImages { get; set; }
	}
}
