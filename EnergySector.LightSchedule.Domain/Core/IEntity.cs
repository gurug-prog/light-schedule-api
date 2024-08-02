namespace EnergySector.LightSchedule.Domain.Core;

public interface IEntity<TKey>
{
    TKey Id { get; }
}
