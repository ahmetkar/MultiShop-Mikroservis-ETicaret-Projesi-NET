using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.DtoLayer.PaymentDtos
{
    public class CreatePaymentDto
    {
        public int OrderingId { get; set; }
        public int PaymentTotal { get; set; }
        
        public string UserId { get; set; }
        public string PaymentType { get; set; }
        public string OwnerName { get; set; }
        public string OwnerSurname { get; set; }
        public string CardType { get; set; }
        public string CardBrand { get; set; }
        public string LastDateYear { get; set; }
        public string LastDateMonth { get; set; }
        public string LastFourNumber { get; set; }
        public string CardBankName { get; set; }
    }
}
