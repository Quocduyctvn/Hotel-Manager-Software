
using Hotel.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppIncurredFeeConfig : IEntityTypeConfiguration<AppIncurredFee>
	{
		public void Configure(EntityTypeBuilder<AppIncurredFee> builder)
		{
			builder.HasKey(x => x.Id);
			builder.HasOne(x => x.appBookingRoom)
				.WithMany(x => x.appIncurredFees)
				.HasForeignKey(x => x.IdBookingRoom);
		}
	}
}
