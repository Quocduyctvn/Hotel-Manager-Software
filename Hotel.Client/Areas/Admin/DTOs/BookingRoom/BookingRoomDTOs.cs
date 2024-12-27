using Hotel.Data.Entities;
using Hotel.Share.Enums;

namespace Hotel.Client.Areas.Admin.DTOs.BookingRoom
{
    public class BookingRoomDTOs
    {
        public string Name { get; set; }
        public double EarlyCheckInFee { get; set; }
        public double LateCheckOutFee { get; set; }
        public int StanderAdult { get; set; }
        public int StanderChildren { get; set; }
        public int MaxAdult { get; set; }
        public int MaxChildren { get; set; }
        public int Position { get; set; }
        public string Desc { get; set; }
        public RoomCateStatus Status { get; set; }
        public int IdHotel { get; set; }

        public double HourlyRate { get; set; }
        public double OvernightRate { get; set; }
        public double FullDayRate { get; set; }


        public AppHotels appHotels { get; set; }
        public ICollection<AppImage> appImages { get; set; }
        public ICollection<AppRentalPrice> appRentalPrices { get; set; }
        public ICollection<AppRoom> appRooms { get; set; }
        public ICollection<AppRoomCateAmenity> appRoomsCateAmenity { get; set; }

    }
}
