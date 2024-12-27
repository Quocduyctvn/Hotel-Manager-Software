using Hotel.Data.Entities.Base;

namespace Hotel.Data.Entities
{
	public class AppTimes : AppEntityBase
	{
		public string Name { get; set; }
		public int Time { get; set; }
	}
}
