using Application.Features.InvoiceLists.Dto;
using Application.Features.InvoiceLists.Queries;
using Application.Features.InvoiceLists.Validators;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.InvoiceLists.handlers
{
    public sealed class GetInvoicesListQueryHandler
        : IRequestHandler<GetInvoicesListQuery, PagedResult<InvoiceListItemDto>>
    {
        private readonly IInvoiceReadRepository _readRepository;

        public GetInvoicesListQueryHandler(IInvoiceReadRepository readRepository)
        {
            _readRepository = readRepository;
        }

        public async Task<PagedResult<InvoiceListItemDto>> Handle(
            GetInvoicesListQuery request,
            CancellationToken cancellationToken)
        {
            //check validator
            GetInvoicesListQueryValidator.Validate(request);

            //get data pages
            return await _readRepository.GetPagedListAsync(
                request.PageNumber,
                request.PageSize,
                cancellationToken);
        }
    }
}
