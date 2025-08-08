using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Order.Domain.Entities
{
    public class Ordering
    {
        public int OrderingId { get; set; }
        public string UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public int ShippingAdressId { get; set; }
        public Adress ShippingAdress { get; set; }  
        public int BillingAddressId { get; set; }
        public Adress BillingAddress { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}   
