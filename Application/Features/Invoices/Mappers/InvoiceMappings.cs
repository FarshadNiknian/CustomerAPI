using Application.Features.InvoiceLists.Dto;
using Application.Features.InvoiceLists.Queries;
using Application.Features.Invoices.Commands;

namespace Application.Features.Invoices.Mappers
{
    public static class InvoiceMappings
    {
        public static CreateInvoiceCommand ToCommand(this CreateInvoiceRequestDto dto)
        {
            var items = dto.Items?
                .Select(x => new CreateInvoiceItemCommand(x.ProductId, x.UnitPrice, x.Quantity))
                .ToList()
                ?? new List<CreateInvoiceItemCommand>();

            return new CreateInvoiceCommand(
                dto.InvoiceNumber,
                dto.CustomerId,
                dto.IssuedAtUtc ?? DateTime.UtcNow,
                items);
        }

        public static GetInvoicesListQuery ToQuery(this GetInvoicesListRequestDto dto)
        {
            return new GetInvoicesListQuery(
                dto.PageNumber <= 0 ? 1 : dto.PageNumber,
                dto.PageSize <= 0 ? 10 : dto.PageSize);
        }
    }
}
