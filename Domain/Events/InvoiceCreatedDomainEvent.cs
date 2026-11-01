using Domain.Common;
using System;
using System.Collections.Generic;

namespace Domain.Events
{
    public sealed record InvoiceCreatedDomainEvent(
        string InvoiceNumber,
        long CustomerId,
        DateTime IssuedAtUtc,
        decimal TotalAmount,
        IReadOnlyList<InvoiceCreatedItemDomainEvent> Items) : DomainEvent; 
}
