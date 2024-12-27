using Hotel.Data.Entities.Base;

namespace Hotel.Data.Entities
{
	public class AppComodityOrder : AppEntityBase
	{
		public int Quantity { get; set; }
		public double Price { get; set; }
		public int IdCommodities { get; set; }
		public AppCommodity appCommodity { get; set; }
		public int IdBookingRoom { get; set; }
		public AppBookingRoom appBookingRoom { get; set; }
	}
}
