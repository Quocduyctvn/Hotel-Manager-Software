using Hotel.Data.Entities.Base;

namespace Hotel.Data.Entities
{
    public class AppHMS : AppEntityBase  // Hotel manager soft ware 
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }

        public int IdGroup { get; set; }
        public AppGroup appGroup { get; set; }
    }
}
