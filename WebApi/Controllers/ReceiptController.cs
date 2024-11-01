namespace WebApi.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Business.Interfaces;
	using Business.Models;
	using Microsoft.AspNetCore.Mvc;

	[ApiController]
	[Route("api/[controller]s")] // Changed to match test's RequestUri = "api/receipts/"
	public class ReceiptController : Controller
	{
		private readonly IReceiptService _receiptService;

		public ReceiptController(IReceiptService receiptService)
		{
			_receiptService = receiptService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<ReceiptModel>>> Get()
		{
			try
			{
				var receipts = await _receiptService.GetAllAsync();
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
				var receipt = await _receiptService.GetByIdAsync(id);
				if (receipt == null)
					return NotFound($"Receipt with id {id} not found");

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
				var details = await _receiptService.GetReceiptDetailsAsync(id);
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
				var total = await _receiptService.ToPayAsync(id);
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
				var receipts = await _receiptService.GetReceiptsByPeriodAsync(startDate, endDate);
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
				await _receiptService.AddAsync(receipt);
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
				await _receiptService.AddProductAsync(productId, receiptId, quantity);
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
				await _receiptService.RemoveProductAsync(productId, receiptId, quantity);
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
				await _receiptService.CheckOutAsync(id);
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

				await _receiptService.UpdateAsync(receipt);
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
				await _receiptService.DeleteAsync(id);
				return Ok();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}
}