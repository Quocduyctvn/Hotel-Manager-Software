using Hotel.Data.Entities.Base;

namespace Hotel.Data.Entities
{
	public class AppImage : AppEntityBase
	{
		public int? IdRoomCate { get; set; }
		public AppRoomCate appRoomCate { get; set; }
		public int? IdServices { get; set; }
		public AppServices appServices { get; set; }
		public int? IdCommodity { get; set; }
		public AppCommodity appCommodity { get; set; }
		public int? IdCustHotel { get; set; }
		public AppCustHotel appCustHotel { get; set; }
		public string Path { get; set; }
	}
}
