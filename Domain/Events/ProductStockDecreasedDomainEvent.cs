using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
    public sealed record ProductStockDecreasedDomainEvent(
       long ProductId,
       string ProductName,
       int ProductCode,
       int DecreasedQuantity,
       int RemainingCount);
}
