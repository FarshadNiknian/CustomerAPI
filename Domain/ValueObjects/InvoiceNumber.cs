using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    [DisplayName("شماره فاکتور")]
    public sealed record InvoiceNumber
    {
        public string Value { get; }

        private InvoiceNumber(string value)
        {
            Value = value;
        }

        public static InvoiceNumber Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("شماره فاکتور نمی‌تواند خالی باشد.");

            var trimmed = value.Trim();

            if (trimmed.Length < 3 || trimmed.Length > 50)
                throw new DomainException("شماره فاکتور باید بین ۳ تا ۵۰ کاراکتر باشد.");

            return new InvoiceNumber(trimmed);
        }

        public static InvoiceNumber CreateUnsafe(string value) => new(value);

        public override string ToString() => Value;
    }

}
