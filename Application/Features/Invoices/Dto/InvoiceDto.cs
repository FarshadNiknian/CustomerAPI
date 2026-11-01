namespace Application.Features.InvoiceLists.Dto
{
    #region Query & list Dto

    public sealed record GetInvoicesListRequestDto(
        int PageNumber = 1,
        int PageSize = 10);

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

    #endregion

    #region Create Invoice DTOs

    public sealed record CreateInvoiceItemRequestDto(
        long ProductId,
        decimal UnitPrice,
        int Quantity);

    public sealed record CreateInvoiceRequestDto(
        string InvoiceNumber,
        long CustomerId,
        DateTime? IssuedAtUtc,
        IReadOnlyList<CreateInvoiceItemRequestDto> Items);

    public sealed record CreateInvoiceResponseDto(
        long Id,
        string InvoiceNumber,
        decimal TotalAmount,
        string Message);

    #endregion
}
