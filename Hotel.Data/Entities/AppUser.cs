using Hotel.Data.Entities.Base;

namespace Hotel.Data.Entities
{
	public class AppUser : AppEntityBase
	{
		public string? Name { get; set; }
		public string? Phone { get; set; }
		public string? Email { get; set; }
		public string Password { get; set; }
		public string? Avatar { get; set; }
		public int IdGroup { get; set; }
		public AppGroup? appGroup { get; set; }

		public int IdRole { get; set; }
		public AppRole? appRole { get; set; }
		public ICollection<AppBookingRoom> appBookingRooms { get; set; }
	}
}
