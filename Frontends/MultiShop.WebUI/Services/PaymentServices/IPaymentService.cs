using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.PaymentDtos;

namespace MultiShop.WebUI.Services.PaymentServices
{
    public interface IPaymentService
    {
        Task<ResultPaymentDto> GetPaymentByOrderingId(int id);
        Task<List<ResultPaymentDto>> GetPaymentsByUserId(string id);
        Task<bool> AddPayment(CreatePaymentDto createPaymentDto);
        Task<bool> CancelPaymentByOrderingId(int id);
    }
}
