using Hotel.Admin.Mapper;

namespace Hotel.Admin.DependencyInjection.Extensions
{
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Register infrastructure EF services
		/// </summary>
		/// <param name="services">Service collection</param>
		/// <param name="configuration">Application configuration</param>
		/// <returns>Service collection</returns>
		public static IServiceCollection AddDJAdmin(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddAutoMapper(typeof(MappingProfile));
			return services;
		}
	}
}
