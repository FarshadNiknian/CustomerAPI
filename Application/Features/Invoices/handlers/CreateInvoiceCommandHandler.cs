using Application.Common;
using Application.Features.InvoiceLists.Dto;
using Application.Features.Invoices.Commands;
using Domain.Common;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Invoices.handlers
{
    public sealed class CreateInvoiceCommandHandler
          : IRequestHandler<CreateInvoiceCommand, CreateInvoiceResponseDto>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateInvoiceCommand> _validator;

        public CreateInvoiceCommandHandler(
            IInvoiceRepository invoiceRepository,
            ICustomerRepository customerRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            IValidator<CreateInvoiceCommand> validator)
        {
            _invoiceRepository = invoiceRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<CreateInvoiceResponseDto> Handle(
            CreateInvoiceCommand request,
            CancellationToken cancellationToken)
        {
            // FluentValidation
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
               
                bool invoiceExists = await _invoiceRepository.ExistsByNumberAsync(
                    request.InvoiceNumber,
                    cancellationToken);

                if (invoiceExists)
                {
                    throw new DomainException($"فاکتوری با شماره '{request.InvoiceNumber.Trim()}' قبلاً ثبت شده است.");
                }

                bool customerExists = await _customerRepository.ExistsAsync(
                    request.CustomerId,
                    cancellationToken);

                if (!customerExists)
                {
                    throw new NotFoundException(nameof(Customer), request.CustomerId);
                }

                var distinctProductIds = request.Items
                    .Select(x => x.ProductId)
                    .Distinct()
                    .ToList();

                var productsById = await _productRepository.GetByIdsAsync(
                    distinctProductIds,
                    cancellationToken);

                var missingProductIds = distinctProductIds
                    .Where(id => !productsById.ContainsKey(id))
                    .ToList();

                if (missingProductIds.Count > 0)
                {
                    throw new NotFoundException(nameof(Product), string.Join(", ", missingProductIds));
                }

                var invoiceNumber = InvoiceNumber.Create(request.InvoiceNumber);
                var invoice = Invoice.Create(invoiceNumber, request.CustomerId, request.IssuedAtUtc);

                foreach (var item in request.Items)
                {
                    var product = productsById[item.ProductId];
                    var unitPrice = Amount.Create(item.UnitPrice);
                    var quantity = InvoiceQuantity.Create(item.Quantity);

                    invoice.AddItem(product, unitPrice, quantity);
                }

               // نهایی‌سازی فاکتور و ثبت رویداد دامنه (InvoiceCreatedDomainEvent)
                invoice.FinalizeCreation();

                await _invoiceRepository.AddAsync(invoice, cancellationToken);

                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                return new CreateInvoiceResponseDto(
                    invoice.Id,
                    invoice.Number.Value,
                    invoice.TotalAmount.Value,
                    "فاکتور با موفقیت ثبت شد.");
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}
