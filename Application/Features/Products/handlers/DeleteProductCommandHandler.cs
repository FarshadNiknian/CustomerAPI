using Application.Common;
using Application.Features.Products.Commands;
using Domain.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Products.handlers
{
    public sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
                if (product is null)
                {
                    throw new DomainException("محصول یافت نشد.");
                }

                product.Delete();

                await _unitOfWork.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}
