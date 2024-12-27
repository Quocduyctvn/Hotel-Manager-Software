using Hotel.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.DataSeeders
{
	public static class AppRolesSeeder
	{

		public static void SeedData(this EntityTypeBuilder<AppRole> builder)
		{
			var now = new DateTime(year: 2024, month: 3, day: 10);

			// Tạo vai trò
			var Admin = new AppRole
			{
				Id = 1,
				Name = "Quản trị hệ thống",
				Desc = "Quản trị toàn bộ hệ thống",
				IdGroup = 1,
				CreatedDate = now,
			};

			builder.HasData(Admin);
		}
	}
}
