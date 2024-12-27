using Hotel.Data.Entities.Base;

namespace Hotel.Data.Entities
{
	public class AppContact : AppEntityBase
	{
		public string Name { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public string Content { get; set; }
		public bool Status { get; set; }

	}
}
