using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.DTOs.CategoryDTOs;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMongoCollection<ProductFilter> _productFilterCollection;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            _mapper = mapper;
            var client = new MongoClient(_databaseSettings.ConnectionStrings);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _categoryCollection = database.GetCollection<Category>(_databaseSettings.CategoryCollectionName);
            _productFilterCollection = database.GetCollection<ProductFilter>(_databaseSettings.ProductFilterCollectionName);
        }

        public async Task CreateCatagoryAsync(CreateCategoryDto createCategoryDto)
        {
            var value = _mapper.Map<Category>(createCategoryDto);
            await _categoryCollection.InsertOneAsync(value);

            if (createCategoryDto.SelectedFilterIds != null && createCategoryDto.SelectedFilterIds.Count > 0)
            {
                var list = createCategoryDto.SelectedFilterIds.Select(fId => new ProductFilter
                {
                    CategoryId = value.CategoryID,
                    FilterId = fId
                }).ToList();
                await _productFilterCollection.InsertManyAsync(list);
            }
        }

        public async Task DeleteCategoryAsync(string id)
        {
            await _categoryCollection.DeleteOneAsync(x => x.CategoryID == id);
            await _productFilterCollection.DeleteManyAsync(x => x.CategoryId == id);
        }

        public async Task<List<ResultCategoryDto>> GetAllCategoryAsync()
        {
            var values = await _categoryCollection.Find(x => true).ToListAsync();
            var dtos = _mapper.Map<List<ResultCategoryDto>>(values);
            foreach (var item in dtos)
            {
                var filters = await _productFilterCollection.Find(x => x.CategoryId == item.CategoryID).ToListAsync();
                item.SelectedFilterIds = filters.Select(x => x.FilterId).ToList();
            }
            return dtos;
        }

        public async Task<GetByIdCategoryDto> GetByIdCategory(string id)
        {
            var values = await _categoryCollection.Find<Category>(x => x.CategoryID == id).FirstOrDefaultAsync();
            var dto = _mapper.Map<GetByIdCategoryDto>(values);
            if (dto != null)
            {
                var filters = await _productFilterCollection.Find(x => x.CategoryId == id).ToListAsync();
                dto.SelectedFilterIds = filters.Select(x => x.FilterId).ToList();
            }
            return dto;
        }

        public async Task UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto)
        {
            var values = _mapper.Map<Category>(updateCategoryDto);
            await _categoryCollection.FindOneAndReplaceAsync(x => x.CategoryID == updateCategoryDto.CategoryID, values);

            await _productFilterCollection.DeleteManyAsync(x => x.CategoryId == updateCategoryDto.CategoryID);
            if (updateCategoryDto.SelectedFilterIds != null && updateCategoryDto.SelectedFilterIds.Count > 0)
            {
                var list = updateCategoryDto.SelectedFilterIds.Select(fId => new ProductFilter
                {
                    CategoryId = updateCategoryDto.CategoryID,
                    FilterId = fId
                }).ToList();
                await _productFilterCollection.InsertManyAsync(list);
            }
        }
    }
}
