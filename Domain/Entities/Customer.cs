using Domain.ValueObjects;

namespace Domain.Entities
{
    public sealed class Customer : AggregateRoot
    {

        #region properties
        //نام و نام خانوادگی مشتری
        public CustomerName CustomerName { get; set; }

        //کد ملی
        public NationalCode NationalCode { get; set; }
        #endregion

        #region constructor
        private Customer() { }

        private Customer(CustomerName customerName, NationalCode nationalCode)
        {
            CustomerName = customerName;
            NationalCode = nationalCode;
        }
        #endregion

        #region Create
        //create
        public static Customer Create(CustomerName customerName, NationalCode nationalCode)
        {
            return new Customer(customerName, nationalCode);
        }
        #endregion

    }
}
