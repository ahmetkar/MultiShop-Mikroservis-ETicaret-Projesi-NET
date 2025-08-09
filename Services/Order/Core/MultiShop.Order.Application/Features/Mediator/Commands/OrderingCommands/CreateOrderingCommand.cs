using MediatR;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Features.Mediator.Commands.OrderingCommands
{
    public class CreateOrderingCommand : IRequest<int>
    {
        public string UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public int ShippingAdressId { get; set; }
        public int BillingAddressId { get; set; }
        public DateTime OrderDate { get; set; }
    }
}