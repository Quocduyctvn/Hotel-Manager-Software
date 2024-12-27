using Hotel.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.DataSeeders
{
	public static class AppGroupSeeder
	{
		public static void SeedData(this EntityTypeBuilder<AppGroup> builder)
		{
			var now = new DateTime(year: 2024, month: 3, day: 10);

			// Tạo thông tin tài khoản admin
			builder.HasData(
				new AppGroup
				{
					Id = 1,
					Name = "Hotel Manager",
					Desc = "Nhóm Quản trị hệ thống Hotel manager",
					CreatedDate = now,
				}
			);
		}
	}
}