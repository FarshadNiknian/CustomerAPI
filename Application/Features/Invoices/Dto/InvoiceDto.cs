

namespace Application.Features.InvoiceLists.Dto
{
    public sealed record InvoiceLineDto(
     long ProductId,
     string ProductName,
     decimal UnitPrice,
     int Quantity,
     decimal TotalPrice);

    public sealed record InvoiceListItemDto(
        long Id,
        string InvoiceNumber,
        long CustomerId,
        string CustomerFullName,
        DateTime IssuedAtUtc,
        decimal TotalAmount,
        IReadOnlyList<InvoiceLineDto> Items);

    public sealed record PagedResult<T>(
        IReadOnlyList<T> Items,
        int TotalCount,
        int PageNumber,
        int PageSize);
}
