using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.DTOs.FilterDTOs;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.FilterServices
{
    public class FilterService : IFilterService
    {
        private readonly IMongoCollection<Filter> _filterCollection;
        private readonly IMapper _mapper;

        public FilterService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionStrings);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _filterCollection = database.GetCollection<Filter>(_databaseSettings.FilterCollectionName);
            _mapper = mapper;
        }

        public async Task CreateFilterAsync(CreateFilterDto createFilterDto)
        {
            var value = _mapper.Map<Filter>(createFilterDto);
            await _filterCollection.InsertOneAsync(value);
        }

        public async Task DeleteFilterAsync(string id)
        {
            await _filterCollection.DeleteOneAsync(x => x.FilterId == id);
        }

        public async Task<List<ResultFilterDto>> GetAllFilterAsync()
        {
            var values = await _filterCollection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultFilterDto>>(values);
        }

        public async Task<GetByIdFilterDto> GetByIdFilterAsync(string id)
        {
            var value = await _filterCollection.Find(x => x.FilterId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetByIdFilterDto>(value);
        }

        public async Task UpdateFilterAsync(UpdateFilterDto updateFilterDto)
        {
            var value = _mapper.Map<Filter>(updateFilterDto);
            await _filterCollection.FindOneAndReplaceAsync(x => x.FilterId == updateFilterDto.FilterId, value);
        }
    }
}
