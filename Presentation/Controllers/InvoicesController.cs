using Application.Common;
using Application.Features.InvoiceLists.Dto;
using Application.Features.Invoices.Mappers;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public sealed class InvoicesController : ControllerBase
    {
        private readonly ISender _sender;

        public InvoicesController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// ثبت فاکتور جدید
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(CreateInvoiceResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create(
            [FromBody] CreateInvoiceRequestDto request,
            CancellationToken cancellationToken)
        {
            try
            {
                var command = request.ToCommand();
                var response = await _sender.Send(command, cancellationToken);

                return Created($"/api/invoices/{response.Id}", response);
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray());

                return BadRequest(new ValidationProblemDetails(errors)
                {
                    Title = "خطای اعتبارسنجی اطلاعات فاکتور",
                    Status = StatusCodes.Status400BadRequest
                });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "عدم وجود اطلاعات پایه",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
            catch (ConflictException ex)
            {
                return Conflict(new ProblemDetails
                {
                    Title = "خطای قوانین دامنه در ثبت فاکتور",
                    Detail = ex.Message,
                    Status = StatusCodes.Status409Conflict
                });
            }
        }

        /// <summary>
        /// دریافت لیست صفحه‌بندی‌شده فاکتورها
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<InvoiceListItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetList(
            [FromQuery] GetInvoicesListRequestDto request,
            CancellationToken cancellationToken)
        {
            try
            {
                var query = request.ToQuery();
                var result = await _sender.Send(query, cancellationToken);

                return Ok(result);
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray());

                return BadRequest(new ValidationProblemDetails(errors)
                {
                    Title = "پارامترهای صفحه‌بندی نامعتبر است",
                    Status = StatusCodes.Status400BadRequest
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "پارامترهای ورودی نامعتبر است",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
        }
    }
}
