using Hotel.Data.Entities;
using Hotel.Share.Enums;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Client.Areas.Admin.DTOs.Room
{
	public class RoomDTOs
	{
		public int? Id { get; set; }
		[Required(ErrorMessage = "Tên phòng là bắt buộc.")]
		public string Name { get; set; }

		public string? Desc { get; set; }

		[Required(ErrorMessage = "Ngày bắt đầu sử dụng là bắt buộc.")]
		[DataType(DataType.Date)]
		public DateTime UseStartDate { get; set; }
		public RoomStatus? Status { get; set; }

		[Required(ErrorMessage = "Hạng phòng là bắt buộc.")]
		public int IdRoomCate { get; set; }

		public AppRoomCate? appRoomCate { get; set; }

		[Required(ErrorMessage = "Tầng là bắt buộc.")]
		public int IdFloor { get; set; }

		public AppFloor? appFloor { get; set; }

		public ICollection<AppBookingRoom>? appBookingRooms { get; set; }
	}
}
