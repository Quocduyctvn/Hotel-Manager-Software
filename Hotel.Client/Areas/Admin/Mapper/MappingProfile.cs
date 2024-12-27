using AutoMapper;
using Hotel.Client.Areas.Admin.DTOs.RoomCate;
using Hotel.Data.Entities;

namespace Hotel.Client.Areas.Admin.Mapper
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<RoomCateDTOs, AppRoomCate>()
				.ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
				.ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
				.ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
				.ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
				.ForMember(dest => dest.Position, opt => opt.Ignore())
				.ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
				.ForMember(dest => dest.appRooms, opt => opt.Ignore());
			CreateMap<AppRoomCate, RoomCateDTOs>();
		}
	}
}
