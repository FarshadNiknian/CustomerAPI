using Application.Common;
using Application.Features.Products.Commands;
using Application.Features.Products.Dto;
using Application.Features.Products.Mappings;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public sealed class ProductsController : ControllerBase
    {
        private readonly ISender _sender;

        public ProductsController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// ثبت محصول جدید
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(CreateProductResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create(
            [FromBody] CreateProductRequestDto request,
            CancellationToken cancellationToken)
        {
            try
            {
                var command = request.ToCommand();
                long productId = await _sender.Send(command, cancellationToken);

                return Created($"/api/products/{productId}", productId.ToCreateResponseDto());
            }
            catch (ConflictException ex)
            {
                return Conflict(new ProblemDetails
                {
                    Title = "خطای دامنه در ثبت محصول",
                    Detail = ex.Message,
                    Status = StatusCodes.Status409Conflict
                });
            }
        }

        /// <summary>
        /// حذف منطقی (Soft Delete)
        /// </summary>
        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            try
            {
                var command = new DeleteProductCommand(id);
                await _sender.Send(command, cancellationToken);

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "عدم وجود محصول",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
            catch (ConflictException ex)
            {
                return Conflict(new ProblemDetails
                {
                    Title = "خطای دامنه در حذف محصول",
                    Detail = ex.Message,
                    Status = StatusCodes.Status409Conflict
                });
            }
        }
    }
}
