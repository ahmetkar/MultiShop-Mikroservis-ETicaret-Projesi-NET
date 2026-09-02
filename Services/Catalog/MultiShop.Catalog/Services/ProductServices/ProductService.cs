using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.DTOs.ProductDTOs;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Product> _productCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        public ProductService(IMapper mapper,IDatabaseSettings _databaseSettings)
        {
            _mapper = mapper;
            var client = new MongoClient(_databaseSettings.ConnectionStrings);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _productCollection = database.GetCollection<Product>(_databaseSettings.ProductCollectionName);
            _categoryCollection = database.GetCollection<Category>(_databaseSettings.CategoryCollectionName);

        }
        public async Task CreateProductAsync(CreateProductDto createProductDto)
        {
            var values = _mapper.Map<Product>(createProductDto);
            await _productCollection.InsertOneAsync(values);
        }

        public async Task DeleteProductAsync(string id)
        {
            await _productCollection.DeleteOneAsync(x=>x.ProductId == id);
        }

        public async Task<List<ResultProductDto>> GetAllProductAsync()
        {
            var values = await _productCollection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultProductDto>>(values);
        }

        public async Task<GetByIdProductDto> GetByIdProduct(string id)
        {
            var values = await _productCollection.Find<Product>(x=>x.ProductId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetByIdProductDto>(values);
        }

        public async Task<List<ResultProductsWithCategoryDto>> GetProductsWithCategoryAsync()
        {
            var values = await _productCollection.Find(x => true).ToListAsync();
            foreach(var item in values)
            {
                var category = await _categoryCollection.Find(x => x.CategoryID == item.CategoryID).FirstOrDefaultAsync();
                item.Category = category;
            }
            return _mapper.Map<List<ResultProductsWithCategoryDto>>(values);
        }

        public async Task<List<ResultProductsWithCategoryDto>> GetProductsWithCategoryByCategoryIdAsync(string CategoryId)
        {
            var values = await _productCollection.Find(x => x.CategoryID == CategoryId).ToListAsync();
            foreach (var item in values)
            {
                var category = await _categoryCollection.Find(x => x.CategoryID == item.CategoryID).FirstOrDefaultAsync();
                item.Category = category;
            }
            return _mapper.Map<List<ResultProductsWithCategoryDto>>(values);
        }

        public async Task<List<ResultProductsWithCategoryDto>> GetProductsWithCategoryByCategoryIdAndFiltersAsync(string CategoryId, List<string>? filterIds, int page = 1, int pageSize = 9)
        {
            var builder = Builders<Product>.Filter;
            var filter = builder.Eq(x => x.CategoryID, CategoryId);

            if (filterIds != null && filterIds.Count > 0)
            {
                filter = filter & builder.AnyIn(x => x.FilterIds, filterIds);
            }

            var values = await _productCollection.Find(filter)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            foreach (var item in values)
            {
                var category = await _categoryCollection.Find(x => x.CategoryID == item.CategoryID).FirstOrDefaultAsync();
                item.Category = category;
            }
            return _mapper.Map<List<ResultProductsWithCategoryDto>>(values);
        }

        public async Task<long> GetProductCountByCategoryIdAndFiltersAsync(string CategoryId, List<string>? filterIds)
        {
            var builder = Builders<Product>.Filter;
            var filter = builder.Eq(x => x.CategoryID, CategoryId);

            if (filterIds != null && filterIds.Count > 0)
            {
                filter = filter & builder.AnyIn(x => x.FilterIds, filterIds);
            }

            return await _productCollection.CountDocumentsAsync(filter);
        }

        public async Task<List<ResultProductsWithCategoryDto>> GetLast20ProductsAsync()
        {
            var values = await _productCollection.Find(x => true)
                .SortByDescending(x => x.ProductId)
                .Limit(20)
                .ToListAsync();

            foreach (var item in values)
            {
                var category = await _categoryCollection.Find(x => x.CategoryID == item.CategoryID).FirstOrDefaultAsync();
                item.Category = category;
            }
            return _mapper.Map<List<ResultProductsWithCategoryDto>>(values);
        }

        public async Task<List<ResultProductsWithCategoryDto>> SearchProductsAsync(string query, int page = 1, int pageSize = 9)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return new List<ResultProductsWithCategoryDto>();
            }

            var builder = Builders<Product>.Filter;
            var q = query.Trim();
            var regex = new MongoDB.Bson.BsonRegularExpression(q, "i");

            var filter = builder.Or(
                builder.Regex(x => x.ProductName, regex),
                builder.Regex(x => x.ProductDescription, regex),
                builder.Regex(x => x.CategoryName, regex)
            );

            var values = await _productCollection.Find(filter)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            foreach (var item in values)
            {
                var category = await _categoryCollection.Find(x => x.CategoryID == item.CategoryID).FirstOrDefaultAsync();
                item.Category = category;
            }
            return _mapper.Map<List<ResultProductsWithCategoryDto>>(values);
        }

        public async Task<long> GetSearchProductCountAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return 0;
            }

            var builder = Builders<Product>.Filter;
            var q = query.Trim();
            var regex = new MongoDB.Bson.BsonRegularExpression(q, "i");

            var filter = builder.Or(
                builder.Regex(x => x.ProductName, regex),
                builder.Regex(x => x.ProductDescription, regex),
                builder.Regex(x => x.CategoryName, regex)
            );

            return await _productCollection.CountDocumentsAsync(filter);
        }

        public async Task UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            var values = _mapper.Map<Product>(updateProductDto);
            await _productCollection.FindOneAndReplaceAsync(x=>x.ProductId == updateProductDto.ProductId,values);

        }
    }
}
