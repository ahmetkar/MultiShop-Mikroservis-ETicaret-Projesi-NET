using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.DtoLayer.BasketDtos
{
    public class BasketTotalDto
    {
        public string UserId { get; set; }
        public string DiscountCode { get; set; }
        public int? DiscountRate { get; set; }
        public List<BasketItemDto> BasketItems { get; set; } = new List<BasketItemDto> { };
        public double TotalPrice { get; set; }
        public double KDVPrice { get; set; }
        public double TotalPriceWithoutKDV { get; set; }

    }
}
