using Application.Features.InvoiceLists.Dto;
using Application.Features.InvoiceLists.Queries;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Repositories
{
    public sealed class InvoiceReadRepository : IInvoiceReadRepository
    {
        private readonly ApplicationDbContext _context;

        public InvoiceReadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<InvoiceListItemDto>> GetPagedListAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            int totalCount = await _context.Invoices
                .AsNoTracking()
                .CountAsync(cancellationToken);

            if (totalCount == 0)
            {
                return new PagedResult<InvoiceListItemDto>(
                    Array.Empty<InvoiceListItemDto>(),
                    0,
                    pageNumber,
                    pageSize);
            }

            var invoices = await _context.Invoices
                .AsNoTracking()
                .AsSplitQuery()
                .Include(i => i.Items)
                .OrderByDescending(i => i.IssuedAtUtc)
                .ThenByDescending(i => i.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var customerIds = invoices
                .Select(i => i.CustomerId)
                .Distinct()
                .ToList();

            var customers = await _context.Customers
                .AsNoTracking()
                .Where(c => customerIds.Contains(c.Id))
                .ToListAsync(cancellationToken);

            var customerNamesById = customers.ToDictionary(
                c => c.Id,
                c => $"{c.CustomerName.FirstName} {c.CustomerName.LastName}".Trim());

            var items = invoices
                .Select(invoice =>
                {
                    var customerFullName = customerNamesById.TryGetValue(invoice.CustomerId, out var name)
                        ? name
                        : "نامشخص";

                    var lines = invoice.Items
                        .Select(line => new InvoiceLineDto(
                            line.ProductId,
                            line.ProductName.Value,
                            line.UnitPrice.Value,
                            line.Quantity.Value,
                            line.TotalPrice.Value))
                        .ToList();

                    return new InvoiceListItemDto(
                        invoice.Id,
                        invoice.Number.Value,
                        invoice.CustomerId,
                        customerFullName,
                        invoice.IssuedAtUtc,
                        invoice.TotalAmount.Value,
                        lines);
                })
                .ToList();

            return new PagedResult<InvoiceListItemDto>(
                items,
                totalCount,
                pageNumber,
                pageSize);
        }
    }
}
