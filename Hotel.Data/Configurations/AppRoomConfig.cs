using Hotel.Data.Entities;
using Hotel.Share.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppRoomConfig : IEntityTypeConfiguration<AppRoom>
	{
		public void Configure(EntityTypeBuilder<AppRoom> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Name).HasMaxLength((int)MaxLength.MAX_256);
			builder.Property(x => x.Desc).HasMaxLength((int)MaxLength.MAX_512);

			builder.HasOne(x => x.appRoomCate)
				.WithMany(x => x.appRooms)
				.HasForeignKey(x => x.IdRoomCate);
			builder.HasOne(x => x.appFloor)
				.WithMany(x => x.appRooms)
				.HasForeignKey(x => x.IdFloor)
				.OnDelete(DeleteBehavior.Restrict);

		}
	}
}
