using Hotel.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppSvcCommoCateConfig : IEntityTypeConfiguration<AppSvcCommoCate>
	{
		public void Configure(EntityTypeBuilder<AppSvcCommoCate> builder)
		{
			builder.HasKey(x => x.Id);

			builder.HasOne(x => x.appHotels)
				.WithMany(x => x.appSvcCommoCates)
				.HasForeignKey(x => x.IdHotel);
		}
	}
}
