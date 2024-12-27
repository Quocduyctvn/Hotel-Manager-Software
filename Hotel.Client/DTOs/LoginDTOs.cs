using Hotel.Share.Const;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Client.DTOs
{
    public class LoginDTOs
    {
        [Required(ErrorMessage = "Vui lòng nhập trường này!")]
        public string HtelCode { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập trường này!")]
        [RegularExpression(RegexConst.EMAIL, ErrorMessage = "Vui lòng nhập đúng định dạng mail (vd: hotel@gmail.com)")]
        public string Email { get; set; }
        [RegularExpression(RegexConst.PASSWORD, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự, bao gồm ít nhất 1 chữ hoa, 1 chữ thường, 1 số và 1 ký tự đặc biệt.")]
        [Required(ErrorMessage = "Vui lòng nhập trường này!")]
        public string Pass { get; set; }
    }
}
