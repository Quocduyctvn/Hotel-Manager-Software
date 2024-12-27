using Hotel.Data.Entities.Base;

namespace Hotel.Data.Entities
{
	public class AppCustHotel : AppEntityBase
	{
		public string Code { get; set; }
		public string Name { get; set; }
		public DateTime? BirthDay { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }
		public string? Country { get; set; }
		public string? Address { get; set; }
		public string? Desc { get; set; }
		public int IdHotel { get; set; }
		public AppHotels appHotels { get; set; }
		public ICollection<AppBookingRoom> appBookingRooms { get; set; }

		public ICollection<AppImage> appImages { get; set; }
	}
}
