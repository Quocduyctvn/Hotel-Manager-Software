using Hotel.Share.Const;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Client.Areas.Admin.DTOs.User
{
	public class CreateUserDTOs
	{
		public string? Name { get; set; }
		public string? Phone { get; set; }
		public string? Email { get; set; }
		[Required(ErrorMessage = "Vui lòng nhập trường này!")]
		[RegularExpression(RegexConst.PASSWORD, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự, bao gồm ít nhất 1 chữ hoa, 1 chữ thường, 1 số và 1 ký tự đặc biệt.")]
		public string Password { get; set; }
		[MinLength(4, ErrorMessage = "Mật khẩu chưa đủ mạnh!")]
		[Compare("Password", ErrorMessage = "Mật khẩu không khớp")]

		public string Cfm_Password { get; set; }
		public IFormFile? fileAvatar { get; set; }

		public string? StrAvatar { get; set; }

		public int IdRole { get; set; }
	}
}
