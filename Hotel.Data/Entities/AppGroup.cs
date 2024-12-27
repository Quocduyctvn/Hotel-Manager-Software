using Hotel.Data.Entities.Base;

namespace Hotel.Data.Entities
{
	public class AppGroup : AppEntityBase
	{
		public AppGroup()
		{
			appUsers = new HashSet<AppUser>();
			appRoles = new HashSet<AppRole>();
		}
		public string Name { get; set; }
		public string? Desc { get; set; }

		public AppHotels appHotels { get; set; }
		public AppHMS appHMS { get; set; }

		public ICollection<AppUser> appUsers { get; set; }
		public ICollection<AppRole> appRoles { get; set; }
		public ICollection<AppPermission> appPermissions { get; set; }
	}
}
