using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
    public sealed record InvoiceCreatedItemDomainEvent(
         long ProductId,
         string ProductName,
         decimal UnitPrice,
         int Quantity,
         decimal TotalPrice);
}
