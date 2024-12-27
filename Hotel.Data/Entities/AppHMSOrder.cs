using Hotel.Data.Entities.Base;
using Hotel.Share.Enums;

namespace Hotel.Data.Entities
{
    public class AppHMSOrder : AppEntityBase
    {
        public AppHMSOrder() { }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double TotalTime { get; set; }
        public int IdTime { get; set; }
        public double TotalFee { get; set; }
        public string? Notes { get; set; }
        public EnumHMSOrder Status { get; set; }
        public int IdPaymentMethod { get; set; }
        public int IdRentalPackage { get; set; }
        public AppRentalPackage appRentalPackage { get; set; }
        public int IdHotel { get; set; }
        public AppHotels appHotels { get; set; }
    }
}
