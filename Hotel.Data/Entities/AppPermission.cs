using Hotel.Data.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Data.Entities
{
    public class AppPermission : AppEntityBase
    {
        [NotMapped] // Loại bỏ thuộc tính Id khỏi ánh xạ database
        public new int Id { get; set; }


        public int PerCode { get; set; }
        public string Code { get; set; }
        public string Table { get; set; }
        public string GroupName { get; set; }
        public string? Desc { get; set; }

        public ICollection<AppRolePermission> appRolePermissions { get; set; }

        public int IdGroup { get; set; }
        public AppGroup appGroup { get; set; }
    }
}
