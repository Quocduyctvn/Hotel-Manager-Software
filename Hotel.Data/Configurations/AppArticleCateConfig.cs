using Hotel.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppArticleCateConfig : IEntityTypeConfiguration<AppArticleCate>
	{
		public void Configure(EntityTypeBuilder<AppArticleCate> builder)
		{
			builder.HasKey(x => x.Id);
			// ten danh muc
			builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
		}
	}
}
