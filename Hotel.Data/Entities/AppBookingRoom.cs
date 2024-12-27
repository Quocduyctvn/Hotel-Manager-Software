using Hotel.Data.Entities.Base;
using Hotel.Share.Enums;

namespace Hotel.Data.Entities
{
	public class AppBookingRoom : AppEntityBase
	{
		public string? Code { get; set; }
		public string? Desc { get; set; }
		public DateTime? CheckInExpectual { get; set; } // Tg nhận phòng dự kiến
		public DateTime? CheckInActual { get; set; }   // Tg nhận phòng thực tế
		public DateTime? CheckOutExpectual { get; set; } // Tg nhận phòng dự kiến
		public DateTime? CheckOutActual { get; set; }   // Tg nhận phòng thực tế
		public BookingStatus Status { get; set; }
		public double Price { get; set; } // gía gốc
		public int Adult { get; set; }
		public int Children { get; set; }
		public string? BookingConfirm { get; set; }
		public string? CheckinConfirm { get; set; }
		public string? CheckoutConfirm { get; set; }


		public int IdRentalPrice { get; set; }
		public AppRentalPrice appRentalPrice { set; get; }
		public int IdUser { get; set; }
		public AppUser appUser { get; set; }
		public int IdCustommer { get; set; }
		public AppCustHotel appCustHotel { get; set; }
		public int IdRoom { get; set; }
		public AppRoom appRoom { get; set; }

		public ICollection<AppComodityOrder> appComodityOrders { get; set; }
		public ICollection<AppServicesOrder> appServicesOrders { get; set; }
		public ICollection<AppIncurredFee> appIncurredFees { get; set; }
		public AppBill appBill { get; set; }
	}
}
