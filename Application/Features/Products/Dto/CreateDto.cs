using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Dto
{
    public sealed record CreateProductRequestDto(string Name, int Code, int Count);

    public sealed record CreateProductResponseDto(long Id, string Message);
}
