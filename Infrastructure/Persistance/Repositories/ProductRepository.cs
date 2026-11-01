using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistance.Repositories
{
    public sealed class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Product product, CancellationToken cancellationToken)
        {
            await _context.Products.AddAsync(product, cancellationToken);
        }

        public async Task<Product?> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted, cancellationToken);
        }

        public async Task<Dictionary<long, Product>> GetByIdsAsync(
      IReadOnlyCollection<long> ids,
      CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .Where(p => ids.Contains(p.Id) && !p.IsDeleted)
                .ToDictionaryAsync(p => p.Id, cancellationToken);
        }

        public async Task<bool> IsCodeUniqueAsync(int code, CancellationToken cancellationToken)
        {
            return !await _context.Products
                .AnyAsync(p => p.Code == ProductCode.Create(code) && !p.IsDeleted, cancellationToken);
        }
    }
}
