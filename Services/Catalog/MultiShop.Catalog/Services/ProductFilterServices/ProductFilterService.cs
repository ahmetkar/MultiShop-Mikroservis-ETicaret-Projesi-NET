using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.DTOs.ProductFilterDTOs;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.ProductFilterServices
{
    public class ProductFilterService : IProductFilterService
    {
        private readonly IMongoCollection<ProductFilter> _productFilterCollection;
        private readonly IMongoCollection<Filter> _filterCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public ProductFilterService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionStrings);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _productFilterCollection = database.GetCollection<ProductFilter>(_databaseSettings.ProductFilterCollectionName);
            _filterCollection = database.GetCollection<Filter>(_databaseSettings.FilterCollectionName);
            _categoryCollection = database.GetCollection<Category>(_databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }

        public async Task AssignFiltersToCategoryAsync(CategoryFilterAssignDto assignDto)
        {
            await _productFilterCollection.DeleteManyAsync(x => x.CategoryId == assignDto.CategoryId);
            if (assignDto.FilterIds != null && assignDto.FilterIds.Count > 0)
            {
                var list = assignDto.FilterIds.Select(fId => new ProductFilter
                {
                    CategoryId = assignDto.CategoryId,
                    FilterId = fId
                }).ToList();
                await _productFilterCollection.InsertManyAsync(list);
            }
        }

        public async Task CreateProductFilterAsync(CreateProductFilterDto createProductFilterDto)
        {
            var value = _mapper.Map<ProductFilter>(createProductFilterDto);
            await _productFilterCollection.InsertOneAsync(value);
        }

        public async Task DeleteProductFilterAsync(string id)
        {
            await _productFilterCollection.DeleteOneAsync(x => x.ProductFilterId == id);
        }

        public async Task<List<ResultProductFilterDto>> GetAllProductFilterAsync()
        {
            var values = await _productFilterCollection.Find(x => true).ToListAsync();
            var dtos = _mapper.Map<List<ResultProductFilterDto>>(values);
            foreach (var item in dtos)
            {
                var filter = await _filterCollection.Find(x => x.FilterId == item.FilterId).FirstOrDefaultAsync();
                if (filter != null)
                {
                    item.FilterTitle = filter.FilterTitle;
                    item.FilterName = filter.FilterName;
                }
                var category = await _categoryCollection.Find(x => x.CategoryID == item.CategoryId).FirstOrDefaultAsync();
                if (category != null)
                {
                    item.CategoryName = category.CategoryName;
                }
            }
            return dtos;
        }

        public async Task<GetByIdProductFilterDto> GetByIdProductFilterAsync(string id)
        {
            var value = await _productFilterCollection.Find(x => x.ProductFilterId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetByIdProductFilterDto>(value);
        }

        public async Task<List<ResultProductFilterDto>> GetProductFiltersByCategoryIdAsync(string categoryId)
        {
            var values = await _productFilterCollection.Find(x => x.CategoryId == categoryId).ToListAsync();
            var dtos = _mapper.Map<List<ResultProductFilterDto>>(values);
            foreach (var item in dtos)
            {
                var filter = await _filterCollection.Find(x => x.FilterId == item.FilterId).FirstOrDefaultAsync();
                if (filter != null)
                {
                    item.FilterTitle = filter.FilterTitle;
                    item.FilterName = filter.FilterName;
                }
                var category = await _categoryCollection.Find(x => x.CategoryID == item.CategoryId).FirstOrDefaultAsync();
                if (category != null)
                {
                    item.CategoryName = category.CategoryName;
                }
            }
            return dtos;
        }

        public async Task UpdateProductFilterAsync(UpdateProductFilterDto updateProductFilterDto)
        {
            var value = _mapper.Map<ProductFilter>(updateProductFilterDto);
            await _productFilterCollection.FindOneAndReplaceAsync(x => x.ProductFilterId == updateProductFilterDto.ProductFilterId, value);
        }
    }
}
