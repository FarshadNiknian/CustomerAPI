using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Commands
{
    public sealed record CreateProductCommand(string Name, int Code, int Count) : IRequest<long>;

}
