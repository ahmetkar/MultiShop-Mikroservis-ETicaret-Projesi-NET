using MultiShop.DtoLayer.PaymentDtos;

namespace MultiShop.WebUI.Models
{
    public class CreatePaymentViewModel
    {
         
            public int OrderingId { get; set; } = 0;
            public string OwnerNameAndSurname { get; set; }
            public string CardType { get; set; }
            public string LastDateYear { get; set; }
            public string LastDateMonth { get; set; }
            public string LastFourNumber { get; set; }
            
            public int PaymentTotal { get; set; }
      
            
    }
}
