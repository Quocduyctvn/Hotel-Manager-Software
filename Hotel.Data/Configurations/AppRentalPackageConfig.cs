using Hotel.Data.Entities;
using Hotel.Share.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppRentalPackageConfig : IEntityTypeConfiguration<AppRentalPackage>
	{
		public void Configure(EntityTypeBuilder<AppRentalPackage> builder)
		{
			//builder.ToTable(("rental_package"));
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Name).HasMaxLength((int)MaxLength.MAX_255).IsRequired();
			builder.Property(x => x.Desc).HasMaxLength((int)MaxLength.MAX_512);

			builder.HasOne(x => x.appRentalPackageCate)
				.WithMany(x => x.appRentalPackage)
				.HasForeignKey(x => x.IdRentalPackageCate);
		}
	}
}