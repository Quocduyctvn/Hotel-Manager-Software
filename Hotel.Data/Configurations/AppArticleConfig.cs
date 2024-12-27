using Hotel.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppArticleConfig : IEntityTypeConfiguration<AppArticle>
	{
		public void Configure(EntityTypeBuilder<AppArticle> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
			builder.Property(x => x.Summary).IsRequired(false).HasMaxLength(500);
			builder.Property(x => x.Content).IsRequired();
			builder.Property(x => x.Images).IsRequired().HasMaxLength(200);

			builder.HasOne(x => x.AppArticleCate)
				.WithMany(x => x.AppArticles)
				.HasForeignKey(x => x.IdCategory);
		}
	}
}
