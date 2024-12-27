using Hotel.Data.Entities;
using Hotel.Share.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppHolidayConfig : IEntityTypeConfiguration<AppHoliday>
	{
		public void Configure(EntityTypeBuilder<AppHoliday> builder)
		{
			builder.HasKey(e => e.Id);
			builder.Property(e => e.Name).HasMaxLength((int)MaxLength.MAX_256);

			builder.HasOne(x => x.appDayType)
				.WithMany(x => x.appHolidays)
				.HasForeignKey(x => x.IdDayType);
		}
	}
}
