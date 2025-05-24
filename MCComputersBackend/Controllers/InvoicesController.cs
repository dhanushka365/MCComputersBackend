using MCComputersBackend.DTOs;
using MCComputersBackend.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MCComputersBackend.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _service;

        public InvoicesController(IInvoiceService service)
        {
            _service = service;
        }


        [HttpPost]
        [ProducesResponseType(typeof(InvoiceResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceDto dto)
        {
            if (dto == null || dto.Products == null || !dto.Products.Any())
                return BadRequest("Invalid invoice data or no products specified.");

            try
            {
                var invoice = await _service.CreateInvoiceAsync(dto);
                return CreatedAtAction(nameof(GetInvoice), new { id = invoice.Id }, invoice);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(InvoiceResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetInvoice(int id)
        {
            try
            {
                var invoice = await _service.GetInvoiceAsync(id);
                if (invoice == null)
                    return NotFound($"Invoice with ID {id} not found.");
                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InvoiceResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllInvoices()
        {
            try
            {
                var invoices = await _service.GetAllInvoicesAsync();
                return Ok(invoices);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
