using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.DTOs.AboutDTOs;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.AboutServices
{
    public class AboutService : IAboutService
    {

        private readonly IMongoCollection<About> _aboutCollection;
        private readonly IMapper _mapper;

        public AboutService(IMapper mapper, IDatabaseSettings _databaseSetings)
        {
            _mapper = mapper;
            var client = new MongoClient(_databaseSetings.ConnectionStrings);
            var database = client.GetDatabase(_databaseSetings.DatabaseName);
            _aboutCollection = database.GetCollection<About>(_databaseSetings.AboutCollectionName);

        }

        public async Task CreateCatagoryAsync(CreateAboutDto createAboutDto)
        {
            var value = _mapper.Map<About>(createAboutDto);
            await _aboutCollection.InsertOneAsync(value);
        }

        public async Task DeleteAboutAsync(string id)
        {
            await _aboutCollection.DeleteOneAsync(x => x.AboutId == id);
        }

        public async Task<ResultAboutDto> GetLastAboutAsync()
        {
            var values = await _aboutCollection.Find(x => true).ToListAsync();
            var lastAbout = values.LastOrDefault();

            return _mapper.Map<ResultAboutDto>(lastAbout);
        }

        public async Task<List<ResultAboutDto>> GetAllAboutAsync()
        {
            var values = await _aboutCollection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultAboutDto>>(values);
        }

        public async Task<GetByIdAboutDto> GetByIdAbout(string id)
        {
            var values = await _aboutCollection.Find<About>(x => x.AboutId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetByIdAboutDto>(values);
        }

        public async Task UpdateAboutAsync(UpdateAboutDto updateAboutDto)
        {
            var values = _mapper.Map<About>(updateAboutDto);
            await _aboutCollection.FindOneAndReplaceAsync(x => x.AboutId == updateAboutDto.AboutId, values);
        }
    }
}
