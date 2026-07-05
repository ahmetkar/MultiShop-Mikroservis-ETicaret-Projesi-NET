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

        public async Task<ResultAboutDto> GetAbout()
        {
            Console.WriteLine(_aboutCollection.CollectionNamespace.FullName);

            var values = await _aboutCollection.Find(x=>true)
                .SortByDescending(x=>x.AboutId).FirstOrDefaultAsync();
            return _mapper.Map<ResultAboutDto>(values);
        }

        public async Task<bool> UpdateAboutAsync(UpdateAboutDto updateAboutDto)
        {
            var values = _mapper.Map<About>(updateAboutDto);
            var res = await _aboutCollection.ReplaceOneAsync(x=>x.AboutId == updateAboutDto.AboutId,values,new ReplaceOptions
            {
                IsUpsert = true
            });
            if(res.UpsertedId != updateAboutDto.AboutId)
            {

                return false;
            }else
            {
                return true;   
            }
        }
    }
}
