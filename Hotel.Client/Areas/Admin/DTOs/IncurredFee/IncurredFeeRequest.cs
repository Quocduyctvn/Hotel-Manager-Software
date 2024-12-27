namespace Hotel.Client.Areas.Admin.DTOs.IncurredFee
{
	public class IncurredFeeRequest
	{
		public int BookingId { get; set; }
		public int Type { get; set; }
		public bool IsChecked { get; set; }
	}
}
