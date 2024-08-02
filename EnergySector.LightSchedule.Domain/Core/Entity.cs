namespace EnergySector.LightSchedule.Domain.Core;

public class Entity<TKey> : IEntity<TKey>
{
    public Entity(TKey id)
    {
        Id = id;
    }

    public virtual TKey Id { get; protected set; }

    public override string ToString()
    {
        return $"[{GetType().Name}]: Id = {Id}";
    }
}
