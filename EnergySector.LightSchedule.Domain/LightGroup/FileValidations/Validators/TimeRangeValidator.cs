using EnergySector.LightSchedule.Domain.Shared;

namespace EnergySector.LightSchedule.Domain.Schedule.Validators;

public class TimeRangeValidator : IScheduleFileValidator
{
    public object Validate(string input)
    {
        var timeParts = input.Split('-');
        if (timeParts.Length != 2)
        {
            throw new BusinessException($"Invalid time range format.");
        }

        return timeParts;
    }
}
