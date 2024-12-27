using AutoMapper;
using Hotel.Admin.Areas.Admin.DTOs.RentalPackage;
using Hotel.Data.Entities;

namespace Hotel.Admin.Mapper
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<CreateRentalPackageDTOs, AppRentalPackage>()
				.ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
				.ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
				.ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
				.ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
				.ForMember(dest => dest.Position, opt => opt.Ignore())
				.ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());
			CreateMap<AppRentalPackage, UpdateRentalPackageDTOs>();
			CreateMap<UpdateRentalPackageDTOs, AppRentalPackage>();
		}
	}
}
