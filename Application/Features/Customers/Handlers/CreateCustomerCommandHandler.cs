using Application.Common;
using Application.Features.Customers.Commands;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Customers.Handlers
{
    public sealed class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, long>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCustomerCommandHandler(
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<long> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            // ۱. ایجاد Value Objectها
            var customerName = new CustomerName(request.CustomerName.Trim());
            var nationalCode = new NationalCode(request.NationalCode.Trim());

            // ۲. بررسی یکتا بودن کد ملی در بیزینس
            var isDuplicate = await _customerRepository.ExistsByNationalCodeAsync(nationalCode, cancellationToken);
            if (isDuplicate)
            {
                throw new InvalidOperationException($"مشتری با کد ملی '{request.NationalCode}' قبلاً ثبت شده است.");
            }

            // ۳. ایجاد انتیتی مشتری از طریق Factory Method
            var customer = Customer.Create(customerName, nationalCode);

            // ۴. درج در ریپازیتوری و ذخیره ترنزاکشن
            await _customerRepository.AddAsync(customer, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return customer.Id;
        }
    }
}
