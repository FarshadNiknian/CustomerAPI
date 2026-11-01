using Application.Features.Customers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Customers.Validators
{
    public sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("نام و نام خانوادگی مشتری الزامی است.")
                .MinimumLength(3).WithMessage("نام مشتری باید حداقل ۳ کاراکتر باشد.")
                .MaximumLength(150).WithMessage("نام مشتری نمی‌تواند بیش از ۱۵۰ کاراکتر باشد.");

            RuleFor(x => x.NationalCode)
                .NotEmpty().WithMessage("کد ملی الزامی است.")
                .Length(10).WithMessage("کد ملی باید دقیقاً ۱۰ رقم باشد.")
                .Matches(@"^\d{10}$").WithMessage("کد ملی تنها باید شامل ارقام باشد.")
                .Must(BeValidIranianNationalCode).WithMessage("کد ملی وارد شده از نظر الگوریتم کنترلی معتبر نیست.");
        }

        private static bool BeValidIranianNationalCode(string nationalCode)
        {
            if (string.IsNullOrWhiteSpace(nationalCode) || nationalCode.Length != 10)
                return false;

            if (nationalCode.Distinct().Count() == 1)
                return false;

            var check = (int)char.GetNumericValue(nationalCode[9]);
            var sum = 0;

            for (var i = 0; i < 9; i++)
            {
                sum += (int)char.GetNumericValue(nationalCode[i]) * (10 - i);
            }

            var remainder = sum % 11;
            return (remainder < 2 && check == remainder) || (remainder >= 2 && check == 11 - remainder);
        }
    }
}
