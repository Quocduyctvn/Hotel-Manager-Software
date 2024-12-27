using Hotel.Data.Entities.Base;

namespace Hotel.Data.Entities
{
	public class AppSvcCommoCate : AppEntityBase
	{
		public string Name { get; set; }
		public int IdHotel { get; set; }
		public AppHotels appHotels { get; set; }
		public ICollection<AppCommodity> appCommodities { get; set; }
		public ICollection<AppServices> appServices { get; set; }
	}
}
