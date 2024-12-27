using Hotel.Data.Entities;
using Hotel.Share.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppUserConfig : IEntityTypeConfiguration<AppUser>
	{
		public void Configure(EntityTypeBuilder<AppUser> builder)
		{
			//builder.ToTable(("user"));
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Name).HasMaxLength((int)MaxLength.MAX_256);
			builder.Property(x => x.Email).HasMaxLength((int)MaxLength.MAX_256);
			builder.Property(x => x.Avatar).HasMaxLength((int)MaxLength.MAX_512);
			builder.Property(x => x.Password).HasMaxLength((int)MaxLength.MAX_512);

			builder.HasOne(x => x.appGroup)
				.WithMany(x => x.appUsers)
				.HasForeignKey(x => x.IdGroup);

			builder.HasOne(x => x.appRole)
				.WithMany(x => x.appUsers)
				.HasForeignKey(x => x.IdRole)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
