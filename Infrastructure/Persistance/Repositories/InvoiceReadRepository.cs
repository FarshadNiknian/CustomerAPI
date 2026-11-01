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
         
            var baseQuery = from invoice in _context.Invoices.AsNoTracking()
                            join customer in _context.Customers.AsNoTracking()
                                on invoice.CustomerId equals customer.Id into customerGroup
                            from customer in customerGroup.DefaultIfEmpty()
                            orderby invoice.IssuedAtUtc descending
                            select new InvoiceListItemDto(
                                invoice.Id,
                                invoice.Number.Value,
                                invoice.CustomerId,
                                customer != null ? $"{customer.CustomerName.FirstName} {customer.CustomerName.LastName}" : "مشتری نامشخص",
                                invoice.IssuedAtUtc,
                                invoice.TotalAmount.Value,
                                invoice.Items.Select(item => new InvoiceLineDto(
                                    item.ProductId,
                                    item.ProductName.Value,
                                    item.UnitPrice.Value,
                                    item.Quantity.Value,
                                    item.TotalPrice.Value
                                )).ToList()
                            );

            //fetch record
            var totalCount = await baseQuery.CountAsync(cancellationToken);

            // skip and take for pages
            var items = await baseQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<InvoiceListItemDto>(items, totalCount, pageNumber, pageSize);
        }
    }
}
