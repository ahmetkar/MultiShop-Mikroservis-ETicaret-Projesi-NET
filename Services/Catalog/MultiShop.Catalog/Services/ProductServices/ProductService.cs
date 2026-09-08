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
        private readonly IMongoCollection<Filter> _filterCollection;
        public ProductService(IMapper mapper,IDatabaseSettings _databaseSettings)
        {
            _mapper = mapper;
            var client = new MongoClient(_databaseSettings.ConnectionStrings);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _productCollection = database.GetCollection<Product>(_databaseSettings.ProductCollectionName);
            _categoryCollection = database.GetCollection<Category>(_databaseSettings.CategoryCollectionName);
            _filterCollection = database.GetCollection<Filter>(_databaseSettings.FilterCollectionName);
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

        public async Task<List<ResultProductsWithCategoryDto>> GetProductsByIdsAsync(List<string> productIds)
        {
            if (productIds == null || productIds.Count == 0)
            {
                return new List<ResultProductsWithCategoryDto>();
            }

            var filter = Builders<Product>.Filter.In(x => x.ProductId, productIds);
            var values = await _productCollection.Find(filter).ToListAsync();

            foreach (var item in values)
            {
                var category = await _categoryCollection.Find(x => x.CategoryID == item.CategoryID).FirstOrDefaultAsync();
                item.Category = category;
            }

            return _mapper.Map<List<ResultProductsWithCategoryDto>>(values);
        }

        public async Task UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            var values = _mapper.Map<Product>(updateProductDto);
            await _productCollection.FindOneAndReplaceAsync(x=>x.ProductId == updateProductDto.ProductId,values);
        }

        public async Task AdjustProductFilterStockAsync(string productId, string filterId, int delta)
        {
            var product = await _productCollection.Find(x => x.ProductId == productId).FirstOrDefaultAsync();
            if (product == null) return;

            if (product.FilterStocks == null) product.FilterStocks = new Dictionary<string, int>();

            if (product.FilterStocks.ContainsKey(filterId))
            {
                product.FilterStocks[filterId] = Math.Max(0, product.FilterStocks[filterId] + delta);
            }
            else
            {
                product.FilterStocks[filterId] = Math.Max(0, delta);
            }

            await _productCollection.ReplaceOneAsync(x => x.ProductId == productId, product);
        }

        public async Task DecreaseProductFilterStockAsync(string productId, List<string>? filterIdentifiers, int amount = 1)
        {
            var product = await _productCollection.Find(x => x.ProductId == productId).FirstOrDefaultAsync();
            if (product == null) return;

            if (product.FilterStocks == null) product.FilterStocks = new Dictionary<string, int>();

            if (filterIdentifiers != null && filterIdentifiers.Count > 0)
            {
                var allFilters = await _filterCollection.Find(x => true).ToListAsync();
                var productFilterIds = product.FilterIds ?? new List<string>();

                var relevantFilters = allFilters.Where(f => productFilterIds.Contains(f.FilterId)).ToList();
                if (relevantFilters.Count == 0) relevantFilters = allFilters;

                var decrementedFilterIds = new HashSet<string>();

                foreach (var desc in filterIdentifiers)
                {
                    if (string.IsNullOrWhiteSpace(desc)) continue;
                    var trimmed = desc.Trim();

                    Filter? matched = null;

                    var colonIdx = trimmed.IndexOf(':');
                    if (colonIdx >= 0)
                    {
                        var title = trimmed.Substring(0, colonIdx).Trim();
                        var val = trimmed.Substring(colonIdx + 1).Trim();

                        matched = relevantFilters.FirstOrDefault(f =>
                            f.FilterTitle.Equals(title, StringComparison.OrdinalIgnoreCase) &&
                            f.FilterName.Equals(val, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        matched = relevantFilters.FirstOrDefault(f => f.FilterId == trimmed);
                        if (matched == null)
                        {
                            matched = relevantFilters.FirstOrDefault(f =>
                                f.FilterName.Equals(trimmed, StringComparison.OrdinalIgnoreCase));
                        }
                    }

                    if (matched != null && !decrementedFilterIds.Contains(matched.FilterId))
                    {
                        var fId = matched.FilterId;
                        if (product.FilterStocks.ContainsKey(fId))
                        {
                            product.FilterStocks[fId] = Math.Max(0, product.FilterStocks[fId] - amount);
                            decrementedFilterIds.Add(fId);
                        }
                    }
                }
            }
            else
            {
                foreach (var key in product.FilterStocks.Keys.ToList())
                {
                    product.FilterStocks[key] = Math.Max(0, product.FilterStocks[key] - amount);
                }
            }

            await _productCollection.ReplaceOneAsync(x => x.ProductId == productId, product);
        }

        public async Task IncreaseProductFilterStockAsync(string productId, List<string>? filterIdentifiers, int amount = 1)
        {
            var product = await _productCollection.Find(x => x.ProductId == productId).FirstOrDefaultAsync();
            if (product == null) return;

            if (product.FilterStocks == null) product.FilterStocks = new Dictionary<string, int>();

            if (filterIdentifiers != null && filterIdentifiers.Count > 0)
            {
                var allFilters = await _filterCollection.Find(x => true).ToListAsync();
                var productFilterIds = product.FilterIds ?? new List<string>();

                var relevantFilters = allFilters.Where(f => productFilterIds.Contains(f.FilterId)).ToList();
                if (relevantFilters.Count == 0) relevantFilters = allFilters;

                var incrementedFilterIds = new HashSet<string>();

                foreach (var desc in filterIdentifiers)
                {
                    if (string.IsNullOrWhiteSpace(desc)) continue;
                    var trimmed = desc.Trim();

                    Filter? matched = null;

                    var colonIdx = trimmed.IndexOf(':');
                    if (colonIdx >= 0)
                    {
                        var title = trimmed.Substring(0, colonIdx).Trim();
                        var val = trimmed.Substring(colonIdx + 1).Trim();

                        matched = relevantFilters.FirstOrDefault(f =>
                            f.FilterTitle.Equals(title, StringComparison.OrdinalIgnoreCase) &&
                            f.FilterName.Equals(val, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        matched = relevantFilters.FirstOrDefault(f => f.FilterId == trimmed);
                        if (matched == null)
                        {
                            matched = relevantFilters.FirstOrDefault(f =>
                                f.FilterName.Equals(trimmed, StringComparison.OrdinalIgnoreCase));
                        }
                    }

                    if (matched != null && !incrementedFilterIds.Contains(matched.FilterId))
                    {
                        var fId = matched.FilterId;
                        if (product.FilterStocks.ContainsKey(fId))
                        {
                            product.FilterStocks[fId] += amount;
                        }
                        else
                        {
                            product.FilterStocks[fId] = amount;
                        }
                        incrementedFilterIds.Add(fId);
                    }
                }
            }
            else
            {
                foreach (var key in product.FilterStocks.Keys.ToList())
                {
                    product.FilterStocks[key] += amount;
                }
            }

            await _productCollection.ReplaceOneAsync(x => x.ProductId == productId, product);
        }
    }
}
