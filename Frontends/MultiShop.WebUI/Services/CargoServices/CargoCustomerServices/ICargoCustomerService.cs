using MultiShop.DtoLayer.CargoDtos.CargoCustotmerDtos;

namespace MultiShop.WebUI.Services.CargoServices.CargoCustomerServices
{ 

    public interface ICargoCustomerService
    {
        Task<GetCargoCustomerByIdDto> GetByIdCargoCustomerInfoAsync(string id);
    }
}
