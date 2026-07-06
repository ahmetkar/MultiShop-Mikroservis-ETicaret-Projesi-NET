namespace MultiShop.Payment.DTOs
{
    public class RefundPaymentDto
    {
        public int OrderingId { get; set; }
        public int PaymentId { get; set; }
        public int UserId { get; set; }
    }
}
