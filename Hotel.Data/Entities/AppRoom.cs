using Hotel.Data.Entities.Base;
using Hotel.Share.Enums;

namespace Hotel.Data.Entities
{
    public class AppRoom : AppEntityBase
    {
        public string Name { get; set; }
        public string? Desc { get; set; }
        public int Position { get; set; }
        public DateTime UseStartDate { get; set; }
        public RoomStatus Status { get; set; }
        public CleanRoomStatus CleanStatus { get; set; }
        public int IdRoomCate { get; set; }
        public AppRoomCate appRoomCate { get; set; }
        public int IdFloor { get; set; }
        public AppFloor appFloor { get; set; }
        public ICollection<AppBookingRoom> appBookingRooms { get; set; }
    }
}
