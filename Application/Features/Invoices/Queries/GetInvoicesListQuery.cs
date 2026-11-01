using Application.Features.InvoiceLists.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.InvoiceLists.Queries
{
    public sealed record GetInvoicesListQuery(int PageNumber = 1, int PageSize = 20)
        : IRequest<PagedResult<InvoiceListItemDto>>;
}
