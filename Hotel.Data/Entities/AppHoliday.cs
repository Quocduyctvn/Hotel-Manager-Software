using Hotel.Data.Entities.Base;

namespace Hotel.Data.Entities
{
	public class AppHoliday : AppEntityBase
	{
		public string Name { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int? Position { get; set; }
		public int IdDayType { get; set; }
		public AppDayType appDayType { get; set; }
		public int? IdHotel { get; set; }
	}
}
