using Hotel.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppComodityOrderConfig : IEntityTypeConfiguration<AppComodityOrder>
	{
		public void Configure(EntityTypeBuilder<AppComodityOrder> builder)
		{
			builder.HasKey(x => x.Id);

			builder.HasOne(x => x.appBookingRoom)
				.WithMany(x => x.appComodityOrders)
				.HasForeignKey(x => x.IdBookingRoom);
			builder.HasOne(x => x.appCommodity)
				.WithMany(x => x.appComodityOrders)
				.HasForeignKey(x => x.IdCommodities);
		}
	}
}
