﻿using MultiShop.Catalog.DTOs.BrandDTOs;

namespace MultiShop.Catalog.Services.BrandServices
{
    public interface IBrandService
    {
        Task<List<ResultBrandDto>> GetAllBrandAsync();
        Task CreateBrandAsync(CreateBrandDto createBrandDto);
        Task UpdateBrandAsync(UpdateBrandDto updateBrandDto);
        Task DeleteBrandAsync(string id);
        Task<GetByIdBrandDto> GetByIdBrand(string id);
    }
}
