using EnergySector.LightSchedule.Application.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EnergySector.LightSchedule.HttpApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ScheduleController : ControllerBase
{
    private readonly ILogger<ScheduleController> _logger;
    private readonly IScheduleAppService _scheduleAppService;

    public ScheduleController(
        ILogger<ScheduleController> logger,
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
        IList<int> groupIds,
        bool exportAll = false)
    {
        _logger.LogInformation($"ScheduleController.ExportSchedules started");
        if (exportAll && groupIds.Count > 0)
        {
            throw new Exception("Either the 'exportAll' flag or the 'groupIds'" +
                "array is required. But both cannot be used.");
        }

        var result = await _scheduleAppService.ExportSchedules(groupIds, exportAll);

        _logger.LogInformation($"ScheduleController.ExportSchedules finished");

        return result;
    }
}
