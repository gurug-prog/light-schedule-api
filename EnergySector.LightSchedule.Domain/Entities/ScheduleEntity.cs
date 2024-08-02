using EnergySector.LightSchedule.Domain.Core;

namespace EnergySector.LightSchedule.Domain.Entities;

public class ScheduleEntity : Entity<int>
{
    public ScheduleEntity(int id) : base(id)
    {
    }

    public required string Day { get; set; }
    public required TimeSpan StartTime { get; set; }
    public required TimeSpan FinishTime { get; set; }

    public LightGroupEntity? Group { get; set; }
    public required int GroupId { get; set; }
}
