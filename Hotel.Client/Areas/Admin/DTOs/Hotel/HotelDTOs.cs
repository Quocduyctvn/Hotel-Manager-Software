namespace Hotel.Client.Areas.Admin.DTOs.Hotel
{
	public class HotelDTOs
	{
		public string Name { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }
		public string? Location { get; set; }
		public string? City { get; set; }
		public string? District { get; set; }
		public string? Avatar { get; set; }
		public IFormFile? FileAvatar { get; set; }
	}
}
