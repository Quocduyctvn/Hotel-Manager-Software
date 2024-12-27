using Hotel.Data.Entities.Base;
using Hotel.Share.Enums;

namespace Hotel.Data.Entities
{
    public class AppRentalPackageCate : AppEntityBase
    {
        public AppRentalPackageCate()
        {
            appRentalPackage = new HashSet<AppRentalPackage>();
        }
        public string Name { get; set; }
        public RentalStatus Status { get; set; }
        public ICollection<AppRentalPackage> appRentalPackage { get; set; }
    }
}
