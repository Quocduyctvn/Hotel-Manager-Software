using Hotel.Data.Entities.Base;

namespace Hotel.Data.Entities
{
	public class AppDayType : AppEntityBase
	{
		public string Name { get; set; }
		public ICollection<AppDateTypeWeek> appDateTypeWeeks { get; set; }
		public ICollection<AppHoliday> appHolidays { get; set; }
		public ICollection<AppRentalPrice> appRentalPrice { get; set; }
	}
}
