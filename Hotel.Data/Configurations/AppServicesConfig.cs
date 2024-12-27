using Hotel.Data.Entities;
using Hotel.Share.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppServicesConfig : IEntityTypeConfiguration<AppServices>
	{
		public void Configure(EntityTypeBuilder<AppServices> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Name).HasMaxLength((int)MaxLength.MAX_256);
			builder.Property(x => x.Desc).HasMaxLength((int)MaxLength.MAX_256);

			builder.HasOne(x => x.appSvcCommoCate)
				.WithMany(x => x.appServices)
				.HasForeignKey(x => x.IdSvcCommocate);
		}
	}
}
