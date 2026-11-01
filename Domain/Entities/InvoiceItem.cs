using Domain.Common;
using Domain.ValueObjects;
using System.ComponentModel;

namespace Domain.Entities
{
    [DisplayName("ردیف فاکتور")]
    public sealed class InvoiceItem : Entity
    {
        #region Properties
        // شناسه فاکتور
        public long InvoiceId { get; private set; }

        // شناسه کالا
        public long ProductId { get; private set; }

        // نام محصول
        public ProductName ProductName { get; private set; } = null!;

        // قیمت واحد
        public Amount UnitPrice { get; private set; } = null!;

        // تعداد
        public InvoiceQuantity Quantity { get; private set; } = null!;

        //مجموع مبلغ فاکتور
        public Amount TotalPrice { get; private set; } = null!;

        #endregion

        #region constructor
        private InvoiceItem() { }

        private InvoiceItem(
            long productId,
            ProductName productName,
            Amount unitPrice,
            InvoiceQuantity quantity)
        {
            if (productId <= 0)
                throw new DomainException("شناسه محصول نامعتبر است.");

            ArgumentNullException.ThrowIfNull(productName);
            ArgumentNullException.ThrowIfNull(unitPrice);
            ArgumentNullException.ThrowIfNull(quantity);

            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
            TotalPrice = unitPrice * quantity.Value;
        }
        #endregion

        #region create
        internal static InvoiceItem Create(
       long productId,
       ProductName productName,
       Amount unitPrice,
       InvoiceQuantity quantity)
        {
            return new InvoiceItem(productId, productName, unitPrice, quantity);
        }
        #endregion

        #region Increase quantity
        internal void IncreaseQuantity(InvoiceQuantity additionalQuantity)
        {
            ArgumentNullException.ThrowIfNull(additionalQuantity);

            Quantity = InvoiceQuantity.Create(Quantity.Value + additionalQuantity.Value);
            TotalPrice = UnitPrice * Quantity.Value;
        }
        #endregion

    }
}
