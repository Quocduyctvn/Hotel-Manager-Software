using Hotel.Data.Entities;
using Hotel.Share.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppCustHotelConfig : IEntityTypeConfiguration<AppCustHotel>
	{
		public void Configure(EntityTypeBuilder<AppCustHotel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Code).HasMaxLength((int)MaxLength.MAX_96);
			builder.Property(x => x.Name).HasMaxLength((int)MaxLength.MAX_256);
			builder.Property(x => x.Email).HasMaxLength((int)MaxLength.MAX_256);
			builder.Property(x => x.Phone).HasMaxLength((int)MaxLength.MAX_16);
			builder.Property(x => x.Country).HasMaxLength((int)MaxLength.MAX_256);
			builder.Property(x => x.Address).HasMaxLength((int)MaxLength.MAX_512);
			builder.Property(x => x.Desc).HasMaxLength((int)MaxLength.MAX_16);

			builder.HasOne(x => x.appHotels)
				.WithMany(x => x.appCustHotels)
				.HasForeignKey(x => x.IdHotel);
		}
	}
}
