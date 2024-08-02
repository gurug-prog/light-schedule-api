using EnergySector.LightSchedule.Domain.Entities;
using EnergySector.LightSchedule.Domain.Repositories;
using EnergySector.LightSchedule.Domain.Schedule.Validators;
using Microsoft.Extensions.Logging;

namespace EnergySector.LightSchedule.Domain.Schedule;

public class ScheduleDomainService
{
    private readonly ILogger<ScheduleDomainService> _logger;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IDictionary<ValidationType, IScheduleFileValidator> _fileValidatorsMap;

    public ScheduleDomainService(
        ILogger<ScheduleDomainService> logger,
        IScheduleRepository scheduleRepository)
    {
        _logger = logger;
        _scheduleRepository = scheduleRepository;
        _fileValidatorsMap = new Dictionary<ValidationType, IScheduleFileValidator>()
        {
            { ValidationType.LineParts, new LineFormatValidator() },
            { ValidationType.ScheduleGroupId, new GroupIdValidator() },
            { ValidationType.TimeRange, new TimeRangeValidator() },
            { ValidationType.StartTime, new StartTimeValidator() },
            { ValidationType.FinishTime, new FinishTimeValidator() }
        };
    }

    public async Task<bool> ImportFile(string fileName, Stream readStream)
    {
        bool result = false;
        _logger.LogInformation("ScheduleDomainService.ImportFile started");
        try
        {
            var schedules = await ParseFile(readStream);
            result = await _scheduleRepository.ImportSchedules(schedules);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ScheduleDomainService.ImportFile failed");
            throw;
        }
        finally
        {
            _logger.LogInformation("ScheduleDomainService.ImportFile finished");
        }

        return result;
    }

    public async Task<List<ScheduleEntity>> ExportSchedules(IList<int> groupIds, bool exportAll = false)
    {
        List<ScheduleEntity> result = [];
        _logger.LogInformation("ScheduleDomainService.ExportSchedules started");
        try
        {
            //var schedules = await ParseFile(readStream);
            //await _scheduleRepository.ImportSchedules(schedules);
            throw new NotImplementedException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ScheduleDomainService.ExportSchedules failed");
        }
        finally
        {
            _logger.LogInformation("ScheduleDomainService.ExportSchedules finished");
        }

        return result;
    }

    private async Task<List<ScheduleEntity>> ParseFile(Stream stream)
    {
        List<ScheduleEntity> result = [];
        using var reader = new StreamReader(stream);
        var line = await reader.ReadLineAsync();
        var lineNumber = 1;
        try
        {
            while (line != null)
            {
                var parts = (string[])_fileValidatorsMap[ValidationType.LineParts].Validate(line);
                var groupId = (int)_fileValidatorsMap[ValidationType.ScheduleGroupId].Validate(parts[0]);
                var day = parts[1].Trim();

                for (int i = 2; i < parts.Length; i++)
                {
                    var timeParts = (string[])_fileValidatorsMap[ValidationType.TimeRange].Validate(parts[i]);
                    var startTime = (TimeSpan)_fileValidatorsMap[ValidationType.StartTime].Validate(timeParts[0]);
                    var finishTime = (TimeSpan)_fileValidatorsMap[ValidationType.FinishTime].Validate(timeParts[1]);

                    result.Add(new ScheduleEntity(0)
                    {
                        Day = day,
                        StartTime = startTime,
                        FinishTime = finishTime,
                        GroupId = groupId
                    });
                }

                line = await reader.ReadLineAsync();
                lineNumber++;
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while reading the file:" +
                Environment.NewLine +
                ex.Message +
                Environment.NewLine +
                $"at line {lineNumber}");
        }

        return result;
    }
}
