using Hotel.Data.Entities;
using Hotel.Share.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppBookingRoomConfig : IEntityTypeConfiguration<AppBookingRoom>
	{
		public void Configure(EntityTypeBuilder<AppBookingRoom> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Desc).HasMaxLength((int)MaxLength.MAX_512);
			builder.Property(x => x.Code).HasMaxLength((int)MaxLength.MAX_256);
			builder.Property(x => x.BookingConfirm).HasMaxLength((int)MaxLength.MAX_512);
			builder.Property(x => x.CheckinConfirm).HasMaxLength((int)MaxLength.MAX_512);
			builder.Property(x => x.CheckoutConfirm).HasMaxLength((int)MaxLength.MAX_512);


			builder.HasOne(x => x.appRoom)
				.WithMany(x => x.appBookingRooms)
				.HasForeignKey(x => x.IdRoom)
				.OnDelete(DeleteBehavior.Restrict);
			builder.HasOne(x => x.appCustHotel)
				.WithMany(x => x.appBookingRooms)
				.HasForeignKey(x => x.IdCustommer)
				.OnDelete(DeleteBehavior.Restrict);
			builder.HasOne(x => x.appUser)
				.WithMany(x => x.appBookingRooms)
				.HasForeignKey(x => x.IdUser)
				.OnDelete(DeleteBehavior.Restrict);
			builder.HasOne(x => x.appRentalPrice)
				.WithMany(x => x.appBookingRooms)
				.HasForeignKey(x => x.IdRentalPrice)
				.OnDelete(DeleteBehavior.Restrict);

		}
	}
}
