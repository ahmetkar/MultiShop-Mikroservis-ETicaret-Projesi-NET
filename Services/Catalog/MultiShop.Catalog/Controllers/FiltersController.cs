using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Catalog.DTOs.FilterDTOs;
using MultiShop.Catalog.Services.FilterServices;

namespace MultiShop.Catalog.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FiltersController : ControllerBase
    {
        private readonly IFilterService _filterService;

        public FiltersController(IFilterService filterService)
        {
            _filterService = filterService;
        }

        [HttpGet]
        public async Task<IActionResult> FilterList()
        {
            var values = await _filterService.GetAllFilterAsync();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFilterById(string id)
        {
            var values = await _filterService.GetByIdFilterAsync(id);
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFilter(CreateFilterDto createFilterDto)
        {
            await _filterService.CreateFilterAsync(createFilterDto);
            return Ok("Filtre başarıyla eklendi");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFilter(string id)
        {
            await _filterService.DeleteFilterAsync(id);
            return Ok("Filtre başarıyla silindi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFilter(UpdateFilterDto updateFilterDto)
        {
            await _filterService.UpdateFilterAsync(updateFilterDto);
            return Ok("Filtre başarıyla güncellendi");
        }
    }
}
