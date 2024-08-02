namespace EnergySector.LightSchedule.Domain.Schedule;

public interface IScheduleFileValidator
{
    object Validate(string input);
}
