using Domain.Common;


namespace Domain.ValueObjects
{
    public sealed record ProductCode
    {
        public int Value { get; }

        private ProductCode(int value)
        {
            Value = value;
        }

        public static ProductCode Create(int value)
        {
            if (value <= 0)
                throw new DomainException("کد محصول باید عددی بزرگتر از صفر باشد.");

            return new ProductCode(value);
        }

        public static ProductCode CreateUnsafe(int value) => new(value);

        public override string ToString() => Value.ToString();
    }
}
