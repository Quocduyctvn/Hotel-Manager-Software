namespace Hotel.Client.Areas.Admin.DTOs.BookingRoom
{
	public class UpdateQuantityDTOs
	{
		public int RoomId { get; set; }
		public string Field { get; set; }
		public int Value { get; set; } // Đổi kiểu thành int
	}

}
