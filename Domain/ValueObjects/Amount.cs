using Domain.Common;


namespace Domain.ValueObjects
{
    public sealed record Amount
    {
        public static Amount Zero => new(0m);

        public decimal Value { get; }

        private Amount(decimal value)
        {
            Value = value;
        }

        public static Amount Create(decimal value)
        {
            if (value < 0)
                throw new DomainException("مبلغ نمی‌تواند منفی باشد.");

            return new Amount(value);
        }

        public static Amount CreateUnsafe(decimal value) => new(value);

        public Amount Add(Amount other)
        {
            ArgumentNullException.ThrowIfNull(other);
            return new Amount(Value + other.Value);
        }

        public Amount Multiply(int factor)
        {
            if (factor < 0)
                throw new DomainException("ضریب تعداد نمی‌تواند منفی باشد.");

            return new Amount(Value * factor);
        }

        public static Amount operator +(Amount left, Amount right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);
            return left.Add(right);
        }

        public static Amount operator *(Amount money, int factor)
        {
            ArgumentNullException.ThrowIfNull(money);
            return money.Multiply(factor);
        }

        public override string ToString() => Value.ToString("N0");
    }
}
