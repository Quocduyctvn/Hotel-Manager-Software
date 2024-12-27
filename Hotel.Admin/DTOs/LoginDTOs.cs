using System.ComponentModel.DataAnnotations;

namespace Hotel.Admin.DTOs
{
	public class LoginDTOs
	{
		[Required(ErrorMessage = "Thuộc tính là bắt buột")]
		public string Phone { get; set; }
		[Required(ErrorMessage = "Thuộc tính là bắt buột")]
		public string Password { get; set; }
	}
}
