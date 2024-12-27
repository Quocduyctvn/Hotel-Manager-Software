using Hotel.Share.Const;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Client.DTOs
{
    public class RegisterDTOs
    {
        [Required(ErrorMessage = "Vui lòng nhập trường này!")]
        public string HtelName { get; set; }
        public string HtelCity { get; set; }
        public string HtelDistrict { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập trường này!")]
        [RegularExpression(RegexConst.PHONE_NUMBER, ErrorMessage = "Vui lòng nhập đúng định dạng (vd: 0925886544)")]
        public string HtelPhone { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập trường này!")]
        public string AdMail { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập trường này!")]
        [RegularExpression(RegexConst.PASSWORD, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự, bao gồm ít nhất 1 chữ hoa, 1 chữ thường, 1 số và 1 ký tự đặc biệt.")]
        public string AdPass { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập trường này!")]
        [Compare("AdPass", ErrorMessage = "Mật khẩu không khớp")]
        public string AdCf_Pass { get; set; }

        [RegularExpression(RegexConst.INT_REGEX, ErrorMessage = "Dữ liệu phải là số nguyên dương")]
        public int? Time { get; set; }
        public int? IdTime { get; set; }
    }
}
