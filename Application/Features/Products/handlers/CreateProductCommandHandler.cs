using Application.Common;
using Application.Features.Products.Commands;
using Domain.Common;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Products.handlers
{
    public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, long>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<long> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                bool isUnique = await _productRepository.IsCodeUniqueAsync(request.Code, cancellationToken);
                if (!isUnique)
                {
                    throw new DomainException("کد محصول تکراری است.");
                }

                var name = ProductName.Create(request.Name);
                var code = ProductCode.Create(request.Code);
                var count = ProductCount.Create(request.Count);

                var product = Product.Create(name, code, count);

                await _productRepository.AddAsync(product, cancellationToken);

                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                return product.Id;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}
