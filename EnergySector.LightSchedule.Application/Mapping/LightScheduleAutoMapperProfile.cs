using AutoMapper;
using EnergySector.LightSchedule.Application.Contracts;
using EnergySector.LightSchedule.Domain.Entities;

namespace EnergySector.LightSchedule.Application.Mapping;

public class LightScheduleAutoMapperProfile : Profile
{
    public LightScheduleAutoMapperProfile()
    {
        CreateMap<ScheduleEntity, ScheduleDto>()
            .ReverseMap();

        CreateMap<LightGroupEntity, LightGroupDto>()
            .ReverseMap();

        CreateMap<AddressEntity, AddressDto>()
            .ReverseMap();
    }
}
