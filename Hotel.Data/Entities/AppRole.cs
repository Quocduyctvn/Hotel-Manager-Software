using Hotel.Data.Entities.Base;

namespace Hotel.Data.Entities
{
    public class AppRole : AppEntityBase
    {
        public AppRole()
        {
            appUsers = new HashSet<AppUser>();
        }
        public string Name { get; set; }
        public string? Desc { get; set; }

        public ICollection<AppUser> appUsers { get; set; }
        public int IdGroup { get; set; }
        public AppGroup appGroup { get; set; }
        public ICollection<AppRolePermission> appRolePers { get; set; }


    }
}
