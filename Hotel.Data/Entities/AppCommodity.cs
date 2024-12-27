using Hotel.Data.Entities.Base;
using Hotel.Share.Enums;

namespace Hotel.Data.Entities
{
	public class AppCommodity : AppEntityBase
	{
		public AppCommodity()
		{
			appImages = new List<AppImage>();
		}
		public string Name { get; set; }
		public decimal CostPrice { get; set; } // Giá vốn
		public decimal SellingPrice { get; set; } // Giá bán
		public int Stock { get; set; } // Tồn kho
		public string? Desc { get; set; }
		public int Position { get; set; }
		public CommodityStatus Status { get; set; }
		public ICollection<AppComodityOrder> appComodityOrders { get; set; }
		public int IdSvcCommoCate { get; set; }
		public AppSvcCommoCate appSvcCommoCate { get; set; }
		public ICollection<AppImage> appImages { get; set; }
	}
}
