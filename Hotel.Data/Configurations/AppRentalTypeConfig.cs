using Hotel.Data.Entities;
using Hotel.Share.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppRentalTypeConfig : IEntityTypeConfiguration<AppRentalType>
	{
		public void Configure(EntityTypeBuilder<AppRentalType> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Name).HasMaxLength((int)MaxLength.MAX_256);
		}
	}
}
