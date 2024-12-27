using Hotel.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppRolePerConfig : IEntityTypeConfiguration<AppRolePermission>
	{
		public void Configure(EntityTypeBuilder<AppRolePermission> builder)
		{
			builder.HasKey(x => x.Id);

			builder.HasOne(x => x.appRole)
					.WithMany(x => x.appRolePers)
					.HasForeignKey(x => x.IdRole)
					  .OnDelete(DeleteBehavior.Restrict);
			builder.HasOne(x => x.appPermission)
					.WithMany(x => x.appRolePermissions)
					.HasForeignKey(x => new { x.IdPermission, x.IdGroup })
					  .OnDelete(DeleteBehavior.Restrict);
		}
	}
}
