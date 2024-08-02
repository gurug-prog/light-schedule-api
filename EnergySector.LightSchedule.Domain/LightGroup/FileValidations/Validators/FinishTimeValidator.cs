using EnergySector.LightSchedule.Domain.Shared;
using System.Globalization;

namespace EnergySector.LightSchedule.Domain.Schedule.Validators;

public class FinishTimeValidator : IScheduleFileValidator
{
    public object Validate(string input)
    {
        var isFinishTimeValid = TimeSpan.TryParseExact(
            input.Trim(), "hh\\:mm", CultureInfo.InvariantCulture, out var finishTime);
        if (!isFinishTimeValid)
        {
            throw new BusinessException("Invalid finish time value.");
        }

        return finishTime;
    }
}
