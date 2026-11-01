using Domain.Common;
using Domain.Events;
using Domain.ValueObjects;
using System.ComponentModel;

namespace Domain.Entities
{
    [DisplayName("شماره فاکتور")]
    public sealed class Invoice : AggregateRoot
    {
        #region properties
        private readonly List<InvoiceItem> _items = new();

        // شماره فاکتور
        public InvoiceNumber Number { get; private set; } = null!;

        // شناسه مشتری
        public long CustomerId { get; private set; }

        // زمان خرید
        public DateTime IssuedAtUtc { get; private set; }

        // مجموع خرید
        public Amount TotalAmount { get; private set; } = null!;

        public IReadOnlyCollection<InvoiceItem> Items => _items.AsReadOnly();
        #endregion

        #region Constructor
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
        #endregion

        #region Create
        public static Invoice Create(InvoiceNumber number, long customerId, DateTime? issuedAtUtc = null)
        {
            return new Invoice(number, customerId, issuedAtUtc ?? DateTime.UtcNow);
        }

        #endregion

        #region AddItem
        public void AddItem(Product product, Amount unitPrice, InvoiceQuantity quantity)
        {
            ArgumentNullException.ThrowIfNull(product);
            ArgumentNullException.ThrowIfNull(unitPrice);
            ArgumentNullException.ThrowIfNull(quantity);

            if (product.IsDeleted)
                throw new DomainException($"محصول '{product.Name.Value}' حذف شده است و امکان ثبت آن در فاکتور وجود ندارد.");

            
            if (product.Count.Value < quantity.Value)
                throw new DomainException(
                    $"موجودی محصول '{product.Name.Value}' کافی نیست. موجودی فعلی: {product.Count.Value}، تعداد درخواستی: {quantity.Value}.");

            //افزودن آیتم تکراری
            AddItem(product.Id, product.Name, unitPrice, quantity);

            //کسر موجودی
            product.DecreaseCount(quantity.Value);
        }


       
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
        #endregion

        #region remove
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
        #endregion

        #region Raise domain Event and total amount
        public void EnsureHasItems()
        {
            if (_items.Count == 0)
                throw new DomainException("امکان ثبت فاکتور بدون ردیف کالا وجود ندارد.");
        }

        /// <summary>
        ///   OutboxMessages
        /// </summary>
        public void FinalizeCreation()
        {
            EnsureHasItems();

            var eventItems = _items
                .Select(x => new InvoiceCreatedItemDomainEvent(
                    x.ProductId,
                    x.ProductName.Value,
                    x.UnitPrice.Value,
                    x.Quantity.Value,
                    x.TotalPrice.Value))
                .ToList()
                .AsReadOnly();

            AddDomainEvent(new InvoiceCreatedDomainEvent(
                Number.Value,
                CustomerId,
                IssuedAtUtc,
                TotalAmount.Value,
                eventItems));
        }

        private void RecalculateTotalAmount()
        {
            var total = Amount.Zero;
            foreach (var item in _items)
            {
                total += item.TotalPrice;
            }

            TotalAmount = total;
        }
        #endregion

    }
}
