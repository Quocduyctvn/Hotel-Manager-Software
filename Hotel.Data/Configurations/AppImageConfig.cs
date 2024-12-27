using Hotel.Data.Entities;
using Hotel.Share.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppImageConfig : IEntityTypeConfiguration<AppImage>
	{
		public void Configure(EntityTypeBuilder<AppImage> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Path).HasMaxLength((int)MaxLength.MAX_512);

			builder.HasOne(x => x.appRoomCate)
				.WithMany(x => x.appImages)
				.HasForeignKey(x => x.IdRoomCate);
			builder.HasOne(x => x.appCommodity)
				.WithMany(x => x.appImages)
				.HasForeignKey(x => x.IdCommodity);
			builder.HasOne(x => x.appServices)
				.WithMany(x => x.appImages)
				.HasForeignKey(x => x.IdServices);
			builder.HasOne(x => x.appCustHotel)
				.WithMany(x => x.appImages)
				.HasForeignKey(x => x.IdCustHotel);
		}
	}
}
