using Domain.Events;
using Domain.ValueObjects;
using System.ComponentModel;

namespace Domain.Entities
{
    [DisplayName("مشتری")]
    public sealed class Customer : AggregateRoot
    {

        #region properties
        //نام و نام خانوادگی مشتری
        public CustomerName CustomerName { get; private set; }

        //کد ملی
        public NationalCode NationalCode { get; private set; }
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
        /// <summary>
        /// CreateCustomer
        /// </summary>
        /// <param name="customerName"></param>
        /// <param name="nationalCode"></param>
        /// <returns></returns>
        public static Customer Create(CustomerName customerName, NationalCode nationalCode)
        {
            var customer = new Customer(customerName, nationalCode);

            customer.AddDomainEvent(new CustomerCreatedDomainEvent(
                customerName.FirstName,
                customerName.LastName,
                nationalCode.Value));
            
            return customer;
        }
        #endregion

    }
}
