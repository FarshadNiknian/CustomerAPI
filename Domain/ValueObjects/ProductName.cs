using Domain.Common;


namespace Domain.ValueObjects
{
    public sealed record ProductName
    {
        public string Value { get; }

        private ProductName(string value)
        {
            Value = value;
        }

        public static ProductName Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("نام محصول نمی‌تواند خالی باشد.");

            var trimmed = value.Trim();

            if (trimmed.Length < 3 || trimmed.Length > 200)
                throw new DomainException("نام محصول باید بین ۳ تا ۲۰۰ کاراکتر باشد.");

            return new ProductName(trimmed);
        }

        public static ProductName CreateUnsafe(string value) => new(value);

        public override string ToString() => Value;
    }
}

