using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Customers.Dto
{
    /// <summary>
    /// قرارداد ورودی ثبت مشتری جدید
    /// </summary>
    public sealed record CreateCustomerRequestDto
    {
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string NationalCode { get; init; } = string.Empty;
    }

    /// <summary>
    /// قرارداد خروجی پس از ثبت موفق مشتری
    /// </summary>
    public sealed record CreateCustomerResponseDto
    {
        public long Id { get; init; }
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string FullName { get; init; } = string.Empty;
        public string NationalCode { get; init; } = string.Empty;
    }
}
