using MultiShop.Catalog.DTOs.ContactDTOs;

namespace MultiShop.Catalog.Services.ContactServices
{
    public interface IContactService
    {
        Task<List<ResultContactDto>> GetAllContactAsync();
        Task CreateContactAsync(CreateContactDto createContactDto);
        Task DeleteContactAsync(string id);
        Task<GetByIdContactDto> GetByIdContact(string id);
    }
}
