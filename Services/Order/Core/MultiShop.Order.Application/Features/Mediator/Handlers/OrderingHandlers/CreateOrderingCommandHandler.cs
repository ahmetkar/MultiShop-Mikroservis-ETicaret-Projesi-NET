using MediatR;
using MultiShop.Order.Application.Features.Mediator.Commands.OrderingCommands;
using MultiShop.Order.Application.Features.Mediator.Results.OrderingResult;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Features.Mediator.Handlers.OrderingHandlers
{
    public class CreateOrderingCommandHandler : IRequestHandler<CreateOrderingCommand,CreateOrderingResult?>
    {
        private readonly IRepository<Ordering> _repository;

        public CreateOrderingCommandHandler(IRepository<Ordering> repository)
        {
            _repository = repository;
        }

        public async Task<CreateOrderingResult?> Handle(CreateOrderingCommand request, CancellationToken cancellationToken)
        {
            var ordering = new Ordering
            {
                OrderDate = request.OrderDate,
                TotalPrice = request.TotalPrice,
                UserId = request.UserId,
                BillingAddressId = request.BillingAddressId,
                ShippingAdressId = request.ShippingAdressId,
            };
            var res = await _repository.CreateAsync(ordering);
            if (res >=1)
            {
                return new CreateOrderingResult
                {
                    OrderingId = ordering.OrderingId,
                    UserId = ordering.UserId,
                    TotalPrice = ordering.TotalPrice,
                    OrderDate = ordering.OrderDate,
                    BillingAddressId = ordering.BillingAddressId,
                    ShippingAdressId = ordering.ShippingAdressId
                };
            }else
            {
                return null;
            }
        }
    }
}