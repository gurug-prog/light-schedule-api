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
    private readonly ILightGroupAppService _scheduleAppService;

    public LightScheduleController(
        ILogger<LightScheduleController> logger,
        ILightGroupAppService scheduleAppService)
    {
        _logger = logger;
        _scheduleAppService = scheduleAppService;
    }

    [HttpPost("ImportSchedules")]
    public async Task<bool> ImportSchedules(IFormFile inputFile)
    {
        _logger.LogInformation("LightScheduleController.ImportSchedules started");

        var result = await _scheduleAppService.ImportSchedules(new ScheduleFileUploadDto
        {
            FileName = inputFile.FileName,
            ReadStream = inputFile.OpenReadStream()
        });

        _logger.LogInformation("LightScheduleController.ImportSchedules finished");

        return result;
    }

    [HttpGet("ExportSchedules")]
    public async Task<List<LightGroupDto>> ExportSchedules(
        [FromQuery] IList<int>? groupIds = null,
        [FromQuery] bool withSchedules = false,
        [FromQuery] bool withAddresses = false)
    {
        _logger.LogInformation($"LightScheduleController.ExportSchedules started");

        var result = await _scheduleAppService
            .ExportSchedules(groupIds, withSchedules, withAddresses);

        _logger.LogInformation($"LightScheduleController.ExportSchedules finished");

        return result;
    }

    [HttpGet("GetGroupShutdown")]
    public async Task<LightGroupShutdownDto> GetGroupShutdown([FromQuery] int groupId)
    {
        _logger.LogInformation($"LightScheduleController.GetGroupShutdown started");

        var result = await _scheduleAppService.GetGroupShutdown(groupId);

        _logger.LogInformation($"LightScheduleController.GetGroupShutdown finished");

        return result;
    }

    [HttpPost("UpdateGroupSchedules")]
    public async Task<LightGroupDto> UpdateGroupSchedules(
        [FromQuery] int groupId,
        [FromBody] IList<ScheduleDto> schedules)
    {
        _logger.LogInformation($"LightScheduleController.UpdateGroupSchedules started");

        var result = await _scheduleAppService.UpdateGroupSchedules(groupId, schedules);

        _logger.LogInformation($"LightScheduleController.UpdateGroupSchedules finished");

        return result;
    }
}
