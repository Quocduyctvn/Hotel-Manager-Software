using Hotel.Data.Entities;
using Hotel.Share.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppRoomCateConfig : IEntityTypeConfiguration<AppRoomCate>
	{
		public void Configure(EntityTypeBuilder<AppRoomCate> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Name).HasMaxLength((int)MaxLength.MAX_256);

			builder.HasOne(x => x.appHotels)
				.WithMany(x => x.appRoomCates)
				.HasForeignKey(x => x.IdHotel);
		}
	}
}
