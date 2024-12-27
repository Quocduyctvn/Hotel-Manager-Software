using Hotel.Data.Entities.Base;

namespace Hotel.Data.Entities
{
	public class AppRoomCateAmenity : AppEntityBase
	{
		public int IdAmenity { get; set; }
		public AppAmenity appAmenity { get; set; }
		public int IdRoomCate { get; set; }
		public AppRoomCate appRoomCate { get; set; }
	}
}
