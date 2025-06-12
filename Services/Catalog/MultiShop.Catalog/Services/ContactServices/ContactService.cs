using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.DTOs.ContactDTOs;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.ContactServices
{
    public class ContactService :IContactService
    {
        private readonly IMongoCollection<Contact> _ContactCollection;
        private readonly IMapper _mapper;

        public ContactService(IMapper mapper, IDatabaseSettings _databaseSetings)
        {
            _mapper = mapper;
            var client = new MongoClient(_databaseSetings.ConnectionStrings);
            var database = client.GetDatabase(_databaseSetings.DatabaseName);
            _ContactCollection = database.GetCollection<Contact>(_databaseSetings.ContactCollectionName);

        }

        public async Task CreateContactAsync(CreateContactDto createContactDto)
        {
            var value = _mapper.Map<Contact>(createContactDto);
            await _ContactCollection.InsertOneAsync(value);
        }

        public async Task DeleteContactAsync(string id)
        {
            await _ContactCollection.DeleteOneAsync(x => x.ContactId == id);
        }

        public async Task<List<ResultContactDto>> GetAllContactAsync()
        {
            var values = await _ContactCollection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultContactDto>>(values);
        }   

        public async Task<GetByIdContactDto> GetByIdContact(string id)
        {
            var values = await _ContactCollection.Find<Contact>(x => x.ContactId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetByIdContactDto>(values);
        }

        
    }
}
