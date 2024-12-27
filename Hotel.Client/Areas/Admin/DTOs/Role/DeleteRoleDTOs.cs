namespace Hotel.Client.Areas.Admin.DTOs.Role
{
	public class DeleteRoleDTOs
	{
		public DeleteRoleDTOs()
		{
			appUsers = new List<RoleDeleteVM_User>();
		}
		public int IdRole { get; set; }
		public int IdNewRole { get; set; }
		public string Name { get; set; }
		public string Desc { get; set; }
		public DateTime CreateDate { get; set; }
		public List<RoleDeleteVM_User> appUsers { get; set; }
	}
	public class RoleDeleteVM_User
	{
		public int IdUser { get; set; }
		public string Name { get; set; }
	}
}
