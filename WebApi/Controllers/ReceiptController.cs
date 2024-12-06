namespace WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Abstraction.IServices;
    using Abstraction.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]s")] // Changed to match test's RequestUri = "api/receipts/"
    [Authorize]
    public class ReceiptController : Controller
    {
        private readonly IReceiptService _receiptService;

        public ReceiptController(IReceiptService receiptService)
        {
            this._receiptService = receiptService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReceiptModel>>> Get()
        {
            try
            {
                var receipts = await this._receiptService.GetAllAsync();
                return Ok(receipts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReceiptModel>> GetById(int id)
        {
            try
            {
                var receipt = await this._receiptService.GetByIdAsync(id);
                if (receipt == null)
                {
                    return NotFound($"Receipt with id {id} not found");
                }

                return Ok(receipt);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}/details")]
        public async Task<ActionResult<IEnumerable<ReceiptDetailModel>>> GetReceiptDetails(int id)
        {
            try
            {
                var details = await this._receiptService.GetReceiptDetailsAsync(id);
                return Ok(details);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}/sum")]// Changed from "total" to "sum" to match test
        public async Task<ActionResult<decimal>> GetSum(int id) // Changed method name to match test
        {
            try
            {
                var total = await this._receiptService.ToPayAsync(id);
                return Ok(total);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("period")]
        public async Task<ActionResult<IEnumerable<ReceiptModel>>> GetReceiptsByPeriod(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                var receipts = await this._receiptService.GetReceiptsByPeriodAsync(startDate, endDate);
                return Ok(receipts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ReceiptModel>> Create([FromBody] ReceiptModel receipt)
        {
            try
            {
                await this._receiptService.AddAsync(receipt);
                return Ok(receipt);  // Changed to match test expectation of returning the receipt
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{receiptId}/products/add/{productId}/{quantity}")]// Changed route to match test
        public async Task<IActionResult> AddProduct(
            int receiptId,
            int productId,
            int quantity)
        {
            try
            {
                await this._receiptService.AddProductAsync(productId, receiptId, quantity);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{receiptId}/products/remove/{productId}/{quantity}")]// Changed route to match test
        public async Task<IActionResult> RemoveProduct(
            int receiptId,
            int productId,
            int quantity)
        {
            try
            {
                await this._receiptService.RemoveProductAsync(productId, receiptId, quantity);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}/checkout")]
        public async Task<IActionResult> CheckOut(int id)
        {
            try
            {
                await this._receiptService.CheckOutAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ReceiptModel receipt)
        {
            try
            {
                if (id != receipt.Id)
                    return BadRequest("Id mismatch");

                await this._receiptService.UpdateAsync(receipt);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await this._receiptService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}