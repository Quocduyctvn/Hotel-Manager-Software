using Hotel.Data.Entities;
using Hotel.Share.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppRoleConfig : IEntityTypeConfiguration<AppRole>
	{
		public void Configure(EntityTypeBuilder<AppRole> builder)
		{
			//builder.ToTable(("role"));
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Name).HasMaxLength((int)MaxLength.MAX_256);
			builder.Property(x => x.Desc).HasMaxLength((int)MaxLength.MAX_512);

			builder.HasOne(x => x.appGroup)
				.WithMany(x => x.appRoles)
				.HasForeignKey(x => x.IdGroup);
		}
	}
}
