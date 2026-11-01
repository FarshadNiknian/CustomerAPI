using Domain.Common;


namespace Domain.ValueObjects
{
    public sealed record ProductCount
    {
        public int Value { get; }

        private ProductCount(int value)
        {
            Value = value;
        }

        public static ProductCount Create(int value)
        {
            if (value < 0)
                throw new DomainException("موجودی محصول نمی‌تواند منفی باشد.");

            return new ProductCount(value);
        }

        public static ProductCount CreateUnsafe(int value) => new(value);

        public override string ToString() => Value.ToString();
    }
}
