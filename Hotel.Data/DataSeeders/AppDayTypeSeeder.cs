using Hotel.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.DataSeeders
{
	public static class AppDayTypeSeeder
	{
		public static void SeedData(this EntityTypeBuilder<AppDayType> builder)
		{
			var now = new DateTime(year: 2024, month: 3, day: 10);

			// Tạo thông tin tài khoản admin
			builder.HasData(
				new AppDayType
				{
					Id = 1,
					Name = "Ngày thường",
					CreatedDate = now,
				},
				new AppDayType
				{
					Id = 2,
					Name = "Ngày cuối tuần",
					CreatedDate = now,
				},
				new AppDayType
				{
					Id = 3,
					Name = "Ngày lễ",
					CreatedDate = now,
				}
			);
		}
	}
}
