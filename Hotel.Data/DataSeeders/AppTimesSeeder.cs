using Hotel.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.DataSeeders
{
	public static class AppTimesSeeder
	{
		public static void SeedData(this EntityTypeBuilder<AppTimes> builder)
		{
			var now = new DateTime(year: 2024, month: 3, day: 10);

			builder.HasData(
				new AppTimes
				{
					Id = 1,
					Name = "Giờ",
					Time = 0,
					CreatedDate = now,
				},
				new AppTimes
				{
					Id = 2,
					Name = "Ngày",
					Time = 1,
					CreatedDate = now,
				},
				new AppTimes
				{
					Id = 3,
					Name = "Tháng",
					Time = 30,
					CreatedDate = now,
				},
				new AppTimes
				{
					Id = 4,
					Name = "Năm",
					Time = 365,
					CreatedDate = now,
				}
			);
		}
	}
}
