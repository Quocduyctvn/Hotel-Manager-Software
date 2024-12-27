using Hotel.Data.Entities;
using Hotel.Share.Enums;

namespace Hotel.Client.Areas.Admin.DTOs.BookingRoom
{
	public class AddBookingRoomDTOs
	{
		public string RoomCateName { get; set; }
		public string? Desc { get; set; }
		public DateTime? CheckInExpectual { get; set; } // Tg nhận phòng dự kiến
		public DateTime? CheckInActual { get; set; }   // Tg nhận phòng thực tế 
		public DateTime? CheckOutExpectual { get; set; } // Tg nhận phòng dự kiến
		public DateTime? CheckOutActual { get; set; }   // Tg nhận phòng thực tế 
		public BookingStatus Status { get; set; }
		public double RoomPrice { get; set; } // gía gốc
		public double DiscountPrice { get; set; }   // giá giảm
		public double FinalPeice { get; set; } // giả cuối cùng


		public int Adult { get; set; }
		public int Children { get; set; }
		public int StanderAdult { get; set; }
		public int StanderChildren { get; set; }
		public int MaxAdult { get; set; }
		public int MaxChildren { get; set; }
		public int IdRentalType { get; set; }

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


		public string? CusName { get; set; }
		public DateTime? BirthDay { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }
		public string? Country { get; set; }
		public string? Address { get; set; }
		public string? CusDesc { get; set; }

		public IFormFile? FormFile1 { get; set; }
		public IFormFile? FormFile2 { get; set; }
		public string? stringFile1 { get; set; }
		public string? stringFile2 { get; set; }

	}
}
