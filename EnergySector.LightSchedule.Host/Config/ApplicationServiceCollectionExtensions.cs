using EnergySector.LightSchedule.Application.Contracts;
using EnergySector.LightSchedule.Application.Schedule;
using EnergySector.LightSchedule.DataAccess.Repositories;
using EnergySector.LightSchedule.Domain.Repositories;
using EnergySector.LightSchedule.Domain.Schedule;
using EnergySector.LightSchedule.Domain.Schedule.Validators;
using System.Reflection;

namespace EnergySector.LightSchedule.Host.Config;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddLogging();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services
            .AddTransient<IScheduleAppService, ScheduleAppService>()
            .AddTransient<ScheduleDomainService>()
            .AddTransient<IScheduleRepository, ScheduleRepository>();

        return services;
    }
}
