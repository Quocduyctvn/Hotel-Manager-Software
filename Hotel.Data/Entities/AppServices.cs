using Hotel.Data.Entities.Base;
using Hotel.Share.Enums;

namespace Hotel.Data.Entities
{
	public class AppServices : AppEntityBase
	{
		public string Name { get; set; }
		public decimal Price { get; set; } // Giá thuê
		public string? Desc { get; set; }
		public int Position { get; set; }
		public ServicesStatus Status { get; set; }
		public int IdSvcCommocate { get; set; }
		public AppSvcCommoCate appSvcCommoCate { get; set; }
		public ICollection<AppServicesOrder> appServicesOrders { get; set; }
		public ICollection<AppImage> appImages { get; set; }
	}
}
