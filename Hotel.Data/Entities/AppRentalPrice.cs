using Hotel.Data.Entities.Base;

namespace Hotel.Data.Entities
{
	public class AppRentalPrice : AppEntityBase
	{
		public int IdRoomCate { get; set; }
		public AppRoomCate appRoomCate { get; set; }
		public int IdRentalType { get; set; }
		public AppRentalType appRentalType { get; set; }
		public int IdDayType { get; set; }
		public AppDayType appDayType { get; set; }
		public double Price { get; set; }
		public ICollection<AppBookingRoom> appBookingRooms { get; set; }
	}
}
