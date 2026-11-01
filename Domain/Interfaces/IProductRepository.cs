using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProductRepository
    {
        Task AddAsync(Product product, CancellationToken cancellationToken);
        Task<Product?> GetByIdAsync(long id, CancellationToken cancellationToken);
        Task<bool> IsCodeUniqueAsync(int code, CancellationToken cancellationToken);

        Task<Dictionary<long, Product>> GetByIdsAsync(IReadOnlyCollection<long> ids, CancellationToken cancellationToken = default);

    }
}
