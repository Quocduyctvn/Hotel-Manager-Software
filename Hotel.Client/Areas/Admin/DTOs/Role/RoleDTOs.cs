using System.ComponentModel.DataAnnotations;

namespace Hotel.Client.Areas.Admin.DTOs.Role
{
    public class RoleDTOs
    {
        [Required(ErrorMessage = "Tên Vai trò là bắt buột")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Thuộc tính là bắt buột")]
        public string Desc { get; set; }
        public string IdPermission { get; set; }
    }
}
