using Hotel.Data.Entities.Base;

namespace Hotel.Data.Entities
{
	public class AppIncurredFee : AppEntityBase  // Phụ thu - (nhận & trả sớm) 
	{
		public string Name { get; set; }
		public int Quantity { get; set; }
		public double Price { get; set; }
		public int IdBookingRoom { get; set; }
		public AppBookingRoom appBookingRoom { get; set; }
	}
}
