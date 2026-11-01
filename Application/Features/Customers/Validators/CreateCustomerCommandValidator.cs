using Application.Features.Customers.Commands;
using FluentValidation;


namespace Application.Features.Customers.Validators
{
    public sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("نام مشتری الزامی است.")
                .MaximumLength(100).WithMessage("نام مشتری نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد.");

            RuleFor(x => x.LastName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("نام خانوادگی مشتری الزامی است.")
                .MaximumLength(100).WithMessage("نام خانوادگی مشتری نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد.");

            RuleFor(x => x.NationalCode)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("کد ملی الزامی است.");
        }
    }
}
