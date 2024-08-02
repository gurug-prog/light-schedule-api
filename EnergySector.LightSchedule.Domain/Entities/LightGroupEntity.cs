using EnergySector.LightSchedule.Domain.Core;

namespace EnergySector.LightSchedule.Domain.Entities;

public class LightGroupEntity : Entity<int>
{
    public LightGroupEntity(int id) : base(id)
    {
    }

    public string? Name { get; set; }
    public string? Description { get; set; }

    public IList<AddressEntity>? Addresses { get; set; }
    public IList<ScheduleEntity>? Schedules { get; set; }
}
