using Microsoft.EntityFrameworkCore;

namespace MultiShop.Payment.DAL.Entities
{
    public class PaymentInfo
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int OrderingId { get; set; }
        public int PaymentTotal { get; set; }
        public string PaymentType { get; set; }
        public string CardInfoId { get; set; }
        public CardInfo CardInfo { get; set; }
    }
}
