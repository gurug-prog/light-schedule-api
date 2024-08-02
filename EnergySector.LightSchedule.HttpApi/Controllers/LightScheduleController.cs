using EnergySector.LightSchedule.Application.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EnergySector.LightSchedule.HttpApi.Controllers;

[ApiController]
[Route("[controller]")]
public class LightScheduleController : ControllerBase
{
    private readonly ILogger<LightScheduleController> _logger;
    private readonly IScheduleAppService _scheduleAppService;

    public LightScheduleController(
        ILogger<LightScheduleController> logger,
        IScheduleAppService scheduleAppService)
    {
        _logger = logger;
        _scheduleAppService = scheduleAppService;
    }

    [HttpPost("ImportSchedules")]
    public async Task<bool> ImportSchedules(IFormFile inputFile)
    {
        _logger.LogInformation("ScheduleController.ImportSchedules started");

        var result = await _scheduleAppService.ImportSchedules(new ScheduleFileUploadDto
        {
            FileName = inputFile.FileName,
            ReadStream = inputFile.OpenReadStream()
        });

        _logger.LogInformation("ScheduleController.ImportSchedules finished");

        return result;
    }

    [HttpGet("ExportSchedules")]
    public async Task<List<LightGroupDto>> ExportSchedules(
        [FromQuery] IList<int>? groupIds = null,
        [FromQuery] bool withSchedules = false,
        [FromQuery] bool withAddresses = false)
    {
        _logger.LogInformation($"ScheduleController.ExportSchedules started");

        var result = await _scheduleAppService
            .ExportSchedules(groupIds, withSchedules, withAddresses);

        _logger.LogInformation($"ScheduleController.ExportSchedules finished");

        return result;
    }

    [HttpGet("GetGroupShutdown")]
    public async Task<LightGroupShutdownDto> GetGroupShutdown([FromQuery] int groupId)
    {
        _logger.LogInformation($"ScheduleController.GetGroupShutdown started");

        var result = await _scheduleAppService.GetGroupShutdown(groupId);

        _logger.LogInformation($"ScheduleController.GetGroupShutdown finished");

        return result;
    }

    [HttpPost("UpdateSchedule")]
    public async Task<LightGroupDto> UpdateSchedule(
        [FromQuery] int groupId,
        [FromBody] IList<ScheduleDto> schedules)
    {
        _logger.LogInformation($"ScheduleController.UpdateSchedule started");

        var result = await _scheduleAppService.UpdateGroupSchedules(groupId, schedules);

        _logger.LogInformation($"ScheduleController.UpdateSchedule finished");

        return result;
    }
}
