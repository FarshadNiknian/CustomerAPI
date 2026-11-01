using Application.Features.InvoiceLists.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Invoices.Commands
{
    public sealed record CreateInvoiceItemCommand(
          long ProductId,
          decimal UnitPrice,
          int Quantity);
}
