using RestaurantManagementSystem.DTO;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentController(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetAllPayments()
        {
            var payments = await _paymentRepository.GetAllPaymentsAsync();
            var paymentDtos = payments.Select(p => new PaymentDto
            {
                Id = p.Id,
                UserId = p.UserId,
                Amount = p.Amount,
                DateTime = p.DateTime
            });
            return Ok(paymentDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDto>> GetPaymentById(int id)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            var paymentDto = new PaymentDto
            {
                Id = payment.Id,
                UserId = payment.UserId,
                Amount = payment.Amount,
                DateTime = payment.DateTime
            };
            return Ok(paymentDto);
        }

        [HttpPost]
        public async Task<ActionResult<PaymentDto>> AddPayment(PaymentDto paymentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payment = new Payment
            {
                UserId = paymentDto.UserId,
                Amount = paymentDto.Amount,
                DateTime = paymentDto.DateTime
            };
            await _paymentRepository.AddPaymentAsync(payment);

            paymentDto.Id = payment.Id;
            return CreatedAtAction(nameof(GetPaymentById), new { id = payment.Id }, paymentDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, PaymentDto paymentDto)
        {
            if (id != paymentDto.Id)
            {
                return BadRequest();
            }

            var payment = await _paymentRepository.GetPaymentByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            payment.UserId = paymentDto.UserId;
            payment.Amount = paymentDto.Amount;
            payment.DateTime = paymentDto.DateTime;

            await _paymentRepository.UpdatePaymentAsync(payment);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            await _paymentRepository.DeletePaymentAsync(id);

            return NoContent();
        }
    }
}

       