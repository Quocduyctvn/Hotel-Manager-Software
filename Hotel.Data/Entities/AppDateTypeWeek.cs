using Hotel.Data.Entities.Base;

namespace Hotel.Data.Entities
{
    public class AppDateTypeWeek : AppEntityBase
    {
        public int WeekDay { get; set; }
        public int IdDayType { get; set; }
        public AppDayType appDayType { get; set; }
        public int? IdHotel { get; set; }
    }
}
