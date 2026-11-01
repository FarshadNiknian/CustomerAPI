using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.OutBox
{
    public sealed class OutboxMessage
    {
        public Guid Id { get; init; }
        public string Type { get; init; } = string.Empty;
        public string Content { get; init; } = string.Empty;
        public DateTime OccurredOnUtc { get; init; }
        public DateTime? ProcessedOnUtc { get; set; }
        public string? Error { get; set; }
    }
}
