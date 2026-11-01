
namespace Domain.Common
{
    public abstract record DomainEvent(DateTimeOffset OccurredOn) : INotification
    {
        protected DomainEvent() : this(DateTimeOffset.UtcNow) { }

    }
}
