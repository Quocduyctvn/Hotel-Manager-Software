using Hotel.Share.Const;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Client.DTOs
{
    public class ValidMailDTOs
    {

        [RegularExpression(RegexConst.EMAIL, ErrorMessage = "Vui lòng nhập đúng định dạng mail (vd: hotel@gmail.com)")]
        public string Gmail { get; set; }
    }
}
