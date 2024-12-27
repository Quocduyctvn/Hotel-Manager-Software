

using Hotel.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppRoomCateAmenityConfig : IEntityTypeConfiguration<AppRoomCateAmenity>
	{
		public void Configure(EntityTypeBuilder<AppRoomCateAmenity> builder)
		{
			builder.HasKey(x => x.Id);

			builder.HasOne(x => x.appRoomCate)
				.WithMany(x => x.appRoomsCateAmenity)
				.HasForeignKey(x => x.IdRoomCate);
			builder.HasOne(x => x.appAmenity)
				.WithMany(x => x.appRoomCateAmenities)
				.HasForeignKey(x => x.IdAmenity);
		}
	}
}
