using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.DTOs.CategoryDTOs;
using MultiShop.Catalog.DTOs.FeatureSliderDTOs;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.FeatureSliderServices
{
    public class FeatureSliderService : IFeatureSliderService
    {
        private readonly IMongoCollection<FeatureSlider> _sliderCollection;
        private readonly IMapper _mapper;

        public FeatureSliderService(IMapper mapper, IDatabaseSettings _databaseSetings)
        {
            _mapper = mapper;
            var client = new MongoClient(_databaseSetings.ConnectionStrings);
            var database = client.GetDatabase(_databaseSetings.DatabaseName);
            _sliderCollection = database.GetCollection<FeatureSlider>(_databaseSetings.FeatureSliderCollectionName);
        }



        public async Task CreateCatagoryAsync(CreateFeatureSliderDto createFeatureSliderDto)
        {
            var value = _mapper.Map<FeatureSlider>(createFeatureSliderDto);
            await _sliderCollection.InsertOneAsync(value);
        }

        public async Task DeleteFeatureSliderAsync(string id)
        {
            await _sliderCollection.DeleteOneAsync(x => x.FeatureSliderID == id);
        }

        public async Task FeatureSliderChangeStatusToFalse(string id)
        {
            throw new NotImplementedException();
        }

        public async Task FeatureSliderChangeStatusToTrue(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ResultFeatureSliderDto>> GetAllFeatureSliderAsync()
        {
            var values = await _sliderCollection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultFeatureSliderDto>>(values);
        }

        public async Task<GetByIdFeatureSliderDto> GetByIdFeatureSlider(string id)
        {
            var values = await _sliderCollection.Find<FeatureSlider>(x => x.FeatureSliderID == id).FirstOrDefaultAsync();
            return _mapper.Map<GetByIdFeatureSliderDto>(values);
        }

        public async Task UpdateFeatureSliderAsync(UpdateFeatureSliderDto updateFeatureSliderDto)
        {
            var values = _mapper.Map<FeatureSlider>(updateFeatureSliderDto);
            await _sliderCollection.FindOneAndReplaceAsync(x => x.FeatureSliderID == updateFeatureSliderDto.FeatureSliderID, values);
        }
    }
}
