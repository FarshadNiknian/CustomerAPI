using Application.Features.Invoices.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Invoices.Validators
{
    public sealed class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
    {
        public CreateInvoiceCommandValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.InvoiceNumber)
                .NotEmpty().WithMessage("شماره فاکتور الزامی است.")
                .Must(v => v.Trim().Length is >= 3 and <= 50)
                .WithMessage("شماره فاکتور باید بین ۳ تا ۵۰ کاراکتر باشد.");

            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("شناسه مشتری معتبر نیست.");

            RuleFor(x => x.Items)
                .NotNull().WithMessage("لیست اقلام فاکتور الزامی است.")
                .Must(items => items.Count > 0)
                .WithMessage("فاکتور باید حداقل دارای یک ردیف کالا باشد.");

            RuleForEach(x => x.Items)
                .SetValidator(new CreateInvoiceItemCommandValidator());
        }
    }
}
