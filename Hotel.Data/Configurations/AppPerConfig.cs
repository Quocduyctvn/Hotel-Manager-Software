using Hotel.Data.Entities;
using Hotel.Share.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
    public class AppPerConfig : IEntityTypeConfiguration<AppPermission>
    {
        public void Configure(EntityTypeBuilder<AppPermission> builder)
        {
            builder.HasKey(x => new { x.PerCode, x.IdGroup });
            builder.Property(x => x.PerCode)
                   .ValueGeneratedNever();
            builder.Property(x => x.GroupName).HasMaxLength((int)MaxLength.MAX_128);
            builder.Property(x => x.Desc).HasMaxLength((int)MaxLength.MAX_256);
            builder.Property(x => x.Table).HasMaxLength((int)MaxLength.MAX_256);

            builder.HasOne(x => x.appGroup)
                .WithMany(x => x.appPermissions)
                .HasForeignKey(x => x.IdGroup);
        }
    }
}
