using MultiShop.Order.Application.Features.CQRS.Queries.OrderDetailQueries;
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
    public class GetOrderDetailByIdQueryHandler
    {
        private readonly IOrderDetailRepository _orderDetailRepository;

        public GetOrderDetailByIdQueryHandler(IOrderDetailRepository repository)
        {
            _orderDetailRepository = repository;
        }
        public async Task<List<GetOrderDetailByIdQueryResult>> Handle(GetOrderDetailByIdQuery query)
        {
            var detaillist = new List<GetOrderDetailByIdQueryResult>();
            var details = _orderDetailRepository.GetOrderDetailsByOrderingId(query.Id);
            foreach (var x in details)
            {
                var n = new GetOrderDetailByIdQueryResult
                {

                    OrderDetailId = x.OrderDetailId,
                    OrderingId = x.OrderingId,
                    ProductAmount = x.ProductAmount,
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    ProductPrice = x.ProductPrice,
                    ProductTotalPrice = x.ProductTotalPrice,
                    ProductFilters = x.ProductFilters

                };

                detaillist.Add(n);

            }
            return detaillist;
        }
    }
}

