using Hotel.Data.Entities;
using Hotel.Share.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
    public class AppHotelConfig : IEntityTypeConfiguration<AppHotels>
    {
        public void Configure(EntityTypeBuilder<AppHotels> builder)
        {
            //builder.ToTable(("hotel"));
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Code).HasMaxLength((int)MaxLength.MAX_32);
            builder.Property(x => x.Name).HasMaxLength((int)MaxLength.MAX_255);
            builder.Property(x => x.Email).HasMaxLength((int)MaxLength.MAX_255);
            builder.Property(x => x.Location).HasMaxLength((int)MaxLength.MAX_512);
            builder.Property(x => x.City).HasMaxLength((int)MaxLength.MAX_512);
            builder.Property(x => x.District).HasMaxLength((int)MaxLength.MAX_255);
            builder.Property(x => x.Avatar).HasMaxLength((int)MaxLength.MAX_512);

            builder.HasOne(x => x.appGroup)
                    .WithOne(x => x.appHotels)
                    .HasForeignKey<AppHotels>(x => x.IdGroup);
        }
    }
}
