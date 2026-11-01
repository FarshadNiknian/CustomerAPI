using Application.Features.Invoices.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Invoices.Validators
{
    public sealed class CreateInvoiceItemCommandValidator : AbstractValidator<CreateInvoiceItemCommand>
    {
        public CreateInvoiceItemCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("شناسه محصول باید بزرگتر از صفر باشد.");

            RuleFor(x => x.UnitPrice)
                .GreaterThanOrEqualTo(0).WithMessage("قیمت واحد نمی‌تواند منفی باشد.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("تعداد کالا در هر ردیف باید حداقل ۱ باشد.");
        }
    }
}
