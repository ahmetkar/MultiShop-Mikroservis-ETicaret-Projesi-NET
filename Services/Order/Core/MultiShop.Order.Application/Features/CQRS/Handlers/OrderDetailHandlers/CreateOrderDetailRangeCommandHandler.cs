using MultiShop.Order.Application.Features.CQRS.Commands.OrderDetailCommands;
using MultiShop.Order.Application.Features.CQRS.Results.OrderDetailResults;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Order.Application.Features.CQRS.Handlers.OrderDetailHandlers
{
    public class CreateOrderDetailRangeCommandHandler
    {
        private readonly IRepository<OrderDetail> _repository;

        public CreateOrderDetailRangeCommandHandler(IRepository<OrderDetail> repository)
        {
            _repository = repository;
        }
        public async Task Handle(List<CreateOrderDetailCommand> commands)
        {
            var list = new List<OrderDetail>();

            foreach (var command in commands) {
               var item =  new OrderDetail()
                {
                    ProductAmount = command.ProductAmount,
                    ProductName = command.ProductName,
                    OrderingId = command.OrderingId,
                    ProductId = command.ProductId,
                    ProductTotalPrice = command.ProductTotalPrice,
                    ProductPrice = command.ProductPrice
                };
                list.Add(item);
            }
            
            await _repository.CreateRangeAsync(list);
        }
    }

}

