using MultiShop.Order.Application.Features.Mediator.Commands.OrderingCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Order.Application.Features.Mediator.Results.OrderingResults
{
    public class GetOrderingByIdQueryResult
    {
        public int OrderingId { get; set; }
        public string UserId { get; set; }
        public bool IsOrderCompleted { get; set; }
        public bool IsOrderDelivered { get; set; }
        public decimal TotalPrice { get; set; }
        public int ShippingAdressId { get; set; }
        public int BillingAddressId { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
