using Domain.Common;


namespace Domain.ValueObjects
{
    public record class CustomerName
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        private CustomerName()
        {

        }

        private CustomerName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public static CustomerName Create(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                throw new DomainException("نام و نام خانوادگی الزامی است.");
            return new CustomerName(firstName.Trim(), lastName.Trim());
        }

        public static CustomerName CreateUnsafe(string firstName, string lastName)
           => new(firstName, lastName);

        public override string ToString() => $"{FirstName} {LastName}".Trim();

    }
}
