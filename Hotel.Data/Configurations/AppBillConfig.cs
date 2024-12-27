using Hotel.Data.Entities;
using Hotel.Share.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppBillConfig : IEntityTypeConfiguration<AppBill>
	{
		public void Configure(EntityTypeBuilder<AppBill> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Path).HasMaxLength((int)MaxLength.MAX_512);
			builder.Property(x => x.Desc).HasMaxLength((int)MaxLength.MAX_1024);

			builder.HasOne(x => x.appBookingRoom)
				.WithOne(x => x.appBill)
				.HasForeignKey<AppBill>(x => x.IdBooking);
		}
	}
}
