using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Catalog.DTOs.ProductFilterDTOs;
using MultiShop.Catalog.Services.ProductFilterServices;

namespace MultiShop.Catalog.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductFiltersController : ControllerBase
    {
        private readonly IProductFilterService _productFilterService;

        public ProductFiltersController(IProductFilterService productFilterService)
        {
            _productFilterService = productFilterService;
        }

        [HttpGet]
        public async Task<IActionResult> ProductFilterList()
        {
            var values = await _productFilterService.GetAllProductFilterAsync();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductFilterById(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || id.Length != 24 || !id.All(c => "0123456789abcdefABCDEF".Contains(c)))
            {
                return NotFound("Geçersiz filtre ID formatı");
            }
            var values = await _productFilterService.GetByIdProductFilterAsync(id);
            if (values == null)
            {
                return NotFound("Ürün filtresi bulunamadı");
            }
            return Ok(values);
        }

        [HttpGet("GetProductFiltersByCategoryId/{categoryId?}")]
        public async Task<IActionResult> GetProductFiltersByCategoryId(string? categoryId)
        {
            if (string.IsNullOrWhiteSpace(categoryId))
            {
                return Ok(new List<ResultProductFilterDto>());
            }
            var values = await _productFilterService.GetProductFiltersByCategoryIdAsync(categoryId);
            return Ok(values);
        }

        [HttpPost("AssignFiltersToCategory")]
        public async Task<IActionResult> AssignFiltersToCategory(CategoryFilterAssignDto assignDto)
        {
            await _productFilterService.AssignFiltersToCategoryAsync(assignDto);
            return Ok("Kategori filtreleri başarıyla atandı");
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductFilter(CreateProductFilterDto createProductFilterDto)
        {
            await _productFilterService.CreateProductFilterAsync(createProductFilterDto);
            return Ok("Ürün filtresi başarıyla eklendi");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductFilter(string id)
        {
            await _productFilterService.DeleteProductFilterAsync(id);
            return Ok("Ürün filtresi başarıyla silindi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProductFilter(UpdateProductFilterDto updateProductFilterDto)
        {
            await _productFilterService.UpdateProductFilterAsync(updateProductFilterDto);
            return Ok("Ürün filtresi başarıyla güncellendi");
        }
    }
}
