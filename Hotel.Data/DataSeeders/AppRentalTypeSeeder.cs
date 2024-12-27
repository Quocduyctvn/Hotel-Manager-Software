

using Hotel.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.DataSeeders
{
    public static class AppRentalTypeSeeder
    {
        public static void SeedData(this EntityTypeBuilder<AppRentalType> builder)
        {
            var now = new DateTime(year: 2024, month: 10, day: 25);

            // Tạo thông tin tài khoản admin
            builder.HasData(
                new AppRentalType
                {
                    Id = 1,
                    Name = "Thuê theo giờ",
                    CreatedDate = now,
                },
                new AppRentalType
                {
                    Id = 2,
                    Name = "Thuê qua đêm",
                    CreatedDate = now,
                },
                new AppRentalType
                {
                    Id = 3,
                    Name = "Thuê nguyên ngày",
                    CreatedDate = now,
                }
            );
        }
    }
}
