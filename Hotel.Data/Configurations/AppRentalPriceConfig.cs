using Hotel.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppRentalPriceConfig : IEntityTypeConfiguration<AppRentalPrice>
	{
		public void Configure(EntityTypeBuilder<AppRentalPrice> builder)
		{
			builder.HasKey(x => x.Id);

			builder.HasOne(x => x.appRoomCate)
				.WithMany(x => x.appRentalPrices)
				.HasForeignKey(x => x.IdRoomCate);
			builder.HasOne(x => x.appRentalType)
				.WithMany(x => x.appRentalPrices)
				.HasForeignKey(x => x.IdRentalType);
			builder.HasOne(x => x.appDayType)
				.WithMany(x => x.appRentalPrice)
				.HasForeignKey(x => x.IdDayType);
		}
	}
}
