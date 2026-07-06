namespace MultiShop.Payment.DTOs
{
    public class ResultCreatePaymentDto
    {

        public int PaymentId { get; set; }
        public int OrderingId { get; set; }
        public int PaymentTotal { get; set; }
        public string UserId { get; set; }
        public int CardInfoId { get; set; }
        
    }
}
