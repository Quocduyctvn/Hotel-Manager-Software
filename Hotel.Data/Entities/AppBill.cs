
using Hotel.Data.Entities.Base;

namespace Hotel.Data.Entities
{
	public class AppBill : AppEntityBase
	{
		public string? Path { get; set; }
		public double RoomPrice { get; set; } // gía gốc
		public double DiscountPrice { get; set; }   // giá giảm
		public double FinalPrice { get; set; } // giả cuối cùng
		public string Desc { get; set; }
		public int IdBooking { get; set; }
		public AppBookingRoom appBookingRoom { get; set; }
	}
}
