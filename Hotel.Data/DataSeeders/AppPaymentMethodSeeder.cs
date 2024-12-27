using Hotel.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.DataSeeders
{
	public static class AppPaymentMethodSeeder
	{
		public static void SeedData(this EntityTypeBuilder<AppPaymentMethod> builder)
		{
			var now = new DateTime(year: 2024, month: 3, day: 10);

			builder.HasData(
				new AppPaymentMethod
				{
					Id = 1,
					Name = "MoMo",
					CreatedDate = now,
				},
				new AppPaymentMethod
				{
					Id = 2,
					Name = "PayPal",
					CreatedDate = now,
				},
				new AppPaymentMethod
				{
					Id = 3,
					Name = "VNPay",
					CreatedDate = now,
				}
			);
		}
	}
}
