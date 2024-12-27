using Hotel.Data.Entities;
using Hotel.Share.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.DataSeeders
{
    public static class AppRentalPackageCateSeeder
    {
        public static void SeedData(this EntityTypeBuilder<AppRentalPackageCate> builder)
        {
            var now = new DateTime(year: 2024, month: 3, day: 10);


            // Tạo thông tin tài khoản admin
            builder.HasData(
                new AppRentalPackageCate
                {
                    Id = 1,
                    Name = "Phiên bản dùng thử",
                    Status = RentalStatus.ACTIVE
                },
                new AppRentalPackageCate
                {
                    Id = 2,
                    Name = "Phiên bản có phí",
                    Status = RentalStatus.ACTIVE
                }
            );
        }
    }
}