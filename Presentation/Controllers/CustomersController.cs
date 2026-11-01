using Application.Features.Customers.Dto;
using Application.Features.Customers.Mappings;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public sealed class CustomersController : ControllerBase
    {
        private readonly ISender _sender;

        public CustomersController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// ثبت مشتری جدید
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(CreateCustomerResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CreateCustomerResponseDto>> Create(
            [FromBody] CreateCustomerRequestDto request,
            CancellationToken cancellationToken)
        {
            try
            {
                var command = request.ToCommand();
                CreateCustomerResponseDto response = await _sender.Send(command, cancellationToken);

                return Created($"/api/customers/{response.Id}", response);
            }
            catch (DomainException ex)
            {
                return Conflict(new ProblemDetails
                {
                    Title = "خطای قوانین دامنه",
                    Detail = ex.Message,
                    Status = StatusCodes.Status409Conflict
                });
            }
        }
    }
}
