using MultiShop.DtoLayer.CatalogDtos.ContactDTOs;

namespace MultiShop.WebUI.Services.CatalogServices.ContactServices
{
    public interface IContactService
    {
        Task<List<ResultContactDto>> GetAllContactAsync();
        Task CreateContactAsync(CreateContactDto createContactDto);
        Task DeleteContactAsync(string id);
        Task<GetByIdContactDto> GetByIdContact(string id);
    }
}
