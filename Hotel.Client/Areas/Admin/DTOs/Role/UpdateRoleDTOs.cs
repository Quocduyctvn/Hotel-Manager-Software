namespace Hotel.Client.Areas.Admin.DTOs.Role
{
	public class UpdateRoleDTOs
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Desc { get; set; }
		public string IdPermission { get; set; }

		public string DeletedIdPermission { get; set; }
		public string AddedIdPermission { get; set; }
	}
}
