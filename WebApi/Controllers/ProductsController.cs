namespace WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Business.Interfaces;
    using Business.Models;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            this._productService = productService;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductModel>>> Get([FromQuery] FilterSearchModel filterSearch)
        {
            var products = await this._productService.GetByFilterAsync(filterSearch);
            return Ok(products);
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetById(int id)
        {
            var product = await this._productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<ProductModel>> Post([FromBody] ProductModel productModel)
        {
            if (!IsValidProduct(productModel))
            {
                return BadRequest(ModelState);
            }

            await this._productService.AddAsync(productModel);
            return CreatedAtAction(nameof(GetById), new { id = productModel.Id }, productModel);
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProductModel productModel)
        {
            if (!IsValidProduct(productModel) || id != productModel.Id)
            {
                return BadRequest(ModelState);
            }

            await this._productService.UpdateAsync(productModel);
            return NoContent();
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await this._productService.DeleteAsync(id);
            return NoContent();
        }

        // GET: api/products/categories
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<ProductCategoryModel>>> GetCategories()
        {
            var categories = await this._productService.GetAllProductCategoriesAsync();
            return Ok(categories);
        }

        // POST: api/products/categories
        [HttpPost("categories")]
        public async Task<ActionResult<ProductCategoryModel>> PostCategory([FromBody] ProductCategoryModel categoryModel)
        {
            if (!IsValidCategory(categoryModel))
            {
                return BadRequest(ModelState);
            }

            await this._productService.AddCategoryAsync(categoryModel);
            return CreatedAtAction(nameof(GetCategories), new { id = categoryModel.Id }, categoryModel);
        }

        // PUT: api/products/categories/{id}
        [HttpPut("categories/{id}")]
        public async Task<ActionResult> PutCategory(int id, [FromBody] ProductCategoryModel categoryModel)
        {
            if (!IsValidCategory(categoryModel) || id != categoryModel.Id)
            {
                return BadRequest(ModelState);
            }

            await this._productService.UpdateCategoryAsync(categoryModel);
            return NoContent();
        }

        // DELETE: api/products/categories/{id}
        [HttpDelete("categories/{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            await this._productService.RemoveCategoryAsync(id);
            return NoContent();
        }

        private bool IsValidProduct(ProductModel product)
        {
            if (string.IsNullOrWhiteSpace(product.ProductName))
            {
                this.ModelState.AddModelError(nameof(product.ProductName), "Name is required.");
                return false;
            }

            if (product.Price < 0)
            {
                this.ModelState.AddModelError(nameof(product.Price), "Price cannot be negative.");
                return false;
            }

            return true;
        }

        private bool IsValidCategory(ProductCategoryModel category)
        {
            if (string.IsNullOrWhiteSpace(category.CategoryName))
            {
                this.ModelState.AddModelError(nameof(category.CategoryName), "Category name is required.");
                return false;
            }

            return true;
        }
    }
}
