using Hotel.Data.Entities.Base;
using Hotel.Share.Enums;

namespace Hotel.Data.Entities
{
	public class AppHotels : AppEntityBase
	{
		public string Code { get; set; }
		public string Name { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }
		public string? Location { get; set; }
		public string? City { get; set; }
		public string? District { get; set; }
		public string? Avatar { get; set; }
		public EnumStatusHotel status { get; set; }
		public AppHMSOrder appHMSOrder { get; set; } // 1: 1

		public int IdGroup { get; set; }
		public AppGroup? appGroup { get; set; }
		public ICollection<AppAmenity> appAmenities { get; set; }
		public ICollection<AppRoomCate>? appRoomCates { get; set; }
		public ICollection<AppFloor>? appFloor { get; set; }
		public ICollection<AppCustHotel>? appCustHotels { get; set; }
		public ICollection<AppSvcCommoCate>? appSvcCommoCates { get; set; }
	}
}
