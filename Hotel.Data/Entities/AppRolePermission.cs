using Hotel.Data.Entities.Base;

namespace Hotel.Data.Entities
{
	public class AppRolePermission : AppEntityBase
	{
		public int IdRole { get; set; }
		public int IdPermission { get; set; }


		public AppRole appRole { get; set; }
        public int? IdGroup { get; set; }
        public AppPermission appPermission { get; set; }
	}
}
