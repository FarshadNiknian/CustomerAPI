using Domain.Common;
using Domain.ValueObjects;
using System.ComponentModel;


namespace Domain.Entities
{
    [DisplayName("شماره فاکتور")]
    public sealed class Invoice : AggregateRoot
    {
        #region properties
        //
        private readonly List<InvoiceItem> _items = new();

        //شماره فاکتور
        public InvoiceNumber Number { get; private set; } = null!;

        //شناسه مشتری
        public long CustomerId { get; private set; }

        //زمان خرید
        public DateTime IssuedAtUtc { get; private set; }

        //مجموع خرید
        public Amount TotalAmount { get; private set; } = null!;

        //
        public IReadOnlyCollection<InvoiceItem> Items => _items.AsReadOnly();
        #endregion


        private Invoice() { }

        private Invoice(InvoiceNumber number, long customerId, DateTime issuedAtUtc)
        {
            ArgumentNullException.ThrowIfNull(number);

            if (customerId <= 0)
                throw new DomainException("شناسه مشتری نامعتبر است.");

            Number = number;
            CustomerId = customerId;
            IssuedAtUtc = issuedAtUtc;
            TotalAmount = Amount.Zero;
        }
        
        //create
        public static Invoice Create(InvoiceNumber number, long customerId, DateTime? issuedAtUtc = null)
        {
            return new Invoice(number, customerId, issuedAtUtc ?? DateTime.UtcNow);
        }

        //addItem
        public void AddItem(Product product, Amount unitPrice, InvoiceQuantity quantity)
        {
            ArgumentNullException.ThrowIfNull(product);

            if (product.IsDeleted)
                throw new DomainException($"محصول '{product.Name.Value}' حذف شده است و امکان ثبت آن در فاکتور وجود ندارد.");

            AddItem(product.Id, product.Name, unitPrice, quantity);
        }

        // افزودن آیتم با مقادیر صریح
        public void AddItem(long productId, ProductName productName, Amount unitPrice, InvoiceQuantity quantity)
        {
            if (productId <= 0)
                throw new DomainException("شناسه محصول نامعتبر است.");

            ArgumentNullException.ThrowIfNull(productName);
            ArgumentNullException.ThrowIfNull(unitPrice);
            ArgumentNullException.ThrowIfNull(quantity);

            var existingItem = _items.Find(x => x.ProductId == productId);

            if (existingItem is not null)
            {
                if (existingItem.UnitPrice != unitPrice)
                    throw new DomainException("این محصول با قیمت واحد متفاوت قبلاً در همین فاکتور ثبت شده است.");

                existingItem.IncreaseQuantity(quantity);
            }
            else
            {
                var newItem = InvoiceItem.Create(productId, productName, unitPrice, quantity);
                _items.Add(newItem);
            }

            RecalculateTotalAmount();
        }

        public void RemoveItem(long productId)
        {
            var existingItem = _items.Find(x => x.ProductId == productId);
            if (existingItem is null)
                throw new DomainException("ردیف مورد نظر در فاکتور یافت نشد.");

            if (_items.Count == 1)
                throw new DomainException("فاکتور باید حداقل دارای یک ردیف کالا باشد.");

            _items.Remove(existingItem);
            RecalculateTotalAmount();
        }

        //
        public void EnsureHasItems()
        {
            if (_items.Count == 0)
                throw new DomainException("امکان ثبت فاکتور بدون ردیف کالا وجود ندارد.");
        }

        //total amount
        private void RecalculateTotalAmount()
        {
            var total = Amount.Zero;
            foreach (var item in _items)
            {
                total += item.TotalPrice;
            }

            TotalAmount = total;
        }
    }
}
