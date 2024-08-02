using EnergySector.LightSchedule.Application.Contracts;
using EnergySector.LightSchedule.Application.Mapping;
using EnergySector.LightSchedule.Application.Schedule;
using EnergySector.LightSchedule.DataAccess.Repositories;
using EnergySector.LightSchedule.Domain.Repositories;
using EnergySector.LightSchedule.Domain.Schedule;

namespace EnergySector.LightSchedule.Host.Config;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddLogging();
        services.AddAutoMapper(mc =>
        {
            mc.AddProfile<LightScheduleAutoMapperProfile>();
        });

        services
            .AddTransient<ILightGroupAppService, LightGroupAppService>()
            .AddTransient<LightGroupDomainService>()
            .AddTransient<ILightGroupRepository, LightGroupRepository>();

        return services;
    }
}
