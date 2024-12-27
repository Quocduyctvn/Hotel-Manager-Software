using Hotel.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Client.Areas.Admin.DTOs.Floor
{
	public class FloorDTOs
	{
		public int? Id { get; set; }
		[Required(ErrorMessage = "Thuộc tính là bắt buột")]
		public string FloorNumber { get; set; }
		public string? Desc { get; set; }
		public int? IdHotel { get; set; }
		public AppHotels? appHotels { get; set; }
		public ICollection<AppRoom>? appRooms { get; set; }
	}
}
