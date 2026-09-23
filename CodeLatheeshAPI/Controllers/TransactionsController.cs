using CodeLatheeshAPI.Models.DomainModels;
using CodeLatheeshAPI.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CodeLatheeshAPI.Data;
using CodeLatheeshAPI.Repositories.IRepository;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using CodeLatheeshAPI.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CodeLatheeshAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        public TransactionsController(ITransactionService transactionService)
        {
            this._transactionService = transactionService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTransactionRequestDto request)
        {
            var transaction = new Transaction
            {
                Name = request.Name,
                Type = request.Type,
                UserId=request.UserId,
                Amount=request.Amount,
                Date=request.Date,
                PaymentMethod=request.PaymentMethod
                
            };
            await _transactionService.CreateTransaction(transaction);
            var response = new TransactionDto
            {
                Id= transaction.Id,
                Name= transaction.Name,
                UserId =transaction.UserId,
                Amount=transaction.Amount,
                Date=transaction.Date.ToString("dd-MM-yyyy"),
                PaymentMethod=transaction.PaymentMethod,
                Type=transaction.Type
            };
            return Ok(response);
        }


        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var selected = await _transactionService.FindTransaction(id);
            if(selected is null)
            {
                return NotFound();
            }
            var response = new TransactionDto
            {
                Id = selected.Id,
                Name = selected.Name,
                UserId = selected.UserId,
                Amount = selected.Amount,
                Date = selected.Date.ToString("dd-MM-yyyy"),
                PaymentMethod = selected.PaymentMethod,
                Type = selected.Type
            };
            return Ok(response);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Edit([FromRoute] Guid id, [FromBody] UpdateTransactionRequestDto request)
        {
            var transaction = new Transaction
            {
                Id = id,
                Type = request.Type,
                UserId = request.UserId,
                Amount = request.Amount,
                Date = request.Date,
                PaymentMethod = request.PaymentMethod,
                Name=request.Name

            };
            transaction = await _transactionService.UpdateTransactionById(transaction);
            if (transaction is null)
            {
                return NotFound();
            }
            var response = new TransactionDto
            {
                Id = transaction.Id,
                Name = transaction.Name,
                UserId = transaction.UserId,
                Amount = transaction.Amount,
                Date = transaction.Date.ToString("dd-MM-yyyy"),
                PaymentMethod = transaction.PaymentMethod,
                Type = transaction.Type
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var transaction = await _transactionService.DeleteTransaction(id);
            if (transaction is null)
            {
                return NotFound();
            }
            var response = new TransactionDto
            {
                Id = transaction.Id,
                Name = transaction.Name,
                UserId = transaction.UserId,
                Amount = transaction.Amount,
                Date = transaction.Date.ToString("dd-MM-yyyy"),
                PaymentMethod = transaction.PaymentMethod,
                Type = transaction.Type
            };
            return Ok(response);
        }

        [HttpGet("usersummary/{userId}/{month}")]
        public async Task<IActionResult> GetUserSummary([FromRoute] int userId, [FromRoute] int month)
        {
            // Your logic here, for example:
            var summary = await _transactionService.GetUserSummary(userId, month);
            return Ok(summary);
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetFiltered(
        [FromQuery] int userId,
        [FromQuery] int month,
        [FromQuery] string? type,
        [FromQuery] string? paymentMethod,
        [FromQuery] string? sortBy,
        [FromQuery] string? sortOrder,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
        {
            var result = await _transactionService.GetFilteredAsync(userId, month, type, paymentMethod, sortBy, sortOrder, pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("test-error")]
        public IActionResult TestError()
        {
            try
            {
                // Simulate an error
                throw new Exception("This is a simulated exception for logging test.");
            }
            catch (Exception ex)
            {
                // Log only the exception message
                Log.Error("Error occurred: {Message}", ex.Message);

                // Optionally, log full exception (stack trace etc.)
                Log.Error(ex, "Full exception logged.");

                return StatusCode(500, "Error has been logged.");
            }
        }



    }
}
