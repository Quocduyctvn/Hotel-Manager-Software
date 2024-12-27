
namespace Hotel.Share.Enums
{
    public enum RoomStatus
    {
        // Trạng thái phòng đang được sử dụng
        OCCUPIED = 0,

        // Trạng thái phòng còn trống
        AVAILABLE = 1,

        // Trạng thái phòng sắp nhận
        CHECKING_IN = 2,

        // Trạng thái phòng sắp trả
        CHECKING_OUT = 3,

        // Trạng thái phòng quá giờ
        OVERDUE = 4,

        // Trạng thái phòng chưa dọn
        //NOT_CLEANED = 5, // Chưa dọn phòng (phòng dơ)

        // Trạng thái phòng ngưng hoạt động
        INACTIVE = 6, // Ngưng hoạt động

        // Đã xóa 
        IS_DELETED = 7,
    }
}
