using Domain.Common;
using Domain.Events;
using Domain.ValueObjects;


namespace Domain.Entities
{
    public sealed class Product : AggregateRoot
    {
        #region properties
        public ProductName Name { get; private set; } = null!;
        public ProductCode Code { get; private set; } = null!;
        public ProductCount Count { get; private set; } = null!;
        public bool IsDeleted { get; private set; }
        #endregion

        #region Constructor
        // EF Core
        private Product() { }

        private Product(ProductName name, ProductCode code, ProductCount count)
        {
            Name = name;
            Code = code;
            Count = count;
            IsDeleted = false;
        }
        #endregion

        #region create
        //create
        public static Product Create(ProductName name, ProductCode code, ProductCount count)
        {
            ArgumentNullException.ThrowIfNull(name);
            ArgumentNullException.ThrowIfNull(code);
            ArgumentNullException.ThrowIfNull(count);

            var product = new Product(name, code, count);

            product.AddDomainEvent(new ProductCreatedDomainEvent(
                name.Value,
                code.Value,
                count.Value));

            return product;
        }
        #endregion

        #region delete
        //Soft Delete
        public void Delete()
        {
            if (IsDeleted)
                throw new DomainException("این محصول قبلاً حذف شده است.");

            IsDeleted = true;
            AddDomainEvent(new ProductDeletedDomainEvent(this.Id));
        }
        #endregion

        #region Inventory
        // Increase count
        public void IncreaseCount(int amount)
        {
            EnsureNotDeleted();

            if (amount <= 0)
                throw new DomainException("مقدار افزایش موجودی باید بزرگتر از صفر باشد.");

            Count = ProductCount.Create(Count.Value + amount);
        }

        //Decrease Count
        public void DecreaseCount(int amount)
        {
            EnsureNotDeleted();

            if (amount <= 0)
                throw new DomainException("مقدار کاهش موجودی باید بزرگتر از صفر باشد.");

            if (Count.Value - amount < 0)
                throw new DomainException("موجودی محصول برای کسر کافی نیست.");

            Count = ProductCount.Create(Count.Value - amount);
        }

        private void EnsureNotDeleted()
        {
            if (IsDeleted)
                throw new DomainException("امکان تغییر وضعیت محصول حذف‌شده وجود ندارد.");
        }
        #endregion



    }
}
