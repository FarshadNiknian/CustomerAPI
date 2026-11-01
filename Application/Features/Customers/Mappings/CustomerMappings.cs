using Application.Features.Customers.Commands;
using Application.Features.Customers.Dto;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Customers.Mappings
{
    public static class CustomerMappings
    {
        public static CreateCustomerCommand ToCommand(this CreateCustomerRequestDto dto)
            => new(
                FirstName: dto.FirstName?.Trim() ?? string.Empty,
                LastName: dto.LastName?.Trim() ?? string.Empty,
                NationalCode: dto.NationalCode?.Trim() ?? string.Empty);

        public static CreateCustomerResponseDto ToCreateResponseDto(this Customer customer)
            => new()
            {
                Id = customer.Id,
                FirstName = customer.CustomerName.FirstName,
                LastName = customer.CustomerName.LastName,
                FullName = customer.CustomerName.ToString(),
                NationalCode = customer.NationalCode.Value
            };
    }
}
