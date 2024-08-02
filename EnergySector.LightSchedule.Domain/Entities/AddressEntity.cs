using EnergySector.LightSchedule.Domain.Core;

namespace EnergySector.LightSchedule.Domain.Entities;

public class AddressEntity : Entity<int>
{
    public AddressEntity(int id) : base(id)
    {
    }

    public required string Address { get; set; }

    public LightGroupEntity? Group { get; set; }
    public int? GroupId { get; set; }
}
