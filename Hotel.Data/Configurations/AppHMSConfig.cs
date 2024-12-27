using Hotel.Data.Entities;
using Hotel.Share.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppHMSConfig : IEntityTypeConfiguration<AppHMS>
	{
		public void Configure(EntityTypeBuilder<AppHMS> builder)
		{
			//builder.ToTable(("hotel_manager_software"));
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Name).HasMaxLength((int)MaxLength.MAX_256);
			builder.Property(x => x.Email).HasMaxLength((int)MaxLength.MAX_256);
			builder.Property(x => x.Location).HasMaxLength((int)MaxLength.MAX_512);

			builder.HasOne(x => x.appGroup)
				.WithOne(x => x.appHMS)
				.HasForeignKey<AppHMS>(x => x.IdGroup);
		}
	}
}
