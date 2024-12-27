using Hotel.Data.Entities.Base;

namespace Hotel.Data.Entities
{
	public class AppRentalType : AppEntityBase
	{
		public string Name { get; set; }
		public ICollection<AppRentalPrice> appRentalPrices { get; set; }
	}
}
