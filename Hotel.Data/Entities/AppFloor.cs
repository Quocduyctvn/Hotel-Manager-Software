using Hotel.Data.Entities.Base;

namespace Hotel.Data.Entities
{
    public class AppFloor : AppEntityBase
    {
        public string FloorNumber { get; set; }
        public string Desc { get; set; }
        public int Position { get; set; }
        public int IdHotel { get; set; }
        public AppHotels appHotels { get; set; }
        public ICollection<AppRoom> appRooms { get; set; }
    }
}
