namespace MultiShop.Payment.DTOs
{
    public class ResultPaymentDto
    {
        public int Id { get; set; }
        public int CardInfoId { get; set; }
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
