using Hotel.Data.Entities;
using Hotel.Share.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppHMSOrderConfig : IEntityTypeConfiguration<AppHMSOrder>
	{
		public void Configure(EntityTypeBuilder<AppHMSOrder> builder)
		{
			//builder.ToTable(("hotel_manager_software_order"));
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Notes).HasMaxLength((int)MaxLength.MAX_512);

			builder.HasOne(x => x.appRentalPackage)
					.WithMany(x => x.appHMSOrders)
					.HasForeignKey(x => x.IdRentalPackage);

			builder.HasOne(x => x.appHotels)
				   .WithOne(x => x.appHMSOrder)
				   .HasForeignKey<AppHMSOrder>(x => x.IdHotel);
		}
	}
}
