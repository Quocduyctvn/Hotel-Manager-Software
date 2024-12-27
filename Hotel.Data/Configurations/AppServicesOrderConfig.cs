using Hotel.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppServicesOrderConfig : IEntityTypeConfiguration<AppServicesOrder>
	{
		public void Configure(EntityTypeBuilder<AppServicesOrder> builder)
		{
			builder.HasKey(x => x.Id);

			builder.HasOne(x => x.appBookingRoom)
				.WithMany(x => x.appServicesOrders)
				.HasForeignKey(x => x.IdBookingRoom);
			builder.HasOne(x => x.appServices)
				.WithMany(x => x.appServicesOrders)
				.HasForeignKey(x => x.IdServices);
		}
	}
}
