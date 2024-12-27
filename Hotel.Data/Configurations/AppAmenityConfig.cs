using Hotel.Data.Entities;
using Hotel.Share.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppAmenityConfig : IEntityTypeConfiguration<AppAmenity>
	{
		public void Configure(EntityTypeBuilder<AppAmenity> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Name).HasMaxLength((int)MaxLength.MAX_256);
			builder.Property(x => x.Desc).HasMaxLength((int)MaxLength.MAX_1024);

			builder.HasOne(x => x.appHotels)
				.WithMany(x => x.appAmenities)
				.HasForeignKey(x => x.IdHotel)
				.OnDelete(DeleteBehavior.NoAction);
		}
	}
}
