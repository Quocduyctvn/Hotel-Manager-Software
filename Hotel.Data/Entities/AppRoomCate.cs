using Hotel.Data.Entities.Base;
using Hotel.Share.Enums;

namespace Hotel.Data.Entities
{
	public class AppRoomCate : AppEntityBase
	{
		public string Name { get; set; }
		public double EarlyCheckInFee { get; set; }
		public double LateCheckOutFee { get; set; }
		public int StanderAdult { get; set; }
		public int StanderChildren { get; set; }
		public int MaxAdult { get; set; }
		public int MaxChildren { get; set; }
		public int Position { get; set; }
		public string? Desc { get; set; }
		public RoomCateStatus Status { get; set; }
		public int IdHotel { get; set; }
		public AppHotels appHotels { get; set; }

		public ICollection<AppImage> appImages { get; set; }
		public ICollection<AppRentalPrice> appRentalPrices { get; set; }
		public ICollection<AppRoom> appRooms { get; set; }
		public ICollection<AppRoomCateAmenity> appRoomsCateAmenity { get; set; }
	}
}
