using Hotel.Data.Entities;
using Hotel.Share.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppRentalPackageCateCofig : IEntityTypeConfiguration<AppRentalPackageCate>
	{
		public void Configure(EntityTypeBuilder<AppRentalPackageCate> builder)
		{
			//builder.ToTable(("rental_package_cate"));
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Name).IsRequired().HasMaxLength((int)MaxLength.MAX_255);
		}
	}
}
