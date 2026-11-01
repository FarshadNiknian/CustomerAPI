
namespace Domain.Entities
{
    public abstract class Entity : IEquatable<Entity>
    {
        public long Id { get; protected init; }

        public override bool Equals(object? obj) =>
            obj is Entity other && Equals(other);

        public bool Equals(Entity? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;

            if (IsTransient() || other.IsTransient()) return false;

            return Id == other.Id;
        }

        public bool IsTransient() => Id == 0;

        public override int GetHashCode() =>
            IsTransient() ? base.GetHashCode() : HashCode.Combine(GetType(), Id);

        public static bool operator ==(Entity? left, Entity? right) => Equals(left, right);
        public static bool operator !=(Entity? left, Entity? right) => !Equals(left, right);
    }
}
