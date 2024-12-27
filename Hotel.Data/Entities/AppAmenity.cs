using Hotel.Data.Entities.Base;

namespace Hotel.Data.Entities
{
	public class AppAmenity : AppEntityBase
	{
		public string Name { get; set; }
		public string? Desc { get; set; }
		public int Position { get; set; }
		public int IdHotel { get; set; }
		public AppHotels appHotels { get; set; }
		public ICollection<AppRoomCateAmenity> appRoomCateAmenities { get; set; }
	}
}
