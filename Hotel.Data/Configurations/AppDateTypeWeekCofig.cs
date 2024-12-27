using Hotel.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppDateTypeWeekCofig : IEntityTypeConfiguration<AppDateTypeWeek>
	{
		public void Configure(EntityTypeBuilder<AppDateTypeWeek> builder)
		{
			builder.HasKey(e => e.Id);

			builder.HasOne(x => x.appDayType)
				.WithMany(x => x.appDateTypeWeeks)
				.HasForeignKey(x => x.IdDayType);
		}
	}
}
