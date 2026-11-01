using Application.Features.Customers.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Customers.Commands
{
    public sealed record CreateCustomerCommand(
       string FirstName,
       string LastName,
       string NationalCode) : IRequest<CreateCustomerResponseDto>;
}
