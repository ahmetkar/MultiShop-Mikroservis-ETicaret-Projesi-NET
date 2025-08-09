using MultiShop.Payment.DTOs;

namespace MultiShop.Payment.Services
{
    public interface IPaymentService
    {
        Task<ResultPaymentDto> GetPaymentByOrderingId(int id);
        Task<bool> AddPayment(CreatePaymentDto createPaymentDto);
        Task<bool> CancelPaymentByOrderingId(int id);
        Task<List<ResultPaymentDto>> GetAllPaymentByUserId(string id);


    }
}
