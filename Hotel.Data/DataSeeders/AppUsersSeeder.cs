using Hotel.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.DataSeeders
{
	public static class AppUsersSeeder
	{

		public static void SeedData(this EntityTypeBuilder<AppUser> builder)
		{
			var now = new DateTime(year: 2024, month: 3, day: 10);

			// Tạo mật khẩu
			var defaultPassword = "Hotel123*";
			var pwdHash = BCrypt.Net.BCrypt.HashPassword(defaultPassword);

			// Tạo thông tin tài khoản admin
			builder.HasData(
				new AppUser
				{
					Id = 1,
					Name = "admin1",
					Password = pwdHash,
					Email = "quocduyctvn@gmail.com",
					Phone = "0901007221",
					Avatar = "/images/manager/avatar_default.png",
					CreatedDate = now,
					IdRole = 1,              // Vai trò được tạo ở AppRoleSeeder
					IdGroup = 1
				},
				new AppUser
				{
					Id = 2,
					Name = "admin2",
					Password = pwdHash,
					Email = "danhkieuhan135@gmail.com",
					Phone = "0945255664",
					Avatar = "/images/manager/avatar_default.png",
					CreatedDate = now,
					IdRole = 1,              // Vai trò được tạo ở AppRoleSeeder
					IdGroup = 1
				}
			);
		}
	}
}