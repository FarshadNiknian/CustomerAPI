using Application.Features.Products.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Validators
{
    public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("نام محصول الزامی است.")
                .MaximumLength(150).WithMessage("نام محصول نمی‌تواند بیشتر از ۱۵۰ کاراکتر باشد.");

            RuleFor(x => x.Code)
                .GreaterThan(0).WithMessage("کد محصول باید عددی بزرگتر از صفر باشد.");

            RuleFor(x => x.Count)
                .GreaterThanOrEqualTo(0).WithMessage("تعداد موجودی محصول نمی‌تواند منفی باشد.");
        }
    }
}
