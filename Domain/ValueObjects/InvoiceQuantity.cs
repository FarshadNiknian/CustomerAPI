using Domain.Common;


namespace Domain.ValueObjects
{
    public sealed record InvoiceQuantity
    {
        public int Value { get; }

        private InvoiceQuantity(int value)
        {
            Value = value;
        }

        public static InvoiceQuantity Create(int value)
        {
            if (value <= 0)
                throw new DomainException("تعداد کالا در ردیف فاکتور باید بزرگتر از صفر باشد.");

            return new InvoiceQuantity(value);
        }

        public static InvoiceQuantity CreateUnsafe(int value) => new(value);

        public override string ToString() => Value.ToString();
    }
}
