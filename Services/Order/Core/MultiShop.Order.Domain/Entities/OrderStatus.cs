using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Order.Domain.Entities
{
    public enum OrderStatus
    {
        OrderCreated = 1,
        OrderNotCreated = 2,
        PendingPayment = 3,
        PaymentCompleted = 4,
        PaymentFailed = 5,
        CargoCreated = 6,
        CargoFailed = 7,
        Cancelled = 8,
        Completed = 9,
  }
}
