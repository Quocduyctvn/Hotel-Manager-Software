using Hotel.Data.DataSeeders;
using Hotel.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Hotel.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public DbSet<AppRentalPackageCate> AppRentalPackageCate { get; set; }
		public DbSet<AppRentalPackage> AppRentalPackage { get; set; }
		public DbSet<AppHMSOrder> AppHMSOrder { get; set; }
		public DbSet<AppHotels> AppHotel { get; set; }
		public DbSet<AppGroup> AppGroup { get; set; }
		public DbSet<AppHMS> AppHMS { get; set; }
		public DbSet<AppUser> AppUser { get; set; }
		public DbSet<AppRole> AppRole { get; set; }
		public DbSet<AppTimes> AppTime { get; set; }
		public DbSet<AppPaymentMethod> AppPaymentMethod { get; set; }
		public DbSet<AppRolePermission> AppRolePermissions { get; set; }
		public DbSet<AppPermission> AppPermissions { get; set; }
		public DbSet<AppRoomCate> AppRoomCates { get; set; }
		public DbSet<AppRentalPrice> AppRentalPrices { get; set; }
		public DbSet<AppRentalType> AppRentalTypes { get; set; }
		public DbSet<AppDayType> AppDayTypes { get; set; }
		public DbSet<AppDateTypeWeek> AppDateTypeWeeks { get; set; }
		public DbSet<AppHoliday> AppHolidays { get; set; }
		public DbSet<AppImage> AppImages { get; set; }
		public DbSet<AppFloor> appFloors { get; set; }
		public DbSet<AppRoom> AppRooms { get; set; }
		public DbSet<AppRoomCateAmenity> AppRoomCateAmenities { get; set; }
		public DbSet<AppAmenity> AppAmenities { get; set; }

		public DbSet<AppCustHotel> AppCustHotels { get; set; }
		public DbSet<AppBookingRoom> AppBookingRooms { get; set; }


		public DbSet<AppServicesOrder> AppServicesOrders { get; set; }
		public DbSet<AppServices> AppServices { get; set; }
		public DbSet<AppComodityOrder> AppComodityOrders { get; set; }
		public DbSet<AppCommodity> AppCommodities { get; set; }
		public DbSet<AppSvcCommoCate> AppSvcCommoCates { get; set; }


		public DbSet<AppBill> AppBill { get; set; }
		public DbSet<AppIncurredFee> AppIncurredFee { get; set; }


		public DbSet<AppArticle> AppArticles { get; set; }
		public DbSet<AppArticleCate> AppArticlesCates { get; set; }
		public DbSet<AppContact> AppContacts { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)  // Fluent API
		{
			//modelBuilder.ApplyConfiguration(new AppRentalPackageCateCofig());
			//modelBuilder.ApplyConfiguration(new AppRentalPackageConfig());
			//modelBuilder.ApplyConfiguration(new AppHMSOrderConfig());
			//modelBuilder.ApplyConfiguration(new AppHotelConfig());
			//modelBuilder.ApplyConfiguration(new AppGroupConfig());
			//modelBuilder.ApplyConfiguration(new AppHMSConfig());
			//modelBuilder.ApplyConfiguration(new AppRoleConfig());
			//modelBuilder.ApplyConfiguration(new AppUserConfig());
			//modelBuilder.ApplyConfiguration(new AppTimesConfig());
			//modelBuilder.ApplyConfiguration(new AppPaymentMethodConfig());

			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

			modelBuilder.Entity<AppGroup>().SeedData();
			modelBuilder.Entity<AppRole>().SeedData();
			modelBuilder.Entity<AppUser>().SeedData();
			modelBuilder.Entity<AppRentalPackageCate>().SeedData();
			modelBuilder.Entity<AppTimes>().SeedData();
			modelBuilder.Entity<AppPaymentMethod>().SeedData();
			modelBuilder.Entity<AppRentalType>().SeedData();
			modelBuilder.Entity<AppDayType>().SeedData();
		}
	}
}
