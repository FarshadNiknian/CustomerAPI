using Application.Features.Products.Commands;
using Application.Features.Products.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Mappings
{
    public static class ProductMappings
    {
        public static CreateProductCommand ToCommand(this CreateProductRequestDto dto)
        {
            return new CreateProductCommand(dto.Name, dto.Code, dto.Count);
        }

        public static CreateProductResponseDto ToCreateResponseDto(this long id)
        {
            return new CreateProductResponseDto(id, "محصول با موفقیت ثبت شد.");
        }
    }
}
