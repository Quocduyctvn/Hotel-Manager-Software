using Hotel.Data.Entities.Base;
using Hotel.Share.Enums;

namespace Hotel.Data.Entities
{
	public class AppRentalPackage : AppEntityBase
	{
		public AppRentalPackage()
		{
			appHMSOrders = new HashSet<AppHMSOrder>();
		}
		public string Name { get; set; }
		public double Price { get; set; }
		public int IdTimeOfPrice { get; set; }
		public int? TrialTime { get; set; }
		public int? IdTimeOfTrial { get; set; }
		public string? Desc { get; set; }
		public int AccountLimit { get; set; }           // giới hạn tài khoản
		public int RoomLimit { get; set; }              // giới hạn phòng
		public string UsageTraining { get; set; }       // đào tạo sử dụng
		public bool VisualReport { get; set; }         // báo cáo trực quan
		public RentalStatus Status { get; set; }        // Active, Inactive
		public int Position { get; set; }
		public int IdRentalPackageCate { get; set; }
		public AppRentalPackageCate appRentalPackageCate { get; set; }
		public ICollection<AppHMSOrder> appHMSOrders { get; set; }
	}
}
