using Hotel.Data.Entities.Base;

namespace Hotel.Data.Entities
{
	public class AppServicesOrder : AppEntityBase
	{
		public int Quantity { get; set; }
		public double Price { get; set; }
		public int IdServices { get; set; }
		public AppServices appServices { get; set; }
		public int IdBookingRoom { get; set; }
		public AppBookingRoom appBookingRoom { get; set; }
	}
}
