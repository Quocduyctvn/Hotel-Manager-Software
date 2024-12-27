
using Hotel.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations
{
	public class AppPaymentMethodConfig : IEntityTypeConfiguration<AppPaymentMethod>
	{
		public void Configure(EntityTypeBuilder<AppPaymentMethod> builder)
		{
			//builder.ToTable("payment_method");
			builder.HasKey(x => x.Id);
		}
	}
}
