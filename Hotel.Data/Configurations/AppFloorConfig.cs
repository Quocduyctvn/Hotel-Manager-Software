using Hotel.Data.Entities;
using Hotel.Share.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppFloorConfig : IEntityTypeConfiguration<AppFloor>
	{
		public void Configure(EntityTypeBuilder<AppFloor> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.FloorNumber).HasMaxLength((int)MaxLength.MAX_128);
			builder.Property(x => x.Desc).HasMaxLength((int)MaxLength.MAX_512);

			builder.HasOne(x => x.appHotels)
				.WithMany(x => x.appFloor)
				.HasForeignKey(x => x.IdHotel);
		}

	}
}
