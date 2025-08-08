using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Order.Application.Features.Mediator.Results.OrderingResult
{
    public class GetOrderingByIdQueryResult
    {
        public int OrderingId { get; set; }
        public string UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public int ShippingAdressId { get; set; }
        public int BillingAddressId { get; set; }
        public DateTime OrderDate { get; set; }
      
    }
}
