using System.Globalization;

namespace EnergySector.LightSchedule.Domain.Schedule.Validators;

public class StartTimeValidator : IScheduleFileValidator
{
    public object Validate(string input)
    {
        var isStartTimeValid = TimeSpan.TryParseExact(
            input.Trim(), "hh\\:mm", CultureInfo.InvariantCulture, out var startTime);
        if (!isStartTimeValid)
        {
            throw new Exception("Invalid start time value.");
        }

        return startTime;
    }
}
