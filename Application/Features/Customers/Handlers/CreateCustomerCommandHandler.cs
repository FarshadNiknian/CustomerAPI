using Application.Common;
using Application.Features.Customers.Commands;
using Application.Features.Customers.Dto;
using Application.Features.Customers.Mappings;
using Domain.Common;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;


namespace Application.Features.Customers.Handlers
{
    public sealed class CreateCustomerCommandHandler
      : IRequestHandler<CreateCustomerCommand, CreateCustomerResponseDto>
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

        public async Task<CreateCustomerResponseDto> Handle(
            CreateCustomerCommand request,
            CancellationToken cancellationToken)
        {
            var customerName = CustomerName.Create(request.FirstName, request.LastName);
            var nationalCode = NationalCode.Create(request.NationalCode);

            bool exists = await _customerRepository.ExistsByNationalCodeAsync(nationalCode, cancellationToken);
            if (exists)
            {
                throw new DomainException("مشتری با این کد ملی قبلاً در سیستم ثبت شده است.");
            }

            var customer = Customer.Create(customerName, nationalCode);

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                await _customerRepository.AddAsync(customer, cancellationToken);

                await _unitOfWork.CommitTransactionAsync(cancellationToken);
             
                return customer.ToCreateResponseDto();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}
