using AutoMapper;
using EnergySector.LightSchedule.Application.Contracts;
using EnergySector.LightSchedule.Domain.Entities;
using EnergySector.LightSchedule.Domain.Schedule;
using Microsoft.Extensions.Logging;

namespace EnergySector.LightSchedule.Application.Schedule;

public class ScheduleAppService : IScheduleAppService
{
    private readonly ILogger<ScheduleAppService> _logger;
    private readonly IMapper _mapper;
    private readonly ScheduleDomainService _scheduleDomainService;

    public ScheduleAppService(
        ILogger<ScheduleAppService> logger,
        IMapper mapper,
        ScheduleDomainService scheduleDomainService)
    {
        _logger = logger;
        _mapper = mapper;
        _scheduleDomainService = scheduleDomainService;
    }

    public async Task<List<LightGroupDto>> ExportSchedules(IList<int> groupIds, bool exportAll = false)
    {
        List<LightGroupDto> result = [];
        _logger.LogInformation("ScheduleAppService.ImportSchedules started");

        try
        {
            var entities = await _scheduleDomainService.ExportSchedules(groupIds, exportAll);
            result = _mapper.Map<List<ScheduleEntity>, List<LightGroupDto>>(entities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ScheduleAppService.ImportSchedules failed");
            throw;
        }
        finally
        {
            _logger.LogInformation("ScheduleAppService.ImportSchedules started finished");
        }

        return result;
    }

    public async Task<bool> ImportSchedules(ScheduleFileUploadDto file)
    {
        bool result = false;
        _logger.LogInformation("ScheduleAppService.ImportSchedules started");
        try
        {
            result = await _scheduleDomainService.ImportFile(file.FileName, file.ReadStream);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ScheduleAppService.ImportSchedules failed");
            throw;
        }
        finally
        {
            _logger.LogInformation("ScheduleAppService.ImportSchedules started finished");
        }

        return result;
    }
}
