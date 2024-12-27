using Hotel.Data.Entities;
using Hotel.Share.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppGroupConfig : IEntityTypeConfiguration<AppGroup>
	{
		public void Configure(EntityTypeBuilder<AppGroup> builder)
		{
			//builder.ToTable(("group"));
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Name).HasMaxLength((int)MaxLength.MAX_256);
			builder.Property(x => x.Desc).HasMaxLength((int)MaxLength.MAX_256);


		}
	}
}
