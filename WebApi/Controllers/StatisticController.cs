namespace WebApi.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Business.Interfaces;
	using Business.Models;
	using Microsoft.AspNetCore.Mvc;

	[ApiController]
	[Route("api/statistics")] // Set route to match test URI
	public class StatisticController : Controller
	{
		private readonly IStatisticService _statisticService;

		public StatisticController(IStatisticService statisticService)
		{
			_statisticService = statisticService;
		}

		// GET /api/statistics/popularProducts?productCount=2
		[HttpGet("popularProducts")]
		public async Task<ActionResult<IEnumerable<ProductModel>>> GetMostPopularProducts(int productCount)
		{
			try
			{
				var products = await _statisticService.GetMostPopularProductsAsync(productCount);
				return Ok(products);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		// GET /api/statistics/customer/{id}/{productCount}
		[HttpGet("customer/{id}/{productCount}")]
		public async Task<ActionResult<IEnumerable<ProductModel>>> GetCustomerMostPopularProducts(int id, int productCount)
		{
			try
			{
				var products = await _statisticService.GetCustomersMostPopularProductsAsync(productCount, id);
				return Ok(products);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		// GET /api/statistics/activity/{customerCount}?startDate=2020-7-21&endDate=2022-7-22
		[HttpGet("activity/{customerCount}")]
		public async Task<ActionResult<IEnumerable<CustomerActivityModel>>> GetMostValuableCustomers(
			int customerCount,
			[FromQuery] DateTime startDate,
			[FromQuery] DateTime endDate)
		{
			try
			{
				var customers = await _statisticService.GetMostValuableCustomersAsync(customerCount, startDate, endDate);
				return Ok(customers);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		// GET /api/statistics/income/{categoryId}?startDate=2020-7-21&endDate=2020-7-22
		[HttpGet("income/{categoryId}")]
		public async Task<ActionResult<decimal>> GetIncomeOfCategoryInPeriod(
			int categoryId,
			[FromQuery] DateTime startDate,
			[FromQuery] DateTime endDate)
		{
			try
			{
				var income = await _statisticService.GetIncomeOfCategoryInPeriod(categoryId, startDate, endDate);
				return Ok(income);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}
}
