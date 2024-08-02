namespace EnergySector.LightSchedule.Domain.Schedule.Validators;

public interface IFileValidatorsMap : IDictionary<ValidationType, IScheduleFileValidator>
{
}
