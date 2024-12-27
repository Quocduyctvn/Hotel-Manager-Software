using Hotel.Data.Entities;
using Hotel.Share.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppTimesConfig : IEntityTypeConfiguration<AppTimes>
	{
		public void Configure(EntityTypeBuilder<AppTimes> builder)
		{
			//builder.ToTable(("time"));
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Name).HasMaxLength((int)MaxLength.MAX_256);
		}
	}
}
