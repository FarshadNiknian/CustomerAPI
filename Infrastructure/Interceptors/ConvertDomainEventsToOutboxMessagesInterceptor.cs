using Domain.Entities;
using Infrastructure.OutBox;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;


namespace Infrastructure.Interceptors
{
    public sealed class ConvertDomainEventsToOutboxMessagesInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var dbContext = eventData.Context;
            if (dbContext is null)
                return base.SavingChangesAsync(eventData, result, cancellationToken);

            var aggregateRoots = dbContext.ChangeTracker
                .Entries<AggregateRoot>()
                .Select(x => x.Entity)
                .Where(x => x.DomainEvents.Any())
                .ToList();

            var outboxMessages = aggregateRoots
                .SelectMany(aggregate =>
                {
                    var events = aggregate.DomainEvents.ToList();
                    aggregate.ClearDomainEvents();
                    return events;
                })
                .Select(domainEvent => new OutboxMessage
                {
                    Id = Guid.NewGuid(),
                    OccurredOnUtc = DateTime.UtcNow,
                    Type = domainEvent.GetType().Name,
                    Content = JsonSerializer.Serialize(domainEvent, domainEvent.GetType())
                })
                .ToList();

            if (outboxMessages.Count > 0)
            {
                dbContext.Set<OutboxMessage>().AddRange(outboxMessages);
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
