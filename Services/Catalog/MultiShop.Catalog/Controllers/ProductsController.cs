using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Catalog.DTOs.ProductDTOs;
using MultiShop.Catalog.Services.ProductServices;

namespace MultiShop.Catalog.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _ProductService;
        public ProductsController(IProductService ProductService)
        {
            _ProductService = ProductService;
        }

        [HttpGet]
        public async Task<IActionResult> ProductList()
        {
            var values = await _ProductService.GetAllProductAsync();
            return Ok(values);
        }

       
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {   
            var values = await _ProductService.GetByIdProduct(id);
            return Ok(values);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto createProductDto)
        {
            await _ProductService.CreateProductAsync(createProductDto);
            return Ok("Product başarıyla eklendi");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            await _ProductService.DeleteProductAsync(id);

            return Ok("Product başarıyla silindi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProductDto)
        {
            await _ProductService.UpdateProductAsync(updateProductDto);
            return Ok("Product başarıyla güncellendi.");
        }

        [HttpGet("ProductListWithCategory")]
        public async Task<IActionResult> GetProductWithCategory()
        {
            var values = await _ProductService.GetProductsWithCategoryAsync();
            return Ok(values);
        }

        [HttpGet("ProductListWithCategoryByCategoryId")]
        public async Task<IActionResult> ProductListWithCategoryByCategoryId(string id)
        {
            var values = await _ProductService.GetProductsWithCategoryByCategoryIdAsync(id);
            return Ok(values);
        }

        [HttpGet("GetProductsByCategoryAndFilters")]
        public async Task<IActionResult> GetProductsByCategoryAndFilters([FromQuery] string categoryId, [FromQuery] List<string>? filterIds, [FromQuery] int page = 1, [FromQuery] int pageSize = 9)
        {
            var values = await _ProductService.GetProductsWithCategoryByCategoryIdAndFiltersAsync(categoryId, filterIds, page, pageSize);
            return Ok(values);
        }

        [HttpGet("GetProductCountByCategoryAndFilters")]
        public async Task<IActionResult> GetProductCountByCategoryAndFilters([FromQuery] string categoryId, [FromQuery] List<string>? filterIds)
        {
            var count = await _ProductService.GetProductCountByCategoryIdAndFiltersAsync(categoryId, filterIds);
            return Ok(count);
        }

        [HttpGet("GetLast20Products")]
        public async Task<IActionResult> GetLast20Products()
        {
            var values = await _ProductService.GetLast20ProductsAsync();
            return Ok(values);
        }

        [HttpGet("SearchProducts")]
        public async Task<IActionResult> SearchProducts([FromQuery] string query, [FromQuery] int page = 1, [FromQuery] int pageSize = 9)
        {
            var values = await _ProductService.SearchProductsAsync(query, page, pageSize);
            return Ok(values);
        }

        [HttpGet("GetSearchProductCount")]
        public async Task<IActionResult> GetSearchProductCount([FromQuery] string query)
        {
            var count = await _ProductService.GetSearchProductCountAsync(query);
            return Ok(count);
        }

        [HttpPost("GetProductsByIds")]
        public async Task<IActionResult> GetProductsByIds([FromBody] List<string> productIds)
        {
            var values = await _ProductService.GetProductsByIdsAsync(productIds);
            return Ok(values);
        }
    }
}
