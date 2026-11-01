using Application.Features.InvoiceLists.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.InvoiceLists.Queries
{
    public interface IInvoiceReadRepository
    {
        Task<PagedResult<InvoiceListItemDto>> GetPagedListAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);
    }
}
