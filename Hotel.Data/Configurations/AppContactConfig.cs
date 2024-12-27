using Hotel.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppContactConfig : IEntityTypeConfiguration<AppContact>
	{
		public void Configure(EntityTypeBuilder<AppContact> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
			builder.Property(x => x.Phone).IsRequired().HasMaxLength(20);
			builder.Property(x => x.Email).IsRequired().HasMaxLength(200);
			builder.Property(x => x.Content).IsRequired();
			builder.Property(x => x.Status).IsRequired();
		}
	}
}
