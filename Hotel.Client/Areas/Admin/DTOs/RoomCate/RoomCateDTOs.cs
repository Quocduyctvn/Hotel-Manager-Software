using Hotel.Data.Entities;
using Hotel.Share.Const;
using Hotel.Share.Enums;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Client.Areas.Admin.DTOs.RoomCate
{
	public class RoomCateDTOs
	{
		[Required(ErrorMessage = MessageConst.REQUIRED)]
		public string Name { get; set; }
		[Required(ErrorMessage = MessageConst.REQUIRED)]
		[RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = MessageConst.VALID_NUMBER_REQUIRED)]
		[Range(0, double.MaxValue, ErrorMessage = MessageConst.GREATER_EQUAL_ZERO)]
		public double? EarlyCheckInFee { get; set; }
		[Required(ErrorMessage = MessageConst.REQUIRED)]
		[RegularExpression(RegexConst.INT_REGEX, ErrorMessage = MessageConst.GREATER_EQUAL_ZERO)]
		[Range(0, double.MaxValue, ErrorMessage = MessageConst.GREATER_EQUAL_ZERO)]
		public double? LateCheckOutFee { get; set; }
		[Required(ErrorMessage = MessageConst.REQUIRED)]
		[RegularExpression(RegexConst.INT_REGEX, ErrorMessage = MessageConst.GREATER_EQUAL_ZERO)]
		public int? StanderAdult { get; set; }
		[Required(ErrorMessage = MessageConst.REQUIRED)]
		[RegularExpression(RegexConst.INT_REGEX, ErrorMessage = MessageConst.GREATER_EQUAL_ZERO)]
		public int? StanderChildren { get; set; }
		[Required(ErrorMessage = MessageConst.REQUIRED)]
		[RegularExpression(RegexConst.INT_REGEX, ErrorMessage = MessageConst.GREATER_EQUAL_ZERO)]
		public int? MaxAdult { get; set; }
		[Required(ErrorMessage = MessageConst.REQUIRED)]
		[RegularExpression(RegexConst.INT_REGEX, ErrorMessage = MessageConst.GREATER_EQUAL_ZERO)]
		public int? MaxChildren { get; set; }
		public string? Desc { get; set; }
		public RoomCateStatus? Status { get; set; }


		public IFormFile? FormFile1 { get; set; }
		public IFormFile? FormFile2 { get; set; }
		public IFormFile? FormFile3 { get; set; }
		public IFormFile? FormFile4 { get; set; }

		public List<string>? FileStrings { get; set; }


		public AppHotels? appHotels { get; set; }

		public ICollection<AppImage>? appImages { get; set; }
		public ICollection<AppRentalPrice>? appRentalPrices { get; set; }
		public ICollection<AppRoom>? appRooms { get; set; }
		public ICollection<AppRoomCateAmenity>? appRoomsCateAmenity { get; set; }
	}
}
