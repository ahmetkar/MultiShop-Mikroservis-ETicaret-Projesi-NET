using MultiShop.Payment.DTOs;

namespace MultiShop.Payment.Services
{
    public interface IPaymentService
    {
        Task<ResultPaymentDto> GetPaymentByOrderingId(int id);
        Task<ResultCreatePaymentDto> AddPayment(CreatePaymentDto createPaymentDto,CancellationToken cancellationToken);
        Task<bool> CancelPaymentByOrderingId(int id);
        Task<List<ResultPaymentDto>> GetAllPaymentByUserId(string id);

        Task<(bool, string)> RefundPayment(RefundPaymentDto refundPaymentDto, CancellationToken cancellationToken);


    }
}
