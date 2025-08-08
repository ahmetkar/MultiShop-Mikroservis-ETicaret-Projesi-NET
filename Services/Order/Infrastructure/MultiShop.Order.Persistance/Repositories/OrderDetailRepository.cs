using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;
using MultiShop.Order.Persistance.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Order.Persistance.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly OrderContext _orderContext;
        public OrderDetailRepository(OrderContext orderContext)
        {
            _orderContext = orderContext;
        }
        public List<OrderDetail> GetOrderDetailsByOrderingId(int id)
        {
            var values = _orderContext.OrderDetails.Where(x => x.OrderingId == id).ToList();
            return values;
        }
    }
}
