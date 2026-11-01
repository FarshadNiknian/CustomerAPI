using Application.Features.InvoiceLists.Dto;
using MediatR;


namespace Application.Features.Invoices.Commands
{
    public sealed record CreateInvoiceCommand(
         string InvoiceNumber,
         long CustomerId,
         DateTime? IssuedAtUtc,
         IReadOnlyList<CreateInvoiceItemCommand> Items) : IRequest<CreateInvoiceResponseDto>;
}
