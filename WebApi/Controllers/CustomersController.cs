namespace WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            this._customerService = customerService;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerModel>>> Get()
        {
            var customers = await this._customerService.GetAllAsync();
            return Ok(customers);
        }

        // GET: api/customers/1
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerModel>> GetById(int id)
        {
            var customer = await this._customerService.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // GET: api/customers/products/1
        [HttpGet("products/{id}")]
        public async Task<ActionResult<IEnumerable<CustomerModel>>> GetByProductId(int id)
        {
            var customers = await this._customerService.GetCustomersByProductIdAsync(id);
            return Ok(customers);
        }

        // POST: api/customers
        [HttpPost]
        public async Task<ActionResult<CustomerModel>> Post([FromBody] CustomerModel value)
        {
            if (!IsValidCustomer(value))
            {
                return BadRequest(ModelState);
            }

            await this._customerService.AddAsync(value);

            return CreatedAtAction(nameof(GetById), new { id = value.Id }, value);
        }

        // PUT: api/customers/1
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CustomerModel value)
        {
            if (id != value.Id)
            {
                return this.BadRequest();
            }

            if (!IsValidCustomer(value))
            {
                return BadRequest(ModelState);
            }

            await this._customerService.UpdateAsync(value);
            return NoContent();
        }

        // DELETE: api/customers/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await this._customerService.DeleteAsync(id);
            return NoContent();
        }

        private bool IsValidCustomer(CustomerModel customer)
        {
            if (string.IsNullOrWhiteSpace(customer.Name))
            {
                this.ModelState.AddModelError(nameof(customer.Name), "Name is required");
                return false;
            }

            if (string.IsNullOrWhiteSpace(customer.Surname))
            {
                this.ModelState.AddModelError(nameof(customer.Surname), "Surname is required");
                return false;
            }

            if (customer.BirthDate > DateTime.Now || customer.BirthDate.Year < 1900)
            {
                this.ModelState.AddModelError(nameof(customer.BirthDate), "Birth date is not valid");
                return false;
            }

            if (customer.DiscountValue < 0)
            {
                this.ModelState.AddModelError(nameof(customer.DiscountValue), "Discount value cannot be negative");
                return false;
            }

            return true;
        }
    }
}
