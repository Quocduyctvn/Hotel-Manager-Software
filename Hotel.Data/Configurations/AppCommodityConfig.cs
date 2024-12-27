using Hotel.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppCommodityConfig : IEntityTypeConfiguration<AppCommodity>
	{
		public void Configure(EntityTypeBuilder<AppCommodity> builder)
		{
			builder.HasKey(x => x.Id);

			builder.HasOne(x => x.appSvcCommoCate)
				.WithMany(x => x.appCommodities)
				.HasForeignKey(x => x.IdSvcCommoCate);
		}
	}
}
