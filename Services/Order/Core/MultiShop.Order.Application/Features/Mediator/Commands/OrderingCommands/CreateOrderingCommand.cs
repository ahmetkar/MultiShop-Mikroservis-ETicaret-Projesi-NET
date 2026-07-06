using MediatR;
using MultiShop.Order.Application.Features.Mediator.Results.OrderingResult;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Features.Mediator.Commands.OrderingCommands
{
    public class CreateOrderingCommand : IRequest<CreateOrderingResult>
    {
        public string UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public int ShippingAdressId { get; set; }
        public int BillingAddressId { get; set; }
        public DateTime OrderDate { get; set; }
    }
}